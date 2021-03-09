using System;
using System.Collections.Generic;
using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Datos;
using NHibernate;
using Pagos.Aplicacion.Consultas.Consultas;
using Pagos.Aplicacion.Consultas.Resultados;
using Pagos.Dominio.IRepositorio;
using Pagos.Dominio.Modelo;

namespace Datos.Repositorios.Pagos
{
    public class RecuperoRepositorio : NhRepositorio<Recupero>, IRecuperoRepositorio
    {
        public RecuperoRepositorio(ISession sesion) : base(sesion)
        {
        }

        public Resultado<BandejaRecuperoResultado> ObtenerBandejaArchivoRecupero(BandejaRecuperoConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_BANDEJA_RECUPERO")
                .AddParam(consulta.FechaDesde)
                .AddParam(consulta.FechaHasta)
                .AddParam(consulta.IdTipoEntidad ?? -1)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<BandejaRecuperoResultado>();

            return CrearResultado(consulta, elementos);
        }

        public bool ValidarNombreArchivoRecupero(string nombreArchivo)
        {
            var existe = Execute("PR_EXISTE_ARCHIVO_BANCO")
                    .AddParam(nombreArchivo)
                .ToEscalarResult<string>();
            return existe == "S";
        }

        public decimal RegistrarCabeceraRecupero(string nombreArchivo, decimal idUsuario, decimal idTipoEntidad, int convenio, DateTime fechaRecupero)
        {
            var resultado = Execute("PR_REGISTRA_ARCHIVO_BANCO")
                .AddParam(nombreArchivo)
                .AddParam(idTipoEntidad)
                .AddParam(idUsuario)
                .AddParam(convenio)
                .AddParam(fechaRecupero)
                .ToSpResult();
            return resultado.Id.Valor;
        }

        public int ActualizarCabeceraRecupero(decimal idArchivoBanco, decimal cantTotal, decimal cantCuotasProc, decimal cantCuotasEspec, decimal cantCuotasIncons, decimal montoRecuperado, decimal montoRechazado)
        {
            return Execute("PR_ACTUALIZA_ARCHIVO_BANCO")
                .AddParam(idArchivoBanco)
                .AddParam(cantTotal)
                .AddParam(cantCuotasProc)
                .AddParam(cantCuotasEspec)
                .AddParam(cantCuotasIncons)
                .AddParam(montoRecuperado)
                .AddParam(montoRechazado)
                .ToEscalarResult<int>();
        }

        public string RegistrarDetalleArchivoRecupero(decimal idArchivoBanco, decimal nroFormulario, decimal nroCuota, decimal montoCuota, DateTime fechapago, decimal idUsuario, decimal nroFila, decimal? motivoRechazo)
        {
            return Execute("PR_REGISTRA_REG_ARCHIVO_BANCO")
                .AddParam(idArchivoBanco)
                .AddParam(nroFormulario)
                .AddParam(nroCuota)
                .AddParam(montoCuota)
                .AddParam(fechapago)
                .AddParam(nroFila)
                .AddParam(motivoRechazo)
                .AddParam(idUsuario)
                .ToMessageResult().Mensaje;
        }

        public void FinalizadoProcesoArchivoRecupero(decimal idCabecera, decimal idUsuario)
        {
            Execute("PR_REGISTRA_FIN_ARCH_BANCO")
                .AddParam(idCabecera)
                .AddParam(idUsuario)
                .ToSpResult();
        }

        public Resultado<BandejaResultadoBancoResultado> ObtenerBandejaResultadoBanco(BandejaRecuperoConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_BANDEJA_ARCHIVO_RESULTADO")
                .AddParam(consulta.FechaDesde)
                .AddParam(consulta.FechaHasta)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<BandejaResultadoBancoResultado>();

            return CrearResultado(consulta, elementos);
        }

        public decimal RegistrarCabeceraResultadoBanco(decimal importe, string idBanco, string periodo, string formaPago, string tipoPago, decimal? motivoRechazo)
        {
            var resultado = Execute("PR_REGISTRA_ARCHIVO_RESULTADO")
                .AddParam(importe)
                .AddParam(periodo)
                .AddParam(formaPago)
                .AddParam(tipoPago)
                .AddParam(idBanco)
                .AddParam(motivoRechazo)
                .ToSpResult();
            return resultado.Id.Valor;
        }

        public string RegistrarDetalleResultadoBanco(decimal idCabecera, string idBanco, string agencia, decimal importe, string nroDocumento, DateTime fechaPago, string periodo, decimal nroFila, decimal idUsuario, decimal? motivoRechazo)
        {
            var resultado = Execute("PR_REGISTRA_DET_ARCH_RESULTADO")
                .AddParam(idCabecera)
                .AddParam(fechaPago)
                .AddParam(periodo)
                .AddParam(agencia)
                .AddParam(importe)
                .AddParam(idBanco)
                .AddParam(nroDocumento)
                .AddParam(idUsuario)
                .AddParam(motivoRechazo)
                .AddParam(nroFila)
                .ToMessageResult();
            return resultado.Mensaje;
        }

        public void FinalizadoProcesoArchivoResultadoBanco(decimal idCabecera, decimal idUsuario)
        {
            Execute("PR_REGISTRA_FIN_ARCH_RESULTADO")
                .AddParam(idCabecera)
                .AddParam(idUsuario)
                .ToSpResult();
        }

        public Resultado<VerInconsistenciaArchivosResultado> ConsultarInconsistenciaArchivoRecupero(VerArchivoInconsistenciaConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_OBTENER_INCONS_RECUPERO")
                .AddParam(consulta.IdCabecera)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<VerInconsistenciaArchivosResultado>();

            return CrearResultado(consulta, elementos);
        }

        public Resultado<VerInconsistenciaArchivosResultado> ConsultarInconsistenciaArchivoResultadoBanco(VerArchivoInconsistenciaConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_OBTENER_INCONS_RESULTADO")
                .AddParam(consulta.IdCabecera)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<VerInconsistenciaArchivosResultado>();

            return CrearResultado(consulta, elementos);
        }

        public void RegistraRechazoCabeceraResultado(decimal idCabecera, decimal idMotivoRechazo)
        {
            Execute("PR_REGISTRA_RECHAZO_RESULTADO")
                .AddParam(idCabecera)
                .AddParam(idMotivoRechazo)
                .ToSpResult();
        }

        public IList<ComboEntidadesRecuperoResultado> ConsultarComboEntidadesRecupero()
        {
            return Execute("PR_OBTENER_TIPOS_ENT_PAGO")
                .ToListResult<ComboEntidadesRecuperoResultado>();
        }

        public ValidacionSolicitanteResultado ExisteFormularioParaSolicitante(string nroDocumento)
        {
            return Execute("PR_VALIDAR_SOLICITANTE_DOC")
                .AddParam(nroDocumento)
                .ToUniqueResult<ValidacionSolicitanteResultado>();
        }

        public IList<Convenio> ObtenerConveniosPago()
        {
            return Execute("PR_OBTENER_CONVENIOS")
                .ToListResult<Convenio>();
        }

        public bool ValidarPlanPagoGenerado(decimal nroFormulario)
        {
            return Execute("PR_VAL_FORM_TIENE_PLAN")
                .AddParam(nroFormulario)
                .ToEscalarResult<bool>();
        }

        public FormularioEstadoNumero ValidarFormulario(decimal nroFormulario)
        {
            return Execute("PR_OBTENER_FORMULARIO_X_NUM")
                .AddParam(nroFormulario)
                .ToUniqueResult<FormularioEstadoNumero>();
        }

    }
}
