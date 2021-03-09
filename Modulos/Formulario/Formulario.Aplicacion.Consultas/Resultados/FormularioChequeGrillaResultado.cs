using System;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class FormularioChequeGrillaResultado
    {
        public int IdFormulario { get; set; }
        public int IdPrestamo { get; set; }
        public string Linea { get; set; }
        public string Departamento { get; set; }
        public string Localidad { get; set; }
        public string ApellidoNombreSolicitante { get; set; }
        public int NroPrestamo { get; set; }
        public int NroFormulario { get; set; }
        public string NroCheque { get; set; }
        public string NroChequeNuevo { get; set; }
        public DateTime? FechaVencimientoCheque { get; set; }
        public string CuilSolicitante { get; set; }
        public string Origen { get; set; }
        public DateTime? FechaAlta { get; set; }
    }

}
