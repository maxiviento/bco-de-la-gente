using System;
using Newtonsoft.Json;
using System.Globalization;

namespace SintysWS.Modelo
{
    public class PensionNoContributiva
    {
        [JsonProperty("TIPO_BENEFICIO")]
        public string TipoBeneficio { get; set; }
        [JsonProperty("DESCRIPCION_BENEFICIO")]
        public string DescripcionBeneficio { get; set; }
        [JsonProperty("PERIODO")]
        public int? Periodo { get; set; }
        [JsonProperty("BASE_ORIGEN")]
        public string BaseOrigen { get; set; }
        [JsonProperty("FECHA_ALTA")]
        public string FechaAltaString { get; set; }
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
