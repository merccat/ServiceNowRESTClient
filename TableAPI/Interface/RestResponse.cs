using System.Collections.Generic;

namespace ServiceNow.Interface
{
    public interface IRestResponse
    {
        string RawJSON { get; set; }
        string ErrorMsg { get; }
        bool IsError { get; }
        int ResultCount { get; } 
    }
    public interface IRestQueryResponse<T> : IRestResponse
    {        
        ICollection<T> Result { get; set; }               
    }

    public interface IRestSingleResponse<T> : IRestResponse
    {
        T Result { get; set; }
    }
}
