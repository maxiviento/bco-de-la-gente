using System.Collections.Generic;
using System.Web.Http;
using System.Web.UI.WebControls;
using Configuracion.Aplicacion.Comandos;
using Configuracion.Aplicacion.Comandos.Resultados;
using Configuracion.Aplicacion.Consultas;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Aplicacion.Servicios;
using Infraestructura.Core.Comun.Presentacion;
using DarDeBajaComando = Configuracion.Aplicacion.Comandos.DarDeBajaComando;

namespace Api.Controllers.Configuracion
{
    public class ItemsController : ApiController
    {
        private readonly ItemServicio _itemServicio;
        private readonly TipoItemServicio _tipoItemServicio;

        public ItemsController(ItemServicio itemServicio, TipoItemServicio tipoItemServicio)
        {
            _itemServicio = itemServicio;
            _tipoItemServicio = tipoItemServicio;
        }

        [Route("tiposItem")]
        public IList<TipoItemResultado> GetTiposItem()
        {
            return _tipoItemServicio.ConsultarTiposItem();
        }

        [Route("itemsPorTipoItem")]
        public IList<ItemsPorTipoItemResultado.Requisito> GetItemsPorTipoItem([FromUri] bool esCreacionDeLinea)
        {
            return _tipoItemServicio.ConsultarItemsPorTipoItem(esCreacionDeLinea);
        }

        [Route("consultar")]
        public Resultado<ItemResultado.Grilla> GetConsultar([FromUri] ItemConsultaPaginada consulta)
        {
            return _itemServicio.ConsultarItemsPorNombre(consulta);
        }

        [Route("consultar-items")]
        public IList<ItemResultado.Grilla> GetConsultarItems()
        {
            return _itemServicio.ConsultarItems();
        }

        [Route("consultar-items-combo")]
        public IList<ItemResultado.Grilla> GetConsultarItemsCombo()
        {
            return _itemServicio.ConsultarItemsCombo();
        }

        [Route("consultar-recursos")]
        public IList<ConsultarRecursoResultado> GetConsultarRecursos()
        {
            return _itemServicio.ConsultarRecursos();
        }

        public ItemResultado.Detalle Get(long id)
        {
            return _itemServicio.ConsultarItemPorId(id);
        }

        public NuevoItemResultado Post([FromBody] ItemComando value)
        {
            return _itemServicio.RegistrarItem(value);
        }

        public IHttpActionResult Delete(int id, [FromUri] DarDeBajaComando comando)
        {
            _itemServicio.DarDeBaja(id, comando);
            return Ok(true);
        }

        public IHttpActionResult Put(int id, [FromBody] ItemComando comando)
        {
            _itemServicio.Modificar(id, comando);
            return Ok(true);
        }

        [Route("posee-hijos/{idItem}")]
        public bool GetPoseeHijo([FromUri]int idItem)
        {
            return _itemServicio.PoseeHijos(idItem);
        }
}
}