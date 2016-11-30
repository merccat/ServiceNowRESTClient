using Newtonsoft.Json;

namespace ServiceNow.TableDefinitions
{
    /// <summary>
    /// Contract for the Location Table (cmn_location)
    /// </summary>
    public class location : ServiceNow.Record
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("full_name")]
        public string full_name { get; set; }

        [JsonProperty("phone")]
        public string phone { get; set; }

        [JsonProperty("street")]
        public string street { get; set; }

        [JsonProperty("city")]
        public string city { get; set; }

        [JsonProperty("state")]
        public string state { get; set; }

        [JsonProperty("zip")]
        public string zip { get; set; }

        [JsonProperty("latitude")]
        public double latitude { get; set; }

        [JsonProperty("longitude")]
        public double longitude { get; set; }

        [JsonProperty("parent")]
        public ResourceLink parent { get; set; }

        [JsonProperty("contact")]
        public ResourceLink contact { get; set; }

        [JsonProperty("contact.first_name")]
        public string contact_first_name { get; set; }

        [JsonProperty("contact.last_name")]
        public string contact_last_name { get; set; }
    }
}
