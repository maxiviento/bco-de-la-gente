using System.Collections.Generic;
using System.Web.Http;
using Configuracion.Aplicacion.Comandos;
using Configuracion.Aplicacion.Comandos.Resultados;
using Configuracion.Aplicacion.Consultas;
using Configuracion.Aplicacion.Consultas.Resultados;
using Formulario.Aplicacion.Servicios;
using Formulario.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Presentacion;

namespace Api.Controllers.Formulario
{
    public class MotivosRechazoController : ApiController
    {
        private readonly MotivoRechazoServicio _motivoRechazoServicio;

        public MotivosRechazoController(MotivoRechazoServicio motivoRechazoServicio)
        {
            _motivoRechazoServicio = motivoRechazoServicio;
        }

        [Route("{ambito}")]
        public IList<MotivoRechazo> Get(string ambito)
        {
            switch (ambito)
            {
                case "Formulario":
                      return _motivoRechazoServicio.ConsultarMotivosRechazo();

                case "Prestamo":
                       return _motivoRechazoServicio.ConsultarMotivosRechazoPrestamo();

                case "Parametro_Tabla_Definida":
                       return _motivoRechazoServicio.ConsultarMotivosRechazoTablaDefinida();

                case "Checklist":
                       return _motivoRechazoServicio.ConsultarMotivosRechazoPorAmbito(Ambito.CHECKLIST.Id.Valor);
            }

            return _motivoRechazoServicio.ConsultarMotivosRechazo();
        }

        [HttpPost]
        public NuevoMotivoRechazoResultado Post([FromBody] MotivoRechazoComando comando)
        {
            return _motivoRechazoServicio.RegistrarMotivoRechazo(comando);
        }

        [HttpGet]
        [Route("consultar")]
        public Resultado<ConsultaMotivoRechazoResultado.Grilla> Get([FromUri] ConsultaMotivoRechazo consulta)
        {
            return _motivoRechazoServicio.ConsultaMotivosDestino(consulta);
        }

        [HttpGet]
        [Route("detalle/{idParametro}/{idAmbito}")]
        public MotivoRechazo Get(int idParametro, int idAmbito)
        {
            return _motivoRechazoServicio.ConsultarMotivoDestinoPorId(idParametro, idAmbito);
        }

        [HttpPost]
        [Route("modificar")]
        public bool PostActualizarMotivoRechazo([FromBody] ModificacionMotivoRechazoComando comando)
        {
            return _motivoRechazoServicio.Modificar(comando);
        }

        [HttpPost]
        [Route("baja")]
        public IHttpActionResult PostBajaMotivoRechazo([FromBody] DarBajaMotivoRechazoComando comando)
        {
            _motivoRechazoServicio.DarDeBaja(comando);
            return Ok(true);
        }

        [HttpGet]
        [Route("ambitos")]
        public IList<Ambito> GetAmbitos()
        {
            return _motivoRechazoServicio.ConsultarAmbitosCombo();
        }

        [HttpGet]
        [Route("obtener-abreviaturas")]
        public IList<MotivoRechazoAbreviaturaResultado> GetAbreviaturas()
        {
            return _motivoRechazoServicio.ConsultarAbreviaturas();
        }

        [HttpGet]
        [Route("motivos-ambito/{idAmbito}")]
        public IList<MotivoRechazo> GetMotivosRechazoPorAmbito([FromUri] decimal idAmbito)
        {
            return _motivoRechazoServicio.ConsultarMotivosRechazoPorAmbito(idAmbito);
        }

        [HttpGet]
        [Route("verificar-nombre/{idAmbito}/{nombre}")]
        public bool ExisteMotivoRechazoConMismoNombre(int idAmbito, string nombre)
        {
            return _motivoRechazoServicio.ExisteMotivoRechazoConMismoNombre(idAmbito, nombre);
        }

        [HttpGet]
        [Route("verificar-abreviatura/{idAmbito}/{abreviatura}")]
        public bool ExisteMotivoRechazoConMismaAbreviatura(int idAmbito, string abreviatura)
        {
            return _motivoRechazoServicio.ExisteMotivoRechazoConMismaAbreviatura(idAmbito, abreviatura);
        }

        [HttpGet]
        [Route("verificar-codigo/{idAmbito}/{codigo}")]
        public bool ExisteMotivoRechazoConMismoCodigo(int idAmbito, string codigo)
        {
            return _motivoRechazoServicio.ExisteMotivoRechazoConMismoCodigo(idAmbito, codigo);
        }
    }
}