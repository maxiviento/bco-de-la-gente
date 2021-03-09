using System;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Dominio.Modelo.ItemDocumentos;

namespace Soporte.Aplicacion.Servicios
{
    public class RentasStrategy : IDocItemStrategy
    {
        private readonly RentasServicio _rentasServicio;
        public RentasStrategy(RentasServicio rentasServicio)
        {
            _rentasServicio = rentasServicio;
        }

        public ItemDocumentoEnum Tipo => ItemDocumentoEnum.Rentas;

        public DocumentoDescargaResultado ObtenerDocumentoPorId(Id idDocumento, string hash)
        {
            var resultado = new DocumentoDescargaResultado();
            var responseRentas = _rentasServicio.ObtenerHistorial(idDocumento.Valor, hash);
            var indiceComa = responseRentas.IndexOf(",", StringComparison.Ordinal);
            responseRentas = responseRentas.Substring(indiceComa + 1);

            resultado.Blob = Convert.FromBase64String(responseRentas);
            resultado.FileName = "";
            resultado.Extension = "application/pdf";

            return resultado;
        }

        public Resultado<DocumentacionResultado> ObtenerDocumentos(DocumentacionConsulta consulta)
        {
            return _rentasServicio.ObtenerTodosHistorialesRentas(consulta);
        }
    }
}