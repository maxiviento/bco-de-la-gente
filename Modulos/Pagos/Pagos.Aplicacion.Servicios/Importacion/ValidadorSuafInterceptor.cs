using System;
using Infraestructura.Core.Comun.Dato;
using Pagos.Dominio.Modelo;
using Utilidades.Importador;

namespace Pagos.Aplicacion.Servicios.Importacion
{
    public class ValidadorSuafInterceptor : IImporterInterceptor<ImportarArchivoSuaf>
    {
        private readonly PagosServicio _pagosServicio;

        public Id IdLote { get; set; }

        public ValidadorSuafInterceptor(Id idLote, PagosServicio pagosServicio)
        {
            IdLote = idLote;
            _pagosServicio = pagosServicio;
        }

        public bool Pre(int rowIndex, string[] columns)
        {
            throw new NotImplementedException();
        }

        public bool Pos(int rowIndex, ref ImportarArchivoSuaf archivoSuaf)
        {
            var resultado = _pagosServicio.ValidarLoteSuafPorNumeroFormulario(IdLote, new Id(archivoSuaf.NumeroFormulario));
            return resultado;
        }

        public int ValidarDevengadoSuaf(int rowIndex, ref ImportarArchivoSuaf t)
        {
            throw new NotImplementedException();
        }
    }
}
