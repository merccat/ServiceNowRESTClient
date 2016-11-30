using System;
using Newtonsoft.Json;

namespace ServiceNow
{
    public class ResourceLink
    {
        [JsonProperty("link")]
        public string link { get; set; }    // REST URL for child record

        [JsonProperty("value")]
        public string value { get; set; }   // Reference to the child record (sys_id)

        // The ToString Method is called on serialization back to servicenow through the ResourceLinkConverter
        public override string ToString() { return value; }
    }
}
