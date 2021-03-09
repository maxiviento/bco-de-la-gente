using System;
using Newtonsoft.Json;
using System.Globalization;

namespace SintysWS.Modelo
{
    public class PersonaFisica
    {
        [JsonProperty("ID_PERSONA")]
        public string IdPersona { get; set; }
        [JsonProperty("CUIL")]
        public long? Cuil { get; set; }
        [JsonProperty("TIPO_DOCUMENTO")]
        public string TipoDocumento { get; set; }
        [JsonProperty("NDOC")]
        public int? Ndoc { get; set; }
        [JsonProperty("DENO")]
        public string Denominacion { get; set; }
        [JsonProperty("SEXO")]
        public string Sexo { get; set; }
        [JsonProperty("FECHA_NACIMIENTO")]
        public string FechaNacimientoString { get; set; }
        [JsonProperty("PROVINCIA")]
        public string Provincia { get; set; }
        [JsonProperty("GRADO_CONFIABILIDAD")]
        public decimal? GradoConfiabilidad { get; set; }
        //[JsonIgnore]
        public DateTime? FechaNacimiento
        {
            get
            {
                if (string.IsNullOrEmpty(FechaNacimientoString))
                    return null;
                return DateTime.ParseExact(FechaNacimientoString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            set
            {
                if(value.HasValue)
                FechaNacimientoString = value.Value.ToString("dd/MM/yyyy");
            }
        }
    }
}