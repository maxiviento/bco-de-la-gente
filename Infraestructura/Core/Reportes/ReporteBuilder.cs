using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Formulario.Aplicacion.Consultas.Resultados;
using Microsoft.Reporting.WebForms;

namespace Infraestructura.Core.Reportes
{
    /*
     http://stackoverflow.com/questions/36693/how-can-i-render-a-png-image-as-a-memory-stream-onto-a-net-reportviewer-repor
         */
    public class ReporteBuilder
    {
        private static readonly string assemblyPattern = ".Aplicacion.Consultas";
        private string _nombreReporte;
        private IList<ReportParameter> _parametros;
        private IList<ReportDataSource> _dataSource;
        private IList<SubReporte> _subReportes;
        private string _nombreSubReporte;
        private string _formato;

        public ReporteBuilder(string nombreReporte, string nombreSubReporte = null)
        {
            _nombreReporte = nombreReporte;
            _parametros = new List<ReportParameter>();
            _dataSource = new List<ReportDataSource>();
            _subReportes = new List<SubReporte>();
            _nombreSubReporte = nombreSubReporte;
            _formato = "pdf";
        }

        public ReporteBuilder AgregarParametro(string paramName, object value)
        {

            if (string.IsNullOrEmpty(paramName))
            {
                throw new ArgumentNullException("El nombre del parametro o valor no pueden ser nulos");
            }

            _parametros.Add(new ReportParameter(paramName, value != null ? value.ToString() : string.Empty));

            return this;
        }

        public ReporteBuilder SeleccionarFormato(string formato)
        {
            if (string.IsNullOrEmpty(formato))
            {
                throw new ArgumentNullException("El formato no puede ser nulo");
            }


            _formato = formato;

            return this;
        }

        public ReporteBuilder AgregarDataSource(string dataSourceName, object dataSource)
        {
            bool esEnumerable = dataSource is IEnumerable;
            _dataSource.Add(new ReportDataSource(dataSourceName, esEnumerable ? dataSource : Enumerable.Repeat(dataSource, 1)));
            return this;
        }

        private IList<CuadranteResultado> _cuad;

        public ReporteBuilder SubReporteDS(IList<SubReporte> subReportes)
        {
            _subReportes = subReportes;
            return this;
        }

        private void SubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            foreach (var subReporte in _subReportes)
            {
                if (subReporte.DataSets.Count == 0)
                {
                    e.DataSources.Add(new ReportDataSource(subReporte.NombreDataSets[0], subReporte.DataSets[0]));
                }
                else if (subReporte.DataSets.Count > 0)
                {
                    for (int i = 0; i < subReporte.DataSets.Count; i++)
                    {
                        e.DataSources.Add(new ReportDataSource(subReporte.NombreDataSets[i], subReporte.DataSets[i]));
                    }
                }
            }
        }

        public Reporte GenerarConSubReporte(bool principalConDataSource = true)
        {

            if (principalConDataSource)
            {
                if (!_dataSource.Any())
                {
                    throw new ApplicationException("No se agrego un datasource");
                }
            }

            var viewer = new ReportViewer();
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.DisplayName = _nombreReporte;
            viewer.LocalReport.LoadReportDefinition(BuscarReporteAsStream(_nombreReporte));

            if (_nombreSubReporte != null && _subReportes.Count > 0)
            {
                string nombreContenedorSubReporte = _nombreSubReporte;
                for (int i = 0; i < _subReportes.Count; i++)
                {
                    var subReporte = _subReportes[i];
                    viewer.LocalReport.LoadSubreportDefinition(nombreContenedorSubReporte, BuscarReporteAsStream(subReporte.Url));
                    viewer.LocalReport.SubreportProcessing += SubreportProcessingEventHandler;
                    nombreContenedorSubReporte = subReporte.NombreSubReporte;
                }
                viewer.LocalReport.LoadSubreportDefinition(nombreContenedorSubReporte, BuscarReporteAsStream("ReporteVacio.rdlc"));
            }

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;

            foreach (var reportDataSource in this._dataSource)
            {
                viewer.LocalReport.DataSources.Add(reportDataSource);
            }

            if (_parametros.Any())
            {
                viewer.LocalReport.SetParameters(_parametros);
            }

            viewer.LocalReport.Refresh();

            try
            {
                var content = viewer.LocalReport.Render(
                _formato, null, out mimeType, out encoding, out filenameExtension,
                out streamids, out warnings);

                var reporte = new Reporte(content, warnings, streamids, mimeType, encoding, filenameExtension);
                return reporte;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Reporte Generar()
        {
            if (!_dataSource.Any())
            {
                throw new ApplicationException("No se agrego un datasource");
            }

            var viewer = new ReportViewer();
            viewer.LocalReport.EnableExternalImages = true;
            viewer.LocalReport.LoadReportDefinition(BuscarReporteAsStream(_nombreReporte));
            viewer.LocalReport.DisplayName = _nombreReporte;

            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;

            foreach (var reportDataSource in this._dataSource)
            {
                viewer.LocalReport.DataSources.Add(reportDataSource);
            }

            if (_parametros.Any())
            {
                viewer.LocalReport.SetParameters(_parametros);
            }

            viewer.LocalReport.Refresh();

            var content = viewer.LocalReport.Render(
                _formato, null, out mimeType, out encoding, out filenameExtension,
                out streamids, out warnings);

            return new Reporte(content, warnings, streamids, mimeType, encoding, filenameExtension);
        }

        public bool ExisteReporte(string nombreReporte = null)
        {
            if (nombreReporte == null)
            {
                nombreReporte = _nombreReporte;
            }
            var existeReporte = !string.IsNullOrEmpty(ObtenerPathCompletoReporte(nombreReporte));
            return existeReporte;
        }

        public string ObtenerPathCompletoReporte(string nombreReporte = null)
        {
            if (nombreReporte == null)
            {
                nombreReporte = _nombreReporte;
            }

            String fullReportName = null;
            IList<Assembly> assemblyWithReport =
                AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.Contains(assemblyPattern)).ToList();

            foreach (var assembly in assemblyWithReport)
            {
                fullReportName = assembly.GetManifestResourceNames().SingleOrDefault(x => x.EndsWith(nombreReporte));

                if (!string.IsNullOrEmpty(fullReportName))
                {
                    break;
                }
            }
            return fullReportName;
        }
        private Stream BuscarReporteAsStream(string nombreReporte)
        {
            Stream stream = null;
            String fullReportName = null;
            IList<Assembly> assemblyWithReport =
                AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.Contains(assemblyPattern)).ToList();

            foreach (var assembly in assemblyWithReport)
            {
                fullReportName = assembly.GetManifestResourceNames().SingleOrDefault(x => x.EndsWith(nombreReporte));

                if (!string.IsNullOrEmpty(fullReportName))
                {
                    stream = assembly.GetManifestResourceStream(fullReportName);
                    break;
                }

            }

            if (string.IsNullOrEmpty(fullReportName))
            {
                throw new ApplicationException(string.Format("No se encuentra el reporte {0}", nombreReporte));
            }

            return stream;
        }
        public static string GenerarUrlReporte(Reporte reportData, string nombreReporte)
        {
            var response =
                new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(reportData.Content) };
            response.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment") { FileName = $"{nombreReporte}.pdf" };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            var url = Convert.ToBase64String(reportData.Content);
            return string.Format("data:application/pdf;base64,{0}", url);
        }

        public static string GenerarUrlReporte(byte[] byteArray, string nombreReporte)
        {
            var response =
                new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(byteArray) };
            response.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment") { FileName = $"{nombreReporte}.pdf" };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            var url = Convert.ToBase64String(byteArray);
            return string.Format("data:application/pdf;base64,{0}", url);
        }

        public static string GenerarUrlExcel(Reporte reportData, string nombreReporte)
        {
            var response =
                new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(reportData.Content) };
            response.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment") { FileName = $"{nombreReporte}.xls" };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            var url = Convert.ToBase64String(reportData.Content);
            return string.Format("data:application/octet-stream;base64,{0}", url);
        }
    }
}
