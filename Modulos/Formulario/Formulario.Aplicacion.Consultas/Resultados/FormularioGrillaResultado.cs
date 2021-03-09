using System;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class FormularioGrillaResultado
    {
        public Id Id { get; set; }
        public string NroFormulario { get; set; }
        public string Localidad { get; set; }
        public string Linea { get; set; }
        public string ApellidoNombreSolicitante { get; set; }
        public string CuilSolicitante { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public string Origen { get; set; }
        public string Estado { get; set; }
        public Id IdEstado { get; set; }
        public bool PuedeConformarPrestamo { get; set; }
        public bool EsAsociativa { get; set; }
        public string NumeroCaja { get; set; }
        public bool EsApoderado { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaSeguimiento { get; set; }
    }
}
