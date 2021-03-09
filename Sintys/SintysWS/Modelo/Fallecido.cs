using System;
using System.Globalization;
using Newtonsoft.Json;

namespace SintysWS.Modelo
{
    public class Fallecido
    {
        [JsonProperty("FALLECIDO")]
        public string Fallecidos { get; set; }
        [JsonProperty("FECHA_FALLECIMIENTO")]
        public string FechaFallecimientoString { get; set; }
        [JsonProperty("IMAGEN")]
        public Byte[] Imagen { get; set; }
        [JsonProperty("EXTENSION")]
        public string Extension { get; set; }
        [JsonProperty("ACTA")]
        public string Acta { get; set; }
        [JsonProperty("TOMO")]
        public string Tomo { get; set; }
        [JsonProperty("FOLIO")]
        public string Folio { get; set; }
        [JsonProperty("ANO_ACTA")]
        public int? AnioActa { get; set; }
        [JsonProperty("BASE_ORIGEN")]
        public string BaseOrigen { get; set; }
        public DateTime? FechaFallecimiento
        {
            get
            {
                if (string.IsNullOrEmpty(FechaFallecimientoString))
                    return null;
                return DateTime.ParseExact(FechaFallecimientoString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            set
            {
                if (value.HasValue)
                    FechaFallecimientoString = value.Value.ToString("dd/MM/yyyy");
            }
        }
    }
}
