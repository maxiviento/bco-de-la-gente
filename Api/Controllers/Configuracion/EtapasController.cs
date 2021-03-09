using System.Collections.Generic;
using System.Web.Http;
using Configuracion.Aplicacion.Comandos;
using Configuracion.Aplicacion.Comandos.Resultados;
using Configuracion.Aplicacion.Consultas;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Aplicacion.Servicios;
using Infraestructura.Core.Comun.Presentacion;
using DarDeBajaComando = Configuracion.Aplicacion.Comandos.DarDeBajaComando;

namespace Api.Controllers.Configuracion
{
    public class EtapasController : ApiController
    {
        private readonly EtapaServicio _etapaServicio;

        public EtapasController(EtapaServicio etapaServicio)
        {
            _etapaServicio = etapaServicio;
        }

        public ConsultaEtapaPorIdResultado Get([FromUri] decimal id)
        {
            return _etapaServicio.ConsultarPorId(id);
        }

        [HttpGet]
        [Route("consultar")]
        public Resultado<EtapaResultado.Consulta> Get([FromUri] ConsultaEtapas consulta)
        {
            return _etapaServicio.ConsultarPorNombre(consulta);
        }

        [HttpGet]
        [Route("consultar-etapas")]
        public IList<EtapaResultado.Consulta> GetEtapas()
        {
            return _etapaServicio.ConsultarEtapas();
        }

        [HttpGet]
        [Route("consultar-etapas-prestamo")]
        public IList<EtapaResultado.Consulta> GetEtapasPorIdPrestamo([FromUri] long idPrestamo)
        {
            return _etapaServicio.ConsultarEtapasPorIdPrestamo(idPrestamo);
        }

        public NuevaEtapaResultado Post([FromBody] RegistrarEtapaComando comando)
        {
            return _etapaServicio.Registrar(comando);
        }

        public IHttpActionResult Delete(int id, [FromUri] DarDeBajaComando comando)
        {
            _etapaServicio.DarDeBaja(id, comando);
            return Ok();
        }

        public IHttpActionResult Put(int id, [FromBody] ModificarEtapaComando comando)
        {
            _etapaServicio.Modificar(id, comando);
            return Ok();
        }
    }
}