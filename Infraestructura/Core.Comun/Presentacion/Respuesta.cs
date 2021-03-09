using System.Collections.Generic;
using Newtonsoft.Json;

namespace Infraestructura.Core.Comun.Presentacion
{
    public class Respuesta
    {
        [JsonProperty(PropertyName = "codigo")]
        public int Status { get; set; }

        [JsonProperty(PropertyName = "errorControlado")]
        public bool ManagedError { get; set; }

        [JsonProperty(PropertyName = "resultado", NullValueHandling = NullValueHandling.Ignore)]
        public dynamic Data { get; private set; }

        [JsonProperty(PropertyName = "errores", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<RespuestaError> Errores { get; private set; }

        public Respuesta(int status,dynamic data)
        {
            Data = data;
            Status = status;
        }

        public Respuesta(IEnumerable<RespuestaError> errores)
        {
            Errores = errores;
        }

        private Respuesta()
        {
        }
    }
}