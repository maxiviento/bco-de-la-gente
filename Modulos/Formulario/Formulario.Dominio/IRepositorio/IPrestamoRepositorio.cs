using System;
using System.Collections.Generic;
using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;

namespace Formulario.Dominio.IRepositorio
{
    public interface IPrestamoRepositorio : IRepositorio<Prestamo>
    {
        Resultado<PrestamoResultado.Seguimiento> ConsultarSeguimientosPrestamo(SeguimientosPrestamoConsulta consulta);
        IList<PrestamoResultado.Seguimiento> ListarSeguimientosPrestamo(SeguimientosPrestamoConsulta consulta);
        PrestamoResultado.Detallado ConsultarPorId(decimal id);
        IList<PrestamoResultado.Integrante> ConsultarIntegrantesPrestamo(decimal id);
        IList<RequisitoResultado.Detallado> ConsultarRequisitosPrestamo(decimal id);
        string ActualizarRequisitosChecklist(decimal idPrestamo, DetallePrestamo detalle, long idFormularioLinea);
        IList<RequisitoResultado.Cargado> ConsultarRequisitosCargados(decimal id, decimal idFormularioLinea);
        bool EsSolicitanteGarante(decimal id); 
        PrestamoResultado.Datos ConsultarDatosPrestamo(decimal id);
        PrestamoResultado.Datos ConsultarEstadoPrestamo(decimal id);
        string ActualizarDatosPrestamo(Prestamo prestamo);
        string ActualizarSeguimientoPrestamo(decimal idPrestamo, SeguimientoPrestamo seguimiento, decimal idFormularioLinea); 
        Resultado<BandejaPrestamoResultado> ObtenerPrestamosPorFiltros(BandejaPrestamosConsulta consulta);
        IList<BandejaPrestamoResultado> ObtenerPrestamosReporte(BandejaPrestamosConsulta consulta);
        string ObtenerTotalziadorPrestamos(BandejaPrestamosConsulta consulta);
        Resultado<BandejaConformarPrestamoResultado> ObtenerFormulariosPorFiltros(BandejaConformarPrestamoConsulta consulta);
        AgruparFormulario ConsultarAgrupamiento(decimal id);
        decimal GenerarPrestamo(int id, Id idUsuario);
        FormularioCanceladoParaPrestamo ValidarFormularioCanceladoParaGarante(DatosPersonaResultado garante, int idLinea);
        decimal Rechazar(Prestamo prestamo, IList<MotivoRechazo> mr);
        bool RegistrarMotivosRechazo(decimal idSeguimiento, MotivoRechazo motivo);
        IList<ConsultaIntegrantesPrestamoRentasResultado> ConsultarIntegrantesPrestamoRentas(Id idPrestamo);
        IList<EstadoPrestamo> ConsultarEstadosPrestamo();
        IList<EtapaEstadoLineaResultado> ObtenerEtapasEstadosLinea(long idLineaPrestamo);
        PrestamoResultado.EncabezadoArchivos ObtenerEncabezadoPrestamoArchivos(long idPrestamo);
        string ActualizarEtapaPrestamo(Prestamo prestamo);
        bool ActualizarFechaPagoFormulario(int idFormulario, DateTime fechaPago, DateTime fechaFinPago, Id idUsuario);
        IList<AgruparFormulario> ObtenerFormulariosPorAgrupamiento(int idAgrupamiento);
        FechaAprobacionResultado ObtenerFechaAprobacion(int idPrestamo);
        PrestamoIdResultado ObtenerIdPrestamo(int idFormulario);
        bool RegistrarRechazoReactivacion(decimal idPrestamo, ICollection<MotivoRechazo> motivosRechazo, string numeroCaja, Id idUsuario);
        bool RegistrarReactivacion(decimal idFormulario, decimal idPrestamo, string observacion, Id idUsuario);
        PrestamoReactivacionResultado ObtenerDatosPrestamoReactivacion(decimal idPrestamo);
        IList<MotivosRechazoPrestamoResultado> ObtenerRechazosPrestamo(decimal idPrestamo);
        bool ActualizarNumeroCaja(decimal idFormularioLinea, string numeroCaja, Id idUsuario);
        IList<PrestamoResultado.Garante> ObtenerGarantesPrestamo(decimal idPrestamo);
        InformeBandejaPrestamosConsulta ObtenerNombresComboPrestamo(BandejaPrestamosConsulta consulta);
        IList<ArchivoPrestamosConsulta> ObtenerPrestamosArchivoConsulta(string idAgrupamiento);
    }
}