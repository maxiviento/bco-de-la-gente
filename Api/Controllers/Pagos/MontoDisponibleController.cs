using System.Collections.Generic;
using System.Web.Http;
using Formulario.Aplicacion.Comandos;
using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Aplicacion.Servicios;
using Infraestructura.Core.Comun.Presentacion;
using DarDeBajaComando = Configuracion.Aplicacion.Comandos.DarDeBajaComando;

namespace Api.Controllers.Pagos
{
    public class MontoDisponibleController : ApiController
    {
        private readonly MontoDisponibleServicio _montoDisponibleServicio;

        public MontoDisponibleController(MontoDisponibleServicio montoDisponibleServicio)
        {
            _montoDisponibleServicio = montoDisponibleServicio;
        }

        public RegistrarMontoDisponibleResultado Post([FromBody] RegistrarMontoDisponibleComando regsitrarMontoDisponibleComando)
        {
            return _montoDisponibleServicio.Registrar(regsitrarMontoDisponibleComando);
        }

        public MontoDisponibleResultado Get(decimal id)
        {
            return _montoDisponibleServicio.ObtenerPorId(id);
        }

        public bool Delete(int id, [FromUri] DarDeBajaComando comando)
        {
            return _montoDisponibleServicio.DarDeBaja(id, comando);
        }

        public EditarMontoDisponibleResultado Put(int id, [FromBody] ModificarMontoDisponibleComando comando)
        {
            return _montoDisponibleServicio.Modificar(id, comando);
        }

        [HttpGet]
        [Route("consultar")]
        public Resultado<BandejaMontoDisponibleResultado> Get([FromUri] BandejaMontoDisponibleConsulta consulta)
        {
            return _montoDisponibleServicio.ConsultaBandeja(consulta);
        }

        [HttpGet]
        [Route("consultar-combo")]
        public IList<ClaveValorResultado<string>> GetCombo()
        {
            return _montoDisponibleServicio.ConsultarCombo();
        }

        [HttpGet]
        [Route("obtener-por-numero")]
        public MontoDisponibleResultado GetMontoDisponiblePorNro([FromUri] decimal nroMonto)
        {
            return _montoDisponibleServicio.ObtenerPorNro(nroMonto);
        }

        [HttpGet]
        [Route("obtener-movimientos")]
        public Resultado<MovimientoMontoResultado> Get([FromUri] MovimientosMontoConsulta consulta)
        {
            return _montoDisponibleServicio.ConsultaMovimientosMonto(consulta);
        }
    }
}