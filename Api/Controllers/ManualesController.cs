using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Infraestructura.Core.Comun.Archivos;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Servicios;

namespace Api.Controllers
{
    public class ManualesController : ApiController
    {
        private readonly ManualesServicio _manualesServicio;

        public ManualesController(ManualesServicio manualesServicio)
        {
            _manualesServicio = manualesServicio;
        }

        public IEnumerable<string> Get()
        {
            return _manualesServicio.ObtenerManuales();
        }

        [Route("descargar")]
        [HttpGet]
        public ArchivoBase64 DescargarManual([FromUri] ConsultaManual consulta)
        {
            return _manualesServicio.DescargarManual(consulta);
        }
    }
}
