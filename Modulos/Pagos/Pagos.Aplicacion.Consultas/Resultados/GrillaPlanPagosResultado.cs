using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.Core.Comun.Dato;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class GrillaPlanPagosResultado
    {
        public Id IdFormulario { get; set; }
        public int NroFormulario { get; set; }
        public int? NroPrestamo { get; set; }
        public string ApellidoNombreSolicitante { get; set; }
        public string Localidad { get; set; }
        public string Departamento { get; set; }
        public string Linea { get; set; }
        public string CuilSolicitante { get; set; }
        public DateTime? FechFinPago { get; set; }
        public decimal? MonPres { get; set; }
        public int? CanCuot { get; set; }
        public bool PuedeCrearPlan { get; set; }
    }
}
