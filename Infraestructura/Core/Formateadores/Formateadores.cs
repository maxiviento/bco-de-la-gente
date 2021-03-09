using System.Collections.Generic;
using System.Web.Http;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Formateadores.MultipartData;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Infraestructura.Core.Formateadores
{
    public class Formateadores
    {
        public static void Registrar(HttpConfiguration config)
        {
            /*var defaultSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter>
                {
                    new IdJsonConverter()
                }
            };
            JsonConvert.DefaultSettings = () => { return defaultSettings; };
            config.Formatters.JsonFormatter
                .SerializerSettings = defaultSettings;
            config.Formatters.Add(new FormMultipartEncodedMediaTypeFormatter());*/


            config.Formatters.JsonFormatter
                .SerializerSettings
                .ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Formatters.Add(new FormMultipartEncodedMediaTypeFormatter());
        }
    }
}
