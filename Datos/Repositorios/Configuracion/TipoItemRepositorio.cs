using System.Collections.Generic;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.IRepositorio;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Configuracion
{
    public class TipoItemRepositorio : NhRepositorio<TipoItem>, ITipoItemRepositorio
    {
        public TipoItemRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<TipoItem> ConsultarTiposItem()
        {
            return ObtenerTodos<TipoItem>("T_TIPOS_ITEM");
        }

        public IList<TipoItem> ConsultarTiposItemPorItem(decimal id)
        {
            var result = Execute("PR_OBTENER_TIPOS_ITEM_X_ITEM")
                .AddParam(id)
                .ToListResult<TipoItem>();
            return result;
        }

        public IList<ItemsPorTipoItemResultado.Consulta> ConsultaItemsPorTipoItem(bool esCreacionDeLinea)
        {
            return Execute("PR_OBTENER_TIPOS_ITEM_X_ITEM_2")
                .AddParam(esCreacionDeLinea ? default(decimal) : default(decimal?)) // el sp espera un valor cualquiera para verdadero y -1 para falso
                .AddParam(1)
                .ToListResult<ItemsPorTipoItemResultado.Consulta>();
        }
    }
}