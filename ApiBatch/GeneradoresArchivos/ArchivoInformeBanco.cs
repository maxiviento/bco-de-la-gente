using ApiBatch.Base;
using ApiBatch.Models;
using ApiBatch.Utilidades;
using System.Collections.Generic;
using Utilidades.Exportador;
using Utilidades.Exportador.Builders;

namespace ApiBatch.GeneradoresArchivos
{
    public class ArchivoInformeBanco: GeneradorArchivos<PruebaExcel>
    {
        public ArchivoInformeBanco() { }
        public ArchivoInformeBanco(IList<PruebaExcel> datos): base(datos)
        {
        }

        public override IList<HttpFile> DefinirArchivo(string processName)
        {
            var contenido = Exporter.ToExcel<PruebaExcel>()
               .AddConfiguration(new ExcelConfiguration
               {
                   FromX = 1,
                   FromY = 1
               })
               .AddColumn("IdDetalleProceso", 0, x => x.IdDetalleProceso)
               .AddColumn("IdFormularioLinea", 1, x => x.IdFormularioLinea)
               .AddColumn("NroFormulario", 2, x => x.NroFormulario)
               .AddColumn("IdSexo", 3, x => x.IdSexo)
               .AddColumn("NroDocumento", 4, x => x.NroDocumento)
               .AddColumn("Nombre", 5, x => x.Nombre)
               .AddColumn("Apellido", 6, x => x.Apellido)
               .GenerateAsString(Datos);

            var file = new HttpFile()
            {
                FileName = processName,
                MediaType = MimeTypeUtils.Obtener_MimeType(MimeTypeUtils.FileExtension.Xlsx),
                Buffer = contenido
            };
            return new List<HttpFile>() { file };
        }
    }
}