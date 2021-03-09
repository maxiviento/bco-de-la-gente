using Newtonsoft.Json;

namespace SintysWS.Modelo
{
    public class Desempleo
    {
        [JsonProperty("CANTIDAD_CUOTAS")]
        public int? CantidadCuotas { get; set; }
        [JsonProperty("CANT_CUOTAS_LIQUIDADAS")]
        public int? CantCuotasLiquidadas { get; set; }
        [JsonProperty("PERIODO")]
        public int? Periodo { get; set; }
        [JsonProperty("BASE_ORIGEN")]
        public string BaseOrigen { get; set; }
    }
}
