using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infraestructura.Core.Comun.Dato
{
    public class IdJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JValue jv = new JValue((long)((Id)value).Valor);
            jv.WriteTo(writer);
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.Value != null)
            {
                return new Id(reader.Value.ToString());
            }
            return existingValue;
        }
        public override bool CanRead
        {
            get { return true; }
        }
        public override bool CanWrite
        {
            get { return true; }
        }
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Id) || objectType == typeof(Id?);
        }
    }
}
