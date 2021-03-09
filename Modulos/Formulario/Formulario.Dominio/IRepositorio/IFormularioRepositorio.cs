using System;
using System.Collections.Generic;
using Formulario.Aplicacion.Comandos;
using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Formulario.Aplicacion.Comandos;

namespace Formulario.Dominio.IRepositorio
{
    public interface IFormularioRepositorio : IRepositorio<Modelo.Formulario>
    {
        decimal Registrar(Modelo.Formulario formulario);
        void RegistrarCursos(int idFormulario, decimal idUsuario, IList<SolicitudCurso> cursosSolicitados);
        void RegistrarDestinoFondos(int idFormulario, decimal idUsuario, OpcionDestinoFondos opcionDestinosFondos);
        void RegistrarCondicionesSolicitadas(int idFormulario, decimal idUsuario,
            CondicionesPrestamo opcionDestinosFondos);
        string ModificarCondicionesSolicitadas(int idFormulario, decimal idUsuario,
            CondicionesPrestamo opcionDestinosFondos);
        void RegistrarGarantes(int idFormulario, decimal idUsuario, List<Persona> garantes);
        decimal ActualizarDatosEmprendimiento(int idAgrupamiento, decimal idUsuario, Emprendimiento emprendimiento);
        void RegistrarDatosComunicacionEmprendimiento(Emprendimiento emprendimiento, decimal idUsuario);
        EmprendimientoResultado ObtenerDatosEmprendimiento(int idFormulario);
        Resultado<FormularioGrillaResultado> ObtenerFormulariosPorFiltros(FormularioGrillaConsulta consulta);
        IList<FormularioGrillaResultado> ObtenerFormulariosReporte(FormularioGrillaConsulta consulta);
        string ObtenerTotalizadorFormularios(FormularioGrillaConsulta consulta);
        Resultado<FormularioFiltradoResultado> ConsultaFormulariosSucursalFiltros(FiltrosFormularioConsulta consulta);
        IList<FormularioFiltradoResultado> ConsultaIdsFormulariosSucursalFiltros(FiltrosFormularioConsulta consulta);
        CondicionesSolicitadasResultado ObtenerCondicionesDePrestamoPorFormulario(int idFormulario);
        CondicionesSolicitadasResultado ObtenerCondicionesDePrestamoPorFormularioApoderado(int idFormulario);
        DatosBasicosFormularioResultado ObtenerSolicitante(int idFormulario);
        IList<DatosPersonaResultado> ObtenerGarantes(int idFormulario);
        decimal ExisteFormularioParaSolicitante(DatosPersonaConsulta consulta);
        decimal ExisteFormularioParaSolicitanteReactivacion(DatosPersonaConsulta consulta);

        bool ExisteDeudaHistorica(DatosPersonaConsulta consulta);
        decimal ExisteFormularioParaIntegranteGrupo(string idSexo, string nroDocumento, string codigoPais, int idNumero);
        void RegistrarCambioDeEstado(decimal idFormulario, decimal idEstadoDestino, decimal idAgrupamiento, decimal idUsuario);
        DatosContactoResultado ObtenerDatosContacto(DatosPersonaConsulta consulta);
        string ActualizarDatosDeContacto(Persona persona);
        decimal Rechazar(global::Formulario.Dominio.Modelo.Formulario formulario, IList<MotivoRechazo> mr);
        decimal RechazarFormularioConPrestamo(int idFormulario, bool esAsociativa, decimal idUsuario, string numeroCaja, IList<MotivoRechazo> mr);
        decimal DarDeBaja(Modelo.Formulario formulario);
        bool RegistrarMotivosRechazo(decimal idSeguimiento, MotivoRechazo motivo);
        TipoIntegranteSocio ConsultarTipoIntegrante(decimal idFormulario);
        string ActualizaSucursal(List<int> idsFormularios, string idBanco, string idSucursal, decimal idUsuario);
        Resultado<FormularioFiltradoResultado> ConsultaFormulariosFiltros(FiltrosFormularioConsulta consulta);
        IList<FormularioFiltradoResultado> ConsultaIdsFormulariosFiltros(FiltrosFormularioConsulta consulta);
        string ConsultaTotalizadorDocumentacion(FiltrosFormularioConsulta consulta);
        string ConsultaTotalizadorSucursalBancaria(FiltrosFormularioConsulta consulta);
        string RegistrarPatrimonioSolicitante(int idFormulario, decimal idUsuario, string patrimonioSolicitante);
        PatrimonioSolicitanteResultado ObtenerPatrimonioSolicitante(int idFormulario);
        void RegistrarOrganizacionEmprendimiento(int idFormulario, decimal idUsuario, Emprendimiento emprendimiento);
        IList<ConsultaDeudaFormularioResultado> ObtenerDatosDeudaFormulario(int idFormulario);
        void RegistrarMiembroEmprendimiento(Emprendimiento emprendimiento, MiembroEmprendimientoFormularioResultado miembro, decimal idUsuario);
        void ActualizarMiembroEmprendimiento(Emprendimiento emprendimiento, MiembroEmprendimientoFormularioResultado miembro, decimal idUsuario);
        void EliminarMiembroEmprendimiento(Emprendimiento emprendimiento, MiembroEmprendimientoFormularioResultado miembro, decimal idUsuario);
        IList<MiembroEmprendimientoResultado> ObtenerMiembrosEmprendimiento(Emprendimiento emp);
        IList<FormularioFiltradoResultado> ConsultaFormulariosParaDeuda(FiltrosFormularioConsulta consulta);
        IList<IngresoGrupoResultado> ObtenerIngresosGrupoFamiliar();
        decimal ObtenerIngresoTotalGrupoFamiliar(int idGrupo);
        IList<EgresoGrupoResultado> ObtenerGastosGrupoFamiliar(int idGrupo);
        IList<IngresoGrupoResultado> ObtenerIngresosGrupoFamiliarFormulario(int idFormulario);
        decimal RegistrarIngresosGrupoFamiliar(int idFormulario, string ingresos, string gastos, decimal idUsuario);
        IList<FormaPagoResultado> ObtenerFormasPagoPorFormulario(decimal idFormulario);
        EstimaClientesResultado ObtenerEstimaClientesPorFormulario(decimal idFormulario);
        IList<ItemsMercadoComerString> ObtenerItemsMercadoComercializacionPorFormulario(decimal idFormulario);
        Resultado<SituacionPersonasResultado> ObtenerSituacionPersonas(SituacionPersonasConsulta consulta);
        IList<SituacionPersonasResultadoVista> ObtenerVistaSituacionPersonas(SituacionPersonasConsulta consulta);
        Resultado<FormulariosSituacionResultado> ObtenerFormulariosSituacionPersonas(SituacionPersonasConsulta consulta);
        decimal ActualizarDatosGeneralesCuadrantePrecioVenta(decimal? precioVenta, decimal idEmprendimiento, decimal unidadesEstimadas, decimal gananciaEstimada, string producto);
        decimal RegistrarCostos(decimal? id, decimal idCuadrante, decimal? idTipoItem, decimal precioUnitario, decimal? valorMensual, string observacion);
        decimal EliminarCostos(decimal id);
        PrecioVentaResultado VerCuadrantePrecioVenta(decimal idEmprendimiento);
        IList<ItemsPrecioVentaResultado>  ObtenerCostoPrecioVentaPorFormulario(decimal idEmprendimiento);
        IList<MotivosRechazoReferenciaResultado>  ObtenerMotivosRechazoReferencia(string[] consulta);
        IList<MotivosRechazoFormularioResultado> ObtenerRechazosFormulario(decimal idFormulario);
        IList<AgruparFormulario> ObtenerFormulariosConAgrupamiento(int idAgrupamiento);
        IList<FormulariosSituacionResultado> ObtenerFormulariosCompletosSituacionPersonas(SituacionPersonasConsulta consulta);
        Id AgruparFormularios(string idsFormularios, Id idUsuario);
        decimal ObtenerAgrupamientoPorFormulario(decimal idFormulario);
        ActualizarAgrupamientoResultado ActualizarAgrupamientoFormularios(Id idAgrupamiento, string idsFormularios, Id idUsuario);
        decimal ObtenerIdAgrupamiento(int idFormulario);
        bool ValidarEstadosFormulariosAgrupados(int idAgrupamiento, int idEstado);
        decimal ObtenerAgrupamiento(Id idFormulario);
        IList<IdsResult> ObtenerIdsFormulariosAgrupamiento(int idAgrupamiento);
        IList<OngLineaResultado> ObtenerOngs(int idLinea);
        decimal ObtenerNumeroGrupo(decimal? idOng, string nombreGrupo, decimal usuario);
        void RegistrarOngParaFormulario(OngFormulario comando, decimal? idFormulario, decimal usuario);
        OngFormulario ObtenerOngDeFormulario(int idFormulario);
        BandejaCargaNumeroControlInternoResultado ObtenerFormulariosPorPrestamo(int idFormularioLinea);
        bool RegistrarSticker(decimal idFormulario, string nroSticker, Id idUsuario);
        FormularioFechaPagoResultado ObtenerFormularioFechaPago(decimal idFormulario);
        IList<Reprogramacion> ObtenerHistorialReprogramacion(int idFormulario);
        IList<SituacionDomicilioIntegranteResultado> ObtenerSituacionDomicilioIntegrante(int idFormularioLinea);
        DatosDomicilioIntegranteResultado ConsultaDatosDomicilioIntegrante(int idFormularioLinea);
        decimal RegistrarSeguimientoAuditoria(SeguimientoAuditoriaComando seguimiento);
        bool CambiarGaranteFormulario(CambiarGaranteComando comando, Id idUsuario);
        bool ExisteGrupoParaSolicitante(int idFormulario);
        bool ExisteGrupoParaPersona(DatosPersonaConsulta persona);
        ImpresionConsultaFormularioResultado ObtenerImpresionConsulta(FormularioGrillaConsulta consulta);
        NroStickerResultado ObtenerNroSuac(decimal idFormulario);
    }
}