using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Infraestructura.Core.CiDi;
using Infraestructura.Core.Comun.Archivos;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Pagos.Aplicacion.Comandos;
using Pagos.Aplicacion.Consultas.Consultas;
using Pagos.Aplicacion.Consultas.Resultados;
using Pagos.Aplicacion.Servicios;
using Convenio = Pagos.Aplicacion.Consultas.Resultados.Convenio;


namespace Api.Controllers.Pagos
{
    public class PagosController : ApiController
    {
        private readonly PagosServicio _pagosServicio;
        private readonly DocumentacionPagosServicio _documentacionPagosServicio;

        public PagosController(PagosServicio pagosServicio, DocumentacionPagosServicio documentacionPagosServicio)
        {
            _pagosServicio = pagosServicio;
            _documentacionPagosServicio = documentacionPagosServicio;
        }

        [HttpPost]
        [Route("consultar")]
        public Resultado<BandejaPagosResultado> Get([FromBody] BandejaPagosConsulta consulta)
        {
            return _pagosServicio.ConsultaBandeja(consulta);
        }


        [HttpPost]
        [Route("consultar-bandeja-completa")]
        public Resultado<BandejaPagosResultado> GetBandejaCompleta([FromBody] BandejaPagosConsulta consulta)
        {
            return _pagosServicio.ConsultaBandejaCompleta(consulta);
        }

        [HttpGet]
        [Route("consultar-montos-disponible")]
        public List<MontoDisponibleSimularLoteResultado> GetMontosDisponible([FromUri] MontosDisponibleConsulta consulta)
        {
            return _pagosServicio.ConsultarMontosDisponibles(consulta);
        }

        [HttpGet]
        [Route("obtener-iva-comision")]
        public TasasLoteResultado GetTasas()
        {
            return _pagosServicio.ObtenerIvaComision();
        }

        [Route("confirmar-lote")]
        [HttpPost]
        public string ConfirmarLote([FromBody] ConfirmarLoteComando comando)
        {
            return _pagosServicio.ConfirmarLote(comando);
        }
        [Route("agregar-prestamo-lote")]
        [HttpPost]
        public string AgregarPrestamoLote([FromBody] AgregarPrestamoComando comando)
        {
            return _pagosServicio.AgregarPrestamoLote(comando);
        }

        [HttpGet]
        [Route("obtener-total-lote")]
        public decimal ObtenerTotalLote([FromUri] decimal idLoteSuaf)
        {
            return _pagosServicio.ObtenerTotalLote(idLoteSuaf);
        }

        [HttpGet]
        [Route("habilitado-adenda")]
        public bool HabilitadoAdenda([FromUri] decimal idLoteSuaf)
        {
            return _pagosServicio.HabilitadoAdenda(idLoteSuaf);
        }

        [Route("confirmar-lote-adenda")]
        [HttpPost]
        public string ConfirmarLoteAdenda([FromBody] ConfirmarLoteComando comando)
        {
            return _pagosServicio.ConfirmarLoteAdenda(comando);
        }

        [HttpPost]
        [Route("consultar-bandeja-adenda")]
        public Resultado<BandejaAdendaResultado> GetBandejaAdenda([FromBody] BandejaAdendaConsulta consulta)
        {
            return _pagosServicio.ConsultaBandejaAdenda(consulta);
        }

        [HttpPost]
        [Route("seleccionar-todos-adenda")]
        public decimal SeleccionarTodosAdenda([FromBody] BandejaAdendaConsulta consulta)
        {
            var idUsuario = User.Identity.UsuarioId();

            return _pagosServicio.SeleccionarTodosParaAdenda(consulta, idUsuario);
        }

        [HttpPost]
        [Route("modificar-detalle-adenda")]
        public decimal ModificarDetalleAdenda([FromBody] DetallesAdenda consulta)
        {
            var idUsuario = User.Identity.UsuarioId();

            return _pagosServicio.ModificarDetalleAdenda(consulta, idUsuario);
        }

        [HttpPost]
        [Route("formularios-seleccionados-adenda")]
        public Resultado<FormulariosSeleccionadosAdendaResultado> GetFormulariosSeleccionadosParaAdenda([FromBody] FormulariosSeleccionadosAdendaConsulta consulta)
        {
            return _pagosServicio.ObtenerFormulariosParaAdenda(consulta);
        }
        
        [Route("reporte")]
        [HttpPost]
        public Task<ReporteResultado> GetReportePagos([FromBody] ReportePagosConsulta consulta)
        {
            var idUsuario = User.Identity.UsuarioId();

            return consulta.IdOpcion == 1 
                ? _documentacionPagosServicio.ObtenerReportePagos(consulta, idUsuario) 
                : _documentacionPagosServicio.ObtenerCuponera(consulta, idUsuario);
        }

        [Route("reporte-no-impreso")]
        [HttpPost]
        public Task<ReporteResultado> GetReportePagosNoImpresos([FromBody] ReportePagosConsulta consulta)
        {
            return consulta.IdOpcion == 1
                ? _documentacionPagosServicio.ObtenerReportePagosNoImpresos(consulta)
                : _documentacionPagosServicio.ObtenerCuponerasNoImpresas(consulta);
        }

        [HttpGet]
        [Route("consultar-bandeja-lotes")]
        public Resultado<BandejaLotesResultado> GetBandejaLotes([FromUri] BandejaLotesConsulta consulta)
        {
            return _pagosServicio.ConsultaBandejaLotes(consulta);
        }

        [HttpGet]
        [Route("consultar-bandeja-cheque")]
        public Resultado<FormularioChequeGrillaResultado> GetBandejaCheques([FromUri] FormularioGrillaChequeConsulta consulta)
        {
            return _pagosServicio.ConsultaBandejaCheques(consulta);
        }

        [HttpGet]
        [Route("consultar-totalizador-cheque")]
        public string GetTotalizadorCheques([FromUri] FormularioGrillaChequeConsulta consulta)
        {
            return _pagosServicio.ConsultaTotalizadorCheques(consulta);
        }

        [Route("cargar-datos-cheque")]
        [HttpPost]
        public bool CargarDatosCheque([FromBody] CargaDatosChequeComando comando)
        {
            return _pagosServicio.ActualizarDatosCheque(comando);
        }

        [HttpGet]
        [Route("consultar-totalizador")]
        public string GetTotalizadorLotes([FromUri] BandejaLotesConsulta consulta)
        {
            return _pagosServicio.ConsultaTotalizadorLotes(consulta);
        }

        [Route("formularios-por-prestamo")]
        [HttpGet]
        public IList<FormularioPrestamoResultado> GetFormulariosPorPrestamo([FromUri] int idPrestamo)
        {
            return _pagosServicio.ObtenerFormulariosPorPrestamo(idPrestamo);
        }

        [Route("liberar-lote")]
        [HttpPost]
        public string LiberarLote([FromBody] decimal idLote)
        {
            return _pagosServicio.LiberarLote(idLote);
        }

        [Route("permite-liberar-lote")]
        [HttpGet]
        public PermiteLiberarLoteResultado PermiteLiberarLote([FromUri] decimal idLote)
        {
            return _pagosServicio.PermiteLiberarLote(idLote);
        }

        [Route("obtener-cabecera-detalle-lote")]
        [HttpPost]
        public DetalleLoteResultado ObtenerDetalleLote([FromBody] decimal idLote)
        {
            return _pagosServicio.ObtenerDetalleLote(idLote);
        }
        
        [Route("obtener-prestamos-detalle-lote")]
        [HttpGet]
        public Resultado<BandejaPagosResultado> ObtenerPrestamosDetalleLote([FromUri] PrestamosDetalleLoteConsulta consulta)
        {
            return _pagosServicio.ObtenerPrestamosDetalleLote(consulta);
        }

        [Route("obtener-historial-detalle-lote")]
        [HttpPost]
        public IList<HistorialLoteResultado> ObtenerHistorialDetalleLote([FromBody] decimal idLote)
        {
            return _pagosServicio.ObtenerHistorialDetalleLote(idLote);
        }

        [Route("desagrupar-lote")]
        [HttpPost]
        public string DesagruparLote([FromBody] DesagruparLoteComando comando)
        {
            return _pagosServicio.DesagruparLote(comando);
        }

        [Route("obtener-excel-para-banco")]
        [HttpGet]
        public ReporteExcelBancoResultado ObtenerExcelBanco([FromUri] decimal idLote)
        {
            return _pagosServicio.ObtenerExcelBanco(idLote);
        }

        [Route("registrar-generacion-cheque")]
        [HttpGet]
        public decimal RegistrarGeneracionCheque([FromUri] decimal idLote)
        {
            return _pagosServicio.RegistrarGeneracionChequeLote(idLote);
        }

        [Route("validar-providencia-lote")]
        [HttpGet]
        public ReporteExcelBancoResultado ObtenerValidacionProvidenciaLote([FromUri] decimal idLote)
        {
            return _pagosServicio.ValidarProvidenciaLote(idLote);
        }

        [Route("generar-archivo-txt/{idLote}/{idTipoPago}")]
        [HttpGet]
        public ReporteExcelBancoResultado GenerarArchivoTxt([FromUri] decimal idLote, int idTipoPago)
        {
            return _pagosServicio.GenerarArchivoTxt(idLote, idTipoPago);
        }

        [HttpGet]
        [Route("obtener-combo-tipo-pago")]
        public IList<ComboTiposPagoResultado> ObtenerComboTipoPago()
        {
            return _pagosServicio.ObtenerComboTipoPago();
        }

        [Route("validar-estados-formularios")]
        [HttpGet]
        public bool VerificarEstadoFormulario([FromUri] decimal idLote)
        {
            return _pagosServicio.VerificarEstadoFormulario(idLote);
        }

        [Route("obtener-nota-pago")]
        [HttpGet]
        public ReporteExcelBancoResultado ObtenerNotaPago([FromUri] CrearNotaBancoConsulta consulta)
        {
            return _pagosServicio.ObtenerNotaPago(consulta);
        }

        [Route("buscar-formularios-en-lote")]
        [HttpPost]
        public Resultado<GrillaPlanPagosResultado> ConsultaFormulariosEnLote([FromBody] FiltrosFormularioConsulta consulta)
        {
            return _pagosServicio.ConsultaFormularios(consulta);
        }

        [Route("buscar-ids-formularios-filtro-en-lote")]
        [HttpPost]
        public IList<GrillaPlanPagosResultado> ConsultaFormulariosSinPaginarEnLote([FromBody] FiltrosFormularioConsulta consulta)
        {
            return _pagosServicio.ConsultaFormulariosSinPaginar(consulta);
        }

        [HttpGet]
        [Route("periodos-plan-pago")]
        public IEnumerable<PeriodoPlanPagoComboResultado> PeriodosPlanPagosCombo()
        {
            return _pagosServicio.PeriodosPlanPagosCombo();
        }

        [Route("actualizar-plan-pagos")]
        [HttpPost]
        public bool ActualizarPlanPagos([FromBody] GenerarPlanPagosComando comando)
        {
            return _pagosServicio.ActualizarPlanDePagos(comando);
        }

        [HttpPost]
        [Route("detalles-plan-pago")]
        public IList<PlanDePagoResultado> ObtenerDetallesPlanPagoFormulario([FromBody] DetallesPlanDePagoConsulta consulta)
        {
            return _pagosServicio.ObtenerDetallesPlanPagoFormulario(consulta);
        }

        [HttpGet]
        [Route("consultar-bandeja-formularios-suaf")]
        public Resultado<BandejaFormulariosSuafResultado> ConsultarBandejaFormulariosSuaf([FromUri] BandejaFormulariosSuafConsulta consulta)
        {
            return _pagosServicio.ConsultarBandejaFormulariosSuaf(consulta);
        }

        [HttpGet]
        [Route("consultar-bandeja-formularios-suaf-seleccionar-todos")]
        public Resultado<BandejaFormulariosSuafResultado> ConsultarBandejaFormulariosSuafSeleccionarTodos([FromUri] BandejaFormulariosSuafConsulta consulta)
        {
            return _pagosServicio.ConsultarBandejaFormulariosSuafSeleccionarTodos(consulta);
        }

        [HttpPost]
        [Route("registrar-lote-suaf")]
        public decimal RegistrarLoteSuaf([FromBody] RegistrarLoteSuafComando comando)
        {
            return _pagosServicio.RegistrarLoteSuaf(comando);
        }

        [HttpGet]
        [Route("obtener-excel-suaf")]
        public ReporteExcelBancoResultado ObtenerExcelSuaf([FromUri] decimal idLote)
        {
            return _pagosServicio.ObtenerExcelSuaf(idLote);
        }

        [HttpGet]
        [Route("generar-excel-activacion-masiva")]
        public ReporteExcelBancoResultado GenerarExcelActivacionMasiva([FromUri] decimal idLote)
        {
            return _pagosServicio.ObtenerExcelActivacionMasiva(idLote);
        }

        [HttpGet]
        [Route("combo-lotes-pagos")]
        public IList<ComboLotesResultado> GetComboLotes([FromUri] decimal? tipoLote)
        {
            return _pagosServicio.ConsultaComboLotes(tipoLote);  //TODO: El fer me hizo un SP especial para esto - 
        }

        [HttpPost, Route("importar-excel-suaf")]
        public ImportacionSuafResultado ImportarLoteSuaf([FromBody] RegistrarExcelSuafComando excelSuafComando)
        {
            var idUsuario = User.Identity.UsuarioId();

            return _pagosServicio.ImportarArchivoSuaf(excelSuafComando, new Id(idUsuario));
        }

        [HttpGet]
        [Route("consultar-bandeja-suaf")]
        public Resultado<BandejaSuafResultado> ConsultarBandejaSuaf([FromUri] BandejaSuafConsulta consulta)
        {
            return _pagosServicio.ConsultarBandejaSuaf(consulta);
        }

        [HttpGet]
        [Route("consultar-totalizador-suaf")]
        public string ConsultarTotalizadorSuaf([FromUri] BandejaSuafConsulta consulta)
        {
            return _pagosServicio.ConsultarTotalizadorSuaf(consulta);
        }

        [HttpGet]
        [Route("obtener-combo-lotes-suaf")]
        public IList<ComboLotesResultado> ObtenerComboLotesSuaf()
        {
            return _pagosServicio.ObtenerComboLotesSuaf();
        }

        [HttpPost]
        [Route("cargar-devengado-manual")]
        public bool CargaDevengadoManual(CargaDevengadoComando comando)
        {
            return _pagosServicio.CargaDevengadoManual(comando);
        }

        [HttpPost]
        [Route("borrar-devengado")]
        public bool BorrarDevengadoManual(CargaDevengadoComando comando)
        {
            return _pagosServicio.BorrarDevengadoManual(comando);
        }

        [Route("modalidades-pago")]
        [HttpGet]
        public IList<ComboLotesResultado> ObtenerModalidadesPago()
        {
            return _pagosServicio.ObtenerModalidades();
        }

        [Route("elementos-pago")]
        [HttpGet]
        public IList<ComboLotesResultado> ObtenerElementosPago()
        {
            return _pagosServicio.ObtenerElementos();
        }

        [Route("convenios")]
        [HttpGet]
        public IList<Convenio> ObtenerConvenios()
        {
            return _pagosServicio.ObtenerConvenios(1); //Obtiene los convenios que no son recupero
        }

        [HttpPost]
        [Route("actualizar-modalidad")]
        public bool ActualizarModalidadPago([FromBody]ActualizaModalidadComando comando)
        {
            return _pagosServicio.ActualizarModalidadPago(comando);
        }

        [HttpGet]
        [Route("obtener-fechas-pago")]
        public FormularioFechaPagoResultado ObtenerFechasPago([FromUri] decimal idLote)
        {
            return _pagosServicio.ObtenerFechasPago(idLote);
        }

        [HttpGet]
        [Route("validar-lote-pago")]
        public bool ValidarLotePago([FromUri] decimal idLote)
        {
            return _pagosServicio.ValidarLotePago(idLote);
        }

        [HttpPost]
        [Route("informes")]
        public ReporteExcelBancoResultado GetReporteFormulario([FromBody] InformePagosConsulta comando)
        {
            var idUsuario = User.Identity.UsuarioId();

            return _pagosServicio.GenerarExcel(comando, idUsuario);
        }

        [HttpGet]
        [Route("imprimir-plan-cuotas")]
        public ReporteExcelBancoResultado ImprimirPlanCuotas([FromUri] DetallesPlanDePagoConsulta consulta)
        {
            return _pagosServicio.ImprimirPlanCuotas(consulta);
        }

        [HttpGet]
        [Route("obtener-lineas-adenda")]
        public IList<LineaAdendaResultado> ObtenerLineasAdenda([FromUri] decimal nroDetalle)
        {
            return _pagosServicio.ObtenerLineasAdenda(nroDetalle);
        }

        [HttpPost]
        [Route("generar-adenda")]
        public bool GenerarAdenda([FromBody] GenerarAdendaComando comando)
        {
            return _pagosServicio.GenerarAdenda(comando);
        }

        [HttpGet]
        [Route("bandeja-cambio-estado")]
        public Resultado<BandejaCambioEstadoResultado> ConsultaBandejaCambioEstado([FromUri] BandejaCambioEstadoConsulta consulta)
        {
            return _pagosServicio.ConsultarBandejaCambioEstado(consulta);
        }

        [HttpGet]
        [Route("consultar-totalizador-estado")]
        public string ConsultarTotalizadorCambioEstado([FromUri] BandejaCambioEstadoConsulta consulta)
        {
            return _pagosServicio.ConsultarTotalizadorCambioEstado(consulta);
        }

        [HttpGet]
        [Route("cambiar-estado-formulario")]
        public bool CambiarEstadoFormulario([FromUri] decimal idFormulario)
        {
            return _pagosServicio.CambiarEstadoFormulario(idFormulario);
        }
    }
}
