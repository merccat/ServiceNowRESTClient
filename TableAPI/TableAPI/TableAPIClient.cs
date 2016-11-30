using System;
using System.Web;
using System.Net;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Reflection;
using System.Configuration;

using ServiceNow;

namespace ServiceNow.TableAPI
{
    /// <summary>
    /// Client for the ServiceNow REST TableAPI.
    /// </summary>
    /// <typeparam name="T">Data type to retrieve for each row retrieved from the ServiceNow table.<para />
    /// Property names in the supplied type must match ServiceNow field names to be retrieved in a record from the specified table.</typeparam>
    public class TableAPIClient<T> : ServiceNow.Interface.ITableAPIClient<T> where T : Record
    {
        private String _TableName;
        private String _InstanceName;
        private String _FieldList;
        private WebClient ServiceNowClient;

        /// <summary>
        /// Constructor for the TableAPI client
        /// </summary>
        /// <param name="tableName">Name of the table to be accessed.  Must match ServiceNow's table name.</param>
        /// <param name="instanceName">Name of your ServiceNow instance to be accessed: ([INSTANCENAME].service-now.com).<para /> Instance Name Only, DO NOT use the full URL of your instance.</param>
        /// <param name="credentials">Credentials for the user who will be used to access the table through the REST API.</param>
        public TableAPIClient(String tableName, String instanceName, NetworkCredential credentials)
        {
            initialize(tableName, instanceName, credentials);
        }

        /// <summary>
        /// Constructor for the TableAPI client
        /// </summary>
        /// <param name="tableName">Name of the table to be accessed.  Must match ServiceNow's table name.</param>
        /// <param name="instanceName">Name of your ServiceNow instance to be accessed: ([INSTANCENAME].service-now.com).<para /> Instance Name Only, DO NOT use the full URL of your instance.</param>
        /// <param name="userName">UserName for the user who will be used to access the table through the REST API.</param>
        /// <param name="password">Password for the user who will be used to access the table through the REST API.</param>
        public TableAPIClient(String tableName, String instanceName, String userName, string password)
        {
            NetworkCredential credentials = new NetworkCredential { UserName = userName, Password = password };
            initialize(tableName, instanceName, credentials);
        }

        /// <summary>
        /// Constructor for the TableAPI Client.  Requires ServiceNow User, password and instance name to be set in your .config file.
        /// </summary>
        /// <param name="tableName">Name of the table to be accessed.  Must match ServiceNow's table name.</param>
        public TableAPIClient(String tableName)
        {
            NetworkCredential credentials = new NetworkCredential
            {
                UserName = ConfigurationManager.AppSettings["ServiceNowUser"],
                Password = ConfigurationManager.AppSettings["ServiceNowPass"]
            };
            initialize(
                tableName,
                ConfigurationManager.AppSettings["ServiceNowInstance"],
                credentials);
        }

        private void initialize(String tableName, String instanceName, NetworkCredential credentials)
        {
            _TableName = tableName;
            _InstanceName = instanceName;

            // Initialize the Web Client
            ServiceNowClient = new WebClient();
            ServiceNowClient.Credentials = credentials;

            // Build the field list from the record type that will be retrieved
            _FieldList = "";
            Type i = typeof(T);
            foreach (var prop in i.GetProperties())
            {
                // We need to build the field list using the JsonProperty attributes since those strings can contain our dot notation.
                var field = prop.CustomAttributes.FirstOrDefault(x => x.AttributeType.Name == "JsonPropertyAttribute");
                if (field != null)
                {
                    var fieldName = field.ConstructorArguments.FirstOrDefault(x => x.ArgumentType.Name == "String");
                    if (fieldName != null)
                    {
                        if (_FieldList.Length > 0) { _FieldList += ","; }
                        _FieldList += fieldName.Value;
                    }
                }
            }
        }

        private String URL
        {
            get
            {
                return "https://" + _InstanceName + ".service-now.com/api/now/table/" + _TableName;
            }
        }

        private string ParseWebException(WebException ex)
        {
            string message = ex.Message + "\n\n";

            if (ex.Response != null)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                dynamic obj = JsonConvert.DeserializeObject(resp);

                message = "status: " + obj.status + "\n";
                message += ex.Message + "\n\n";
                message += "message: " + obj.error.message + "\n";
                message += "detail: " + obj.error.detail + "\n";
            }
            
            return message;
        }

        /// <summary>
        /// Retrieves a single record contained in the RESTSingleResponse of the type T defined for the table client.<para />
        /// Any errors will be fully captured and returned in the ErrorMsg property of the response.
        /// </summary>
        /// <param name="id">sys_id of the record to be retrieved.</param>
        /// <returns>A RestResponse containing a single result of T (if successful) along with any error messages (if any).</returns>
        public RESTSingleResponse<T> GetById(string id)
        {
            var response = new RESTSingleResponse<T>();

            try
            {
                response.RawJSON = ServiceNowClient.DownloadString(URL + "/" + id + "?&sysparm_fields=" + _FieldList);
            }
            catch (WebException ex)
            {
                response.ErrorMsg = ParseWebException(ex);
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "An error occured retrieving the REST response: " + ex.Message;
            }

            RESTSingleResponse<T> tmp = JsonConvert.DeserializeObject<RESTSingleResponse<T>>(response.RawJSON);
            if (tmp != null) { response.Result = tmp.Result; }

            return response;
        }

        /// <summary>
        /// Retrieves a record set in the response (as a list) based on the query result.
        /// </summary>
        /// <param name="query">A standard service-now table query.</param>
        /// <returns>A RestResponse containing a result list of T (if successful) along with any error messages (if any).</returns>
        public RESTQueryResponse<T> GetByQuery(string query)
        {
            var response = new RESTQueryResponse<T>();

            try
            {
                response.RawJSON = ServiceNowClient.DownloadString(URL + "?&sysparm_fields=" + _FieldList + "&sysparm_query=" + query);
            }
            catch (WebException ex)
            {
                response.ErrorMsg = ParseWebException(ex);
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "An error occured retrieving the REST response: " + ex.Message;
            }

            RESTQueryResponse<T> tmp = JsonConvert.DeserializeObject<RESTQueryResponse<T>>(response.RawJSON);
            if (tmp != null) { response.Result = tmp.Result; }

            return response;
        }

        /// <summary>
        /// Updates an existing record in the table.  Make sure your record has sys_id populated.  <para />
        /// All fields of T will be sent in the update so make sure they are ALL populated unless you want the field to be emptied. <para />
        /// A good practice is to first retrieve the record using GetById, then only edit the fields you want to change before submitting a Put.
        /// </summary>
        /// <param name="record">The record of type T to be updated in ServiceNow.  Make sure sys_id is included.</param>
        /// <returns>A RestResponse containing a single result of T with your updated record (if successfull) along with any error messages (if any).</returns>
        public RESTSingleResponse<T> Put(T record)
        {
            var response = new RESTSingleResponse<T>();

            try
            {
                string data = JsonConvert.SerializeObject(record, new ResourceLinkConverter());
                response.RawJSON = ServiceNowClient.UploadString(URL + "/" + record.sys_id + "?&sysparm_fields=" + _FieldList, "PUT", data);
            }
            catch (WebException ex)
            {
                response.ErrorMsg = ParseWebException(ex);
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "An error occured retrieving the REST response: " + ex.Message;
            }

            RESTSingleResponse<T> tmp = JsonConvert.DeserializeObject<RESTSingleResponse<T>>(response.RawJSON);
            if (tmp != null) { response.Result = tmp.Result; }

            return response;
        }

        /// <summary>
        /// Creates a new record in the table using the fields provided in the record of type T.
        /// </summary>
        /// <param name="record">The record of Type T to be added to the table in ServiceNow.</param>
        /// <returns>A RestResponse containing a single result of T (if successful) for your newly created record, along with any error messages (if any).</returns>
        public RESTSingleResponse<T> Post(T record)
        {
            var response = new RESTSingleResponse<T>();

            try
            {                
                string data = JsonConvert.SerializeObject(record, new ResourceLinkConverter());
                response.RawJSON = ServiceNowClient.UploadString(URL + "?&sysparm_fields=" + _FieldList, "POST", data);
            }
            catch (WebException ex)
            {
                response.ErrorMsg = ParseWebException(ex);
            }
            catch (Exception ex)
            {
                response.ErrorMsg = "An error occured retrieving the REST response: " + ex.Message;
            }

            RESTSingleResponse<T> tmp = JsonConvert.DeserializeObject<RESTSingleResponse<T>>(response.RawJSON);
            if (tmp != null) { response.Result = tmp.Result; }

            return response;
        }

        /// <summary>
        /// Deletes a record in the active table with the specified id.<para />
        /// WARNING: Since there is no return type, you will need to catch any errors externally.  The exception message will still include all details including any response from Service Now.
        /// </summary>
        /// <param name="id">sys_id of the record to delete.</param>
        public void Delete(string id)
        {
            try
            {
                ServiceNowClient.UploadString(URL + "/" + id, "DELETE", "");
            }
            catch (WebException ex)
            {
                string ErrorMsg = ParseWebException(ex);
                throw new Exception(ErrorMsg);
            }
        }
    }
}
