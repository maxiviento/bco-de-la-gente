using System.Collections.Generic;
using System.Web.Http;
using Configuracion.Aplicacion.Comandos;
using Configuracion.Aplicacion.Comandos.Resultados;
using Configuracion.Aplicacion.Consultas;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Aplicacion.Servicios;
using Infraestructura.Core.Comun.Archivos;
using Infraestructura.Core.Comun.Presentacion;
using DarDeBajaComando = Configuracion.Aplicacion.Comandos.DarDeBajaComando;

namespace Api.Controllers.Configuracion
{
    public class AreasController : ApiController
    {
        private readonly AreaServicio _areaServicio;

        public AreasController(AreaServicio areaServicio)
        {
            _areaServicio = areaServicio;
        }

        public NuevaAreaResultado Post([FromBody] RegistrarAreaComando regsitrarAreaComando)
        {
            return _areaServicio.RegistrarArea(regsitrarAreaComando);
        }

        [HttpGet]
        [Route("consultar")]
        public Resultado<AreaResultado.Consulta> Get([FromUri] ConsultarAreas consulta)
        {
            return _areaServicio.ConsultarAreas(consulta);
        }

        public ConsultarAreaPorIdResultado Get(decimal id)
        {
            return _areaServicio.ConsultarAreaPorId(id);
        }

        public IHttpActionResult Delete(int id, [FromUri] DarDeBajaComando comando)
        {
            _areaServicio.DarDeBaja(id, comando);
            return Ok();
        }

        public IHttpActionResult Put(int id, [FromBody] ModificarEtapaComando comando)
        {
            _areaServicio.Modificar(id, comando);
            return Ok();
        }

        [HttpGet]
        [Route("consultar-areas")]
        public IList<AreaResultado.Consulta> GetAreasCombo()
        {
            return _areaServicio.ConsultarAreasCombo();
        }

        [HttpGet]
        [Route("reporte-providencia-masiva/{cantidad}")]
        public ReporteResultado GetProvidenciaMasiva([FromUri] int cantidad)
        {
            return _areaServicio.ObtenerReporteDeudaGrupoConviviente(cantidad);
        }
    }
}