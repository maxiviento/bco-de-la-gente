using System.Collections.Generic;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.Modelo;

namespace Configuracion.Dominio.IRepositorio
{
    public interface ITipoItemRepositorio
    {
        IList<TipoItem> ConsultarTiposItem();
        IList<TipoItem> ConsultarTiposItemPorItem(decimal id);
        IList<ItemsPorTipoItemResultado.Consulta> ConsultaItemsPorTipoItem(bool esCreacionDeLinea);
    }
}