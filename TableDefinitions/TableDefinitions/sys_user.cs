using Newtonsoft.Json;

namespace ServiceNow.TableDefinitions
{
    public class sys_user : ServiceNow.Record
    {
        [JsonProperty("employee_number")]
        public string employee_number { get; set; }

        [JsonProperty("first_name")]
        public string first_name { get; set; }

        [JsonProperty("last_name")]
        public string last_name { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

        [JsonProperty("phone")]
        public string phone { get; set; }

        [JsonProperty("u_notes")]
        public string u_notes { get; set; } 

        [JsonProperty("location.name")]
        public string Location_name { get; set; }

        [JsonProperty("location")]
        public ResourceLink location { get; set; }
    }
}
