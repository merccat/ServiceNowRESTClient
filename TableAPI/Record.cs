using System;
using Newtonsoft.Json;

namespace ServiceNow
{

    /// <summary>
    /// Base table for a service now record.  All records should include this at minimum
    /// </summary>
    public abstract class Record
    {
        [JsonProperty("sys_id")]
        public string sys_id { get; set; }

        // We don't want to serialize sys_id because if we have it, we should always use it in the table path rather than putting it into a query.  Also, if we are performing a POST we won't have a vaid sys_id anyway.
        public bool ShouldSerializesys_id() { return false; }
    }

}