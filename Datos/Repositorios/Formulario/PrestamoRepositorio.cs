using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Datos;
using NHibernate;
using Infraestructura.Core.Comun.Dato;
using NHibernate.Util;

namespace Datos.Repositorios.Formulario
{
    public class PrestamoRepositorio : NhRepositorio<Prestamo>, IPrestamoRepositorio
    {
        private IPrestamoRepositorio _prestamoRepositorioImplementation;

        public PrestamoRepositorio(ISession sesion) : base(sesion)
        {
        }

        public PrestamoResultado.Detallado ConsultarPorId(decimal id)
        {
            var resultado = Execute("")
                .AddParam(id)
                .ToUniqueResult<PrestamoResultado.Detallado>();
            return resultado;
        }

        public AgruparFormulario ConsultarAgrupamiento(decimal id)
        {
            var resultado = Execute("PR_OBTENER_FORM_AGRUPADOS")
                .AddParam(id)
                .ToListResult<AgruparFormulario>();
            //Devuelvo solo el primer resultado ya que todos los elementos de la lista tienen los mismos valores necesarios para las validaciones.
            return resultado.First();
        }

        public IList<PrestamoResultado.Integrante> ConsultarIntegrantesPrestamo(decimal id)
        {
            var resultado = Execute("PR_OBTENER_SOLIC_X_PRESTAMO")
                .AddParam(id)
                .ToListResult<PrestamoResultado.Integrante>();
            return resultado;
        }

        public Resultado<BandejaPrestamoResultado> ObtenerPrestamosPorFiltros(BandejaPrestamosConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_BANDEJA_PRESTAMOS")
                .AddParam(consulta.Nombre)
                .AddParam(consulta.Apellido)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.NroFormulario ?? "-1")
                .AddParam(consulta.NroPrestamo)
                .AddParam(consulta.NroSticker)
                .AddParam(consulta.IdEstadoPrestamo)
                .AddParam(consulta.IdOrigen)
                .AddParam(consulta.IdLinea)
                .AddParam(consulta.IdUsuario)
                .AddParam(consulta.FechaDesde)
                .AddParam(consulta.FechaHasta)
                .AddParam(consulta.DepartamentoIds)
                .AddParam(consulta.LocalidadIds)
                .AddParam(consulta.TipoPersona)
                .AddParam(consulta.Dni)
                .AddParam(consulta.QuiereReactivar)
                .AddParam(consulta.OrderByDes)
                .AddParam(consulta.ColumnaOrderBy)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)

                .ToListResult<BandejaPrestamoResultado>();

            return CrearResultado(consulta, elementos);
        }

        public string ObtenerTotalziadorPrestamos(BandejaPrestamosConsulta consulta)
        {
            var total = Execute("PR_BANDEJA_PRESTAMOS_TOT")
                .AddParam(consulta.Nombre)
                .AddParam(consulta.Apellido)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.NroFormulario ?? "-1")
                .AddParam(consulta.NroPrestamo)
                .AddParam(consulta.NroSticker)
                .AddParam(consulta.IdEstadoPrestamo)
                .AddParam(consulta.IdOrigen)
                .AddParam(consulta.IdLinea)
                .AddParam(consulta.IdUsuario)
                .AddParam(consulta.FechaDesde)
                .AddParam(consulta.FechaHasta)
                .AddParam(consulta.DepartamentoIds)
                .AddParam(consulta.LocalidadIds)
                .AddParam(consulta.TipoPersona)
                .AddParam(consulta.Dni)
                .AddParam(consulta.QuiereReactivar)
                .ToEscalarResult<string>();

            return total;
        }

        public Resultado<BandejaConformarPrestamoResultado> ObtenerFormulariosPorFiltros(
            BandejaConformarPrestamoConsulta consulta)
        {
            consulta.TamañoPagina++;
            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            //por defecto buscara los formularios en estado INICIADO que son los que proporcionan valor a la interfaz y al usuario
            var estadoFormulariosIds = string.IsNullOrEmpty(consulta.EstadoFormularioIds)
                ? consulta.EstadoFormularioIds
                : null;
            var elementos = Execute("PR_BANDEJA_FORM_AGRUPADOS")
                .AddParam(consulta.FechaDesde)
                .AddParam(consulta.FechaHasta)
                .AddParam(consulta.LocalidadIds)
                .AddParam(consulta.DepartamentoIds)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Dni)
                .AddParam(consulta.Nombre)
                .AddParam(consulta.Apellido)
                .AddParam(consulta.IdOrigen)
                .AddParam(estadoFormulariosIds)
                .AddParam(consulta.IdLinea)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<BandejaConformarPrestamoResultado>();

            return CrearResultado(consulta, elementos);
        }

        public decimal GenerarPrestamo(int id, Id idUsuario)
        {
            Id idPrestamo = Execute("PR_REGISTRA_PRESTAMO")
                .AddParam(id)
                .AddParam(default(string))
                .AddParam(idUsuario.Valor)
                .ToSpResult()
                .Id;
            return idPrestamo.Valor;
        }

        public FormularioCanceladoParaPrestamo ValidarFormularioCanceladoParaGarante(DatosPersonaResultado garante,
            int idLinea)
        {
            return Execute("PR_FORMULARIO_SOLICITANTE")
                .AddParam(garante.SexoId)
                .AddParam(garante.NroDocumento)
                .AddParam(garante.CodigoPais)
                .AddParam(garante.IdNumero)
                .AddParam(idLinea)
                .ToUniqueResult<FormularioCanceladoParaPrestamo>();
        }

        public IList<PrestamoResultado.Garante> ObtenerGarantesPrestamo(decimal idPrestamo)
        {
            return Execute("PR_OBTENER_GARANTES_X_PREST")
                .AddParam(idPrestamo)
                .ToListResult<PrestamoResultado.Garante>();
        }

        public IList<RequisitoResultado.Detallado> ConsultarRequisitosPrestamo(decimal id)
        {
            var resultado = Execute("PR_OBTENER_REQS_X_PRESTAMO")
                .AddParam(id)
                .ToListResult<RequisitoResultado.Detallado>();
            return resultado;
        }

        public string ActualizarRequisitosChecklist(decimal idPrestamo, DetallePrestamo detalle, long idFormularioLinea)
        {
            var spResult = Execute("PR_REGISTRA_CHECKLIST")
                .AddParam(idPrestamo)
                .AddParam(detalle.Requisito.Id)
                .AddParam(detalle.Garante ? 'S' : 'N')
                .AddParam(detalle.Solicitante ? 'S' : 'N')
                .AddParam(detalle.SolicitGarante ? 'S' : 'N')
                .AddParam(idFormularioLinea)
                .ToSpResult();

            if (spResult.Mensaje == "OK")
            {
                return spResult.Mensaje;
            }

            throw new ErrorTecnicoException(spResult.Error);
        }

        public IList<RequisitoResultado.Cargado> ConsultarRequisitosCargados(decimal id, decimal idFormularioLinea)
        {
            var resultado = Execute("PR_OBTENER_REQS_CONFORMADOS")
                .AddParam(id)
                .AddParam(idFormularioLinea)
                .ToListResult<RequisitoResultado.Cargado>();
            return resultado;
        }

        public PrestamoResultado.Datos ConsultarDatosPrestamo(decimal id)
        {
            var resultado = Execute("PR_OBTENER_DATOS_PRESTAMO")
                .AddParam(id)
                .ToUniqueResult<PrestamoResultado.Datos>();
            return resultado;
        }

        public PrestamoResultado.Datos ConsultarEstadoPrestamo(decimal id)
        {
            var resultado = Execute("PR_OBTENER_ESTADO_PRESTAMO")
                .AddParam(id)
                .ToUniqueResult<PrestamoResultado.Datos>();
            return resultado;
        }

        public bool EsSolicitanteGarante(decimal id)
        {
            var esSG = Execute("PR_VALIDAR_SOLIC_GARANTE")
                .AddParam(id)
                .ToEscalarResult<string>();
            return esSG == "S";
        }


        public string ActualizarDatosPrestamo(Prestamo prestamo)
        {
            var spResult = Execute("PR_ACTUALIZA_PRESTAMO")
                .AddParam(prestamo.Id)
                .AddParam(prestamo.TotalFolios)
                .ToSpResult();

            if (spResult.Mensaje == "OK")
            {
                return spResult.Mensaje;
            }

            throw new ErrorTecnicoException(spResult.Error);
        }

        public string ActualizarSeguimientoPrestamo(decimal idPrestamo, SeguimientoPrestamo seguimiento,
            decimal idFormularioLinea)
        {
            var spResult = Execute("PR_REGISTRA_SEGUIMIENTO_PREST")
                .AddParam(idPrestamo)
                .AddParam(seguimiento.Estado.Id.Valor)
                .AddParam(seguimiento.Usuario.Id.Valor)
                .AddParam(seguimiento.Observaciones)
                .AddParam(idFormularioLinea)
                .ToSpResult();


            if (spResult.Mensaje == "OK")
            {
                return spResult.Mensaje;
            }

            throw new ErrorTecnicoException(spResult.Error);
        }


        public Resultado<PrestamoResultado.Seguimiento> ConsultarSeguimientosPrestamo(
            SeguimientosPrestamoConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_OBTENER_SEGUIM_PRESTAMO")
                .AddParam(consulta.IdPrestamo)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<PrestamoResultado.Seguimiento>();

            return CrearResultado(consulta, elementos);
        }

        public IList<PrestamoResultado.Seguimiento> ListarSeguimientosPrestamo(
            SeguimientosPrestamoConsulta consulta)
        {
            return Execute("PR_OBTENER_SEGUIM_PRESTAMO")
                .AddParam(Int64.Parse(consulta.IdPrestamo))
                .AddParam(0)
                .AddParam(20)
                .ToListResult<PrestamoResultado.Seguimiento>();
        }

        public IList<BandejaCargaNumeroControlInternoResultado> ObtenerFormulariosPorPrestamo(int idPrestamo)
        {
            return Execute("PR_OBTENER_FORM_X_PRESTAMO")
                .AddParam(idPrestamo)
                .ToListResult<BandejaCargaNumeroControlInternoResultado>();
        }

        public bool RegistrarSticker(decimal idFormulario, string nroSticker, Id idUsuario)
        {
            var spResult = Execute("PR_REGISTRA_STICKER")
                .AddParam(idFormulario)
                .AddParam(nroSticker)
                .AddParam(idUsuario.Valor)
                .ToEscalarResult<bool>();

            return spResult;
        }

        public decimal Rechazar(Prestamo prestamo, IList<MotivoRechazo> mr)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var session = Sesion)
                using (var transaction = session.BeginTransaction())
                {
                    var resultado = Execute("PR_REGISTRA_RECHAZO_PRESTAMO")
                        .AddParam(prestamo.Id)
                        .AddParam(prestamo.ObservacionesRechazo)
                        .AddParam(prestamo.NumeroCaja)
                        .AddParam(prestamo.UsuarioModificacion.Id)
                        .ToSpResult();
                    prestamo.Id = resultado.Id;

                    foreach (MotivoRechazo motivoRechazo in mr)
                    {
                        this.RegistrarMotivosRechazo(prestamo.Id.Valor, motivoRechazo);
                    }

                    transaction.Commit();
                }

                scope.Complete();
            }
            return prestamo.Id.Valor;
        }

        public bool RegistrarMotivosRechazo(decimal idSeguimiento, MotivoRechazo motivo)
        {
            bool valido = false;
            var spResult = Execute("PR_REGISTRA_MOT_REC_PREST")
                .AddParam(idSeguimiento)
                .AddParam(motivo.Id)
                .AddParam(motivo.Observaciones)
                .ToSpResult();

            if (spResult.Mensaje == "OK")
            {
                valido = true;
            }

            return valido;
        }


        public IList<ConsultaIntegrantesPrestamoRentasResultado> ConsultarIntegrantesPrestamoRentas(Id idFormularioLinea)
        {
            return Execute("PR_REPORTE_RENTAS")
                .AddParam(idFormularioLinea)
                .ToListResult<ConsultaIntegrantesPrestamoRentasResultado>();
        }

        public IList<EstadoPrestamo> ConsultarEstadosPrestamo()
        {
            var result = Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_ESTADOS_PRESTAMO")
                .ToListResult<EstadoPrestamo>();
            return result;
        }

        public IList<EtapaEstadoLineaResultado> ObtenerEtapasEstadosLinea(long idLineaPrestamo)
        {
            return Execute("PR_OBTENER_ETAPA_ESTADO_LINEA")
                .AddParam(idLineaPrestamo)
                .ToListResult<EtapaEstadoLineaResultado>();
        }

        public PrestamoResultado.EncabezadoArchivos ObtenerEncabezadoPrestamoArchivos(long id)
        {
            var resultado = Execute("PR_OBTENER_CABECERA_ARCHIVOS")
                .AddParam(id)
                .ToUniqueResult<PrestamoResultado.EncabezadoArchivos>();

            return resultado;
        }

        public string ActualizarEtapaPrestamo(Prestamo prestamo)
        {
            var spResult = Execute("PR_ACTUALIZA_ETAPA_PREST")
                .AddParam(prestamo.Id)
                .ToSpResult();

            if (spResult.Mensaje == "OK")
            {
                return spResult.Mensaje;
            }

            throw new ErrorTecnicoException(spResult.Error);
        }

        public IList<AgruparFormulario> ObtenerFormulariosPorAgrupamiento(int idAgrupamiento)
        {
            return Execute("PR_OBTENER_FORM_AGRUPADOS")
                .AddParam(idAgrupamiento)
                .ToListResult<AgruparFormulario>();
        }

        public bool ActualizarFechaPagoFormulario(int idFormulario, DateTime fechaPago, DateTime fechaFinPago,
            Id idUsuario)
        {
            var resultado = Execute("PR_REGISTRA_REPROGRAMACION_IND")
                .AddParam(default(string))
                .AddParam(idFormulario)
                .AddParam(fechaPago)
                .AddParam(fechaFinPago)
                .AddParam(idUsuario)
                .ToSpResult();

            return resultado.Mensaje == "OK";
        }

        public bool RegistrarRechazoReactivacion(decimal idPrestamo, ICollection<MotivoRechazo> motivosRechazo,
            string numeroCaja, Id idUsuario)
        {
            var registroOk = false;

            if (motivosRechazo == null || motivosRechazo.Count == 0)
                return false;

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                using (var session = Sesion)
                using (var transaction = session.BeginTransaction())
                {
                    var resultado = Execute("PR_REGISTRA_RECH_PRESTAMO_REAC")
                        .AddParam(idPrestamo)
                        .AddParam(default(string))
                        .AddParam(numeroCaja)
                        .AddParam(idUsuario.Valor)
                        .ToSpResult();
                    var idSeguimiento = resultado.Id.Valor;

                    foreach (MotivoRechazo motivoRechazo in motivosRechazo)
                    {
                        this.RegistrarMotivosRechazo(idSeguimiento, motivoRechazo);
                    }
                    transaction.Commit();
                }
                scope.Complete();
                registroOk = true;
            }
            return registroOk;
        }

        public bool RegistrarReactivacion(decimal idFormulario, decimal idPrestamo, string observacion, Id idUsuario)
        {
            var resultado = Execute("PR_REGISTRA_REACTIVACION")
                .AddParam(idFormulario)
                .AddParam(idPrestamo)
                .AddParam(observacion)
                .AddParam(idUsuario.Valor)
                .ToSpResult();
            return resultado.Mensaje == "OK";
        }

        public FechaAprobacionResultado ObtenerFechaAprobacion(int idPrestamo)
        {
            return Execute("PR_OBTENER_FECH_APROB_PREST")
                .AddParam(idPrestamo)
                .ToUniqueResult<FechaAprobacionResultado>();
        }

        public PrestamoIdResultado ObtenerIdPrestamo(int idFormulario)
        {
            return Execute("PR_OBTENER_PREST_X_FORM")
                .AddParam(idFormulario)
                .ToUniqueResult<PrestamoIdResultado>();
        }

        public PrestamoReactivacionResultado ObtenerDatosPrestamoReactivacion(decimal idPrestamo)
        {
            return Execute("PR_OBTENER_DATOS_X_PRESTAMO")
                .AddParam(idPrestamo)
                .ToUniqueResult<PrestamoReactivacionResultado>();
        }

        public IList<MotivosRechazoPrestamoResultado> ObtenerRechazosPrestamo(decimal idPrestamo)
        {
            return Execute("PR_OBTENER_MOT_RECH_X_PREST")
                .AddParam(idPrestamo)
                .ToListResult<MotivosRechazoPrestamoResultado>();
        }

        public bool ActualizarNumeroCaja(decimal idFormularioLinea, string numeroCaja, Id idUsuario)
        {
            var resultado = Execute("PR_ACTUALIZA_NUMERO_CAJA")
                .AddParam(idFormularioLinea)
                .AddParam(numeroCaja)
                .AddParam(idUsuario.Valor)
                .ToSpResult();
            return resultado.Mensaje == "OK";
        }

        public IList<BandejaPrestamoResultado> ObtenerPrestamosReporte(BandejaPrestamosConsulta consulta)
        {
            var elementos = Execute("PR_BANDEJA_PRESTAMOS")
                .AddParam(consulta.Nombre)
                .AddParam(consulta.Apellido)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.NroFormulario ?? "-1")
                .AddParam(consulta.NroPrestamo)
                .AddParam(consulta.NroSticker)
                .AddParam(consulta.IdEstadoPrestamo)
                .AddParam(consulta.IdOrigen)
                .AddParam(consulta.IdLinea)
                .AddParam(consulta.IdUsuario)
                .AddParam(consulta.FechaDesde)
                .AddParam(consulta.FechaHasta)
                .AddParam(consulta.DepartamentoIds)
                .AddParam(consulta.LocalidadIds)
                .AddParam(consulta.TipoPersona)
                .AddParam(consulta.Dni)
                .AddParam(consulta.QuiereReactivar)
                .AddParam(consulta.OrderByDes)
                .AddParam(consulta.ColumnaOrderBy)
                .AddParam(-1)
                .AddParam(-1)

                .ToListResult<BandejaPrestamoResultado>();

            return elementos;
        }

        public InformeBandejaPrestamosConsulta ObtenerNombresComboPrestamo(BandejaPrestamosConsulta consulta)
        {
            var resultado = Execute("PR_BANDEJA_PRESTAMOS_IMP")
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(consulta.IdOrigen)
                .AddParam(default(string))
                .AddParam(consulta.IdLinea)
                .AddParam(consulta.TipoPersona)
                .AddParam(consulta.IdUsuario)

                .ToUniqueResult<InformeBandejaPrestamosConsulta>();
            return resultado;
        }

        public IList<ArchivoPrestamosConsulta> ObtenerPrestamosArchivoConsulta(string idAgrupamiento)
        {
            return Execute("PR_OBTENER_DATOS_CONF_PREST")
                .AddParam(idAgrupamiento)
                .ToListResult<ArchivoPrestamosConsulta>();
        }
    }
}