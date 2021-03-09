using System.Collections.Generic;
using System.Web.Http;
using Infraestructura.Core.Comun.Presentacion;
using Pagos.Aplicacion.Comandos;
using Pagos.Aplicacion.Consultas.Consultas;
using Pagos.Aplicacion.Consultas.Resultados;
using Pagos.Aplicacion.Servicios;

namespace Api.Controllers.Pagos
{
    public class RecuperoController : ApiController
    {
        private readonly RecuperoServicio _recuperoServicio;

        public RecuperoController(RecuperoServicio recuperoServicio)
        {
            _recuperoServicio = recuperoServicio;
        }

        [HttpGet]
        [Route("consultar-bandeja")]
        public Resultado<BandejaRecuperoResultado> ConsultarBandeja([FromUri] BandejaRecuperoConsulta consulta)
        {
            return _recuperoServicio.ConsultarBandeja(consulta);
        }

        [HttpPost, Route("importar-archivo-recupero")]
        public ImportarArchivoRecuperoResultado ImportarArchivoRecupero([FromBody] ImportarArchivoRecuperoComando comando)
        {
            return _recuperoServicio.ImportarArchivoRecupero(comando);
        }

        [HttpPost, Route("importar-archivo-resultado-banco")]
        public ImportarArchivoResultadoBancoResultado ImportarArchivoResultadoBanco([FromBody] ImportarArchivoResultadoBancoComando comando)
        {
            return _recuperoServicio.ImportarArchivoResultadoBanco(comando);
        }

        [HttpGet]
        [Route("consultar-bandeja-resultado-banco")]
        public Resultado<BandejaResultadoBancoResultado> ConsultarBandejaResultadoBanco([FromUri] BandejaRecuperoConsulta consulta)
        {
            return _recuperoServicio.ConsultarBandejaResultadoBanco(consulta);
        }

        [HttpGet]
        [Route("consultar-inconsistencia-importacion-archivo-recupero")]
        public Resultado<VerInconsistenciaArchivosResultado> ConsultarInconsistenciaArchivoRecupero([FromUri] VerArchivoInconsistenciaConsulta consulta)
        {
            return _recuperoServicio.ConsultarInconsistenciaArchivoRecupero(consulta);
        }

        [HttpGet]
        [Route("consultar-inconsistencia-importacion-archivo-resultado")]
        public Resultado<VerInconsistenciaArchivosResultado> ConsultarInconsistenciaArchivoResultado([FromUri] VerArchivoInconsistenciaConsulta consulta)
        {
            return _recuperoServicio.ConsultarInconsistenciaResultadoBanco(consulta);
        }

        [Route("convenios-recupero")]
        [HttpGet]
        public IList<Convenio> ObtenerConveniosRecupero()
        {
            return _recuperoServicio.ObtenerConvenios(2); //Obtiene los convenios de recupero
        }

        [HttpGet]
        [Route("consultar-combo-entidades-recupero")]
        public IList<ComboEntidadesRecuperoResultado> ConsultarComboEntidadesRecupero()
        {
            return _recuperoServicio.ConsultarComboEntidadesRecupero();
        }}
}