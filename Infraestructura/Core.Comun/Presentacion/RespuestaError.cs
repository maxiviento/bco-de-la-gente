using Newtonsoft.Json;

namespace Infraestructura.Core.Comun.Presentacion
{
   public class RespuestaError
    {
       [JsonProperty(PropertyName = "titulo")]
        public string Titulo { get; set; }

       [JsonProperty(PropertyName = "descripcion", NullValueHandling = NullValueHandling.Ignore)]
        public string Descripcion { get; set; }

       [JsonProperty(PropertyName = "origen", NullValueHandling = NullValueHandling.Ignore)]
        public string Origen { get; set; }
    }
}
