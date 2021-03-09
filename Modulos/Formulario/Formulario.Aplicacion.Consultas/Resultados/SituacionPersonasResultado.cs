using System;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class SituacionPersonasResultado
    {
        public decimal? IdNumero { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string SexoId { get; set; }
        public string NombreSexo { get; set; }
        public string TipoDocumento { get; set; }
        public string CodigoPais { get; set; }
        public string NroDocumento { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Edad { get; set; }
        public string Departamento { get; set; }
        public string Localidad { get; set; }
        public string Cuil { get; set; }
        public string CodAreaTelefono { get; set; }
        public string NroTelefono { get; set; }
        public string CodAreaCelular { get; set; }
        public string NroCelular { get; set; }
        public string Mail { get; set; }
    }
}
