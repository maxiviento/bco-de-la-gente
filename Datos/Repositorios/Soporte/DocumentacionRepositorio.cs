using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Datos;
using NHibernate;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Dominio.IRepositorio;
using Soporte.Dominio.Modelo;

namespace Datos.Repositorios.Soporte
{
    public class DocumentacionRepositorio : NhRepositorio<Documentacion>, IDocumentacionRepositorio
    {
        public DocumentacionRepositorio(ISession sesion) : base(sesion)
        {
        }

        public DocumentacionResultado ObtenerDocumentacionPorId(Id documentacionId)
        {
            var resultado = Execute("PR_OBTENER_DOCUMENTACION_X_ID")
                .AddParam(documentacionId)
                .ToUniqueResult<DocumentacionResultado>();
            return resultado;
        }

        public Id RegistrarDocumento(Documentacion documentacion, Id idItem, Id idFormularioLinea)
        {
            var result = Execute("PR_REGISTRA_DOCUMENTACION")
                .AddParam(idFormularioLinea)
                .AddParam(idItem)
                .AddParam(documentacion.Nombre)
                .AddParam(documentacion.IdDocumentoCdd)
                .AddParam(documentacion.Extension)
                .AddParam(documentacion.Usuario.Id)
                .ToSpResult();

            return result.Id;
        }

        public Resultado<DocumentacionResultado> ObtenerDocumentos(DocumentacionConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_OBTENER_DOCUMENTACION")
                .AddParam(consulta.IdFormularioLinea)
                .AddParam(consulta.IdItem)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<DocumentacionResultado>();

            return CrearResultado(consulta, elementos);
        }

        public Item ObtenerItemPorId(Id idItem)
        {
            var resultado = Execute("PR_OBTENER_ITEM_POR_ID")
                .AddParam(idItem)
                .ToUniqueResult<Item>();
            return resultado;
        }
    }
}