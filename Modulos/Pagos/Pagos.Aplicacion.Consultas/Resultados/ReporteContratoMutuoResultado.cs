using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class ReporteContratoMutuoResultado
    {
        public string NumeroFormulario { get; set; }
        public string Fecha { get; set; }
        public string FechaDiaLetras { get; set; }
        public string FechaMesLetras { get; set; }
        public string FechaAnioLetras { get; set; }
        public string SolicitanteNombre { get; set; }
        public string SolicitanteDocumento { get; set; }
        public string ValorPrestamo { get; set; }
        public string ValorPrestamoString { get; set; }
        public string MontoCuota { get; set; }
        public string MontoCuotaString { get; set; }
        public string Cuotas { get; set; }
        public string CuotasString { get; set; }
        public string FechaPrimerVencimientoPago { get; set; }
        public string SolicitanteDomicilioCompleto { get; set; }
        public string SolicitanteEstadoCivil { get; set; }
        public string ListadoGarantes { get; set; }
        public string DescripcionLinea { get; set; }
        public string NroLinea { get; set; }
        public string DatosSolicitantes { get; set; }
        public string CantidadFormularios { get; set; }
        public string Destino { get; set; }
        public string SolicitanteCuil { get; set; }
    }
}
