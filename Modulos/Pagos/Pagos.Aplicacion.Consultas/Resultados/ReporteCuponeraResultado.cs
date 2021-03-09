using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class ReporteCuponeraResultado
    {
        public string LineaPrestamo { get; set; }
        public string Beneficiario { get; set; }
        public string NumeroDocumento { get; set; }
        public string NumeroPrestamo { get; set; }
        public string NumeroCuota { get; set; }
        public string ImporteCuota { get; set; }
        public string NumeroReferencia { get; set; }
        public string CodigoBarras { get; set; }
        public string DataCB { get; set; }
        public string FechaVencimiento { get; set; }

    }
}
