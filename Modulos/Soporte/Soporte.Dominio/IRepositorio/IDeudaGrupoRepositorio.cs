using System;
using System.Collections.Generic;
using Formulario.Aplicacion.Consultas.Resultados;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Dominio.Modelo;

namespace Soporte.Dominio.IRepositorio
{
    public interface IDeudaGrupoRepositorio : IRepositorio<DatoHistorialDeuda>
    {
        decimal RegistrarDatoHistorial(string nroPrestamo, Id idUsuario, int idLinea, DateTime fechaConsulta, decimal idFormularioLinea);
        decimal RegistrarCabeceraHistorial(int idNumero, string sexoId, string codPais, string nroDocumento, string nroSticker, int idVin, bool esSolicitante, decimal idHistorial);
        decimal RegistrarDetalleHistorial(decimal idCabecera, int idNumero, string sexoId, string codPais, string tipoDocumento, string numeroDocumento, 
            string sexo, string fechaNacimiento, string edad, string numeroFormulario, string fechaUltimoMovimiento, 
            string prestamoBeneficio, string importe, string cantCuotas, string cantCuotasPagas, string cantCuotasImpagas, string cantCuotasVencidas, 
            string motivoBaja, long idEstado, string fechaDefuncion, string idFormularioLinea);
        Resultado<DocumentacionResultado> ObtenerTodosHistorialesDeudaGrupo(DocumentacionConsulta consulta);
        List<CabeceraHistorialDeudaGrupo> ObtenerCabeceraHistorialDeudaGrupo(decimal idHistorial);
        List<ReporteDeudaGrupoConvivienteResultado> ObtenerDetalleHistorialDeuda(decimal idHistorial);
        DatoHistorialDeuda ObtenerDatoHistorialDeudaGrupo(decimal idDocumento);
        DatoHistorialDeuda ActualizarMotivosRechazoHistorial(decimal idHistorial, string motivosRechazo);
        decimal ActualizarEstadoAlerta(decimal? idPrestamoItem, bool esAlerta);
    }
}