
namespace ServiceNow.Interface
{
    interface ITableAPIClient<T>
     where T : ServiceNow.Record
    {
        void Delete(string id);
        RESTSingleResponse<T> GetById(string id);
        RESTQueryResponse<T> GetByQuery(string query);
        RESTSingleResponse<T> Post(T record);
        RESTSingleResponse<T> Put(T record);
    }
}
