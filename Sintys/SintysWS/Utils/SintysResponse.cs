using Newtonsoft.Json;

namespace SintysWS.Utils
{
    public class SintysResponse<TResultado>
    {
        [JsonProperty("operacion")]
        public string Operacion { get; set; }

        [JsonProperty("estadoTematica")]
        public string EstadoTematica { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("idConsulta")]
        public string IdConsulta { get; set; }

        [JsonProperty("resultado")]
        public TResultado Resultado { get; set; }
        
        public bool Ok => string.IsNullOrEmpty(Error);
    }
}