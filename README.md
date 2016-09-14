# ServiceNowRESTClient
REST Client for ServiceNow's TableAPI written in C#
Overview

The ServiceNow TableAPI Client is designed to provide access to your ServiceNow instance's data through ServiceNow's REST TableAPI in a .Net application.  It is written in C#, however once compiled and referenced as a DLL, you could certainly use it in a VB project as well if you were so inclined.  JSON Serialization / DeSerialization is done with the Newtonsoft.JSON package and I used System.Net.WebClient for the actual communication.

 

There are only 4 classes including the client itself.  Those classes are ServiceNowRecord, ServiceNowLink, TableAPIClient and RESTResponse.

    Record (Abstract) - All record types your implementation passes between the client will need to inherit from this base class.  Types derived from ServiceNowRecord will simply contain a list of fields which you want to manipulate with the selected table.  The properties much match the field names in ServiceNow.  sys_id is already included.
    ResourceLink - A pre-built representation of a standard Link as returned by Service Now that can be included as a property in your record type.
    RESTResponse<T> - All methods that have a response will package the response in a RestReponse where T is your record type.  It is packaged this way so that errors returned by service now can be included separately.  NOTE: This is actually a base class, from which there are two classes you'll actually use derived:
        RESTQueryResponse<T> - The response property is a List<T>
        RESTSingleResponse<T> - The response property is simply T.
    TableAPIClient<T> (where T is a ServiceNowRecord) - The actual client.  On initialization it uses reflection to build a t query string which is part of why the field names in your Class derived from ServiceNowRecord need to use the actual field names.list of fields for the web reques
