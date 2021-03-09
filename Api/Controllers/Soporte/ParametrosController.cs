using System.Collections.Generic;
using System.Web.Http;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Comandos;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Aplicacion.Servicios;

namespace Api.Controllers
{
    public class ParametrosController: ApiController
    {
        private readonly ParametrosServicio _parametrosServicio;

        public ParametrosController(ParametrosServicio parametrosServicio)
        {
            _parametrosServicio = parametrosServicio;
        }

        [HttpGet]
        [Route("detallados")]
        public Resultado<ParametrosGrillaResultado> Get([FromUri] ParametrosConsultaFiltros consulta)
        {
            return _parametrosServicio.ConsultarParametrosPorFiltros(consulta);
        }

        [HttpGet]
        [Route("Totalizador")]
        public ContadorParametrosGrilla ObtenerTotalizadorParametros([FromUri] ParametrosConsultaFiltros consulta)
        {
            return _parametrosServicio.ObtenerTotalizadorParametros(consulta);
        }

        public VigenciaParametroIdResultado Put([FromBody]ActualizarParametroComando comando)
        {
            return _parametrosServicio.ActualizarVigenciaParametro(comando);
        }

        #region Tablas satelite

        public IList<ParametrosGrillaResultado> Get()
        {
            return _parametrosServicio.ObtenerParametros();
        }

        #endregion
    }
}