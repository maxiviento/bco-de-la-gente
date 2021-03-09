using Newtonsoft.Json;

namespace SintysWS.Modelo
{
    public class EmpleoPresunto
    {
        [JsonProperty("CUIT_EMPLEADOR")]
        public long? CuitEmpleador { get; set; }
        [JsonProperty("EMPLEADOR")]
        public string Empleador { get; set; }
        [JsonProperty("PERIODO")]
        public int? Periodo { get; set; }
        [JsonProperty("BASE_ORIGEN")]
        public string BaseOrigen { get; set; }
    }
}
