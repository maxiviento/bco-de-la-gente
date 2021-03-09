using AppComunicacion.ApiModels;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Identidad.Aplicacion.Servicios;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.CiDi.Api;
using Infraestructura.Core.Comun;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Reportes;
using SintysWS;
using SintysWS.Modelo;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Dominio.IRepositorio;
using Soporte.Dominio.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Soporte.Aplicacion.Servicios
{
    public class SintysServicio
    {
        private readonly ISesionUsuario _sesionUsuario;
        private readonly IPrestamoRepositorio _prestamoRepositorio;
        private readonly ISintysRepositorio _sintysRepositorio;
        private readonly SintysServicioWs _sintysServicioWs;
        private readonly UsuarioServicio _usuarioServicio;
        private readonly int _edadDesde = int.Parse(ParametrosSingleton.Instance.GetValue("6"));
        private readonly int _edadHasta = int.Parse(ParametrosSingleton.Instance.GetValue("5"));
        private bool usarWebService = false;

        public SintysServicio(ISesionUsuario sesionUsuario, IPrestamoRepositorio prestamoRepositorio,
            ISintysRepositorio sintysRepositorio,
            SintysServicioWs sintysServicioWs, UsuarioServicio usuarioServ)
        {
            _sesionUsuario = sesionUsuario;
            _prestamoRepositorio = prestamoRepositorio;
            _sintysServicioWs = sintysServicioWs;
            _sintysRepositorio = sintysRepositorio;
            _usuarioServicio = usuarioServ;
#if STAGING || RELEASE
            usarWebService = true;
#endif
        }

        public string GenerarReporteSintysIndividual(RespuestaAPIGrupoFamiliar grupo, string cookieValue)
        {
            var mensaje = "";

            foreach (var familiar in grupo.Grupo.Integrantes)
            {
                var personaIntegrante = ObtenerPersona(familiar.Sexo.IdSexo, familiar.NroDocumento, familiar.PaisTD.IdPais, familiar.Id_Numero, cookieValue);
                familiar.FechaNacimiento = personaIntegrante.FechaNacimiento;
            }
            var persona = ObtenerPersona(grupo.Persona.Sexo.IdSexo, grupo.Persona.NroDocumento, grupo.Persona.PaisTD.IdPais, grupo.Persona.Id_Numero, cookieValue);

            grupo.Persona = persona;

            try
            {
                var usuario = _sesionUsuario.Usuario;
                mensaje = "Buscar usuario";
                var fechaActual = DateTime.Now;

                var integrante = new ConsultaIntegrantesPrestamoRentasResultado
                {
                    ApellidoNombre = grupo?.Persona?.NombreCompleto,
                    NroDocumento = grupo?.Persona?.NroDocumento,
                    NroSticker = "",
                    NroPrestamo = "",
                    Departamento = grupo?.Persona?.DomicilioGrupoFamiliar?.Departamento?.Nombre,
                    Localidad = grupo?.Persona?.DomicilioGrupoFamiliar?.Localidad?.Nombre,
                    Domicilio = grupo?.Persona?.DomicilioGrupoFamiliar?.DireccionCompleta,
                    Edad = grupo?.Persona?.Edad()
                };
                mensaje = "Generar entidad de consulta";

                var grupoFamiliarDataSet = ConsultaGrupoFamiliarSintys(grupo, integrante, -1);
                mensaje = "Buscar datos de Sintys";


                var domicilio = grupo?.Grupo?.Domicilio?.DireccionCompleta;
                var barrio = string.IsNullOrEmpty(grupo?.Grupo?.Domicilio?.Barrio?.Nombre) ? string.Empty :
                    $" - Barrio: {grupo.Grupo.Domicilio.Barrio.Nombre}";

                var reportData = new ReporteBuilder("Sintys.rdlc")
                    .AgregarDataSource("SintysPlanoDS", grupoFamiliarDataSet)
                    .AgregarParametro("Usuario", usuario.Apellido + ", " + usuario.Nombre)
                    .AgregarParametro("Fecha", fechaActual)
                    .AgregarParametro("ApellidoNombre", grupo?.Persona?.NombreCompleto)
                    .AgregarParametro("NroDocumento", grupo?.Persona?.NroDocumento)
                    .AgregarParametro("CondicionEdad", ObtenerCondicionEdad(grupo?.Persona?.Edad()))
                    .AgregarParametro("Domicilio", !string.IsNullOrEmpty(domicilio) ? domicilio + barrio : "-")
                    .AgregarParametro("Localidad", grupo?.Grupo?.Domicilio?.Localidad?.Nombre ?? "-")
                    .AgregarParametro("Departamento", grupo?.Grupo?.Domicilio?.Departamento?.Nombre ?? "-")
                    .Generar();
                mensaje = "Generar el reporte";

                var url = ReporteBuilder.GenerarUrlReporte(reportData, "Rentas");

                mensaje = "Generar el url del reporte";
                return url;
            }
            catch (Exception e)
            {
                throw new RespuestaServiceApiExternaException($"{mensaje} en GenerarReporteSintysIndividual", e);
            }
        }

        public string GenerarReporteSintysPrestamo(decimal idFormularioLinea, string cookieValue)
        {
            var mensaje = "";

            try
            {
                var usuario = _sesionUsuario.Usuario;
                mensaje = "Buscar usuario";
                var datosSolicitantePlano = new List<SintysGrupoUnicoPlanoResultado>();
                var datosGarantePlano = new List<SintysGrupoUnicoPlanoResultado>();


                var integrantesPrestamo = _prestamoRepositorio.ConsultarIntegrantesPrestamoRentas(new Id(idFormularioLinea));
                mensaje = "Buscar integrantes del prestamo";

                decimal idCabecera = -1;

                var fechaActual = DateTime.Now;

                if (integrantesPrestamo.Count > 0)
                {
                    idCabecera =
                        _sintysRepositorio.RegistrarCabeceraDatosSintys(integrantesPrestamo[0].NroPrestamo,
                            _sesionUsuario.Usuario.Id, fechaActual, idFormularioLinea);
                }

                mensaje = "Guardar Cabecera historial";

                foreach (var integrante in integrantesPrestamo)
                {
                    var grupoFamiliar = ApiGruposFamiliares.ApiConsultaGrupos(cookieValue, integrante.Sexo,
                        integrante.NroDocumento, integrante.CodigoPais, integrante.IdNumero);
                    mensaje = "Buscar datos en grupo unico";

                    foreach (var familiar in grupoFamiliar.Grupo.Integrantes)
                    {
                        var personaIntegrante = ObtenerPersona(familiar.Sexo.IdSexo, familiar.NroDocumento, familiar.PaisTD.IdPais, familiar.Id_Numero, cookieValue);
                        familiar.FechaNacimiento = personaIntegrante.FechaNacimiento;
                        familiar.FechaDefuncion = personaIntegrante.FechaDefuncion;
                    }
                    var persona = ObtenerPersona(integrante.Sexo, integrante.NroDocumento, integrante.CodigoPais, integrante.IdNumero, cookieValue);

                    grupoFamiliar.Persona = persona;
                    integrante.Edad = persona.Edad();

                    var grupoFamiliarDataSet = ConsultaGrupoFamiliarSintys(grupoFamiliar, integrante, idCabecera);
                    mensaje = "Buscar datos de Sintys";

                    if (integrante.EsSolicitante)
                        datosSolicitantePlano.AddRange(grupoFamiliarDataSet);
                    else
                        datosGarantePlano.AddRange(grupoFamiliarDataSet);
                }

                mensaje = "Generar los data set con todos los datos de sintys de todos";

                var reportData = new ReporteBuilder("SintysPrestamo.rdlc")
                    .AgregarDataSource("SintysSolicitantePlanoDS",
                        datosSolicitantePlano.OrderBy(x => x.ApellidoNombre).ThenBy(x => x.GF_ApellidoNombre))
                    /* Se comenta el garante de sintys, no se elimina el código por posibles cambios futuros.
                     .AgregarDataSource("SintysGarantePlanoDS",
                     datosGarantePlano.OrderBy(x => x.ApellidoNombre).ThenBy(x => x.GF_ApellidoNombre)) */
                    .AgregarParametro("Usuario", usuario.Apellido + ", " + usuario.Nombre)
                    .AgregarParametro("Fecha", fechaActual)
                    .AgregarParametro("NroPrestamo", integrantesPrestamo[0].NroPrestamo)
                    .AgregarParametro("Linea", integrantesPrestamo[0].Linea)
                    .Generar();
                mensaje = "Generar el reporte";

                var url = ReporteBuilder.GenerarUrlReporte(reportData, "SintysPrestamo");

                mensaje = "Generar el url del reporte";
                return url;
            }
            catch (Exception e)
            {
                throw new RespuestaServiceApiExternaException($"{mensaje} en GenerarReporteSintysPrestamo", e);
            }
        }

        public string ObtenerReporteHistorialSintys(decimal idHistorial, string cookieValue)
        {
            var datosCabecera = _sintysRepositorio.ObtenerCabeceraHistorialSintys(idHistorial);
            var usuario = _usuarioServicio.ObtenerUsuarioPorId(new Id(datosCabecera.IdUsuario));
            var datosSolicitantePlano = new List<SintysGrupoUnicoPlanoResultado>();
            var datosGarantePlano = new List<SintysGrupoUnicoPlanoResultado>();

            var integrantesPrestamo =
                _prestamoRepositorio.ConsultarIntegrantesPrestamoRentas(new Id(datosCabecera.IdFormularioLinea));

            foreach (var integrante in integrantesPrestamo)
            {
                var grupoFamiliar = ApiGruposFamiliares.ApiConsultaGrupos(cookieValue, integrante.Sexo,
                    integrante.NroDocumento,
                    integrante.CodigoPais,
                    integrante.IdNumero);

                foreach (var familiar in grupoFamiliar.Grupo.Integrantes)
                {
                    var personaIntegrante = ObtenerPersona(familiar.Sexo.IdSexo, familiar.NroDocumento, familiar.PaisTD.IdPais, familiar.Id_Numero, cookieValue);
                    familiar.FechaNacimiento = personaIntegrante.FechaNacimiento;
                    familiar.FechaDefuncion = personaIntegrante.FechaDefuncion;
                }
                var persona = ObtenerPersona(integrante.Sexo, integrante.NroDocumento, integrante.CodigoPais, integrante.IdNumero, cookieValue);

                grupoFamiliar.Persona = persona;
                integrante.Edad = persona.Edad();

                var datosHistorial = _sintysRepositorio.ObtenerHistorialSintys(idHistorial);

                var grupoFamiliarHistorico = ObtenerHistorialIntegrante(grupoFamiliar, integrante, datosHistorial);

                if (integrante.EsSolicitante)
                    datosSolicitantePlano.AddRange(grupoFamiliarHistorico);
                else
                    datosGarantePlano.AddRange(grupoFamiliarHistorico);
            }

            var reportData = new ReporteBuilder("SintysPrestamo.rdlc")
                .AgregarDataSource("SintysSolicitantePlanoDS",
                    datosSolicitantePlano.OrderBy(x => x.ApellidoNombre).ThenBy(x => x.GF_ApellidoNombre))
            /* Se comenta el garante de sintys, no se elimina el código por posibles cambios futuros.
             .AgregarDataSource("SintysGarantePlanoDS",
                 datosGarantePlano.OrderBy(x => x.ApellidoNombre).ThenBy(x => x.GF_ApellidoNombre)) */
             .AgregarParametro("Usuario", usuario.Apellido + ", " + usuario.Nombre)
             .AgregarParametro("Fecha", datosCabecera.FechaAlta)
             .AgregarParametro("NroPrestamo", datosCabecera.NroPrestamo)
             .AgregarParametro("Linea", datosCabecera.NroLinea)
             .Generar();

            var url = ReporteBuilder.GenerarUrlReporte(reportData, "SintysPrestamo");

            return url;
        }

        /**
        Metodo que retorna un listado con todos los historiales de rentas creados para un prestamo.
        */
        public Resultado<DocumentacionResultado> ObtenerTodosHistorialesSintys(DocumentacionConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new DocumentacionConsulta { NumeroPagina = 0 };
            }

            consulta.TamañoPagina = 5;

            var resultados = _sintysRepositorio.ObtenerTodosHistorialesSintys(consulta);

            foreach (var resultado in resultados.Elementos)
            {
                resultado.NombreArchivo = $"CONTROLAR SINTyS - {resultado.FechaAlta:d}";
                resultado.Extension = "PDF";
            }

            return resultados;
        }

        private List<SintysGrupoUnicoPlanoResultado> ConsultaGrupoFamiliarSintys(RespuestaAPIGrupoFamiliar grupo,
            ConsultaIntegrantesPrestamoRentasResultado integrantePrestamo, decimal idCabecera)
        {
            var mensaje = "";
            try
            {
                var sintysGrupoFamiliarPlano = new List<SintysGrupoUnicoPlanoResultado>();
                if (grupo.Grupo == null)
                {
                    var persona = new IntegranteGrupo
                    {
                        Nombre = grupo.Persona?.Nombre,
                        Apellido = grupo.Persona?.Apellido,
                        NroDocumento = grupo.Persona?.NroDocumento,
                        Sexo = grupo.Persona?.Sexo,
                        PaisTD = grupo.Persona?.PaisTD,
                        Id_Numero = grupo.Persona?.Id_Numero ?? 0
                    };
                    mensaje = "Generar entidad de consulta";
                    sintysGrupoFamiliarPlano.AddRange(ObtenerDatosSintys(persona, integrantePrestamo, idCabecera));
                    mensaje = "Generar listado de datos sintys del integrante solo";
                    return sintysGrupoFamiliarPlano;
                }

                grupo.Grupo.Integrantes = filterGrupoFamiliarSintys(grupo.Grupo.Integrantes);

                foreach (var persona in grupo.Grupo.Integrantes)
                {
                    sintysGrupoFamiliarPlano.AddRange(ObtenerDatosSintys(persona, integrantePrestamo, idCabecera));
                }

                mensaje = "Generar listado de datos sintys del grupo";

                return sintysGrupoFamiliarPlano;
            }
            catch (Exception e)
            {
                throw new RespuestaServiceApiExternaException($"{mensaje} en ConsultaGrupoFamiliarSintys", e);
            }
        }

        private IList<IntegranteGrupo> filterGrupoFamiliarSintys(IList<IntegranteGrupo> grupo)
        {
            return grupo.Where(x => int.Parse(x.Edad()) >= int.Parse(ParametrosSingleton.Instance.GetValue("23"))).ToList();
        }

        private IEnumerable<SintysGrupoUnicoPlanoResultado> ObtenerDatosSintys(IntegranteGrupo integrante,
            ConsultaIntegrantesPrestamoRentasResultado integrantePrestamo, decimal idCabecera)
        {
            var mensaje = "";
            try
            {
                var persona = ObtenerDatosPersonaSintys(integrante.NroDocumento, integrante.Sexo.IdSexo.Remove(0, 1));
                mensaje = "Buscar persona sintys";
                var listado = new List<SintysGrupoUnicoPlanoResultado>();

                var condicionEdad = ObtenerCondicionEdad(integrantePrestamo.Edad);
                var dato = new SintysGrupoUnicoPlanoResultado
                {
                    ApellidoNombre = integrantePrestamo.ApellidoNombre,
                    NroDocumento = integrantePrestamo.NroDocumento,
                    NroSticker = string.IsNullOrEmpty(integrantePrestamo.NroSticker)
                        ? "S/A"
                        : integrantePrestamo.NroSticker,
                    NroPrestamo = integrantePrestamo.NroPrestamo,
                    Departamento = integrantePrestamo.Departamento,
                    Localidad = integrantePrestamo.Localidad,
                    Domicilio = integrantePrestamo.Domicilio,
                    CondicionEdad = condicionEdad,
                    GF_ApellidoNombre =
                        integrante.Apellido + ", " + integrante.Nombre,
                    GF_NroDocumento = integrante.NroDocumento
                };

                if (persona == null)
                {
                    listado.Add(dato);
                    return listado;
                }

                var empleosPresuntos = ObtenerEmpleoPresunto(persona.IdPersona);
                mensaje = "Buscar empleo presunto sintys";
                var empleosFormal = ObtenerEmpleoFormal(persona.IdPersona);
                mensaje = "Buscar empleo formal sintys";
                var jubilaciones = ObtenerPensionesJubilacion(persona.IdPersona);
                mensaje = "Buscar pensiones jubilaciones sintys";
                var pensiones = ObtenerPensionesNoContributiva(persona.IdPersona);
                mensaje = "Buscar pensiones no contributivas sintys";
                var fallecido = ObtenerFallecimiento(persona.IdPersona);
                mensaje = "Buscar datos fellecimiento sintys";
                var desempleo = ObtenerDesempleos(persona.IdPersona);
                mensaje = "Buscar datos desempleos sintys";

                if (idCabecera >= 0)
                {
                    RegistrarHistorialDatosPrestamo(empleosPresuntos, empleosFormal, jubilaciones, pensiones,
                        fallecido,
                        desempleo,
                        integrante, idCabecera, integrantePrestamo.EsSolicitante);
                    mensaje = "Registrar datos historial";
                }

                mensaje = "Obtener condicion edad";

                foreach (var empleo in empleosFormal)
                {
                    var datoCopia = (SintysGrupoUnicoPlanoResultado)dato.Clone();

                    datoCopia.GF_Activo_PeridoBase = empleo.Periodo.ToString();
                    datoCopia.GF_Activo_MontoIngreso = empleo.Monto?.ToString() ?? "";
                    datoCopia.GF_Activo_Empleador = empleo.DenominacionEmpleador;
                    datoCopia.GF_Activo_Cuil = empleo.CuitEmpleador?.ToString() ?? "";
                    datoCopia.GF_EmpleadoPublico = false;
                    datoCopia.GF_FechaDefuncion = fallecido?.FechaFallecimiento ?? integrante?.FechaDefuncion;
                    datoCopia.GF_Des_Cant_Cuotas = desempleo?.CantidadCuotas ?? 0;
                    datoCopia.GF_Des_Cant_Liq = desempleo?.CantCuotasLiquidadas ?? 0;
                    datoCopia.GF_Des_Periodo = desempleo?.Periodo ?? 0;
                    listado.Add(datoCopia);
                }

                mensaje = "Generar listado empleosFormal";

                foreach (var empleo in empleosPresuntos)
                {
                    var datoCopia = (SintysGrupoUnicoPlanoResultado)dato.Clone();

                    datoCopia.GF_Activo_PeridoBase = empleo.Periodo?.ToString() ?? "";
                    datoCopia.GF_Activo_Empleador = empleo.Empleador;
                    datoCopia.GF_Activo_Cuil = empleo.CuitEmpleador?.ToString() ?? "";
                    datoCopia.GF_EmpleadoPublico = false;
                    datoCopia.GF_FechaDefuncion = fallecido?.FechaFallecimiento ?? integrante?.FechaDefuncion;
                    datoCopia.GF_Des_Cant_Cuotas = desempleo?.CantidadCuotas ?? 0;
                    datoCopia.GF_Des_Cant_Liq = desempleo?.CantCuotasLiquidadas ?? 0;
                    datoCopia.GF_Des_Periodo = desempleo?.Periodo ?? 0;

                    listado.Add(datoCopia);
                }

                mensaje = "Generar listado empleosPresuntos";

                foreach (var jubilacion in jubilaciones)
                {
                    var datoCopia = (SintysGrupoUnicoPlanoResultado)dato.Clone();

                    datoCopia.GF_Pasivo_PeriodoBase = jubilacion.Periodo?.ToString() ?? "";
                    datoCopia.GF_Pasivo_LeyAplicada = jubilacion.DescripcionBeneficio;
                    datoCopia.GF_Pasivo_MontoIngreso = jubilacion.Monto?.ToString() ?? "";
                    datoCopia.GF_FechaDefuncion = fallecido?.FechaFallecimiento ?? integrante?.FechaDefuncion;
                    datoCopia.GF_Des_Cant_Cuotas = desempleo?.CantidadCuotas ?? 0;
                    datoCopia.GF_Des_Cant_Liq = desempleo?.CantCuotasLiquidadas ?? 0;
                    datoCopia.GF_Des_Periodo = desempleo?.Periodo ?? 0;

                    listado.Add(datoCopia);
                }

                mensaje = "Generar listado jubilaciones";

                foreach (var pension in pensiones)
                {
                    var datoCopia = (SintysGrupoUnicoPlanoResultado)dato.Clone();

                    datoCopia.GF_Pasivo_PeriodoBase = pension.Periodo?.ToString() ?? "";
                    datoCopia.GF_Pasivo_LeyAplicada = pension.DescripcionBeneficio;
                    datoCopia.GF_FechaDefuncion = fallecido?.FechaFallecimiento ?? integrante?.FechaDefuncion;
                    datoCopia.GF_Des_Cant_Cuotas = desempleo?.CantidadCuotas ?? 0;
                    datoCopia.GF_Des_Cant_Liq = desempleo?.CantCuotasLiquidadas ?? 0;
                    datoCopia.GF_Des_Periodo = desempleo?.Periodo ?? 0;

                    listado.Add(datoCopia);
                }

                mensaje = "Generar listado pensiones";

                /*Reviso que el listado no este vacio, si lo esta le pongo un dato vacio para que se muestre en el reporte*/
                if (listado.Count == 0)
                {
                    dato.GF_FechaDefuncion = fallecido?.FechaFallecimiento ?? integrante?.FechaDefuncion;
                    dato.GF_Des_Cant_Cuotas = desempleo?.CantidadCuotas ?? 0;
                    dato.GF_Des_Cant_Liq = desempleo?.CantCuotasLiquidadas ?? 0;
                    dato.GF_Des_Periodo = desempleo?.Periodo ?? 0;
                    listado.Add(dato);
                }


                return listado;
            }
            catch (Exception e)
            {
                throw new RespuestaServiceApiExternaException($"{mensaje} en ObtenerDatosSintys", e);
            }
        }

        private void RegistrarHistorialDatosPrestamo(List<EmpleoPresunto> empleosPresuntos,
            List<EmpleoFormal> empleosFormal,
            List<PensionJubilacion> jubilaciones, List<PensionNoContributiva> pensiones, Fallecido fallecido,
            Desempleo desempleo,
            IntegranteGrupo integrante, decimal idCabecera, bool delSolicitante)
        {

            if (empleosPresuntos.Count == 0
                && empleosFormal.Count == 0
                && jubilaciones.Count == 0
                && pensiones.Count == 0
                && fallecido == null
                && desempleo == null)
            {
                _sintysRepositorio.RegistrarDetalleDatoSintys(integrante, idCabecera, delSolicitante);
            }
            _sintysRepositorio.RegistrarEmpleoPresunto(empleosPresuntos, integrante, idCabecera, delSolicitante);
            _sintysRepositorio.RegistrarEmpleoFormal(empleosFormal, integrante, idCabecera, delSolicitante);
            _sintysRepositorio.RegistrarPensionJubilacion(jubilaciones, integrante, idCabecera, delSolicitante);
            _sintysRepositorio.RegistrarPensionNoContributiva(pensiones, integrante, idCabecera, delSolicitante);
            _sintysRepositorio.RegistrarFallecimiento(fallecido, integrante, idCabecera, delSolicitante);
            _sintysRepositorio.RegistrarDesempleo(desempleo, integrante, idCabecera, delSolicitante);
        }

        private IEnumerable<SintysGrupoUnicoPlanoResultado> ObtenerHistorialIntegrante(RespuestaAPIGrupoFamiliar grupo,
            ConsultaIntegrantesPrestamoRentasResultado integrantePrestamo, List<HistorialSintys> datosHistorial)
        {
            var sintysGrupoFamiliarPlano = new List<SintysGrupoUnicoPlanoResultado>();
            var condicionEdad = ObtenerCondicionEdad(integrantePrestamo.Edad);

            IList<IntegranteGrupo> integrantes;

            if (grupo?.Grupo == null)
            {
                integrantes = new List<IntegranteGrupo>
                {
                    new IntegranteGrupo()
                    {
                        Id_Numero = integrantePrestamo.IdNumero,
                        Sexo = {IdSexo = integrantePrestamo.Sexo},
                        PaisTD = {IdPais = integrantePrestamo.CodigoPais},
                        NroDocumento = integrantePrestamo.NroDocumento,
                        Apellido = integrantePrestamo.ApellidoNombre
                    }
                };
            }
            else
            {
                integrantes = grupo.Grupo.Integrantes;
            }
            foreach (var persona in integrantes)
            {
                var historicoPersona = datosHistorial.Where(x =>
                        x.IdNum == persona.Id_Numero &&
                        x.IdSexo == persona.Sexo.IdSexo &&
                        x.CodPais == persona.PaisTD.IdPais &&
                        x.NroDoc == persona.NroDocumento &&
                        x.EsSolicitante == integrantePrestamo.EsSolicitante)
                    .Select(x => new SintysGrupoUnicoPlanoResultado
                    {
                        ApellidoNombre = integrantePrestamo.ApellidoNombre,
                        NroDocumento = integrantePrestamo.NroDocumento,
                        NroSticker = string.IsNullOrEmpty(integrantePrestamo.NroSticker)
                            ? "S/A"
                            : integrantePrestamo.NroSticker,
                        NroPrestamo = integrantePrestamo.NroPrestamo,
                        Departamento = integrantePrestamo.Departamento,
                        Localidad = integrantePrestamo.Localidad,
                        Domicilio = integrantePrestamo.Domicilio,
                        CondicionEdad = condicionEdad,
                        EsSolicitante = x.EsSolicitante,
                        GF_ApellidoNombre =
                            persona.Apellido + ", " + persona.Nombre,
                        GF_NroDocumento = x.NroDoc,
                        GF_Activo_PeridoBase = x.ActPeriodo,
                        GF_Activo_MontoIngreso = x.ActMonto,
                        GF_Activo_Empleador = x.ActEmpleador,
                        GF_Activo_Cuil = x.ActCuit,
                        GF_Pasivo_PeriodoBase = x.PasPeriodo,
                        GF_Pasivo_LeyAplicada = x.PasDesc,
                        GF_Pasivo_MontoIngreso = x.PasMonto,
                        GF_EmpleadoPublico = false,
                        GF_FechaDefuncion = x.Fallecido,
                        GF_Des_Cant_Cuotas = x.CantCuota,
                        GF_Des_Cant_Liq = x.CantLiq,
                        GF_Des_Periodo = x.Periodo
                    }).ToList();

                sintysGrupoFamiliarPlano.AddRange(historicoPersona);
            }

            return sintysGrupoFamiliarPlano;
        }

        private string ObtenerCondicionEdad(string edadIntegrante)
        {
            if (string.IsNullOrEmpty(edadIntegrante))
                return "";

            var edad = Convert.ToInt32(edadIntegrante);
            return (edad >= _edadDesde && edad <= _edadHasta) ? "Si" : "No";
        }

        private PersonaFisica ObtenerDatosPersonaSintys(string nroDocumento, string sexo)
        {
            if (!usarWebService)
                return new PersonaFisica
                {
                    IdPersona = "1",
                    Cuil = 35265265452,
                    TipoDocumento = "DNI",
                    Ndoc = 5625625,
                    Sexo = "Masculino",
                    FechaNacimiento = new DateTime(1994, 2, 8)
                };
            try
            {
                var personas = _sintysServicioWs.ObtenerPersonaFisica(nroDocumento, sexo);
                if (personas != null && personas.Count > 0)
                    return personas.OrderByDescending(x => x.GradoConfiabilidad).First();
                return null;
            }
            catch (Exception ex)
            {
                throw new RespuestaServiceApiExternaException("Error WS SINTyS.", ex);
            }
        }

        private List<EmpleoPresunto> ObtenerEmpleoPresunto(string idPersona)
        {
            if (usarWebService)
            {
                try
                {
                    return _sintysServicioWs.ObtenerEmpleoPresunto(idPersona);
                }
                catch (Exception ex)
                {
                    throw new RespuestaServiceApiExternaException("Error WS SINTyS.", ex);
                }
            }

            return new List<EmpleoPresunto>
            {
                new EmpleoPresunto
                {
                    Empleador = "Lucas, Jorge",
                    Periodo = 20170501,
                    CuitEmpleador = 20585865852,
                    BaseOrigen = "101"
                },
                new EmpleoPresunto
                {
                    Empleador = "Hamill, Marcos",
                    Periodo = 20180322,
                    CuitEmpleador = 274535365852,
                    BaseOrigen = "101"
                }
            };
        }

        private List<EmpleoFormal> ObtenerEmpleoFormal(string idPersona)
        {
            if (usarWebService)
            {
                try
                {
                    return _sintysServicioWs.ObtenerEmpleoFormalConMonto(idPersona);
                }
                catch (Exception ex)
                {
                    throw new RespuestaServiceApiExternaException("Error WS SINTyS.", ex);
                }
            }

            return new List<EmpleoFormal>
            {
                new EmpleoFormal
                {
                    DenominacionEmpleador = "Cranston, Braian",
                    Periodo = 20170501,
                    CuitEmpleador = 20585865852,
                    ActividadTrabajador = "Una que SI se que",
                    Monto = 28978,
                    BaseOrigen = "101"
                },
                new EmpleoFormal
                {
                    DenominacionEmpleador = "Paulo, Aaron",
                    Periodo = 20180322,
                    CuitEmpleador = 274535365852,
                    ActividadTrabajador = "Una que NO se que",
                    Monto = null,
                    BaseOrigen = "101"
                }
            };
        }

        private List<PensionJubilacion> ObtenerPensionesJubilacion(string idPersona)
        {
            if (usarWebService)
            {
                try
                {
                    return _sintysServicioWs.ObtenerJubilacionPension(idPersona);
                }
                catch (Exception ex)
                {
                    throw new RespuestaServiceApiExternaException("Error WS SINTyS.", ex);
                }
            }

            return new List<PensionJubilacion>
            {
                new PensionJubilacion
                {
                    DescripcionBeneficio = "Jubilación 1",
                    TipoBeneficio = "Jubilacion 1",
                    Periodo = 20170501,
                    FechaAlta = new DateTime(2018, 2, 9),
                    Monto = 18978,
                    BaseOrigen = "101"
                },
                new PensionJubilacion
                {
                    DescripcionBeneficio = "Jubilación 2",
                    TipoBeneficio = "Jubilacion 2",
                    Periodo = 20180322,
                    FechaAlta = new DateTime(2017, 9, 22),
                    Monto = 15365,
                    BaseOrigen = "101"
                }
            };
        }

        private List<PensionNoContributiva> ObtenerPensionesNoContributiva(string idPersona)
        {
            if (usarWebService)
            {
                try
                {
                    return _sintysServicioWs.ObtenerPensionNoContributiva(idPersona);
                }
                catch (Exception ex)
                {
                    throw new RespuestaServiceApiExternaException("Error WS SINTyS.", ex);
                }
            }

            return new List<PensionNoContributiva>
            {
                new PensionNoContributiva
                {
                    DescripcionBeneficio = "Pensión no contributiva 1",
                    TipoBeneficio = "Pension 1",
                    Periodo = 20170501,
                    FechaAlta = new DateTime(2017, 9, 22),
                    BaseOrigen = "101"
                },
                new PensionNoContributiva
                {
                    DescripcionBeneficio = "Pensión 2",
                    TipoBeneficio = "Pension 2",
                    Periodo = 20180322,
                    FechaAlta = new DateTime(2018, 5, 22),
                    BaseOrigen = "101"
                }
            };
        }

        private Fallecido ObtenerFallecimiento(string idPersona)
        {
            if (usarWebService)
            {
                try
                {
                    var fallecidos = _sintysServicioWs.ObtenerFallecido(idPersona);
                    if (fallecidos != null && fallecidos.Count > 0)
                    {
                        return fallecidos.OrderByDescending(f => f.FechaFallecimiento).FirstOrDefault();
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    throw new RespuestaServiceApiExternaException("Error WS SINTyS.", ex);
                }
            }

            /* var lista = new List<Fallecido>
            {
                new Fallecido
                {
                    FechaFallecimiento = new DateTime(2017, 2, 2),
                    Acta = "1",
                    AnioActa = 2017,
                    Fallecidos = "SI",
                    BaseOrigen = "101"
                },
                new Fallecido
                {
                    FechaFallecimiento = new DateTime(2018, 2, 2),
                    Acta = "1",
                    AnioActa = 2018,
                    Fallecidos = "SI",
                    BaseOrigen = "101"
                }
            };

            return lista.OrderByDescending(f => f.FechaFallecimiento).FirstOrDefault();*/
            return null;
        }

        private Desempleo ObtenerDesempleos(string idPersona)
        {
            if (usarWebService)
            {
                try
                {
                    var desempleos = _sintysServicioWs.ObtenerDesempleos(idPersona);
                    if (desempleos != null && desempleos.Count > 0)
                    {
                        return desempleos.OrderByDescending(f => f.Periodo).FirstOrDefault();
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    throw new RespuestaServiceApiExternaException("Error WS SINTyS.", ex);
                }
            }

            /*return new List<Desempleo>
                {
                    new Desempleo {Periodo = 201801, CantidadCuotas = 6, CantCuotasLiquidadas = 2, BaseOrigen = "101"},
                    new Desempleo {Periodo = 201807, CantidadCuotas = 6, CantCuotasLiquidadas = 6, BaseOrigen = "101"}
                }
                .OrderByDescending(x => x.Periodo).FirstOrDefault();*/
            return null;
        }

        private PersonaUnica ObtenerPersona(string sexo, string documento, string pais, int numero, string cookieValue)
        {
            return ApiGruposFamiliares.ApiConsultaPersona(cookieValue, sexo, documento, pais, numero);
        }
    }
}