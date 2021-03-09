using System;
using Formulario.Aplicacion.Comandos;
using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Aplicacion.Servicios;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using Configuracion.Aplicacion.Consultas.Resultados;

namespace Api.Controllers.Formulario
{
    public class LineasPrestamoController : ApiController
    {
        private readonly LineaPrestamoServicio _lineaPrestamoServicio;

        public LineasPrestamoController(LineaPrestamoServicio lineaPrestamoServicio)
        {
            _lineaPrestamoServicio = lineaPrestamoServicio;
        }

        public DetalleLineaParaFormularioResultado Get(int id)
        {
            return _lineaPrestamoServicio.DetalleLineaParaFormulario(id);
        }

        [Route("detalles-seleccion")]
        // Usado por el componente seleccion-linea para presentarle al usuario las opciones al crear un formulario
        public IList<DetalleLineaParaFormularioResultado> GetDetalleLineasActivas()
        {
            return _lineaPrestamoServicio.ObtenerDetallesLinea();
        }

        // Obtiene cuadrantes por id detalle. usado desde formulario
        [Route("cuadrantes/{idLinea}")]
        public IList<CuadranteResultado> GetCuadrantesPorLinea(decimal idLinea)
        {
            return _lineaPrestamoServicio.ObtenerCuadrantesPorLinea(idLinea);
        }

        // Obtiene cuadrantes por id linea. usado desde configuracion de formularios
        [Route("cuadrantes-id-linea/{idLinea}")]
        public IList<CuadranteResultado> GetCuadrantesPorIdLinea(decimal idLinea)
        {
            return _lineaPrestamoServicio.ObtenerCuadrantesPorIdLinea(idLinea);
        }

        // Obtiene cuadrantes por id linea. usado desde configuracion de formularios
        [Route("convenios")]
        public IList<Convenio> GetConvenios()
        {
            return _lineaPrestamoServicio.ObtenerConvenios();
        }

        public IList<LineaPrestamoResultado> Get()
        {
            return _lineaPrestamoServicio.ConsultarLineas();
        }

        public Id Post([FromBody] RegistrarLineaComando comando)
        {
            return _lineaPrestamoServicio.RegistrarLineaPrestamo(comando);
        }

        [Route("configurar-orden-cuadrantes")]
        [HttpPost]
        public string ConfigurarOrden([FromBody] RegistrarOrdenFormularioComando comando)
        {
            return _lineaPrestamoServicio.RegistrarOrdenFormulario(comando);
        }

        [Route("consulta-cuadrantes")]
        [HttpGet]
        public IList<Cuadrante> GetCuadrantes()
        {
            return _lineaPrestamoServicio.ConsultarCuadrantes();
        }

        [Route("consultar")]
        public Resultado<LineaPrestamoGrillaResultado> GetLineaPorFiltros([FromUri] LineaPrestamoConsulta consulta)
        {
            return _lineaPrestamoServicio.ConsultarLineasPorFiltro(consulta);
        }

        [Route("consultar/{id}")]
        public LineaPrestamoResultado GetLineaPorId([FromUri] decimal id)
        {
            return _lineaPrestamoServicio.ConsultarLineaPorId(new Id(id));
        }

        [Route("consultar-localidades/{id}")]
        public IList<LocalidadResultado> GetLocalidadesLineaPorId([FromUri] decimal id)
        {
            return _lineaPrestamoServicio.ConsultarLocalidadesLineaPorId(new Id(id));
        }

        [Route("consultar/detalle/grilla")]
        public Resultado<DetalleLineaGrillaResultado> GetDetallePorIdLineaPaginado(
            [FromUri] DetalleLineaConsulta consulta)
        {
            return _lineaPrestamoServicio.ConsultarDetallePorIdLinea(consulta);
        }

        [Route("consultar/detalles/{idLinea}")]
        public IList<DetalleLineaGrillaResultado> GetDetallePorIdLinea([FromUri] decimal idLinea)
        {
            var resultado = _lineaPrestamoServicio.ConsultarDetallePorIdLineaSinPaginar(new Id(idLinea));
            return resultado;
        }

        [Route("consultar/detalle/{idDetalle}")]
        public DetalleLineaResultado GetDetallePorIdDetalle([FromUri] decimal idDetalle)
        {
            return _lineaPrestamoServicio.ConsultarDetallePorId(new Id(idDetalle));
        }
        [HttpGet, Route("obtener-detalles-linea-combo/{idLinea}")]
        public IList<DetalleLineaCombo> ObtenerDetallesLineaCombo([FromUri] decimal idLinea)
        {
            return _lineaPrestamoServicio.ObtenerDetallesLineaCombo(idLinea); ;
        }

        [Route("consultar/requisitos/{idLinea}")]
        public IList<RequisitosLineaResultado> GetRequisitosPorLinea([FromUri] decimal idLinea)
        {
            return _lineaPrestamoServicio.ConsultarRequisitosPorIdLinea(new Id(idLinea), false);
        }

        [Route("consultar/ong/{idLinea}")]
        public IList<OngLinea> GetOngPorLinea([FromUri] decimal idLinea)
        {
            return _lineaPrestamoServicio.ObtenerOngsLinea(new Id(idLinea));
        }

        public Id Delete([FromUri] BajaComando comando)
        {
            return _lineaPrestamoServicio.DarDeBajaLinea(comando);
        }

        [Route("baja-detalle")]
        public Id DeleteDetalle([FromUri] BajaComando comando)
        {
            return _lineaPrestamoServicio.DarDeBajaDetalle(comando);
        }

        [Route("logo/{id}")]
        [HttpGet]
        public string Logo(int id)
        {
            var detalleLinea = _lineaPrestamoServicio.DetalleLineaParaFormulario(id);
            return !string.IsNullOrEmpty(detalleLinea.Logo)
                ? System.Convert.ToBase64String(File.ReadAllBytes(detalleLinea.Logo))
                : null;
        }

        public Id Put([FromBody] ModificarLineaComando comando)
        {
            return _lineaPrestamoServicio.ModificarLinea(comando);
        }

        [Route("actualizar-detalle")]
        public Id PutDetalleLinea([FromBody] DetalleLineaPrestamoComando comando)
        {
            return _lineaPrestamoServicio.ActualizarDetalle(comando);
        }

        [HttpGet]
        [Route("descargarArchivo")]
        public HttpResponseMessage DescargarArchivo(string path)
        {
            HttpResponseMessage response;

            var extension = path?.Substring(path.LastIndexOf(".", StringComparison.Ordinal) + 1);
            var contenType = $"image/{extension?.ToLower() ?? "jpg"}";

            var archivo = _lineaPrestamoServicio.DescargarArchivo(path);
            if (archivo != null)
            {
                response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(archivo)
                    {
                        Headers =
                        {
                            ContentType = new MediaTypeHeaderValue(contenType),
                            ContentDisposition = new ContentDispositionHeaderValue("attachment")
                        }
                    }
                };
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            return response;
        }

        [Route("consultar/requisitosParaCuadrante/{idLinea}")]
        public IList<RequisitosCuadranteResultado> GetRequisitosPorLineaParaCuadrante([FromUri] decimal idLinea)
        {
            return _lineaPrestamoServicio.ConsultarRequisitosParaCuadrantePorIdLinea(new Id(idLinea), true);
        }

        [Route("GetLineasParaCombo")]
        public IList<LineaParaComboResultado> GetLineasParaCombo([FromUri] bool multiple)
        {
            return _lineaPrestamoServicio.ConsultarLineaParaCombo(multiple);
        }

        [Route("requisitos-linea")]
        public IList<ItemResultado.BandejaConfiguracionChecklist> GetConsultar(
            [FromUri] ConsultaConfiguracionChecklist consulta)
        {
            var requisitos = _lineaPrestamoServicio.ObtenerItemsConfiguracionChecklist(consulta);
            return requisitos;
        }

        [Route("registrar-configuracion")]
        [HttpPost]
        public string RegistrarConfiguracionEtapa([FromBody] ConfiguracionChecklistComando comando)
        {
            return _lineaPrestamoServicio.RegistrarConfiguracionChecklist(comando);
        }

        [Route("version-checklist")]
        [HttpGet]
        public VersionChecklistResultado ObtenerVersionChecklistVigente([FromUri] decimal idLinea)
        {
            return _lineaPrestamoServicio.ObtenerVersionChecklistVigente(idLinea);
        }

        [Route("consultar-nombres-lineas")]
        [HttpGet]
        public IEnumerable<LineaParaComboResultado> ConsultarNombresLineas()
        {
            return _lineaPrestamoServicio.ConsultarNombresLineas();
        }

        [Route("etapas-estados-linea")]
        [HttpGet]
        public IList<EtapaEstadoLineaResultado> ObtenerEtapasxEstadosLinea([FromUri] long idLinea, [FromUri] long? idPrestamo)
        {
            return _lineaPrestamoServicio.ObtenerEtapasxEstadosLinea(idLinea, idPrestamo);
        }

        [Route("obtener-ongs")]
        [HttpGet]
        public IList<OngComboResultado> ObtenerOngs()
        {
            return _lineaPrestamoServicio.ObtenerOngs();
        }

        [Route("obtener-ongs-por-nombre/{nombre}")]
        [HttpGet]
        public IList<OngComboResultado> ObtenerOngsPorNombre([FromUri]string nombre)
        {
            return _lineaPrestamoServicio.ObtenerOngsPorNombre(nombre);
        }

        [Route("modificar-ong-linea")]
        [HttpPost]
        public bool ModificarOngLinea([FromBody] ModificacionOngLineaComando comando)
        {
            _lineaPrestamoServicio.ModificarOngLinea(comando);
            return true;
        }
    }
}