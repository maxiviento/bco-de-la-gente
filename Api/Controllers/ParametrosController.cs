using System;
using System.Collections.Generic;
using System.Web.Http;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Comandos;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Aplicacion.Consultas.TablasSatelite;
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
        [Route("vigente")]
        public VigenciaParametroResultado GetVigenciaParametro([FromUri] ParametroConsulta consulta)
        {
            return _parametrosServicio.ObtenerValorVigenciaParametroPorFecha(consulta.Id, consulta.FechaDesde ?? DateTime.Now);
        }

        [HttpGet]
        [Route("detallados")]
        public Resultado<ConsultarParametrosResultado> Get([FromUri] ConsultaParametro consulta)
        {
            return _parametrosServicio.ConsultarParametrosPorFiltros(consulta);
        }

        public VigenciaParametroIdResultado Put(int id, [FromBody]ActualizarParametroComando comando)
        {
            return _parametrosServicio.RegistrarVigenciaParametro(comando);
        }

        [HttpGet]
        [Route("existeVigencia")]
        public ConsultarParametrosResultado ExisteVigenciaEnFecha([FromUri] ParametroConsulta consulta)
        {
            return _parametrosServicio.ExisteVigenciaEnFecha(consulta.Id, consulta.FechaDesde);
        }

        [Route("actualizarVigencia")]
        public VigenciaParametroIdResultado Post([FromBody]ActualizarParametroComando comando)
        {
            return _parametrosServicio.ActualizarVigenciaExistente(comando);
        }

        #region Tablas satelite

        public IEnumerable<ConsultaParametrosSatelite> Get()
        {
            return _parametrosServicio.ObtenerParametros();
        }

        #endregion
    }
}