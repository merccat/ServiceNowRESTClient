using Newtonsoft.Json;
using ServiceNow;

namespace ServiceNow.TableDefinitions
{
    public class sys_user : Record
    {
        [JsonProperty("u_employee_number")]
        public string u_employee_number { get; set; }

        [JsonProperty("first_name")]
        public string first_name { get; set; }

        [JsonProperty("last_name")]
        public string last_name { get; set; }

        [JsonProperty("u_unavailable")]
        public bool u_unavailable { get; set; }

        [JsonProperty("u_location")]
        public string u_location { get; set; }

        [JsonProperty("u_date_available")]
        public string u_date_available { get; set; }
    }
}
