using System;
using Newtonsoft.Json;

namespace ServiceNow
{
    public class ResourceLink
    {
        [JsonProperty("link")]
        public string link { get; set; }    // REST URL for child record

        [JsonProperty("value")]
        public string value { get; set; }   // Sys_ID of child record
    }
}
