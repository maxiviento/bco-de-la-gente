using System;
using System.Collections.Generic;
using System.Web.Http;
using Formulario.Aplicacion.Comandos;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Aplicacion.Servicios;
using Infraestructura.Core.Comun.Presentacion;
using Formulario.Aplicacion.Consultas.Consultas;
using Infraestructura.Core.Comun.Archivos;
using Soporte.Aplicacion.Consultas.Resultados;
using DocumentoDescargaResultado = Formulario.Aplicacion.Servicios.DocumentoDescargaResultado;

namespace Api.Controllers.Formulario
{
    public class PrestamosController : ApiController
    {
        private readonly PrestamoServicio _prestamoServicio;

        public PrestamosController(PrestamoServicio prestamoServicio)
        {
            _prestamoServicio = prestamoServicio;
        }

        [Route("consulta-integrantes/{id:decimal}")]
        [HttpGet]
        public IList<PrestamoResultado.Integrante> GetIntegrantes(decimal id)
        {
            return _prestamoServicio.ConsultarIntegrantes(id);
        }

        [Route("consultar-bandeja")]
        public Resultado<BandejaPrestamoResultado> ObtenerPrestamo([FromBody] BandejaPrestamosConsulta comando)
        {
            return _prestamoServicio.ObtenerPrestamosPorFiltros(comando);
        }

        [Route("consultar-totalizador")]
        public string ObtenerTotalizador([FromBody] BandejaPrestamosConsulta consulta)
        {
            return _prestamoServicio.ObtenerTotalziadorPrestamos(consulta);
        }

        [Route("consultar-conformar-prestamo")]
        public Resultado<BandejaConformarPrestamoResultado> Get([FromUri] BandejaConformarPrestamoConsulta consulta)
        {
            return _prestamoServicio.ObtenerFormulariosPorFiltros(consulta);
        }

        [Route("generar-prestamo")]
        public RegistrarPrestamoResultado GetGenerarPrestamo([FromUri] int id)
        {
            return _prestamoServicio.GenerarPrestamo(id);
        }

        [Route("permitir-agrupacion-para-garantes-faltantes")]
        public bool PostValidarFormularioCanceladoParaGarante([FromBody] DatosPersonaResultado garante,
            [FromBody] List<DatosPersonaResultado> solicitantes, [FromBody] int idLinea)
        {
            return _prestamoServicio.ValidarFormularioCanceladoParaGarante(garante, solicitantes, idLinea);
        }

        [Route("consulta-requisitos/{id:decimal}")]
        [HttpGet]
        public IList<RequisitoResultado.Detallado> GetRequisitos(decimal id)
        {
            return _prestamoServicio.ConsultarRequisitosPrestamo(id);
        }

        [Route("consulta-requisitos-cargados/{id:decimal}/{idFormularioLinea:decimal}")]
        [HttpGet]
        public IList<RequisitoResultado.Cargado> GetRequisitosCargados(decimal id, decimal idFormularioLinea)
        {
            return _prestamoServicio.ConsultarRequisitosCargados(id, idFormularioLinea);
        }

        [Route("consulta-datos-prestamo/{id:decimal}")]
        [HttpGet]
        public PrestamoResultado.Datos GetDatosPrestamo(decimal id)
        {
            return _prestamoServicio.ConsultaDatosPrestamo(id);
        }

        [Route("consulta-datos-garante-prestamo/{id:decimal}")]
        [HttpGet]
        public IList<PrestamoResultado.Garante> GetDatosGarantePrestamo(decimal id)
        {
            return _prestamoServicio.ConsultaDatosGarantePrestamo(id);
        }

        [Route("aceptacion-requisitos")]
        [HttpPost]
        public string AceptarRequisitos([FromBody] PrestamoComando comando)
        {
            return _prestamoServicio.AceptarCheckList(comando);
        }

        [Route("guardado-requisitos")]
        [HttpPost]
        public string GuardarRequisitos([FromBody] PrestamoComando comando)
        {
            return _prestamoServicio.GuardarCheckList(comando);
        }

        [Route("consulta-seguimientos")]
        [HttpGet]
        public Resultado<PrestamoResultado.Seguimiento> ConsultaSeguimientos(
            [FromUri] SeguimientosPrestamoConsulta consulta)
        {
            return _prestamoServicio.ConsultarSeguimientos(consulta);
        }

        [Route("rechazar-prestamo")]
        [HttpPost]
        public decimal RechazarPrestamo([FromBody] RechazarPrestamoComando comando)
        {
            return _prestamoServicio.RegistrarRechazoPrestamo(comando);
        }

        [Route("estados-prestamo")]
        public IList<ClaveValorResultado<string>> GetEstadosPrestamo()
        {
            return _prestamoServicio.ConsultarEstadosPrestamo();
        }

        [Route("etapas-estados-linea")]
        [HttpGet]
        public IList<EtapaEstadoLineaResultado> ObtenerEtapasxEstadosLinea([FromUri] long idLinea)
        {
            return _prestamoServicio.ObtenerEtapasxEstadosLinea(idLinea);
        }

        [HttpGet, Route("encabezado-archivos/{id}")]
        public PrestamoResultado.EncabezadoArchivos ObtenerEncabezadoPrestamoArchivos(long id)
        {
            return _prestamoServicio.ObtenerEncabezadoPrestamoArchivos(id);
        }

        [Route("obtener-formularios-agrupamiento/{idAgrupamiento}")]
        [HttpGet]
        public IList<AgruparFormulario> ObtenerFormulariosPorAgrumiento(int idAgrupamiento)
        {
            return _prestamoServicio.ObtenerFormulariosPorAgrumiento(idAgrupamiento);
        }

        [Route("obtener-fecha-aprobacion/{idPrestamo}")]
        [HttpGet]
        public FechaAprobacionResultado obtenerFechaAprobacion(int idPrestamo)
        {
            return _prestamoServicio.ObtenerFechaAprobacion(idPrestamo);
        }

        [Route("actualizar-fecha-pago-formulario")]
        [HttpPost]
        public bool ActualizarFechaPagoFormulario([FromBody] ActualizarFechaPagoFormularioComando comando)
        {
            return _prestamoServicio.ActualizarFechaPagoFormulario(comando);
        }

        [Route("obtener-id-prestamo/{idFormulario}")]
        [HttpGet]
        public decimal ObtenerIdPrestamo(int idFormulario)
        {
            return _prestamoServicio.ObtenerIdPrestamo(idFormulario);
        }

        [HttpPost, Route("registrar-rechazo-reactivacion-prestamo")]
        public bool RegistrarRechazoReactivacion(RegistrarRechazoReactivacionPrestamoComando comando)
        {
            return _prestamoServicio.RegistrarRechazoReactivacion(comando);
        }

        [HttpPost,Route("registrar-reactivacion-prestamo")]
        public bool RegistrarReactivacion(RegistrarReactivacionPrestamoComando comando)
        {
            return _prestamoServicio.RegistrarReactivacion(comando);
        }

        [HttpGet, Route("obtener-datos-reactivacion/{idPrestamo}")]
        public PrestamoReactivacionResultado ObtenerDatosPrestamoReactivacion(decimal idPrestamo)
        {
            return _prestamoServicio.ObtenerDatosPrestamoReactivacion(idPrestamo);
        }

        [HttpGet, Route("obtener-motivos-rechazo-prestamo/{idPrestamo}")]
        public IList<MotivosRechazoPrestamoResultado> ObtenerRechazosPrestamo(decimal idPrestamo)
        {
            return _prestamoServicio.ObtenerRechazosPrestamo(idPrestamo);
        }

        [Route("editar-numero-caja")]
        [HttpPost]
        public bool ActualizarNumeroCaja(NumeroCajaComando comando)
        {
            return _prestamoServicio.ActualizarNumeroCaja(comando);
        }

        [Route("obtener-reporte-excel-bandeja-prestamos")]
        [HttpPost]
        public ArchivoBase64 GetExcelBandejaFormularios([FromBody] BandejaPrestamosConsulta consulta)
        {
            return _prestamoServicio.ObtenExcelBandejaPrestamos(consulta);
        }


        [HttpPost]
        [Route("obtener-reporte-pdf-bandeja-prestamos")]
        public ReporteResultado GetPDFBandejaFormularios([FromBody] BandejaPrestamosConsulta consulta)
        {
            return _prestamoServicio.ObtenPDFBandejaPrestamos(consulta);

        }
        [Route("imprimir-txt-generar-prestamo")]
        [HttpGet]
        public DocumentoDescargaResultado GetReportePagosNoImpresos([FromUri] string idsAgrupamiento, bool generado)
        {
            return _prestamoServicio.GenerarTxtConformarPrestamo(idsAgrupamiento, generado);
        }
    }
}