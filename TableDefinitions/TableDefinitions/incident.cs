using Newtonsoft.Json;

namespace ServiceNow.TableDefinitions
{
    public class incident : Record
    {
        // Example of dot traversing
        [JsonProperty("caller_id.first_name")]
        public string caller_first_name { get; set; }

        [JsonProperty("caller_id.last_name")]
        public string caller_last_name { get; set; }

        [JsonProperty("caller_id.location.name")]
        public string caller_location_name { get; set; }

        [JsonProperty("caller_id.location.latitude")]
        public string caller_location_latitude { get; set; }

        [JsonProperty("caller_id.location.longitude")]
        public string Location_name_longitude { get; set; }

        // Example of including a related resource (returned as a JSON Link)
        [JsonProperty("caller_id")]
        public ResourceLink caller_id { get; set; }

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
