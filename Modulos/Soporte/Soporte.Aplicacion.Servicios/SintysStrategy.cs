using System;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Dominio.Modelo.ItemDocumentos;

namespace Soporte.Aplicacion.Servicios
{
    public class SintysStrategy : IDocItemStrategy
    {
        private readonly SintysServicio _sintysServicio;
        public SintysStrategy(SintysServicio sintysServicio)
        {
            _sintysServicio = sintysServicio;
        }

        public ItemDocumentoEnum Tipo => ItemDocumentoEnum.Sintys;

        public DocumentoDescargaResultado ObtenerDocumentoPorId(Id idDocumento, string hash)
        {
            var resultado = new DocumentoDescargaResultado();
            var response = _sintysServicio.ObtenerReporteHistorialSintys(idDocumento.Valor, hash);
            var indiceComa = response.IndexOf(",", StringComparison.Ordinal);
            response = response.Substring(indiceComa + 1);

            resultado.Blob = Convert.FromBase64String(response);
            resultado.FileName = "";
            resultado.Extension = "application/pdf";

            return resultado;
        }

        public Resultado<DocumentacionResultado> ObtenerDocumentos(DocumentacionConsulta consulta)
        {
            return _sintysServicio.ObtenerTodosHistorialesSintys(consulta);
        }
    }
}