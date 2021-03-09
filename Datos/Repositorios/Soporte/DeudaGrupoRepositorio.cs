using System;
using System.Collections.Generic;
using System.Globalization;
using Formulario.Aplicacion.Consultas.Resultados;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Datos;
using NHibernate;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Dominio.IRepositorio;
using Soporte.Dominio.Modelo;

namespace Datos.Repositorios.Soporte
{
    public class DeudaGrupoRepositorio : NhRepositorio<DatoSintys>, IDeudaGrupoRepositorio
    {
        public DeudaGrupoRepositorio(ISession sesion) : base(sesion)
        {
        }

        public decimal RegistrarDatoHistorial(string nroPrestamo, Id idUsuario, int idLinea, DateTime fechaConsulta, decimal idFormularioLinea)
        {
            var resultado = Execute("PR_REGISTRA_HIST_DEUDA")
                .AddParam(nroPrestamo)
                .AddParam(idUsuario)
                .AddParam(fechaConsulta)
                .AddParam(idLinea)
                .AddParam(idFormularioLinea)
                .ToSpResult();
            return resultado.Id.Valor;
        }

        public decimal RegistrarCabeceraHistorial(int idNumero, string sexoId, string codPais, string nroDocumento, string nroSticker, int idVin, bool esSolicitante, decimal idHistorial)
        {
            var resultado = Execute("PR_REGISTRA_CAB_HIST_DEUDA")
                .AddParam(idHistorial)
                //.AddParam(nombreCompleto)
                .AddParam(sexoId)
                .AddParam(nroDocumento)
                .AddParam(idNumero)
                .AddParam(codPais)
                .AddParam(nroSticker)
                .AddParam(idVin)
                //.AddParam(domicilioCompleto)
                //.AddParam(idGrupoConviviente)
                //.AddParam(localidad)
                //.AddParam(departamento)
                .AddParam(esSolicitante)
                .ToSpResult();
            return resultado.Id.Valor;
        }

        public decimal RegistrarDetalleHistorial(decimal idCabecera, int idNumero, string sexoId, string codPais, string tipoDocumento, string numeroDocumento,
            string sexo, string fechaNacimiento, string edad, string numeroFormulario, string fechaUltimoMovimiento,
            string prestamoBeneficio, string importe, string cantCuotas, string cantCuotasPagas, string cantCuotasImpagas, string cantCuotasVencidas,
            string motivoBaja, long idEstado, string fechaDefuncion, string idFormularioLinea)
        {
            var fechaNacimientoDt = ObtenerDateTime(fechaNacimiento);
            var fechaUltimoMovimientoDt = ObtenerDateTime(fechaUltimoMovimiento);
            //var fechaDefuncionDt = ObtenerDateTime(fechaDefuncion);
            long? estado = null;
            if (idEstado != 0) estado = idEstado;


            var resultado = Execute("PR_REGISTRA_DET_HIST_DEUDA")
                .AddParam(idCabecera)
                //.AddParam(nombreCompleto)
                .AddParam(sexoId)
                .AddParam(numeroDocumento)
                .AddParam(idNumero)
                .AddParam(codPais)
                //.AddParam(tipoDocumento)
                //.AddParam(numeroDocumento)
                //.AddParam(sexo)
                //.AddParam(fechaNacimientoDt)
                .AddParam(numeroFormulario)
                .AddParam(fechaUltimoMovimientoDt)
                .AddParam(prestamoBeneficio)
                .AddParam(importe)
                .AddParam(cantCuotas)
                .AddParam(cantCuotasPagas)
                .AddParam(cantCuotasImpagas)
                .AddParam(cantCuotasVencidas)
                .AddParam(motivoBaja)
                .AddParam(estado)
                .AddParam(idFormularioLinea)
                //.AddParam(fechaDefuncionDt)
                .ToSpResult();
            return resultado.Id.Valor;
        }

        private DateTime? ObtenerDateTime(string fechaString)
        {
            if (string.IsNullOrEmpty(fechaString) || fechaString.Equals(" - "))
            {
                return null;
            }

            var fechaArray = fechaString.Split('/');
            return new DateTime(int.Parse(fechaArray[2]), int.Parse(fechaArray[1]), int.Parse(fechaArray[0]));
        }

        public Resultado<DocumentacionResultado> ObtenerTodosHistorialesDeudaGrupo(DocumentacionConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_OBTENER_HIST_DEUDA")
                .AddParam(consulta.IdFormularioLinea)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<DocumentacionResultado>();
            return CrearResultado(consulta, elementos);
        }

        public List<CabeceraHistorialDeudaGrupo> ObtenerCabeceraHistorialDeudaGrupo(decimal idHistorial)
        {
            return (List<CabeceraHistorialDeudaGrupo>)
                Execute("PR_OBTENER_CABEC_HIST_DEUDA")
                    .AddParam(idHistorial)
                    .ToListResult<CabeceraHistorialDeudaGrupo>();
        }

        public List<ReporteDeudaGrupoConvivienteResultado> ObtenerDetalleHistorialDeuda(decimal idHistorial)
        {
            return (List<ReporteDeudaGrupoConvivienteResultado>)
                Execute("PR_OBTENER_DET_HIST_DEUDA")
                    .AddParam(idHistorial)
                    .ToListResult<ReporteDeudaGrupoConvivienteResultado>();
        }

        public DatoHistorialDeuda ObtenerDatoHistorialDeudaGrupo(decimal idDocumento)
        {
            return Execute("PR_OBTENER_DATO_HIST")
                    .AddParam(idDocumento)
                    .ToUniqueResult<DatoHistorialDeuda>();
        }


        public DatoHistorialDeuda ActualizarMotivosRechazoHistorial(decimal idHistorial, string motivosRechazo)
        {
            return Execute("PR_ACTUALIZA_DATO_HIST")
                .AddParam(idHistorial)
                .AddParam(motivosRechazo)
                .ToUniqueResult<DatoHistorialDeuda>();
        }

        public decimal ActualizarEstadoAlerta(decimal? idPrestamoItem, bool esAlerta)
        {
            var resultado = Execute("PR_ACTUALIZA_ESTADO_ALERTA")
                .AddParam(idPrestamoItem)
                .AddParam(esAlerta ? 'S' : 'N')
                .ToSpResult();
            return resultado.Id.Valor;
        }
    }
}