using Newtonsoft.Json;

namespace SintysWS.Modelo
{
    public class EmpleoFormal
    {
        [JsonProperty("CUIT_EMPLEADOR")]
        public long? CuitEmpleador { get; set; }
        [JsonProperty("DENOMINACION_EMPLEADOR")]
        public string DenominacionEmpleador { get; set; }
        [JsonProperty("SITUACION_LABORAL")]
        public string SituacionLaboral { get; set; }
        [JsonProperty("ACTIVIDAD_TRABAJADOR")]
        public string ActividadTrabajador { get; set; }
        [JsonProperty("PROVINCIA")]
        public string Provincia { get; set; }
        [JsonProperty("MONTO")]
        public decimal? Monto { get; set; }
        [JsonProperty("PERIODO")]
        public int Periodo { get; set; }
        [JsonProperty("BASE_ORIGEN")]
        public string BaseOrigen { get; set; }
    }
}
