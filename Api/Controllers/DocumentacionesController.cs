using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Http;
using Configuracion.Aplicacion.Servicios;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Comandos;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Aplicacion.Servicios;

namespace Api.Controllers
{
    public class DocumentacionesController : ApiController
    {
        private readonly DocumentacionServicio _documentacionServicio;
        private readonly TipoDocumentacionServicio _tipoDocumentacionServicio;

        public DocumentacionesController(DocumentacionServicio documentacionServicio,
            TipoDocumentacionServicio tipoDocumentacionServicio, RentasServicio rentasServicio,
            SintysServicio sintysServicio)
        {
            _documentacionServicio = documentacionServicio;
            _tipoDocumentacionServicio = tipoDocumentacionServicio;
        }

        public Resultado<DocumentacionResultado> Get([FromUri] DocumentacionConsulta consulta)
        {
            return _documentacionServicio.ObtenerDocumentos(consulta);
        }

        public decimal Post([FromBody] RegistrarDocumentoComando comando)
        {
            var idDocumento = _documentacionServicio.RegistrarDocumentacion(comando);
            return idDocumento;
        }

        [HttpGet, Route("tipos-documentacion")]
        public IList<TipoDocumentacion> ConsultarTiposDocumentacion()
        {
            var tiposDocumentacion = _tipoDocumentacionServicio.ConsultarTiposDocumentacion();
            return tiposDocumentacion;
        }

        public DocumentoDescargaResultado GetDocumentacionCddPorid([FromUri] Id idDocumento, [FromUri] Id idItem)
        {
            var hash = HttpContext.Current.Request.Cookies["CiDi"]?.Value;
            return _documentacionServicio.ObtenerDocumentoPorId(idDocumento, idItem, hash);
        }
    }
}