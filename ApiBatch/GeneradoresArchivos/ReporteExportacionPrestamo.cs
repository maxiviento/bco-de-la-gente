using System;
using ApiBatch.Base;
using ApiBatch.Models;
using ApiBatch.Utilidades;
using System.Collections.Generic;
using ApiBatch.Base.QueueManager;
using Infraestructura.Core.Reportes;
using Utilidades.Exportador;
using Utilidades.Exportador.Builders;

namespace ApiBatch.GeneradoresArchivos
{
    public class ReporteExportacionPrestamo: GeneradorArchivos<ExportacionPrestamo>
    {
        public ReporteExportacionPrestamo() { }
        public ReporteExportacionPrestamo(IList<ExportacionPrestamo> datos): base(datos)
        {
        }

        public override IList<HttpFile> DefinirArchivo(string processName)
        {
            var contenido = new ReporteBuilder("ReportePagos_ExportacionPrestamo.rdlc")
                .AgregarDataSource("DSExportacionPrestamoBatch", Datos)
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