using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Core.Reportes
{
    public class ReporteConcatenado
    {
        public ReporteConcatenado() { }

        public ReporteConcatenado(Reporte reportData, string fileName)
        {
            ReportData = reportData;
            FileName = fileName;
        }

        public Reporte ReportData { get; set; }
        public string FileName { get; set; }
    }
}
