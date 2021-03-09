using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Globalization;

namespace SintysWS.Modelo
{
    public class PensionJubilacion
    {
        [JsonProperty("TIPO_BENEFICIO")]
        public string TipoBeneficio { get; set; }
        [JsonProperty("DESCRIPCION_BENEFICIO")]
        public string DescripcionBeneficio { get; set; }
        [JsonProperty("FECHA_ALTA")]
        public string FechaAltaString { get; set; }
        [JsonProperty("MONTO")]
        public decimal? Monto { get; set; }
        [JsonProperty("PERIODO")]
        public int? Periodo { get; set; }
        [JsonProperty("BASE_ORIGEN")]
        public string BaseOrigen { get; set; }
        public DateTime? FechaAlta
        {
            get
            {
                if (string.IsNullOrEmpty(FechaAltaString))
                    return null;
                return DateTime.ParseExact(FechaAltaString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            set
            {
                if(value.HasValue)
                FechaAltaString = value.Value.ToString("dd/MM/yyyy");
            }
        }
    }
}
