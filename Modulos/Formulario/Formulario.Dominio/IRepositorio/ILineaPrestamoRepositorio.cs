using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using System.Collections.Generic;
using Configuracion.Aplicacion.Consultas;
using Configuracion.Aplicacion.Consultas.Resultados;
using Identidad.Dominio.Modelo;

namespace Formulario.Dominio.IRepositorio
{
    public interface ILineaPrestamoRepositorio : IRepositorio<LineaPrestamo>
    {
        DetalleLineaParaFormularioResultado DetalleLineaParaFormulario(int idDetalleLinea);
        IList<DetalleLineaParaFormularioResultado> ObtenerDetallesLinea();
        List<CuadranteResultado> ObtetenerCuadrantesPorLinea(decimal id);
        List<CuadranteResultado> ObtetenerCuadrantesPorDetalleLinea(decimal idDetalleLinea);
        IList<Convenio> ObtenerConvenios();
        IList<LineaPrestamo> ConsultarLineas();
        Id RegistrarLineaPrestamo(LineaPrestamo lineaPrestamo);
        Id ModificarLineaPrestamo(LineaPrestamo lineaPrestamo);
        Id RegistrarDetalleLineaPrestamo(Id idLinea, DetalleLineaPrestamo detalle);
        Id ActualizarDetalleLineaPrestamo(Id idLinea, DetalleLineaPrestamo detalle);
        string RegistrarOrdenCuadrantes(decimal idDetalleLinea, IList<OrdenCuadrante> ordenCuadrantes);
        IList<Cuadrante> ConsultarCuadrantes();
        Resultado<LineaPrestamoGrillaResultado> ConsultarLineasPorFiltro(LineaPrestamoConsulta consulta);
        Resultado<DetalleLineaGrillaResultado> ConsultarDetallePorIdLinea(DetalleLineaConsulta consulta);
        Id DarDeBajaLineaPrestamo(LineaPrestamo linea);
        Id DarDeBajaDetalleLineaPrestamo(Id idLinea, DetalleLineaPrestamo detalle);
        IList<RequisitosLineaResultado> ConsultarRequisitosPorIdLinea(Id lineaId, bool incluyeItemsCheckList);
        DetalleLineaResultado ConsultarDetallePorId(Id detalleId);
        LineaPrestamoResultado ConsultarLineaPorId(Id lineaId);
        IList<DetalleLineaGrillaResultado> ConsultarDetallePorIdLineaSinPaginar(Id lineaId);
        void AsignarRequisitosLinea(Id idLinea, string listaItems, Id idUsuario);
        IList<LineaParaComboResultado> ConsultarLineaParaCombo();
        bool ExisteListaConMismoNombre(string nombre);
        IList<ItemResultado.BandejaConfiguracionChecklist> ObtenerItemsConfiguracionChecklists(
            ConsultaConfiguracionChecklist consulta);
        string RegistrarConfiguracionChecklist(Id idLinea, Id idEtapa, IEnumerable<RequisitoPrestamo> requisitos, Usuario usuario);
        IEnumerable<LineaPrestamoGrillaResultado> ConsultarLineasParaFiltrarPorTexto();
        IList<EtapaEstadoLineaResultado> ObtenerEtapasEstadosLinea(long idLineaPrestamo, long? idPrestamo);
        string RegistrarEtapaEstadoLinea(EtapaEstadosLineas etapaEstado, Id versionChecklist, Usuario usuario);
        string EliminarEtapaEstadoLinea(Id idEtapaEstadoLinea, Usuario usuario);
        IList<LocalidadResultado> ConsultarLocalidadesLineaPorId(Id lineaId);
        IList<DetalleLineaCombo> ObtenerDetallesLineaCombo(decimal lineaId);
        IList<OngComboResultado> ObtenerOngs();
        bool RegistrarOngLinea(decimal idLineaOng, decimal idLinea, decimal idOng, decimal porcentajeRecupero, decimal porcentajePago, decimal idUsuario);
        IList<OngLineaResultado> ObtenerOngsLinea(Id idLinea);
        IList<OngComboResultado> ObtenerOngsPorNombre(string nombre);
    }
}