using System.Collections.Generic;
using System.Web.Http;
using Configuracion.Aplicacion.Servicios;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Presentacion;

namespace Api.Controllers.Configuracion
{
    public class TablasDefinidasController : ApiController
    {
        private readonly TablasDefinidasServicio _tablasDefinidasServicio;

        public TablasDefinidasController(TablasDefinidasServicio tablasDefinidasServicio)
        {
            _tablasDefinidasServicio = tablasDefinidasServicio;
        }

        public IList<TablaDefinida> Get()
        {
            return _tablasDefinidasServicio.ObtenerTablasDefinidas();
        }

        [Route("consultar-tablas-paginadas")]
        [HttpGet]
        public Resultado<TablaDefinida> ObtenerTablasDefinidasPaginadas([FromUri]ConsultaTablasDefinidas consulta)
        {
            return _tablasDefinidasServicio.ObtenerTablasDefinidasPaginadas(consulta);
        }

        [Route("consultar-tabla/{id}")]
        [HttpGet]
        public TablaDefinida GetDatosTablaDefinida(int id)
        {
            return _tablasDefinidasServicio.ObtenerDatosTablaDefinida(id);
        }

        [Route("consultar-parametros")]
        [HttpGet]
        public Resultado<ParametroTablaDefinidaResult> GetParametrosTablaDefinida([FromUri]ConsultaParametrosTablasDefinidas consulta)
        {
            return _tablasDefinidasServicio.ObtenerDatosTablaDefinida(consulta);
        }

        [Route("consultar-parametros-combo")]
        [HttpGet]
        public IList<ParametroTablaDefinida> GetParametrosComboTablaDefinida([FromUri]ConsultaParametrosTablasDefinidas consulta)
        {
            return _tablasDefinidasServicio.ObtenerParametrosComboTablaDefinida(consulta);
        }

        [Route("rechazar-parametro")]
        [HttpPost]
        public decimal RechazarPrestamo([FromBody] RechazarParametroTablaDefinida comando)
        {
            return _tablasDefinidasServicio.RegistrarRechazoParametro(comando);
        }

        [Route("registrar-parametro/{idTabla}")]
        public decimal Post([FromBody] ParametroTablaDefinida parametro, int idTabla)
        {
            return _tablasDefinidasServicio.RegistrarParametro(parametro, idTabla);
        }

        [Route("parametro/{id}")]
        [HttpGet]
        public TablaDefinida GetParametroTablaSatelites(int id)
        {
            return _tablasDefinidasServicio.ObtenerParametro(id);
        }
    }
}