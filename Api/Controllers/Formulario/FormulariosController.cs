using System;
using Formulario.Aplicacion.Comandos;
using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Aplicacion.Servicios;
using Infraestructura.Core.Comun.Presentacion;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Archivos;
using Infraestructura.Core.Comun.Dato;
using Soporte.Aplicacion.Servicios;

namespace Api.Controllers.Formulario
{
    public class FormulariosController : ApiController
    {
        private readonly FormularioServicio _formularioServicio;
        private readonly DeudaGrupoServicio _deudaGrupoServicio;
        private readonly DeudaEmprendimientoServicio _deudaEmprendimientoServicio;
        private readonly DetalleInversionEmprendimientoServicio _detalleInversionEmprendimientoServicio;

        public FormulariosController(FormularioServicio formularioServicio,
            DeudaGrupoServicio deudaGrupoServicio,
            DeudaEmprendimientoServicio deudaEmprendimientoServicio,
            DetalleInversionEmprendimientoServicio detalleInversionEmprendimientoServicio)
        {
            _formularioServicio = formularioServicio;
            _deudaGrupoServicio = deudaGrupoServicio;
            _deudaEmprendimientoServicio = deudaEmprendimientoServicio;
            _detalleInversionEmprendimientoServicio = detalleInversionEmprendimientoServicio;
        }

        public IdFormularioAgrupamiento Post([FromBody] RegistrarFormularioComando comando)
        {
            return _formularioServicio.Registrar(comando);
        }

        public DatosFormularioResultado Get([FromUri] int id)
        {
            return _formularioServicio.ObtenerPorId(id);
        }

        [Route("registrar-formularios")]
        public IList<FormularioXDni> RegistrarFormularios([FromBody] RegistrarFormularioComando comando)
        {
            return _formularioServicio.RegistrarFormularios(comando);
        }
        [HttpPost]
        [Route("consultar")]
        public Resultado<FormularioGrillaResultado> ObtenerFormularios([FromBody] FormularioGrillaConsulta consulta)
        {
            return _formularioServicio.ObtenerFormulariosPorFiltros(consulta);
        }

        [HttpPost]
        [Route("consultar-totalizador")]
        public string ObtenerTotalizador([FromBody] FormularioGrillaConsulta consulta)
        {
            var resultado = _formularioServicio.ObtenerTotaliziadorFormularios(consulta);
            return resultado;
        }

        [Route("consultar-totalizador-documentacion")]
        public string GetTotalizadorDocumentacion([FromUri] FiltrosFormularioConsulta consulta)
        {
            var resultado = _formularioServicio.ConsultaTotalizadorDocumentacion(consulta);
            return resultado;
        }
        [Route("consultar-totalizador-sucursal")]
        public string GetTotalizadorSucursalBancaria([FromUri] FiltrosFormularioConsulta consulta)
        {
            var resultado = _formularioServicio.ConsultaTotalizadorSucursalBancaria(consulta);
            return resultado;
        }

        [Route("consulta-bandeja-carga-numero-control-interno")]
        [HttpGet]
        public BandejaCargaNumeroControlInternoResultado ConsultaBandejaCargaNumeroControlInterno(
            [FromUri] int idFormularioLinea)
        {
            return _formularioServicio.ConsultarBandejaCargaNumeroControlInterno(idFormularioLinea);
        }

        [Route("consulta-situacion-domicilio-integrante")]
        [HttpGet]
        public IList<GrupoFamiliarDomicilioResultado> ConsultaSituacionDomicilioIntegrante(
            [FromUri] int idFormularioLinea)
        {
            return _formularioServicio.ConsultaSituacionDomicilioIntegrante(idFormularioLinea);
        }

        [Route("consulta-datos-domicilio-integrante")]
        [HttpGet]
        public DatosDomicilioIntegranteResultado ConsultaDatosDomicilioIntegrante(
            [FromUri] int idFormularioLinea)
        {
            return _formularioServicio.ConsultaDatosDomicilioIntegrante(idFormularioLinea);
        }

        [Route("existeFormularioEnCursoParaSolicitante")]
        public decimal Get([FromUri] DatosPersonaConsulta consulta)
        {
            return _formularioServicio.ExisteFormularioEnCursoParaPersona(consulta);
        }

        [Route("existeFormularioEnCursoParaSolicitanteReactivacion")]
        public decimal GetReactivacion([FromUri] DatosPersonaConsulta consulta)
        {
            return _formularioServicio.ExisteFormularioEnCursoParaPersonaReactivacion(consulta);
        }

        [Route("existeDeudaHistorica")]
        public bool GetDeudaHistorica([FromUri] DatosPersonaConsulta consulta)
        {
            return _formularioServicio.ExisteDeudaHistorica(consulta);
        }

        [Route("existeGrupoSolicitante")]
        [HttpGet]
        public bool GetGrupoSolicitante([FromUri] int idFormulario)
        {
            return _formularioServicio.ExisteGrupoParaSolicitante(idFormulario);
        }

        [Route("existeGrupoPersona")]
        [HttpGet]
        public bool GetGrupoSolicitantePersona([FromUri] DatosPersonaConsulta consulta)
        {
            return _formularioServicio.ExisteGrupoParaPersona(consulta);
        }

        [Route("existeFormularioEnCursoParaGrupoFamiliar")]
        public List<string> GetIntegrantesGrupoConFormulariosVigentes([FromUri] DatosPersonaConsulta consulta)
        {
            return _formularioServicio.ExisteFormularioEnCursoParaIntegranteGrupo(consulta);
        }

        [Route("familia-pertenece-agrupamiento")]
        [HttpGet]
        public List<string> MiembroGrupoPerteneceAgrupamiento([FromUri] GrupoFamiliarAgrupadosConsulta consulta)
        {
            return _formularioServicio.MiembroGrupoPerteneceAgrupamiento(consulta);
        }

        [Route("actualizar-cursos/{idFormulario}")]
        public IHttpActionResult ActualizarCursos(int idFormulario, IList<SolicitudCursoComando> solicitudesCurso)
        {
            _formularioServicio.ActualizarCursos(idFormulario, solicitudesCurso);
            return Ok(true);
        }

        [Route("actualizar-cursos-asociativas/{idAgrupamiento}")]
        public IHttpActionResult ActualizarCursosAsociativas(int idAgrupamiento, IList<SolicitudCursoComando> solicitudesCurso)
        {
            _formularioServicio.ActualizarCursosAsociativas(idAgrupamiento, solicitudesCurso);
            return Ok(true);
        }

        [Route("actualizar-destinos-fondos/{idFormulario}")]
        public IHttpActionResult ActualizarDestinosFondos(int idFormulario, OpcionDestinoFondosComando comando)
        {
            _formularioServicio.ActualizarDestinosFondos(idFormulario, comando);
            return Ok(true);
        }

        [Route("actualizar-destinos-fondos-asociativas/{idAgrupamiento}")]
        public IHttpActionResult ActualizarDestinosFondosAsociativas(int idAgrupamiento, OpcionDestinoFondosComando comando)
        {
            _formularioServicio.ActualizarDestinosFondosAsociativas(idAgrupamiento, comando);
            return Ok(true);
        }

        [Route("actualizar-condiciones-solicitadas/{idFormulario}")]
        public IHttpActionResult ActualizarCondicionesSolicitadas(int idFormulario, CondicionesSolicitadasComando comando)
        {
            _formularioServicio.ActualizarCondicionesSolicitadas(idFormulario, comando);
            return Ok(true);
        }

        [Route("modificar-condiciones-solicitadas/{idFormulario}")]
        public string ModificarCondicionesSolicitadas(int idFormulario, CondicionesSolicitadasComando comando)
        {
            return _formularioServicio.ModificarCondicionesSolicitadas(idFormulario, comando);
        }

        [Route("actualizar-condiciones-solicitadas-asociativas/{idAgrupamiento}")]
        public IHttpActionResult ActualizarCondicionesSolicitadasAsociativas(int idAgrupamiento, CondicionesSolicitadasComando comando)
        {
            _formularioServicio.ActualizarCondicionesSolicitadasAsociativas(idAgrupamiento, comando);
            return Ok(true);
        }

        [Route("obtener-condiciones-solicitadas/{idFormulario}")]
        [HttpGet]
        public DatosFormularioResultado ObtenerCondicionesSolicitada(int idFormulario)
        {
            return _formularioServicio.ObtenerCondicionesDePrestamoPorFormulario(idFormulario);
        }

        [Route("actualizar-garantes/{idFormulario}")]
        public IHttpActionResult ActualizarGarantes(int idFormulario, List<DatosPersonaComando> comando)
        {
            _formularioServicio.ActualizarGarantes(idFormulario, comando);
            return Ok(true);
        }

        [Route("actualizar-emprendimiento/{idAgrupamiento}")]
        public decimal ActualizarDatosEmprendimiento(int idAgrupamiento, EmprendimientoComando comando)
        {
            return _formularioServicio.ActualizarDatosEmprendimiento(idAgrupamiento, comando);
        }

        [Route("rechazar-formulario")]
        [HttpPost]
        public decimal RechazarFormulario([FromBody] RechazarFormularioComando comando)
        {
            return _formularioServicio.RegistrarRechazoFormulario(comando);
        }

        [HttpPost, Route("rechazar-formulario-con-prestamo")]
        public decimal RechazarFormularioConPrestamo([FromBody] RechazarFormularioComando comando)
        {
            return _formularioServicio.RegistrarRechazoFormularioConPrestamo(comando);
        }

        [Route("baja-formulario")]
        [HttpPost]
        public decimal DarDeBajaFormulario([FromBody] RechazarFormularioComando comando)
        {
            return _formularioServicio.DarDeBajaFormulario(comando);
        }

        [Route("rechazar-grupo-formularios")]
        [HttpPost]
        public IHttpActionResult RechazarGrupoFormularios([FromBody] RechazarFormularioComando comando)
        {
            _formularioServicio.RechazarGrupoFormularios(comando);
            return Ok(true);
        }

        [Route("consulta-datos-contacto")]
        [HttpGet]
        public DatosContactoResultado ObtenerDatosContacto([FromUri] DatosPersonaConsulta consulta)
        {
            return _formularioServicio.ObtenerDatosContacto(consulta);
        }

        [Route("actualizar-datos-contacto")]
        [HttpPost]
        public string ActualizarDatosContacto([FromBody] ActualizarDatosDeContactoComando comando)
        {
            return _formularioServicio.ActualizarDatosContacto(comando);
        }

        [Route("enviar")]
        [HttpPost]
        public decimal RegistrarEnvio([FromBody] RegistrarFormularioComando comando)
        {
            return _formularioServicio.RegistrarEnvio(comando);
        }

        [Route("iniciar")]
        [HttpPost]
        public decimal RegistrarInicio([FromBody] RegistrarFormularioComando comando)
        {
            return _formularioServicio.RegistrarInicio(comando);
        }

        [Route("reporte")]
        [HttpGet]
        public Task<string> GetReporteFormulario([FromUri] int id)
        {
            return _formularioServicio.ObtenerReporteFormulario(id);
        }

        [Route("reporte-linea")]
        [HttpGet]
        public Task<string> GetReporteFormularioVacio([FromUri] IdsDetalleLinea consulta)
        {
            return _formularioServicio.ObtenerReporteFormularioVacio(consulta.IdDetalleLinea, consulta.IdLinea);
        }

        [Route("actualiza-sucursal")]
        [HttpPost]
        public string ActualizarSucursal([FromBody] ActualizaSucursalComando comando)
        {
            return _formularioServicio.ActualizaSucursal(comando);
        }

        [Route("buscar-formularios")]
        [HttpPost]
        public Resultado<FormularioFiltradoResultado> ConsultaFormularios([FromBody] FiltrosFormularioConsulta consulta)
        {
            return _formularioServicio.ConsultaFormularios(consulta);
        }

        [Route("buscar-ids-formularios-filtro")]
        [HttpPost]
        public IList<FormularioFiltradoResultado> ConsultaFormulariosSinPaginar(
            [FromBody] FiltrosFormularioConsulta consulta)
        {
            return _formularioServicio.ConsultaFormulariosSinPaginar(consulta);
        }

        [Route("buscar-formularios-sucursal")]
        [HttpPost]
        public Resultado<FormularioFiltradoResultado> ConsultaFormulariosSucursal(
            [FromBody] FiltrosFormularioConsulta consulta)
        {
            return _formularioServicio.ConsultaFormulariosSucursal(consulta);
        }

        [Route("buscar-ids-formularios-sucursal-filtro")]
        [HttpPost]
        public IList<FormularioFiltradoResultado> ConsultaFormulariosSucursalSinPaginar(
            [FromBody] FiltrosFormularioConsulta consulta)
        {
            return _formularioServicio.ConsultaFormulariosSucursalSinPaginar(consulta);
        }

        [Route("actualizar-patrimonio-solicitante/{idFormulario}")]
        [HttpPost]
        public string ActualizarPatrimonioSolicitante(int idFormulario, PatrimonioSolicitanteComando comando)
        {
            return _formularioServicio.ActualizarPatrimonioSolicitante(idFormulario, comando);
        }

        [Route("actualizar-patrimonio-solicitante-asociativas/{idAgrupamiento}")]
        [HttpPost]
        public string ActualizarPatrimonioSolicitanteAsociativas(int idAgrupamiento, PatrimonioSolicitanteComando comando)
        {
            return _formularioServicio.ActualizarPatrimonioSolicitanteAsociativas(idAgrupamiento, comando);
        }

        [Route("reporte-deuda-grupo-conviviente")]
        [HttpGet]
        public ReporteResultado GetReporteDeudaGrupoConviviente([FromUri] FiltrosFormularioConsulta consulta)
        {
            return _deudaGrupoServicio.ObtenerReporteDeudaGrupoConviviente(consulta);
        }

        [Route("datos-emprendimiento/{id}")]
        [HttpGet]
        public EmprendimientoResultado GetDatosEmprendimientoFormulario([FromUri] int id)
        {
            return _formularioServicio.ObtenerDatosEmprendimientoPorFormulario(id);
        }

        [Route("actualizar-organizacion-emprendimiento/{idFormulario}")]
        [HttpPost]
        public IHttpActionResult ActualizarDatosOrganizacionEmprendimiento([FromUri] int idFormulario, [FromBody] OrganizacionEmprendimientoComando comando)
        {
            _formularioServicio.ActualizarDatosOrganizacionEmprendimiento(idFormulario, comando);
            return Ok();
        }

        [Route("ingresos-grupo")]
        [HttpGet]
        public IList<IngresoGrupoResultado> GetIngresosGrupoFamiliar()
        {
            return _formularioServicio.ObtenerIngresosGrupoFamiliar();
        }

        [Route("ingreso-total-grupo-familiar/{idGrupo}")]
        [HttpGet]
        public decimal ObtenerIngresoTotalGrupoFamiliar([FromUri] int idGrupo)
        {
            return _formularioServicio.ObtenerIngresoTotalGrupoFamiliar(idGrupo);
        }

        [Route("ingresos-grupo/{id}")]
        [HttpGet]
        public IList<IngresoGrupoResultado> GetIngresosGrupoFamiliarFormulario([FromUri] int id)
        {
            return _formularioServicio.ObtenerIngresosGrupoFamiliarFormulario(id);
        }

        [Route("gastos-grupo/{idGrupo}")]
        [HttpGet]
        public IList<EgresoGrupoResultado> GetGastosGrupoFamiliar([FromUri] int idGrupo)
        {
            return _formularioServicio.ObtenerGastosGrupoFamiliar(idGrupo);
        }

        [Route("actualizar-ingresos-grupo")]
        [HttpPost]
        public decimal ActualizarIngresosGrupoFamiliar(RegistrarIngresosGrupoFamiliarComando comando)
        {
            return _formularioServicio.RegistrarIngresosGrupoFamiliar(comando);
        }

        [Route("registrar-seguimiento-auditoria")]
        [HttpPost]
        public decimal RegistrarSeguimientoAuditoria(SeguimientoAuditoriaComando comando)
        {
            return _formularioServicio.RegistrarSeguimientoAuditoria(comando);
        }

        [Route("obtener-items-inversion")]
        [HttpGet]
        public IList<ItemInversionResultado> GetItemsInversion()
        {
            return _formularioServicio.ObtenerItemsInversion();
        }

        [Route("obtener-fuentes-financiamiento")]
        [HttpGet]
        public IList<FuenteFinanciamientoResultado> GetFuentesFinanciamiento()
        {
            return _formularioServicio.ObtenerFuentesFinanciamiento();
        }

        [Route("obtener-tipos-deuda")]
        [HttpGet]
        public IList<TipoDeudaResultado> GetTiposDeuda()
        {
            return _deudaEmprendimientoServicio.ObtenerTiposDeuda();
        }

        [HttpPost, Route("actualizar-deuda-emprendimiento/{idFormulario}")]
        public IHttpActionResult ActualizarDeudaEmprendimiento([FromUri] int idFormulario,
            [FromBody] List<RegistrarDeudaEmprendimientoComando> comando)
        {
            _formularioServicio.ActualizarDeudaEmprendimiento(idFormulario, comando);
            return Ok();
        }

        [HttpPost, Route("actualizar-inversiones-realizadas/{idFormulario}")]
        public IList<FormulariosInversionRealizadaResultado> ActualizarInversionesRealizadas([FromUri] int idFormulario,
            [FromBody] List<RegistrarInversionRealizadaComando> comando)
        {
            return _formularioServicio.ActualizarInversionesRealizadas(idFormulario, comando);
        }

        [HttpPost, Route("actualizar-necesidades-inversion/{idFormulario}")]
        public IHttpActionResult ActualizarNecesidadesInversion([FromUri] int idFormulario,
            [FromBody] RegistrarNecesidadInversionComando comando)
        {
            _formularioServicio.ActualizarNecesidadesInversion(idFormulario, comando);
            return Ok();
        }

        [HttpPost, Route("eliminar-detalles-inversion")]
        public IHttpActionResult EliminarDetallesInversion([FromBody] List<Id> comando)
        {
            _detalleInversionEmprendimientoServicio.EliminarDetalleInversion(comando);
            return Ok();
        }

        [HttpPost, Route("actualizar-cuadrante-precio-venta/{idFormulario}")]
        public decimal ActualizarCuadrantePrecioVenta([FromUri] int idFormulario, [FromBody] ActualizarPrecioVentaComando comando)
        {
            return _formularioServicio.ActualizarCuadrantePrecioVenta(idFormulario, comando);
        }

        [Route("obtener-situacion-persona")]
        [HttpGet]
        public Resultado<SituacionPersonasResultado> GetSituacionPersona([FromUri] SituacionPersonasConsulta consulta)
        {
            return _formularioServicio.ObtenerSituacionPersona(consulta);
        }

        [Route("obtener-vista-situacion-persona")]
        [HttpGet]
        public IList<SituacionPersonasResultadoVista> GetVistaSituacionPersona([FromUri] SituacionPersonasConsulta consulta)
        {
            return _formularioServicio.ObtenerVistaSituacionPersona(consulta);
        }

        [Route("obtener-formularios-situacion-persona")]
        [HttpGet]
        public Resultado<FormulariosSituacionResultado> GetFormulariosSituacionPersona([FromUri] SituacionPersonasConsulta consulta)
        {
            return _formularioServicio.ObtenerFormulariosSituacionPersona(consulta);
        }

        [HttpGet, Route("obtener-motivos-rechazo-prestamo/{idPrestamo}")]
        public IList<MotivosRechazoFormularioResultado> ObtenerRechazosFormulario(decimal idFormulario)
        {
            return _formularioServicio.ObtenerRechazosFormulario(idFormulario);
        }

        [Route("precio-venta/costos/{idFormulario}")]
        [HttpGet]
        public IList<ItemsPrecioVentaResultado> GetFuentesFinanciamiento(int idFormulario)
        {
            return _formularioServicio.ObtenerCostosPrecioVenta(idFormulario);
        }

        [Route("obtener-reporte-excel-situacion-persona")]
        [HttpGet]
        public ArchivoBase64 GetExcelSituacionPersona([FromUri] SituacionPersonasConsulta consulta)
        {
            return _formularioServicio.ObtenExcelPersonaResultado(consulta);
        }

        [Route("obtener-motivos-rechazo-referencia")]
        [HttpGet]
        public IList<MotivosRechazoReferenciaResultado> GetMotivosRechazoReferencia([FromUri] MotivosRechazoReferenciaConsulta consulta)
        {
            return _formularioServicio.ObtenerMotivosRechazoReferencia(consulta);
        }

        [Route("obtener-motivos-rechazo-formulario/{idFormulario}")]
        [HttpGet]
        public IList<MotivosRechazoFormularioResultado> GetMotivosRechazoXForm([FromUri] decimal idFormulario)
        {
            return _formularioServicio.ObtenerRechazosFormulario(idFormulario);
        }

        [Route("obtener-agrupamiento/{idFormulario}")]
        [HttpGet]
        public decimal GetIdAgrupamiento(int idFormulario)
        {
            return _formularioServicio.ObtenerIdAgrupamiento(idFormulario);
        }

        [Route("validar-estados-formularios/{idAgrupamiento}")]
        [HttpGet]
        public bool ValidarEstadosFormularios(int idAgrupamiento)
        {
            return _formularioServicio.ValidarEstadosFormulariosAgrupados(idAgrupamiento);
        }

        [Route("obtener-formularios-agrupados/{idAgrupamiento}")]
        [HttpGet]
        public IList<AgruparFormulario> GetFormulariosConAgrupamiento([FromUri] int idAgrupamiento)
        {
            var res = _formularioServicio.ObtenerFormulariosConAgrupamiento(idAgrupamiento);
            return res;
        }

        [Route("obtener-estado-formulario/{idEstado}")]
        [HttpGet]
        public string GetEstadoFormulario([FromUri] int idEstado)
        {
            return _formularioServicio.ObtenerEstadoConId(idEstado);
        }

        [Route("obtener-ongs/{idLinea}")]
        [HttpGet]
        public IList<OngComboResultado> ObtenerOngs([FromUri]int idLinea)
        {
            return _formularioServicio.ObtenerOngs(idLinea);
        }
        

        [Route("obtener-numero-grupo")]
        [HttpPost] 
        public decimal RegistrarOng([FromBody] OngFormulario comando)
        {
            return _formularioServicio.ObtenerNumeroGrupo(comando);
        }


        [Route("registrar-ong-para-formulario/{idAgrupamiento}")]
        public IHttpActionResult RegistrarOngParaFormulario(int idAgrupamiento, OngFormulario comando)
        {
            _formularioServicio.RegistrarOngParaFormulario(idAgrupamiento, comando);
            return Ok(true);
        }

        [Route("cargar-numero-control-interno")]
        [HttpPost]
        public bool CargarNumerosControlInterno([FromBody] CargarNumerosControlInternoComando comando)
        {
            return _formularioServicio.CargarNumerosControlInterno(comando);
        }

        [Route("obtener-formulario-fecha-pago/{idFormulario}")]
        public FormularioFechaPagoResultado GetFormularioFechaPago(int idFormulario)
        {
            return _formularioServicio.ObtenerFormularioFechaPago(idFormulario);
        }

        [Route("obtener-tiempo-reprogramacion")]
        public decimal GetTiemporeprogramacion()
        {
            return _formularioServicio.ObtenerTiempoReprogramacion();
        }

        [HttpGet, Route("obtener-historial-reprogramacion/{idFormulario}")]
        public IList<Reprogramacion> GetHistorialReprogramacion(int idFormulario)
        {
            return _formularioServicio.ObtenerHistorialReprogramacion(idFormulario);
        }

        [HttpPost, Route("cambiar-garante-formulario")]
        public bool CambiarGaranteFormulario([FromBody] CambiarGaranteComando comando)
        {
            return _formularioServicio.CambiarGaranteFormulario(comando);
        }

        [Route("obtener-reporte-excel-bandeja-formularios")]
        [HttpPost]
        public ArchivoBase64 GetExcelBandejaFormularios([FromBody] FormularioGrillaConsulta consulta)
        {
            return _formularioServicio.ObtenExcelBandejaformularios(consulta);
        }

        
        [HttpPost]
        [Route("obtener-reporte-pdf-bandeja-formularios")]
        public ReporteResultado GetPDFBandejaFormularios([FromBody] FormularioGrillaConsulta consulta)
        {
            return _formularioServicio.ObtenPDFBandejaformularios(consulta);

        }

        [HttpGet]
        [Route("obtener-nro-suac/{idFormulario}")]
        public string ObtenerNroSuac(decimal idFormulario)
        {
            return _formularioServicio.ObtenerNroSuac(idFormulario);
        }

    }
}