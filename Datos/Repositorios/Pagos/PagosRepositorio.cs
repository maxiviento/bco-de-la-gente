using System;
using System.Collections.Generic;
using System.Linq;
using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Datos;
using NHibernate;
using NHibernate.Mapping;
using Pagos.Aplicacion.Consultas.Consultas;
using Pagos.Aplicacion.Consultas.Resultados;
using Pagos.Dominio.IRepositorio;
using Pagos.Dominio.Modelo;

namespace Datos.Repositorios.Pagos
{
    public class PagosRepositorio : NhRepositorio<Pago>, IPagosRepositorio
    {
        public PagosRepositorio(ISession sesion) : base(sesion)
        {

        }

        public Resultado<BandejaPagosResultado> ObtenerPagosBandeja(BandejaPagosConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            string lineas = "";
            if (consulta.IdsLineas != null)
            {
                lineas = string.Join(",", consulta.IdsLineas);
            }
            
            string departamentos = string.Join(",", consulta.DepartamentoIds);
            string localidades = string.Join(",", consulta.LocalidadIds);

            //En caso de estar consultando por dni o por cuil no tiene en cuenta las fechas
            consulta.RevisarInclusionDeFechas();

            var elementos = Execute("PR_BANDEJA_PAGOS")
                .AddParam(consulta.FechaInicioTramite)
                .AddParam(consulta.FechaFinTramite)
                .AddParam(localidades)
                .AddParam(departamentos)
                .AddParam(lineas)
                .AddParam(consulta.IdOrigen == 0 ? -1 : consulta.IdOrigen)
                .AddParam(consulta.IdLugarOrigen == 0 ? -1 : consulta.IdLugarOrigen)
                .AddParam(consulta.NroPrestamoChecklist == 0 ? -1 : consulta.NroPrestamoChecklist)
                .AddParam(consulta.NroFormulario == 0 ? -1 : consulta.NroFormulario)
                .AddParam(consulta.TipoPersona)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Apellido)
                .AddParam(consulta.Nombre)
                .AddParam(consulta.Dni)
                .AddParam(consulta.OrderByDes)
                .AddParam(consulta.ColumnaOrderBy)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<BandejaPagosResultado>();

            return CrearResultado(consulta, elementos);
        }
        public Resultado<BandejaPagosResultado> ObtenerPagosBandejaCompleta(BandejaPagosConsulta consulta)
        {
            var lineas = "";
            if (consulta.IdsLineas != null)
            {
                lineas = string.Join(",", consulta.IdsLineas);
            }
            var departamentos = string.Join(",", consulta.DepartamentoIds);
            var localidades = string.Join(",", consulta.LocalidadIds);


            consulta.RevisarInclusionDeFechas();

            var elementos = Execute("PR_BANDEJA_PAGOS")
                .AddParam(consulta.FechaInicioTramite)
                .AddParam(consulta.FechaFinTramite)
                .AddParam(localidades)
                .AddParam(departamentos)
                .AddParam(lineas)
                .AddParam(consulta.IdOrigen == 0 ? -1 : consulta.IdOrigen)
                .AddParam(consulta.IdLugarOrigen == 0 ? -1 : consulta.IdLugarOrigen)
                .AddParam(consulta.NroPrestamoChecklist == 0 ? -1 : consulta.NroPrestamoChecklist)
                .AddParam(consulta.NroFormulario == 0 ? -1 : consulta.NroFormulario)
                .AddParam(consulta.TipoPersona)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Apellido)
                .AddParam(consulta.Nombre)
                .AddParam(consulta.Dni)
                .AddParam(consulta.OrderByDes)
                .AddParam(consulta.ColumnaOrderBy)
                .AddParam(-1)
                .AddParam(-1)
                .ToListResult<BandejaPagosResultado>();

            consulta.TamañoPagina = elementos.Count + 1;
            return CrearResultado(consulta, elementos);
        }

        public IEnumerable<BandejaPagosResultado> ObtenerPagosBandejaSinPaginacion(BandejaPagosConsulta consulta)
        {
            string lineas = string.Join(",", consulta.IdsLineas);
            var departamentos = string.Join(",", consulta.DepartamentoIds);
            var localidades = string.Join(",", consulta.LocalidadIds);


            //En caso de estar consultando por dni o por cuil no tiene en cuenta las fechas
            consulta.RevisarInclusionDeFechas();

            return Execute("PR_BANDEJA_PAGOS")
                .AddParam(consulta.FechaInicioTramite)
                .AddParam(consulta.FechaFinTramite)
                .AddParam(localidades)
                .AddParam(departamentos)
                .AddParam(lineas)
                .AddParam(consulta.IdOrigen == 0 ? -1 : consulta.IdOrigen)
                .AddParam(consulta.IdLugarOrigen == 0 ? -1 : consulta.IdLugarOrigen)
                .AddParam(consulta.NroPrestamoChecklist == 0 ? -1 : consulta.NroPrestamoChecklist)
                .AddParam(consulta.NroFormulario == 0 ? -1 : consulta.NroFormulario)
                .AddParam(consulta.TipoPersona)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Apellido)
                .AddParam(consulta.Nombre)
                .AddParam(consulta.Dni)
                .AddParam(consulta.OrderByDes)
                .AddParam(consulta.ColumnaOrderBy)
                .AddParam(-1)
                .AddParam(-1)
                .ToListResult<BandejaPagosResultado>();
        }

        public IEnumerable<Convenio> ObtenerConvenios()
        {
            return Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_CONVENIOS")
                .ToListResult<Convenio>();
        }

        public IEnumerable<MontoDisponibleSimularLoteResultado> ConsultarMontosDisponibles(decimal montoLote)
        {
            return Execute("PR_OBTENER_MONTO_SIN_USAR")
                .AddParam(montoLote)
                .ToListResult<MontoDisponibleSimularLoteResultado>();
        }

        public string ConfirmarLote(IList<int> prestamos, string nombreLote, int nroMontoDisponible, decimal monto, decimal comision, decimal iva, decimal idUser, int convenioId, decimal mesesGracia, decimal idTipoLote)
        {
            var prestamosString = prestamos.Aggregate("", (current, prestamo) => current + (prestamo.ToString() + ','));

            var spResult = Execute("PR_REGISTRA_LOTE")
                .AddParam(prestamosString.TrimEnd(','))
                .AddParam(nombreLote)
                .AddParam(nroMontoDisponible)
                .AddParam(monto)
                .AddParam(comision)
                .AddParam(iva)
                .AddParam(idUser)
                .AddParam(mesesGracia)
                .AddParam(idTipoLote)
                .ToSpResult();

            if (spResult.Mensaje == "OK")
            {
                var registroConvenio = RegistrarConvenioLote(spResult.Id.Valor, convenioId);
                if (registroConvenio == "OK")
                {
                    return spResult.Mensaje;
                }
            }
            throw new ErrorTecnicoException(spResult.Error);
        }

        public string AgregarPrestamoLote(decimal idLote, string idsPrestamo, decimal idMontoDisponible,decimal monto, decimal usuario)
        {
          
            var spResult = Execute("PR_REGISTRA_ADHESIONES_LOTE")
                .AddParam(idLote)
                .AddParam(idMontoDisponible)
                .AddParam(monto)
                .AddParam(idsPrestamo)
                .AddParam(usuario)
  
                .ToSpResult();

            if (spResult.Mensaje == "OK")
            {
                return spResult.Mensaje;
            }
            throw new ErrorTecnicoException(spResult.Error);
        }

        public Resultado<BandejaAdendaResultado> ObtenerBandejaAdenda(BandejaAdendaConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            string lineas = "";
            if (consulta.IdsLineas != null)
            {
                lineas = string.Join(",", consulta.IdsLineas);
            }

            string departamentos = string.Join(",", consulta.DepartamentoIds);
            string localidades = string.Join(",", consulta.LocalidadIds);

            //En caso de estar consultando por dni o por cuil no tiene en cuenta las fechas
            consulta.RevisarInclusionDeFechas();

            var elementos = Execute("PR_BANDEJA_ADENDA_LOTE")
                .AddParam(consulta.idLote)
                .AddParam(consulta.nroDetalle)
                .AddParam(localidades)
                .AddParam(departamentos)
                .AddParam(lineas)
                .AddParam(consulta.IdOrigen == 0 ? -1 : consulta.IdOrigen)
                .AddParam(consulta.NroPrestamoChecklist == 0 ? -1 : consulta.NroPrestamoChecklist)
                .AddParam(consulta.NroFormulario == 0 ? -1 : consulta.NroFormulario)
                .AddParam(consulta.TipoPersona)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Apellido)
                .AddParam(consulta.Nombre)
                .AddParam(consulta.Dni)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<BandejaAdendaResultado>();

            return CrearResultado(consulta, elementos);
        }

        public IList<BandejaAdendaResultado> ObtenerPrestamosCompletosParaAdenda(BandejaAdendaConsulta consulta)
        {
            string lineas = "";
            if (consulta.IdsLineas != null)
            {
                lineas = string.Join(",", consulta.IdsLineas);
            }

            string departamentos = string.Join(",", consulta.DepartamentoIds);
            string localidades = string.Join(",", consulta.LocalidadIds);

            //En caso de estar consultando por dni o por cuil no tiene en cuenta las fechas
            consulta.RevisarInclusionDeFechas();

            return Execute("PR_BANDEJA_ADENDA_LOTE")
                .AddParam(consulta.idLote)
                .AddParam(consulta.nroDetalle)
                .AddParam(localidades)
                .AddParam(departamentos)
                .AddParam(lineas)
                .AddParam(consulta.IdOrigen == 0 ? -1 : consulta.IdOrigen)
                .AddParam(consulta.NroPrestamoChecklist == 0 ? -1 : consulta.NroPrestamoChecklist)
                .AddParam(consulta.NroFormulario == 0 ? -1 : consulta.NroFormulario)
                .AddParam(consulta.TipoPersona)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Apellido)
                .AddParam(consulta.Nombre)
                .AddParam(consulta.Dni)
                .AddParam(-1)
                .AddParam(-1)
                .ToListResult<BandejaAdendaResultado>();
        }

        public Resultado<FormulariosSeleccionadosAdendaResultado> ObtenerFormulariosParaAdenda(
            FormulariosSeleccionadosAdendaConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;


            var elementos = Execute("PR_BANDEJA_ADENDA_LOTE_DET")
                .AddParam(consulta.nroDetalle)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<FormulariosSeleccionadosAdendaResultado>();

          return CrearResultado(consulta, elementos);
        }

        public decimal AgregarPrestamoAdenda(DetallesAdenda detalle, string idUser)
        {
            var result = Execute("PR_REGISTRA_DETALLE_LOTE")
                .AddParam(detalle.NroDetalle)
                .AddParam(detalle.IdLote)
                .AddParam(detalle.NroPrestamoChecklist)
                .AddParam(idUser)
                .ToUniqueResult<ModificacionAdendaResultado>();

            if (result.NroDetalle > 0)
            {
                return result.NroDetalle;
            }
            throw new ErrorTecnicoException("No se pudo agregar el detalle");
        }

        public decimal QuitarPrestamoAdenda(DetallesAdenda detalle)
        {
            var result = Execute("PR_ELIMINA_DETALLE_LOTE")
                .AddParam(detalle.NroDetalle)
                .AddParam(detalle.IdLote)
                .AddParam(detalle.NroPrestamoChecklist)
                .ToUniqueResult<ModificacionAdendaResultado>();

            if (result.Mensaje == "OK")
            {
                return detalle.NroDetalle;
            }
            throw new ErrorTecnicoException("No se pudo eliminar el detalle");
        }

        public string ConfirmarLoteAdenda(int idLoteSuaf, string nombreLote, int nroMontoDisponible, decimal monto, decimal comision, decimal iva, decimal idUser, int convenioId, decimal mesesGracia, decimal idTipoLote)
        {

            var spResult = Execute("PR_REGISTRA_LOTE_ADENDA")
                .AddParam(idLoteSuaf)
                .AddParam(nombreLote)
                .AddParam(nroMontoDisponible)
                .AddParam(monto)
                .AddParam(comision)
                .AddParam(iva)
                .AddParam(idUser)
                .AddParam(mesesGracia)
                .AddParam(idTipoLote)
                .ToSpResult();

            if (spResult.Mensaje == "OK")
            {
                var registroConvenio = RegistrarConvenioLote(spResult.Id.Valor, convenioId);
                if (registroConvenio == "OK")
                {
                    return spResult.Mensaje;
                }
            }
            throw new ErrorTecnicoException(spResult.Error);
        }

        public string RegistrarConvenioLote(decimal idLote, int convenioId)
        {
            var spResult = Execute("PR_REGISTRAR_CONVENIO_LOTE")
                .AddParam(idLote)
                .AddParam(convenioId)
                .ToSpResult();

            if (spResult.Mensaje == "OK")
            {
                return spResult.Mensaje;
            }
            throw new ErrorTecnicoException(spResult.Error);
        }

        public IList<FormularioxLoteResultado> ObtenerFormulariosxLote(long idLote)
        {
            return Execute("PR_OBTENER_FORM_POR_ID_LOTE")
                .AddParam(idLote)
                .ToListResult<FormularioxLoteResultado>();
        }

        public IList<FormularioPrestamoResultado> ObtenerFormulariosPrestamo(decimal idPrestamo)
        {
            return Execute("PR_OBTENER_FORM_X_PREST_PAGO")
                .AddParam(idPrestamo)
                .ToListResult<FormularioPrestamoResultado>();
        }

        public decimal ObtenerTotalLote(decimal idLoteSuaf)
        {
            var e = Execute("PR_OBT_VALOR_POR_LOTE")
                .AddParam(idLoteSuaf)
                .ToEscalarResult<decimal?>();
            return e ?? 0;
        }

        public bool HabilitadoAdenda(decimal idLoteSuaf)
        {
            var resultado = Execute("PR_VALIDAR_LOTE_DEVEN")
                                .AddParam(idLoteSuaf)
                                .ToEscalarResult<string>() == "S";
            return resultado;
        }

        public Resultado<BandejaLotesResultado> ObtenerBandejaLotes(BandejaLotesConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_BANDEJA_LOTES")
                .AddParam(consulta.FechaInicio)
                .AddParam(consulta.FechaFin)
                .AddParam(consulta.NroLoteDesde)
                .AddParam(consulta.NroLoteHasta)
                .AddParam(consulta.IdLinea)
                .AddParam(consulta.NroPrestamo)
                .AddParam(consulta.TipoPersona)
                .AddParam(consulta.Apellido)
                .AddParam(consulta.Nombre)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Dni)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<BandejaLotesResultado>();

            return CrearResultado(consulta, elementos);
        }

        public bool ActualizarDatosCheque(decimal idFormulario, string nroCheque, DateTime? fechaVencimientoCheque, decimal usuario)
        {
            return Execute("PR_ACTUALIZA_FORM_DATOS_CHEQUE")
                .AddParam(idFormulario)
                .AddParam(nroCheque)
                .AddParam(fechaVencimientoCheque)
                .AddParam(usuario)
                .ToEscalarResult<bool>();
        }

        public Resultado<FormularioChequeGrillaResultado> ObtenerBandejaCheques(FormularioGrillaChequeConsulta consulta, decimal usuario)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_BANDEJA_FORM_CHEQUES")
                .AddParam(consulta.FechaDesde)
                .AddParam(consulta.FechaHasta)
                .AddParam(consulta.IdLinea?.Valor)
                .AddParam(consulta.IdLote?.Valor)
                .AddParam(consulta.idDepartamento?.Valor)
                .AddParam(consulta.IdLocalidad?.Valor)
                .AddParam(consulta.NumeroPrestamo)
                .AddParam(consulta.NumeroFormulario)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Dni)
                .AddParam(consulta.IdOrigen?.Valor)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<FormularioChequeGrillaResultado>();

            return CrearResultado(consulta, elementos);
        }

        public string ConsultaTotalizadorCheques(FormularioGrillaChequeConsulta consulta, decimal usuario)
        {

            var total = Execute("PR_BANDEJA_FORM_CHEQUES_TOT")
                .AddParam(consulta.FechaDesde)
                .AddParam(consulta.FechaHasta)
                .AddParam(consulta.IdLinea?.Valor)
                .AddParam(consulta.IdLote?.Valor)
                .AddParam(consulta.idDepartamento?.Valor)
                .AddParam(consulta.IdLocalidad?.Valor)
                .AddParam(consulta.NumeroPrestamo)
                .AddParam(consulta.NumeroFormulario)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Dni)
                .AddParam(consulta.IdOrigen?.Valor)
                .ToEscalarResult<string>();

            return total;
        }

        public string ObtenerTotalizadorLotes(BandejaLotesConsulta consulta)
        {
            var total = Execute("PR_BANDEJA_LOTES_TOT")
                .AddParam(consulta.FechaInicio)
                .AddParam(consulta.FechaFin)
                .AddParam(consulta.NroLoteDesde)
                .AddParam(consulta.NroLoteHasta)
                .AddParam(consulta.IdLinea)
                .AddParam(consulta.NroPrestamo)
                .AddParam(consulta.TipoPersona)
                .AddParam(consulta.Apellido)
                .AddParam(consulta.Nombre)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Dni)
                .ToEscalarResult<string>();

            return total;
        }

        public string LiberarLote(decimal idLote, decimal idUsuario)
        {
            var spResult = Execute("PR_REGISTRA_LOTE_LIBERAR")
                .AddParam(idLote)
                .AddParam(idUsuario)
                .ToSpResult();

            if (spResult.Mensaje == "OK")
            {
                return spResult.Mensaje;
            }
            throw new ErrorTecnicoException(spResult.Error);
        }

        public PermiteLiberarLoteResultado PermiteLiberarLote(decimal idLote)
        {
            var permite =Execute("PR_PERMITE_LIBERAR_LOTE")
                .AddParam(idLote)
                .ToEscalarResult<bool>();
            return new PermiteLiberarLoteResultado() {Valor = permite};
        }

        public string ValidarProvidenciaLote(decimal idLote)
        {
            return Execute("PR_VAL_FORM_PROV_X_LOTE")
                .AddParam(idLote)
                .ToEscalarResult<string>();
        }

        public DetalleLoteResultado ObtenerCabeceraDetalleLote(decimal idLote)
        {
            return Execute("PR_OBTENER_LOTE_CABECERA")
                .AddParam(idLote)
                .ToUniqueResult<DetalleLoteResultado>();
        }

        public bool ValidarLotePago(decimal idLote)
        {
            return Execute("PR_VALIDAR_LOTE_PAGO")
                .AddParam(idLote)
                .ToEscalarResult<bool>();
        }
        public Resultado<BandejaPagosResultado> ObtenerPrestamosDetalleLote(PrestamosDetalleLoteConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_OBTENER_LOTE_PRESTAMOS")
                .AddParam(consulta.IdLote)
                .AddParam(consulta.EsVer)
                .AddParam(true)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<BandejaPagosResultado>();

            return CrearResultado(consulta, elementos);
        }

        public IList<HistorialLoteResultado> ObtenerHistorialDetalleLote(decimal idLote)
        {
            return Execute("PR_OBTENER_LOTE_SEGUIMIENTO")
                .AddParam(idLote)
                .ToListResult<HistorialLoteResultado>();
        }

        public string DesagruparLote(decimal idLote, IList<int> idPrestamosDesagrupar, decimal usuario)
        {
            var pretamos = "";
            if (idPrestamosDesagrupar != null)
            {
                pretamos = idPrestamosDesagrupar.Aggregate(pretamos, (current, idPrestamo) => current + (idPrestamo + ","));
            }

            var spResult = Execute("PR_REGISTRA_LOTE_DESAGRUPAR")
                .AddParam(idLote)
                .AddParam(pretamos.TrimEnd(','))
                .AddParam(usuario)
                .ToSpResult();

            if (spResult.Mensaje == "OK")
            {
                return spResult.Mensaje;
            }
            throw new ErrorTecnicoException(spResult.Error);
        }

        public IList<FilaExcelBanco> ObtenerDatosExcelBanco(decimal idLote, decimal idUsuario)
        {
            return Execute("PR_GENERAR_SALIDA_BANCO")
                .AddParam(idLote)
                .AddParam(idUsuario)
                .ToListResult<FilaExcelBanco>();
        }

        public decimal RegistrarChequeLote(decimal idLote, decimal idUsuario)
        {
            return Execute("PR_ACTUALIZA_FORM_CHEQUE_LOTE")
                .AddParam(idLote)
                .AddParam(idUsuario)
                .ToSpResult().Id.Valor;
        }

        public Resultado<GrillaPlanPagosResultado> ConsultaFormulariosFiltrosEnLote(FiltrosFormularioConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_ARMAR_PLAN_PAGOS")
                .AddParam(consulta.IdLote)
                .AddParam(consulta.IdPrestamo)
                .AddParam(consulta.IdLocalidad)
                .AddParam(consulta.IdDepartamento)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Dni)
                .AddParam(consulta.NroPrestamo)
                .AddParam(consulta.NroFormulario)
                .AddParam(consulta.IdFormularioLinea)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<GrillaPlanPagosResultado>();

            return CrearResultado(consulta, elementos);
        }
        public IList<GrillaPlanPagosResultado> ConsultaIdsFormulariosFiltrosEnLote(FiltrosFormularioConsulta consulta)
        {
            var elementos = Execute("PR_ARMAR_PLAN_PAGOS")
                   .AddParam(consulta.IdLote)
                   .AddParam(consulta.IdPrestamo)
                   .AddParam(consulta.IdLocalidad)
                   .AddParam(consulta.IdDepartamento)
                   .AddParam(consulta.Cuil)
                   .AddParam(consulta.Dni)
                   .AddParam(consulta.NroPrestamo)
                   .AddParam(consulta.NroFormulario)
                   .AddParam(consulta.IdFormularioLinea)
                   .AddParam(-1)
                   .AddParam(-1)
                   .ToListResult<GrillaPlanPagosResultado>();

            return elementos;
        }

        public IEnumerable<PeriodoPlanPagoComboResultado> ObtenerPeriodosDePlanDePagos()
        {
            return Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_PERIODOS_PAGO")
                .ToListResult<PeriodoPlanPagoComboResultado>();
        }

        public bool ActualizarPlanPagos(string idsFormularios, int mesesDeGracia, DateTime? fechaPago, decimal idUsuario)
        {
            var spResult = Execute("PR_ARMAR_DETALLE_CUOTAS")
                .AddParam(idsFormularios)
                .AddParam(mesesDeGracia)
                .AddParam(fechaPago)
                .AddParam(idUsuario)
                .ToSpResult();

            if (spResult.Mensaje == "OK")
            {
                return true;
            }
            throw new ErrorTecnicoException(spResult.Error);
        }

        public IList<DetallePlanDePagoGrillaResultado> ObtenerDetallesPlanPagosFormulario(string idsFormularios)
        {
            return Execute("PR_MUESTRA_DETALLE_CUOTAS")
                .AddParam(idsFormularios)
                .ToListResult<DetallePlanDePagoGrillaResultado>();
        }


        public DatosNotaPago ObtenerDatosNota(decimal idLote)
        {
            return Execute("PR_OBTENER_DATOS_NOTA")
                .AddParam(idLote)
                .ToUniqueResult<DatosNotaPago>();
        }

        public Resultado<BandejaFormulariosSuafResultado> ObtenerBandejaFormulariosSuaf(BandejaFormulariosSuafConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var departamentos = string.Join(",", consulta.DepartamentoIds);
            var localidades = string.Join(",", consulta.LocalidadIds);

            var elementos = Execute("PR_BANDEJA_SUAF")
                .AddParam(consulta.FechaDesde)
                .AddParam(consulta.FechaHasta)
                .AddParam(departamentos)
                .AddParam(localidades)
                .AddParam(consulta.IdLinea ?? -1)
                .AddParam(consulta.IdOrigen == 0 ? -1 : consulta.IdOrigen)
                .AddParam(consulta.NroFormulario == 0 ? -1 : consulta.NroFormulario)
                .AddParam(consulta.NroPrestamoChecklist == 0 ? -1 : consulta.NroPrestamoChecklist)
                .AddParam(consulta.Apellido)
                .AddParam(consulta.Nombre)
                .AddParam(consulta.NroDocumento)
                .AddParam(consulta.Devengado ?? -1)
                .AddParam(consulta.IdLoteSuaf ?? -1)
                .AddParam(consulta.TipoPersona)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.EsCargaDevengado)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<BandejaFormulariosSuafResultado>();

            return CrearResultado(consulta, elementos);
        }

        public Resultado<BandejaFormulariosSuafResultado> ObtenerBandejaFormulariosSuafSeleccionarTodos(BandejaFormulariosSuafConsulta consulta)
        {
            var departamentos = string.Join(",", consulta.DepartamentoIds);
            var localidades = string.Join(",", consulta.LocalidadIds);

            var elementos = Execute("PR_BANDEJA_SUAF")
                .AddParam(consulta.FechaDesde)
                .AddParam(consulta.FechaHasta)
                .AddParam(departamentos)
                .AddParam(localidades)
                .AddParam(consulta.IdLinea ?? -1)
                .AddParam(consulta.IdOrigen == 0 ? -1 : consulta.IdOrigen)
                .AddParam(consulta.NroFormulario == 0 ? -1 : consulta.NroFormulario)
                .AddParam(consulta.NroPrestamoChecklist == 0 ? -1 : consulta.NroPrestamoChecklist)
                .AddParam(consulta.Apellido)
                .AddParam(consulta.Nombre)
                .AddParam(consulta.NroDocumento)
                .AddParam(consulta.Devengado ?? -1)
                .AddParam(consulta.IdLoteSuaf ?? -1)
                .AddParam(consulta.TipoPersona)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.EsCargaDevengado)
                .AddParam(-1)
                .AddParam(-1)
                .ToListResult<BandejaFormulariosSuafResultado>();

            consulta.TamañoPagina = elementos.Count +1;

            return CrearResultado(consulta, elementos);
        }

        public Id RegistrarLoteSuaf(IList<int> prestamos, string nroLote, decimal usuario)
        {
            var prestamosString = prestamos.Aggregate("", (current, prestamo) => current + (prestamo.ToString() + ','));

            var spResult = Execute("PR_REGISTRA_LOTE_SUAF")
                .AddParam(prestamosString)
                .AddParam(nroLote)
                .AddParam(usuario)
                .ToSpResult();

            if (spResult.Mensaje == "OK")
            {
                return spResult.Id;
            }
            throw new ErrorTecnicoException(spResult.Error);
        }

        public IList<FilaExcelSuaf> GenerarExcelSuaf(decimal nroLote)
        {
            return Execute("PR_GENERAR_SALIDA_SUAF")
                .AddParam(nroLote)
                .ToListResult<FilaExcelSuaf>();
        }

        public IList<FilaExcelActivacionMasiva> DatosExcelActivacionMasiva(decimal nroLote)
        {
            return Execute("PR_OBTENER_FORM_X_LOTE")
                .AddParam(nroLote)
                .ToListResult<FilaExcelActivacionMasiva>();
        }

        public Resultado<BandejaSuafResultado> ObtenerBandejaSuaf(BandejaSuafConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_BANDEJA_LOTES_SUAF")
                .AddParam(consulta.FechaDesde)
                .AddParam(consulta.FechaHasta)
                .AddParam(consulta.IdLote == 0 ? -1 : consulta.IdLote)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<BandejaSuafResultado>();

            return CrearResultado(consulta, elementos);
        }

        public string ObtenerTotalizadorSuaf(BandejaSuafConsulta consulta)
        {
            var total = Execute("PR_BANDEJA_LOTES_SUAF_TOT")
                .AddParam(consulta.FechaDesde)
                .AddParam(consulta.FechaHasta)
                .AddParam(consulta.IdLote == 0 ? -1 : consulta.IdLote)
                .ToEscalarResult<string>();
            return  total;
        }

        public IList<BandejaLotesResultado> ObtenerBandejaLotesCompleto(BandejaLotesConsulta consulta)
        {
            string convenios = null;
            return Execute("PR_BANDEJA_LOTES")
                .AddParam(consulta.FechaInicio)
                .AddParam(consulta.FechaFin)
                .AddParam(consulta.NroLoteDesde)
                .AddParam(consulta.NroLoteHasta)
                .AddParam(convenios)
                .AddParam(consulta.NroPrestamo)
                .AddParam(-1)
                .AddParam(-1)
                .ToListResult<BandejaLotesResultado>();
        }

        public bool ValidarLoteSuafPorNumeroFormulario(Id idFormulario, Id idLote)
        {
            var result =
                Execute("PR_VALIDA_FORMULARIO_EN_LOTE")
                    .AddParam(idFormulario)
                    .AddParam(idLote)
                    .ToEscalarResult<bool>();
            return result;
        }

        public void ActualizarEstadosPrestamos(Id idLote, Id idUsuario)
        {
            Execute("PR_VALIDAR_SUAF_LOTE")
                .AddParam(idLote)
                .AddParam(idUsuario)
                .JustExecute();
        }

        public IEnumerable<ComboLotesResultado> ObtenerLotesSuaf()
        {
            return Execute("PR_OBTENER_LOTES_SUAF_COMBO")
                .ToListResult<ComboLotesResultado>();
        }
        public IEnumerable<ComboTiposPagoResultado> ObtenerTipoPago()
        {
            return Execute("PR_OBTENER_TIPOS_PAGO")
                .AddParam(1)
                .ToListResult<ComboTiposPagoResultado>();
        }
        public bool CargaDevengado(decimal nroFormulario, string devengado, DateTime fecha, decimal usuario)
        {
                var spResult = Execute("PR_ACTUALIZA_DEVENGADO")
                    .AddParam(nroFormulario)
                    .AddParam(devengado)
                    .AddParam(fecha)
                    .AddParam(usuario)
                    .ToMessageResult();
                return spResult.Mensaje == "OK";
        }
        
        public bool BorrarDevengado(decimal nroFormulario, decimal usuario)
        {
            var spResult = Execute("PR_ELIMINA_DEVENGADO")
                .AddParam(nroFormulario)
                .AddParam(usuario)
                .ToMessageResult();
            return spResult.Mensaje == "OK";
        }

        public bool RegistrarFechaPagoFormularios(string idAgrupamiento, DateTime fechaPago, DateTime fechaFinPago, int modalidad, int elemento, decimal usuario)
        {
            var spResult = Execute("PR_REG_FECH_PAGO_AGRUPAMIENTO")
                .AddParam(fechaPago)
                .AddParam(fechaFinPago)
                .AddParam(idAgrupamiento)
                .AddParam(modalidad)
                .AddParam(elemento)
                .AddParam(usuario)
                .ToMessageResult();
            return spResult.Mensaje == "OK";
        }

        public bool RegistrarFechaPagoFormulariosAdenda(int idLoteSuaf, DateTime fechaPago, DateTime fechaFinPago, int modalidad, int elemento, decimal usuario)
        {
            var spResult = Execute("PR_REG_FECH_PAGO_AGRUP_ADENDA")
                .AddParam(fechaPago)
                .AddParam(fechaFinPago)
                .AddParam(idLoteSuaf)
                .AddParam(modalidad)
                .AddParam(elemento)
                .AddParam(usuario)
                .ToMessageResult();
            return spResult.Mensaje == "OK";
        }

        public IList<ComboLotesResultado> ObtenerModalidadesPago()
        {
            return Execute("PR_OBTENER_MOD_PAGO")
                .ToListResult<ComboLotesResultado>();
        }

        public IList<ComboLotesResultado> ObtenerElementosPago()
        {
            return Execute("PR_OBTENER_ELEM_PAGO")
                .ToListResult<ComboLotesResultado>();
        }

        public IList<Convenio> ObtenerConveniosPago()
        {
            return Execute("PR_OBTENER_CONVENIOS")
                .ToListResult<Convenio>();
        }

        public IList<BandejaPagosResultado> ObtenerPrestamos(decimal idLote)
        {
            return Execute("PR_OBTENER_LOTE_PRESTAMOS")
                .AddParam(idLote)
                .AddParam(true)
                .AddParam(true)
                .AddParam(-1)
                .AddParam(-1)
                .ToListResult<BandejaPagosResultado>();
        }

        public IList<BandejaPagosResultado> ObtenerDatosLote(decimal idLote)
        {
            return Execute("PR_OBTENER_LOTE_PRESTAMOS")
                .AddParam(idLote)
                .AddParam("S")
                .AddParam(false)
                .AddParam(-1)
                .AddParam(-1)
                .ToListResult<BandejaPagosResultado>();
        }

        public DatosFechaFormResultado ObtenerFechaInicioFormulario(int idFormulario)
        {
            return Execute("PR_OBTENER_FEC_INICIO_FORM")
                .AddParam(idFormulario)
                .ToUniqueResult<DatosFechaFormResultado>();
        }

        public bool ActualizarModalidadPago(decimal idLote, DateTime fechaPago, DateTime fechaFinPago, int modalidad, int elemento, int meses, bool actualiza, decimal usuario)
        {
            var spResult = Execute("PR_ACTUALIZAR_DATOS_COMPLEM")
                .AddParam(fechaPago)
                .AddParam(fechaFinPago)
                .AddParam(idLote)
                .AddParam(modalidad)
                .AddParam(elemento)
                .AddParam(meses)
                .AddParam(actualiza)
                .AddParam(usuario)
                .ToMessageResult();
            return spResult.Mensaje == "OK";
        }

        public IList<FormularioFiltradoResultado> ConsultaDatosFormulario(int idFormulario)
        {
            var res =  Execute("PR_BANDEJA_FORM_GENERICA")
                .AddParam(-1)
                .AddParam(-1)
                .AddParam(-1)
                .AddParam(-1)
                .AddParam("")
                .AddParam("")
                .AddParam(-1)
                .AddParam(-1)
                .AddParam(idFormulario)
                .AddParam(0)
                .AddParam(20)
                .ToListResult<FormularioFiltradoResultado>();
            return res;
        }

        public decimal ObtenerAgrupId(int idFormulario)
        {
            return Execute("PR_OBTENER_AGRUP_DEL_FORM")
                       .AddParam(idFormulario)
                       .ToEscalarResult<decimal?>() ?? 0;
        }

        public IList<AgruparFormulario> ObtenerListaForms(int idAgrupamiento)
        {
            return Execute("PR_OBTENER_FORM_AGRUPADOS")
                .AddParam(idAgrupamiento)
                .ToListResult<AgruparFormulario>();
        }
        
        public FormularioFechaPagoResultado ObtenerFormularioFechaPago(decimal idFormulario)
        {
            return Execute("PR_OBTENER_FEC_INICIO_FORM")
                .AddParam(idFormulario)
                .ToUniqueResult<FormularioFechaPagoResultado>();
        }

        public IList<FormularioFechaPagoResultado> ObtenerFechasFormularioLote(decimal idLote)
        {
            return Execute("PR_OBTENER_FEC_INICIO_FORM_LO")
                .AddParam(idLote)
                .ToListResult<FormularioFechaPagoResultado>();
        }

        public IList<ComboLotesResultado> ObtenerComboLotes(decimal? tipoLote)
        {
            return Execute("PR_OBTENER_LOTES_COMBO")
                .AddParam(tipoLote)
                .ToListResult<ComboLotesResultado>();
        }
        public IList<ReporteCuadroCreditosResultado> ObtenerExcelCuadroCreditos(InformePagosConsulta comando)
        {
            return Execute("PR_REPORTE_CREDITOS")
               .AddParam(comando.FechaDesde)
               .AddParam(comando.FechaHasta)
               .ToListResult<ReporteCuadroCreditosResultado>();

        }
        public IList<ReporteCuadroPagados> ObtenerExcelCuadroPagados(InformePagosConsulta comando, bool esHistorico)
        {
           return  Execute("PR_OBTENER_CUADRO_PAGOS")
               .AddParam(comando.FechaDesde)
               .AddParam(comando.FechaHasta)
               .AddParam(esHistorico)
               .ToListResult<ReporteCuadroPagados>();

        }
        public IList<ReporteHistoricoPagados> ObtenerExcelHistoricoPagados(InformePagosConsulta comando, bool esHistorico)
        {
          return Execute("PR_OBTENER_HIST_PAG_PROY")
               .AddParam(comando.FechaDesde)
               .AddParam(comando.FechaHasta)
               .AddParam(esHistorico)
               .ToListResult<ReporteHistoricoPagados>();

        }

        public int RegistrarProceshoBatch(string txSp, string idUsuario, int tipoProcesoBatch, int nroGrupo)
        {
            var resultado = Execute("PR_REGISTRA_PROCESO_BATCH")
                .AddParam(txSp)
                .AddParam(idUsuario)
                .AddParam(tipoProcesoBatch)
                .AddParam(nroGrupo)
                .ToUniqueResult<ResultadoRegistroBatch>();

            return resultado.NroGrupoProceso;
        }

        public bool RegistrarDetallesProcesoBatch(int nroGrupo, string listadoFormularios, string idUsuario)
        {
            var resultado = Execute("PR_REGISTRA_DETALLES_PROCESO")
                .AddParam(nroGrupo)
                .AddParam(listadoFormularios)
                .AddParam(idUsuario)
                .ToSpResult();

            return resultado.Mensaje == "OK";
        }


        public IList<ExportacionRecupero> ObtenerExportacionRecupero(InformePagosConsulta comando)
        {
            return Execute("PR_REPORTE_EXP_RECUPERO")
                .AddParam(comando.FechaDesde)
                .AddParam(comando.FechaHasta)
                .ToListResult<ExportacionRecupero>();
        }

        public IList<ExportacionPrestamo> ObtenerExportacionPrestamos(InformePagosConsulta comando)
        {
            return Execute("PR_REPORTE_PRESTAMOS")
                .AddParam(comando.FechaDesde)
                .AddParam(comando.FechaHasta)
                .ToListResult<ExportacionPrestamo>();
        }


        public DatosBasicosFormularioResultado ObtenerSolicitante(int idFormulario)
        {
            return Execute("PR_OBTENER_SOLICITANTE_X_FORM")
                .AddParam(idFormulario)
                .ToUniqueResult<DatosBasicosFormularioResultado>();
        }

        public IList<LineaAdendaResultado> ObtenerLineasAdenda(decimal nroDetalle)
        {
            return Execute("PR_OBTENER_LINEAS_ADENDA")
                .AddParam(nroDetalle)
                .ToListResult<LineaAdendaResultado>();
        }

        public bool GenerarAdenda(decimal nroDetalle, string comando, decimal idUsuario)
        {
            var res = Execute("PR_DUPLICAR_LOTE_PAGO")
                .AddParam(nroDetalle)
                .AddParam(comando)
                .AddParam(idUsuario)
                .ToSpResult();
            return res.Mensaje == "OK";
        }

        public Resultado<BandejaCambioEstadoResultado> ConsultarBandejaCambioEstado(BandejaCambioEstadoConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            Int64 nroSticker = consulta.NroSticker == null ? -1 : Convert.ToInt64(consulta.NroSticker);

            var elementos = Execute("PR_BANDEJA_CAMBIO_ESTADO")
                .AddParam(consulta.FechaDesde)
                .AddParam(consulta.FechaHasta)
                .AddParam(consulta.IdLinea)
                .AddParam(consulta.NroFormulario)
                .AddParam(consulta.NroPrestamo)
                .AddParam(consulta.IdEstadoFormulario)
                .AddParam(nroSticker)
                .AddParam(consulta.IdElementoPago)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Dni)
                .AddParam(consulta.IdDepartamento)
                .AddParam(consulta.IdLocalidad)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<BandejaCambioEstadoResultado>();

            return CrearResultado(consulta, elementos);
        }

        public string ObtenerTotalizadorCambioEstado(BandejaCambioEstadoConsulta consulta)
        {
            Int64 nroSticker = consulta.NroSticker == null ? -1 : Convert.ToInt64(consulta.NroSticker);

            var total = Execute("PR_BANDEJA_CAMBIO_ESTADO_TOT")
                .AddParam(consulta.FechaDesde)
                .AddParam(consulta.FechaHasta)
                .AddParam(consulta.IdLinea)
                .AddParam(consulta.NroFormulario)
                .AddParam(consulta.NroPrestamo)
                .AddParam(consulta.IdEstadoFormulario)
                .AddParam(nroSticker)
                .AddParam(consulta.IdElementoPago)
                .AddParam(consulta.Cuil)
                .AddParam(consulta.Dni)
                .AddParam(consulta.IdDepartamento)
                .AddParam(consulta.IdLocalidad)
                .ToEscalarResult<string>();

            return total;
        }
        public bool CambiarEstadoFormulario(decimal idFormulario, Id idUsuario)
        {
            var resultado = Execute("PR_CAMBIA_ESTADO_FORMULARIO")
                .AddParam(idFormulario)
                .AddParam(idUsuario)
                .ToSpResult();
            return resultado.Mensaje == "OK";
        }
    }
}
