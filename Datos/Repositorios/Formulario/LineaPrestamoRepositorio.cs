using System;
using System.Collections.Generic;
using System.Linq;
using Configuracion.Aplicacion.Consultas.Resultados;
using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Datos;
using NHibernate;
using NHibernate.Util;

namespace Datos.Repositorios.Formulario
{
    public class LineaPrestamoRepositorio : NhRepositorio<LineaPrestamo>, ILineaPrestamoRepositorio
    {
        public LineaPrestamoRepositorio(ISession sesion) : base(sesion)
        {
        }

        public DetalleLineaParaFormularioResultado DetalleLineaParaFormulario(int idDetalleLinea)
        {
            var response = Execute("PR_DETALLE_LINEA_PARA_FORM")
                .AddParam(idDetalleLinea)
                .ToUniqueResult<DetalleLineaParaFormularioResultado>();
            response.ConCurso = response.ConCursoStr == "S";

            return response;
        }


        public IList<DetalleLineaParaFormularioResultado> ObtenerDetallesLinea()
        {
            return Execute("PR_DETALLE_LINEA_ACTIVAS")
                .ToListResult<DetalleLineaParaFormularioResultado>();
        }

        public IList<Convenio> ObtenerConvenios()
        {
            return Execute("PR_OBTENER_CONVENIOS")
                .ToListResult<Convenio>();
        }

        // Obtiene cuadrantes por id detalle. usado desde formulario
        public List<CuadranteResultado> ObtetenerCuadrantesPorLinea(decimal id)
        {
            return Execute("PR_OBTENER_CUADRANTE_POR_LINEA")
                .AddParam(id)
                .ToListResult<CuadranteResultado>()
                .ToList();
        }

        // Obtiene cuadrantes por id detalle linea. usado desde configuracion de formularios
        public List<CuadranteResultado> ObtetenerCuadrantesPorDetalleLinea(decimal idLinea)
        {
            return Execute("PR_OBTENER_CUADR_POR_DET_LINEA")
                .AddParam(idLinea)
                .ToListResult<CuadranteResultado>()
                .ToList();
        }

        public IList<LineaPrestamo> ConsultarLineas()
        {
            var result = Execute("PR_OBTENER_LINEAS")
                .ToListResult<LineaPrestamo>();
            return result;
        }

        public string RegistrarOrdenCuadrantes(decimal idDetalleLinea, IList<OrdenCuadrante> ordenCuadrantes)
        {
            string[] ordenesParam = { "" };
            ordenCuadrantes.ForEach(cuadrante =>
            {
                ordenesParam[0] += cuadrante.Cuadrante.Id + "," + cuadrante.TipoSalida.Id + "," + cuadrante.Orden +
                                   ",";
            });

            var spResult = Execute("PR_REGISTRA_ORDEN_CUADRANTE")
                .AddParam(idDetalleLinea)
                .AddParam(ordenesParam[0].TrimEnd(','))
                .ToSpResult();

            if (spResult.Mensaje == "OK")
            {
                return spResult.Mensaje;
            }
            throw new ErrorTecnicoException(spResult.Error);
        }

        public IList<Cuadrante> ConsultarCuadrantes()
        {
            return Execute("PR_OBTENER_CUADRANTES")
                .ToListResult<Cuadrante>();
        }

        public Resultado<LineaPrestamoGrillaResultado> ConsultarLineasPorFiltro(LineaPrestamoConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_BANDEJA_LINEAS")
                .AddParam(consulta.Nombre)
                .AddParam(consulta.ConOng)
                .AddParam(consulta.ConPrograma)
                .AddParam(consulta.IdDestinatario == 0 ? -1 : consulta.IdDestinatario)
                .AddParam(consulta.IdMotivoDestino == 0 ? -1 : consulta.IdMotivoDestino)
                .AddParam(consulta.DadosBaja)
                .AddParam(consulta.IdConvenioPago == 0 ? -1 : consulta.IdConvenioPago)
                .AddParam(consulta.IdConvenioRecupero == 0 ? -1 : consulta.IdConvenioRecupero)
                .AddParam(consulta.ConDepartamento)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<LineaPrestamoGrillaResultado>();

            return CrearResultado(consulta, elementos);
        }

        public Resultado<DetalleLineaGrillaResultado> ConsultarDetallePorIdLinea(DetalleLineaConsulta consulta)
        {
            consulta.TamañoPagina++;

            var paginaDesde = consulta.PaginaDesde == 0
                ? consulta.PaginaDesde + 1
                : consulta.PaginaDesde - consulta.NumeroPagina + 1;
            var paginaHasta = paginaDesde == 1 ? consulta.PaginaHasta : consulta.PaginaHasta - consulta.NumeroPagina;

            var elementos = Execute("PR_OBTENER_DET_LINEA_POR_ID")
                .AddParam(consulta.LineaId)
                //.AddParam(consulta.DadosBaja) //Manolo: por el diseño del CU, traigo todos los detalles y manejo en el front.
                .AddParam(true)
                .AddParam(paginaDesde)
                .AddParam(paginaHasta)
                .ToListResult<DetalleLineaGrillaResultado>();

            return CrearResultado(consulta, elementos);
        }

        public Id DarDeBajaLineaPrestamo(LineaPrestamo linea)
        {
            var resultado = Execute("PR_ABM_LINEA")
                .AddParam(linea.Id)
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(linea.MotivoBaja.Id)
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(linea.UsuarioUltimaModificacion.Id)
                .ToSpResult();
            return resultado.Id;
        }

        public IList<RequisitosLineaResultado> ConsultarRequisitosPorIdLinea(Id lineaId, bool incluyeItemsCheckList)
        {
            var resultado = Execute("PR_OBTENER_REQS_POR_LINEA")
                .AddParam(lineaId)
                .AddParam(incluyeItemsCheckList ? default(decimal?) : default(decimal))
                .ToListResult<RequisitosLineaResultado>();
            return resultado;
        }

        public DetalleLineaResultado ConsultarDetallePorId(Id detalleId)
        {
            var resultado = Execute("PR_OBTENER_DET_LINEA_POR_DET")
                .AddParam(detalleId)
                .ToUniqueResult<DetalleLineaResultado>();
            return resultado;
        }
        public IList<DetalleLineaCombo> ObtenerDetallesLineaCombo(decimal detalleId)
        {
            return Execute("PR_OBT_DET_LINEA_X_LINEA")
                .AddParam(detalleId)
                .ToListResult<DetalleLineaCombo>();
        }


        public LineaPrestamoResultado ConsultarLineaPorId(Id lineaId)
        {
            var resultados = Execute("PR_OBTENER_LINEA_POR_ID")
                .AddParam(lineaId)
                .ToListResult<LineaPrestamoResultado>()
                .ToList();

            var resultado = resultados.First();
            resultado.Id = lineaId;

            return resultado;
        }

        public IList<OngLineaResultado> ObtenerOngsLinea(Id idLinea)
        {
            return Execute("PR_OBTENER_ONGS_LINEA")
                .AddParam(idLinea.Valor)
                .ToListResult<OngLineaResultado>();
        }

        public IList<LocalidadResultado> ConsultarLocalidadesLineaPorId(Id lineaId)
        {
            var resultados = Execute("PR_OBTENER_LOCS_X_LINEA")
                .AddParam(lineaId)
                .ToListResult<LocalidadResultado>();
            return resultados;
        }

        public IList<DetalleLineaGrillaResultado> ConsultarDetallePorIdLineaSinPaginar(Id lineaId)
        {
            var resultado = Execute("PR_OBTENER_DET_LIN_POR_ID_SP")
                .AddParam(lineaId)
                .ToListResult<DetalleLineaGrillaResultado>();
            return resultado;
        }

        public void AsignarRequisitosLinea(Id idLinea, string listaItems, Id idUsuario)
        {
            Execute("PR_REGISTRA_REQS_LINEA")
                .AddParam(idLinea)
                .AddParam(listaItems)
                .AddParam(idUsuario)
                .ToSpResult();
        }

        public Id RegistrarLineaPrestamo(LineaPrestamo linea)
        {
            var result = Execute("PR_ABM_LINEA")
                .AddParam(default(Id))
                .AddParam(linea.ConOng)
                .AddParam(linea.ConCurso)
                .AddParam(linea.ConPrograma)
                .AddParam(linea.Nombre)
                .AddParam(linea.Descripcion)
                .AddParam(linea.SexoDestinatario.Id)
                .AddParam(linea.MotivoDestino.Id)
                .AddParam(linea.Objetivo)
                .AddParam(linea.Color)
                .AddParam(linea.PathLogo)
                .AddParam(linea.PathPieDePagina)
                .AddParam(default(Id))
                .AddParam(linea.Programa.Id)
                .AddParam(linea.DeptoLocalidad)
                .AddParam(linea.LocalidadIds)
                .AddParam(linea.UsuarioUltimaModificacion.Id)
                .ToSpResult();

            return result.Id;
        }

        public bool RegistrarOngLinea(decimal idLineaOng, decimal idLinea, decimal idOng, decimal porcentajeRecupero, decimal porcentajePago, decimal idUsuario)
        {
            return Execute("PR_ABM_LINEAS_ONG")
                .AddParam(idLineaOng)
                .AddParam(idLinea)
                .AddParam(idOng)
                .AddParam(porcentajeRecupero)
                .AddParam(porcentajePago)
                .AddParam(idUsuario)
                .ToSpResult().Mensaje == "OK";
        }

        public Id ModificarLineaPrestamo(LineaPrestamo linea)
        {
            var result = Execute("PR_ABM_LINEA")
                .AddParam(linea.Id)
                .AddParam(linea.ConOng)
                .AddParam(linea.ConCurso)
                .AddParam(linea.ConPrograma ? 'S' : 'N')
                .AddParam(linea.Nombre)
                .AddParam(linea.Descripcion)
                .AddParam(linea.SexoDestinatario.Id)
                .AddParam(linea.MotivoDestino.Id)
                .AddParam(linea.Objetivo)
                .AddParam(linea.Color)
                .AddParam(linea.PathLogo)
                .AddParam(linea.PathPieDePagina)
                .AddParam(default(Id))
                .AddParam(linea.Programa.Id)
                .AddParam(linea.DeptoLocalidad)
                .AddParam(linea.LocalidadIds)
                .AddParam(linea.UsuarioUltimaModificacion.Id)
                .ToSpResult();

            return result.Id;
        }

        public Id RegistrarDetalleLineaPrestamo(Id idLinea, DetalleLineaPrestamo detalle)
        {
            var result = Execute("PR_ABM_DETALLE_LINEA")
                .AddParam(default(Id))
                .AddParam(idLinea)
                .AddParam(detalle.TipoIntegranteSocio.Id)
                .AddParam(detalle.MontoTopeIntegrante ?? default(decimal))
                .AddParam(detalle.MontoPrestable)
                .AddParam(detalle.CantidadMaxIntegrante)
                .AddParam(detalle.CantidadMinIntegrante)
                .AddParam(detalle.TipoFinanciamiento.Id)
                .AddParam(detalle.TipoInteres.Id)
                .AddParam(detalle.PlazoDevolucion)
                .AddParam(detalle.ValorCuotaSolidaria)
                .AddParam(detalle.ValorCuotaSolidaria)
                .AddParam(detalle.TipoGarantia.Id)
                .AddParam(detalle.Visualizacion)
                .AddParam(default(Id))
                .AddParam(detalle.Apoderado ? 'S' : 'N')
                .AddParam(detalle.ConvenioRecupero.Id)
                .AddParam(detalle.ConvenioPago.Id)
                .AddParam(detalle.UsuarioModificacion.Id)
                .ToSpResult();

            return result.Id;
        }

        public Id ActualizarDetalleLineaPrestamo(Id idLinea, DetalleLineaPrestamo detalle)
        {
            var result = Execute("PR_ABM_DETALLE_LINEA")
                .AddParam(detalle.Id == new Id() ? default(Id) : detalle.Id)
                .AddParam(idLinea)
                .AddParam(detalle.TipoIntegranteSocio.Id)
                .AddParam(detalle.MontoTopeIntegrante ?? default(decimal))
                .AddParam(detalle.MontoPrestable)
                .AddParam(detalle.CantidadMaxIntegrante)
                .AddParam(detalle.CantidadMinIntegrante)
                .AddParam(detalle.TipoFinanciamiento.Id)
                .AddParam(detalle.TipoInteres.Id)
                .AddParam(detalle.PlazoDevolucion)
                .AddParam(detalle.ValorCuotaSolidaria)
                .AddParam(detalle.ValorCuotaSolidaria)
                .AddParam(detalle.TipoGarantia.Id)
                .AddParam(detalle.Visualizacion)
                .AddParam(default(Id))
                .AddParam(detalle.Apoderado ? 'S' : 'N')
                .AddParam(detalle.ConvenioRecupero.Id)
                .AddParam(detalle.ConvenioPago.Id)
                .AddParam(detalle.UsuarioModificacion.Id)
                .ToSpResult();

            return result.Id;
        }

        public Id DarDeBajaDetalleLineaPrestamo(Id idLinea, DetalleLineaPrestamo detalle)
        {
            var result = Execute("PR_ABM_DETALLE_LINEA")
                .AddParam(detalle.Id)
                .AddParam(idLinea)
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(detalle.MotivoDeBaja.Id)
                .AddParam(default(bool))
                .AddParam(default(string))
                .AddParam(default(string))
                .AddParam(detalle.UsuarioModificacion.Id)
                .ToSpResult();
            return result.Id;
        }

        public IList<LineaParaComboResultado> ConsultarLineaParaCombo()
        {
            var result = Execute("PR_OBTENER_LINEAS_COMBO")
                .ToListResult<LineaParaComboResultado>();
            return result;
        }

        public bool ExisteListaConMismoNombre(string nombre)
        {
            var existeLinea = Execute("PR_EXISTE_LINEA")
                .AddParam(nombre)
                .ToEscalarResult<string>();
            return existeLinea == "S";
        }

        public IList<ItemResultado.BandejaConfiguracionChecklist> ObtenerItemsConfiguracionChecklists(
            ConsultaConfiguracionChecklist consulta)
        {
            var resultado = Execute("PR_BANDEJA_ITEMS_CONFIG")
                .AddParam(consulta.IdLinea)
                .AddParam(consulta.IdEtapa)
                .ToListResult<ItemResultado.BandejaConfiguracionChecklist>();
            return resultado;
        }

        public string RegistrarConfiguracionChecklist(Id idLinea, Id idEtapa, IEnumerable<RequisitoPrestamo> requisitos,
            Usuario usuario)
        {
            string[] configuraciones = { "" };
            requisitos.ForEach(requisito =>
            {
                configuraciones[0] += requisito.Item.Id + "," + requisito.Etapa.Id + "," + requisito.Area.Id + "," +
                                      requisito.NroOrden +
                                      ";";
            });

            var spResult = Execute("PR_REGISTRA_CONFIGURACION")
                .AddParam(configuraciones[0].TrimEnd(';'))
                .AddParam(idLinea)
                .AddParam(idEtapa)
                .AddParam(usuario.Id)
                .ToSpResult();

            if (spResult.Mensaje == "OK")
            {
                return spResult.Mensaje;
            }
            throw new ErrorTecnicoException(spResult.Error);
        }

        public IEnumerable<LineaPrestamoGrillaResultado> ConsultarLineasParaFiltrarPorTexto()
        {
            return Execute("PR_BANDEJA_LINEAS")
                .AddParam(default(string))
                .AddParam('N')
                .AddParam('N')
                .AddParam(-1)
                .AddParam(-1)
                .AddParam(true)
                .AddParam(-1)
                .AddParam(-1)
                .AddParam('N')
                .AddParam(-1)
                .AddParam(-1)
                .ToListResult<LineaPrestamoGrillaResultado>();
        }

        public IList<EtapaEstadoLineaResultado> ObtenerEtapasEstadosLinea(long idLineaPrestamo, long? idPrestamo)
        {
            return Execute("PR_OBTENER_ETAPA_ESTADO_LINEA")
                .AddParam(idLineaPrestamo)
                .AddParam(idPrestamo ?? -1)
                .ToListResult<EtapaEstadoLineaResultado>();
        }

        public IList<OngComboResultado> ObtenerOngs()
        {
            return Execute("PR_OBTENER_ONG")
                .ToListResult<OngComboResultado>();
        }

        public IList<OngComboResultado> ObtenerOngsPorNombre(string nombre)
        {
            return Execute("PR_OBTENER_ONG_NOMBRE")
                .AddParam(nombre)
                .ToListResult<OngComboResultado>();
        }

        public string RegistrarEtapaEstadoLinea(EtapaEstadosLineas etapaEstado, Id versionChecklist, Usuario usuario)
        {
            var spResult = Execute("PR_ABM_ETAPA_ESTADO_LINEA")
                .AddParam(etapaEstado.Id)
                .AddParam(etapaEstado.Orden)
                .AddParam(etapaEstado.EtapaAnterior.Id)
                .AddParam(etapaEstado.EstadoAnterior.Id)
                .AddParam(etapaEstado.EtapaSiguiente.Id)
                .AddParam(etapaEstado.EstadoSiguiente.Id)
                .AddParam(etapaEstado.Linea.Id)
                .AddParam(versionChecklist)
                .AddParam(usuario.Id)
                .ToSpResult();

            if (spResult.Mensaje == "OK")
            {
                return spResult.Mensaje;
            }
            throw new ErrorTecnicoException(spResult.Error);
        }

        public string EliminarEtapaEstadoLinea(Id idEtapaEstadoLinea, Usuario usuario)
        {
            var spResult = Execute("PR_ELIMINA_ETAPA_ESTADO_LINEA")
                .AddParam(idEtapaEstadoLinea)
                .ToSpResult();

            if (spResult.Mensaje == "OK")
            {
                return spResult.Mensaje;
            }
            throw new ErrorTecnicoException(spResult.Error);
        }
    }
}