using System.Collections.Generic;
using System.Linq;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.IRepositorio;

namespace Configuracion.Aplicacion.Servicios
{
    public class TipoItemServicio
    {
        private readonly ITipoItemRepositorio _tipoItemRepositorio;

        public TipoItemServicio(ITipoItemRepositorio tipoItemRepositorio)
        {
            _tipoItemRepositorio = tipoItemRepositorio;
        }

        public IList<TipoItemResultado> ConsultarTiposItem()
        {
            var tiposItem = _tipoItemRepositorio.ConsultarTiposItem();
            var tiposItemResultado = tiposItem.Select(tipoItem => new TipoItemResultado
                    {
                        Id = (int) tipoItem.Id.Valor,
                        Nombre = tipoItem.Descripcion
                    }
                )
                .ToList();
            return tiposItemResultado;
        }

        public IList<ItemsPorTipoItemResultado.Requisito> ConsultarItemsPorTipoItem(bool esCreacionDeLinea)
        {
            var resultados = _tipoItemRepositorio.ConsultaItemsPorTipoItem(esCreacionDeLinea);

            var resultadoAgrupadoPorItem = resultados.GroupBy(resultado => resultado.IdItem)
                .Select(resultadoAgrupado => new ItemsPorTipoItemResultado.Requisito
                {
                    Id = resultadoAgrupado.Key,
                    Nombre = resultadoAgrupado.Select(item => item.NombreItem).First(),
                    TiposItem = resultadoAgrupado.Select(item => new ItemResultado.Requisito
                    {
                        Id = item.IdTipoItem,
                        Nombre = item.NombreTipoItem
                    })
                }).ToList();

            return resultadoAgrupadoPorItem;
        }
    }
}