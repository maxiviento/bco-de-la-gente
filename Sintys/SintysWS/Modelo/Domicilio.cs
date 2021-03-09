using Newtonsoft.Json;

namespace SintysWS.Modelo
{
    public class Domicilio
    {

        [JsonProperty("PROVINCIA")]
        public string Provincia { get; set; }
        [JsonProperty("LOCALIDAD")]
        public string Localidad { get; set; }
        [JsonProperty("CODIGO_POSTAL")]
        public string CodigoPostal { get; set; }
        [JsonProperty("CALLE")]
        public string Calle { get; set; }
        [JsonProperty("NUMERO")]
        public int? Numero { get; set; }
        [JsonProperty("PISO")]
        public string Piso { get; set; }
        [JsonProperty("DEPTO")]
        public string Depto { get; set; }
        [JsonProperty("BASE_ORIGEN")]
        public string BaseOrigen { get; set; }
    }
}
