using Newtonsoft.Json;
using ServiceNow;

namespace ServiceNow.TableDefinitions
{
    public class task_time_worked : Record
    {
        // Note the custom fields... these are specific to our environment, update as appropriate for your environment.
        [JsonProperty("user.u_employee_number")]
        public string u_employee_number { get; set; }

        [JsonProperty("u_actual_work_date")]
        public string u_actual_work_date { get; set; }

        [JsonProperty("sys_updated_on")]
        public string sys_updated_on { get; set; }

        // Example of including a related resource (returned as a JSON Link)
        [JsonProperty("task")]
        public ResourceLink task { get; set; }

        [JsonProperty("time_in_seconds")]
        public string time_in_seconds { get; set; }

        [JsonProperty("comments")]
        public string comments { get; set; }

        // Example of dot traversing
        [JsonProperty("task.short_description")]
        public string task_short_description { get; set; }

        [JsonProperty("task.number")]
        public string task_number { get; set; }
    }
}
