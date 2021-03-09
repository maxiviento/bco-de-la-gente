using System;
using System.Collections.Generic;
using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Infraestructura.Core.Comun.Presentacion;
using Pagos.Aplicacion.Consultas.Consultas;
using Pagos.Aplicacion.Consultas.Resultados;

namespace Pagos.Dominio.IRepositorio
{
    public interface IRecuperoRepositorio
    {
        #region Recupero
        Resultado<BandejaRecuperoResultado> ObtenerBandejaArchivoRecupero(BandejaRecuperoConsulta consulta);
        bool ValidarNombreArchivoRecupero(string nombreArchivo);
        decimal RegistrarCabeceraRecupero(string nombreArchivo, decimal idUsuario, decimal idTipoEntidad, int convenio, DateTime fechaRecupero);
        int ActualizarCabeceraRecupero(decimal idCabecera, decimal cantTotal, decimal cantCuotasProc, decimal cantCuotasEspec, decimal cantCuotasIncons, decimal montoRecuperado, decimal montoRechazado);
        string RegistrarDetalleArchivoRecupero(decimal idCabecera, decimal nroFormulario, decimal nroCuota, decimal montoCuota, DateTime fechapago, decimal idUsuario, decimal nroLinea, decimal? motivoRechazo = null);
        void FinalizadoProcesoArchivoRecupero(decimal idCabecera, decimal idUsuario);
        Resultado<VerInconsistenciaArchivosResultado> ConsultarInconsistenciaArchivoRecupero(VerArchivoInconsistenciaConsulta consulta);
        #endregion  

        #region Resultado
        Resultado<BandejaResultadoBancoResultado> ObtenerBandejaResultadoBanco(BandejaRecuperoConsulta consulta);
        decimal RegistrarCabeceraResultadoBanco(decimal importe,string idBanco, string periodo, string formaPago, string tipoPago, decimal? motivoRechazo = null);
        string RegistrarDetalleResultadoBanco(decimal idCabecera, string idBanco, string agencia, decimal importe, string nroDocumento, DateTime fechaPago, string periodo, decimal nroFila, decimal idUsuario, decimal? motivoRechazo = null);
        void FinalizadoProcesoArchivoResultadoBanco(decimal idCabecera, decimal idUsuario);
        Resultado<VerInconsistenciaArchivosResultado> ConsultarInconsistenciaArchivoResultadoBanco(VerArchivoInconsistenciaConsulta consulta);
        void RegistraRechazoCabeceraResultado(decimal idCabecera, decimal idMotivoRechazo);
        IList<ComboEntidadesRecuperoResultado> ConsultarComboEntidadesRecupero();
        ValidacionSolicitanteResultado ExisteFormularioParaSolicitante(string nroDocumento);
        IList<Convenio> ObtenerConveniosPago();
        bool ValidarPlanPagoGenerado(decimal nroFormulario);
        FormularioEstadoNumero ValidarFormulario(decimal nroFormulario);
        #endregion
    }
}
