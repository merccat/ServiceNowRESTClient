using Newtonsoft.Json;
using ServiceNow;

namespace ServiceNow.TableDefinitions
{
    public class incident : Record
    {
        // Example of dot traversing
        [JsonProperty("caller.first_name")]
        public string caller_first_name { get; set; }

        [JsonProperty("caller.last_name")]
        public string caller_last_name { get; set; }


        // Example of including a related resource (returned as a JSON Link)
        [JsonProperty("opened_by")]
        public ResourceLink opened_by { get; set; }


        // Example of regular fields
        [JsonProperty("number")]
        public string number { get; set; }

        [JsonProperty("short_description")]
        public string short_description { get; set; }

        [JsonProperty("description")]
        public string description { get; set; }

        [JsonProperty("opened_at")]
        public string opened_at { get; set; }


        // Example of other data types
        [JsonProperty("active")]
        public bool active { get; set; }

        [JsonProperty("impact")]
        public int impact { get; set; }

    }
}
