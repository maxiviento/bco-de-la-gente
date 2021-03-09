using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;
namespace Infraestructura.Core.Reportes

{
    public class SubReporte
    {
        public SubReporte() { }

        public SubReporte(Id idCuadrante, string nombre, string nombreSubReporte, string url, List<string> nombreDataSet)
        {
            IdCuadrante = idCuadrante;
            Nombre = nombre;
            Url = url;
            NombreSubReporte = nombreSubReporte;
            NombreDataSets = nombreDataSet;
        }

        public SubReporte(Id idCuadrante, string nombre, string nombreSubReporte, string url, List<string> nombreDataSet, List<object> dataSet)
        {
            IdCuadrante = idCuadrante;
            Nombre = nombre;
            Url = url;
            NombreSubReporte = nombreSubReporte;
            NombreDataSets = nombreDataSet;
            DataSets = dataSet;
        }

        public SubReporte AsignarDataSet(object dataSet)
        {
            if(DataSets == null) DataSets = new List<object>();
            DataSets.Add(dataSet);
            return this;
        }

        public string Nombre { get; protected set; }
        public string Url { get; protected set; }
        public string NombreSubReporte { get; protected set; }
        public List<string> NombreDataSets { get; protected set; }
        public List<object> DataSets { get; protected set; }
        public Id IdCuadrante { get; protected set; }

        #region Subreportes Formulario
        public static SubReporte DatosPersonales() => new SubReporte(new Id((int) Formulario.DatosPersonales), "ReporteFormulario_DatosPersonales", "SubReporteDatosPersonales", "ReporteFormulario_DatosPersonales.rdlc", new List<string>() { "DSDatosPersonales" });
        public static SubReporte Integrantes() => new SubReporte(new Id((int)Formulario.DatosPersonales), "ReporteFormulario_Integrantes", "SubReporteIntegrantes", "ReporteFormulario_Integrantes.rdlc", new List<string>() { "DSIntegrantes" });
        public static SubReporte GrupoFamiliar() => new SubReporte(new Id((int)Formulario.GrupoFamiliar), "ReporteFormulario_GrupoFamiliar", "SubReporteGrupoFamiliar", "ReporteFormulario_GrupoFamiliar.rdlc", new List<string>() { "DSGrupoFamiliar" });
        public static SubReporte DestinoFondos() => new SubReporte(new Id((int)Formulario.DestinoFondos), "ReporteFormulario_DestinoFondos", "SubReporteDestinoFondos", "ReporteFormulario_DestinoFondos.rdlc", new List<string>() { "DSDestinoFondos", "DSDestinoFondos2", "DSDestinoFondos3" });
        public static SubReporte CondicionesMicroprestamo() => new SubReporte(new Id((int)Formulario.CondicionesMicroprestamo), "ReporteFormulario_CondicionesMicroprestamo", "SubReporteCondicionesMicroprestamo", "ReporteFormulario_CondicionesMicroprestamo.rdlc", new List<string>() { "DSCondicionesMicroprestamo" });
        public static SubReporte Cursos() => new SubReporte(new Id((int)Formulario.Cursos), "ReporteFormulario_Cursos", "SubReporteCursos", "ReporteFormulario_Cursos.rdlc", new List<string>() { "DSCursosSalidaLaboral", "DSCursosCapacitacion", "DSCursosSalidaLaboral2", "DSCursosCapacitacion2", "DSNombreDescripcionOtros" });
        public static SubReporte DatosGarante() => new SubReporte(new Id((int)Formulario.DatosGarante), "ReporteFormulario_DatosGarante", "SubReporteDatosGarante", "ReporteFormulario_DatosGarante.rdlc", new List<string>() { "DSDatosGarante" });
        public static SubReporte Requisitos() => new SubReporte(new Id((int)Formulario.Requisitos), "ReporteFormulario_Requisitos", "SubReporteRequisitos", "ReporteFormulario_Requisitos.rdlc", new List<string>() { "DSRequisitosSolicitante", "DSRequisitosGarante" });
        public static SubReporte DatosEmprendimiento() => new SubReporte(new Id((int)Formulario.DatosEmprendimiento), "ReporteFormulario_DatosEmprendimiento", "SubReporteDatosEmprendimiento", "ReporteFormulario_DatosEmprendimiento.rdlc", new List<string>() { "DSDatosEmprendimiento", "DSTiposProyecto", "DSTiposInmuebles", "DSSectoresDesarrollo" });
        public static SubReporte OrganizacionEmprendimiento() => new SubReporte(new Id((int)Formulario.OrganizacionEmprendimiento), "ReporteFormulario_OrganizacionEmprendimiento", "SubReporteOrganizacionEmprendimiento", "ReporteFormulario_OrganizacionEmprendimiento.rdlc", new List<string>() { "DSOrganizacionEmprendimiento", "DSTiposOrganizaciones" });
        public static SubReporte MercadoComercializacion() => new SubReporte(new Id((int)Formulario.MercadoComercializacion), "ReporteFormulario_MercadoComercializacion", "SubReporteMercadoComercializacion", "ReporteFormulario_MercadoComercializacion.rdlc", new List<string>() { "DSItemsMercadoComercializacion", "DSFormasPago", "DSEstima"});
        public static SubReporte IngresosYGastosActuales() => new SubReporte(new Id((int)Formulario.IngresosYGastosActuales), "ReporteFormulario_IngresosYGastos", "SubReporteIngresosYGastos", "ReporteFormulario_IngresosYGastos.rdlc", new List<string>() { "DSIngresos", "DSGastos" });
        public static SubReporte InversionRealizada() => new SubReporte(new Id((int)Formulario.InversionRealizada), "ReporteFormulario_InversionRealizada", "SubReporteInversionRealizada", "ReporteFormulario_InversionRealizada.rdlc", new List<string>() { "DSInversionRealizada" });
        public static SubReporte DeudaEmprendimiento() => new SubReporte(new Id((int)Formulario.DeudaEmprendimiento), "ReporteFormulario_DeudaEmprendimiento", "SubReporteDeudaEmprendimiento", "ReporteFormulario_DeudaEmprendimiento.rdlc", new List<string>() { "DSDeudaEmprendimiento"});
        public static SubReporte NecesidadInversion() => new SubReporte(new Id((int)Formulario.NecesidadInversion), "ReporteFormulario_NecesidadesInversion", "SubReporteNecesidadInversion", "ReporteFormulario_NecesidadesInversion.rdlc", new List<string>() { "DSNecesidadInversion", "DSNecesidadInversionRealizada"});
        public static SubReporte PrecioVenta() => new SubReporte(new Id((int)Formulario.PrecioVenta), "ReporteFormulario_PrecioVenta","SubReportePrecioVenta", "ReporteFormulario_PrecioVenta.rdlc", new List<string>() { "DSPrecioVenta", "DSCostosVariables", "DSCostosFijos"});
        public static SubReporte ResultadoMensual() => new SubReporte(new Id((int)Formulario.ResultadoMensual), "ReporteFormulario_ResultadoMensual","SubReporteResultadoMensual", "ReporteFormulario_ResultadoMensual.rdlc", new List<string>() { "DSResultadoMensual", "DSCostos"});
        public static SubReporte PatrimonioSolicitante() => new SubReporte(new Id((int)Formulario.PatrimonioSolicitante), "ReporteFormulario_PatrimonioSolicitante", "SubReportePatrimonioSolicitante", "ReporteFormulario_PatrimonioSolicitante.rdlc", new List<string>() { "DSPatrimonioSolicitante"});
        public static SubReporte Ong() => new SubReporte(new Id((int)Formulario.PatrimonioSolicitante), "ReporteFormulario_Ong", "SubReporteOng", "ReporteFormulario_Ong.rdlc", new List<string>() { "DSOng" });


        #endregion

        #region Subreportes Pagos

        public static List<SubReporte> GetAllPagos()
        {
            return new List<SubReporte>
            {
                Caratula(),
                Providencia(),
                Recibo(),
                Pagare(),
            };
        }

        public static SubReporte Providencia() { return new SubReporte(new Id((int)Pagos.Providencia), "ReportePagos_Providencia", "SubReporteProvidencia", "ReportePagos_Providencia.rdlc", new List<string>() { "DSProvidencia" }); }
        public static SubReporte Recibo() { return new SubReporte(new Id((int)Pagos.Recibo), "ReportePagos_Recibo", "SubReporteRecibo", "ReportePagos_Recibo.rdlc", new List<string>() { "DSRecibo" }); }
        public static SubReporte Pagare() { return new SubReporte(new Id((int)Pagos.Pagare), "ReportePagos_Pagare", "SubReportePagare", "ReportePagos_Pagare.rdlc", new List<string>() { "DSPagare" }); }
        public static SubReporte Caratula() { return new SubReporte(new Id((int)Pagos.Caratula), "ReportePagos_Caratula", "SubReporteCaratula", "ReportePagos_Caratula.rdlc", new List<string>() { "DSCaratula" }); }
        #endregion

        public enum Formulario
        {
            DatosPersonales = 1,
            GrupoFamiliar = 2,
            DestinoFondos = 3,
            CondicionesMicroprestamo = 4,
            Cursos = 5,
            DatosGarante = 6,
            DatosEmprendimiento = 7,
            PatrimonioSolicitante = 8,
            OrganizacionEmprendimiento = 9,
            MercadoComercializacion = 10,
            InversionRealizada = 11,
            DeudaEmprendimiento = 12,
            NecesidadInversion = 13,
            PrecioVenta = 15,
            ResultadoMensual = 16,
            IngresosYGastosActuales = 17,
            Requisitos = 21,
            Integrantes = 22,
            Ong = 23
        }

        public enum Pagos
        {
            Caratula = 1,
            Recibo = 2,
            Pagare = 3,
            Providencia = 4,
            ContratoMutuo = 5
        }

    }
}
