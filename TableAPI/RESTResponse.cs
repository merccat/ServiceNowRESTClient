using System.Collections.Generic;
using ServiceNow.Interface;

namespace ServiceNow
{
    public abstract class RESTResponse
    {
        public string RawJSON { get; set; }
        public string ErrorMsg { get; set; }

        public RESTResponse()
        {
            this.RawJSON = "";
            this.ErrorMsg = "";
        }

        public bool IsError
        {
            get
            {
                if (ErrorMsg.Length > 0) { return true; }
                return false;
            }
        }
    }

    public class RESTQueryResponse<T> : RESTResponse, IRestQueryResponse<T>
    {        
        public RESTQueryResponse()
        {
            this.Result = new List<T>();
        }

        public ICollection<T> Result { get; set; }

        public int ResultCount
        {
            get
            {
                if (Result == null) { return 0; }
                return Result.Count;
            }
        }
    }

    public class RESTSingleResponse<T> : RESTResponse, IRestSingleResponse<T>
    {
        public RESTSingleResponse() { }

        public T Result { get; set; }

        public int ResultCount
        {
            get
            {
                if (Result == null) { return 0; }
                return 1;
            }
        }
    }
}
