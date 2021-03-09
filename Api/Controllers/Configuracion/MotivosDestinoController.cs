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
    public class MotivosDestinoController : ApiController
    {
        private readonly MotivoDestinoServicio _motivoDestinoServicio;

        public MotivosDestinoController(MotivoDestinoServicio motivoDestinoServicio)
        {
            _motivoDestinoServicio = motivoDestinoServicio;
        }

        public IList<MotivoDestinoResultado.Combo> Get()
        {
            return _motivoDestinoServicio.ConsultarMotivosDestino();
        }

        public MotivoDestinoResultado.Detallado Get(int id)
        {
            return _motivoDestinoServicio.ConsultarMotivoDestinoPorId(id);
        }

        public NuevoMotivoDestinoResultado Post([FromBody] MotivoDestinoComando comando)
        {
            return _motivoDestinoServicio.RegistrarMotivoDestino(comando);
        }

        public IHttpActionResult Put(int id, [FromBody] MotivoDestinoComando comando)
        {
            _motivoDestinoServicio.Modificar(id, comando);
            return Ok();
        }

        public IHttpActionResult Delete(int id, [FromUri] DarDeBajaComando comando)
        {
            _motivoDestinoServicio.DarDeBaja(id, comando);
            return Ok();
        }

        [HttpGet]
        [Route("consultar")]
        public Resultado<MotivoDestinoResultado.Grilla> Get([FromUri] ConsultaMotivoDestino consulta)
        {
            return _motivoDestinoServicio.ConsultaMotivosDestino(consulta);
        }
    }
}