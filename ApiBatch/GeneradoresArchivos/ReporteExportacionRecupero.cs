using System;
using ApiBatch.Utilidades;
using System.Collections.Generic;
using Infraestructura.Core.Reportes;
using ExportacionRecupero = Formulario.Aplicacion.Consultas.Resultados.ExportacionRecupero;

namespace ApiBatch.GeneradoresArchivos
{
    public class ReporteExportacionRecupero: GeneradorArchivos<ExportacionRecupero>
    {
        public ReporteExportacionRecupero() { }
        public ReporteExportacionRecupero(IList<ExportacionRecupero> datos): base(datos)
        {
        }

        public override IList<HttpFile> DefinirArchivo(string processName)
        {
            var contenido = new ReporteBuilder("ReportePagos_ExportacionRecupero.rdlc")
                .AgregarDataSource("DSExportacionRecuperoBatch", Datos)
                .SeleccionarFormato("Excel")
                .Generar()
                .Content;

            var file = new HttpFile()
            {
                FileName = processName + " " + DateTime.Now.ToString("dd-MM-yyyy") + " " + DateTime.Now.ToString("HH") + "hs" + " " + DateTime.Now.ToString("mm") + "min",
                MediaType = MimeTypeUtils.Obtener_MimeType(MimeTypeUtils.FileExtension.Xls),
                BufferArray = contenido
            };
            return new List<HttpFile>() { file };
        }
    }
}