using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Reporting.WebForms;


namespace Infraestructura.Core.Reportes
{
    public class ReportFactory
    {
        public static byte[] GeneratePDF<T>(string reportName, List<T> datasource)
        {
            return GeneratePDF<T>(".Aplicacion.Consultas", reportName, "DataSurceName", datasource);
        }

        public static byte[] GeneratePDF<T>(string assemblyPattern, string reportName, string dataSourceName, List<T> datasource)
        {
            var reportAsBytes = new byte[] { };
            var assemblyWithReport =
                AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.Contains(assemblyPattern)).ToList();

            foreach (var assembly in assemblyWithReport)
            {
                var fullReportName = assembly.GetManifestResourceNames().SingleOrDefault(x => x.EndsWith(reportName));

                if (string.IsNullOrEmpty(fullReportName))
                {
                    throw new ApplicationException(string.Format("No se encuentra el reporte {0}", reportName));
                }

                using (Stream stream = assembly.GetManifestResourceStream(fullReportName))
                {
                    var viewer = new ReportViewer();
                    viewer.LocalReport.EnableExternalImages = true;
                    viewer.LocalReport.LoadReportDefinition(stream);

                    Warning[] warnings;
                    string[] streamids;
                    string mimeType;
                    string encoding;
                    string filenameExtension;

                    viewer.LocalReport.DataSources.Add(new ReportDataSource(dataSourceName, datasource));

                    viewer.LocalReport.Refresh();

                    reportAsBytes = viewer.LocalReport.Render(
                        "PDF", null, out mimeType, out encoding, out filenameExtension,
                        out streamids, out warnings);
                    break;
                }


            }

            return reportAsBytes;

        }

    }
}
