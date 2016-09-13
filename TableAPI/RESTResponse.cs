using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceNow
{
    public abstract class RESTResponse
    {
        public String RawJSON { get; set; }
        public String ErrorMsg { get; set; }

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

    public class RESTQueryResponse<T> : RESTResponse
    {
        public RESTQueryResponse()
        {
            this.result = new List<T>();
        }

        public List<T> result { get; set; }

    }

    public class RESTSingleResponse<T> : RESTResponse
    {
        public RESTSingleResponse() { }

        public T result { get; set; }

    }
}
