using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ServiceNow.TableAPI
{
    /// <summary>
    /// Since Service Now requires related objects (returned as a ResourceLink) to be updated as a string reference to
    /// the related entity (such as sys_id), this class will on Serialization for transmission back to Service Now, convert 
    /// objects of type ResourceLink to a simple string value using the ToString method of the ReourceLink.
    /// </summary>
    public class ResourceLinkConverter : JsonConverter
    {
        public override bool CanRead { get { return false; } }
        public override bool CanWrite { get { return base.CanWrite; } }
        public override bool CanConvert(Type objectType) { return objectType == typeof(ResourceLink); }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);

            if(t.Type != JTokenType.Object)
            {
                t.WriteTo(writer);
            }
            else
            {
                JToken sys_id = JToken.FromObject(value.ToString());                
                sys_id.WriteTo(writer);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // We have set this to not operate on Read
            throw new NotImplementedException();
        }
    }
}
