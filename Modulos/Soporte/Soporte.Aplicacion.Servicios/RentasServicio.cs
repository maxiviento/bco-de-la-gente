using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AppComunicacion;
using AppComunicacion.ApiModels;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Identidad.Aplicacion.Consultas.Resultados;
using Identidad.Aplicacion.Servicios;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.CiDi.Api;
using Infraestructura.Core.CiDi.Util;
using Infraestructura.Core.Comun;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Reportes;
using Newtonsoft.Json;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Dominio.IRepositorio;
using Soporte.Dominio.Modelo;

namespace Soporte.Aplicacion.Servicios
{
    public class RentasServicio
    {
        private readonly ISesionUsuario _sesionUsuario;
        private readonly IPrestamoRepositorio _prestamoRepositorio;
        private readonly IRentaRepositorio _rentasRepositorio;
        private readonly UsuarioServicio usuarioServicio;
        private readonly int _valorPesos = int.Parse(ParametrosSingleton.Instance.GetValue("15"));
        private readonly int _antiguedad = int.Parse(ParametrosSingleton.Instance.GetValue("14"));
        private readonly int _edadDesde = int.Parse(ParametrosSingleton.Instance.GetValue("6"));
        private readonly int _edadHasta = int.Parse(ParametrosSingleton.Instance.GetValue("5"));
        private const string reporteRentasPrestamo = "RentasPrestamo.rdlc";
        private readonly string proxyString = ConfigurationManager.AppSettings["ProxyUrl"];

        public RentasServicio(ISesionUsuario sesionUsuario, IPrestamoRepositorio prestamoRepositorio, IRentaRepositorio rentasRepositorio,
            UsuarioServicio usuarioServ)
        {
            _sesionUsuario = sesionUsuario;
            _prestamoRepositorio = prestamoRepositorio;
            _rentasRepositorio = rentasRepositorio;
            usuarioServicio = usuarioServ;
        }

        /**
        Metodo que se conecta y obtiene los datos de rentas segun un cuit
        */
        public async Task<List<DatoRentaResultado>> ObtenerDatosRentas(string cuit)
        {
            await ObtenerCertificado(cuit);
            var datos = await ConsultarWsRentas(cuit);
            return datos;
        }

        public async Task ObtenerCertificado(string cuit)
        {
            try
            {
                const string resourceAddress = "https://app.rentascordoba.gob.ar/WSRestAuth/api/auth/login";

                var data = JsonConvert.SerializeObject(new
                {
                    username = cuit,
                    password = "test1234"
                });

                var httpClient = new HttpClient();
                if (!string.IsNullOrEmpty(proxyString))
                {
                    httpClient = new HttpClient(new HttpClientHandler()
                    {
                        Proxy = new WebProxy(proxyString),
                        UseProxy = true
                    });
                }

                httpClient.DefaultRequestHeaders
                    .Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

                var response =
                    await httpClient.PostAsync(resourceAddress,
                        new StringContent(data, Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                httpClient.Dispose();

                var receiveResponse = await response.Content.ReadAsStringAsync();

                CertificadoSingleton.Instance.certificado =
                    JsonConvert.DeserializeObject<CertificadoRentas>(receiveResponse);
            }
            catch (Exception e)
            {
                /*var mensaje = e.Message + "  -  " + e.InnerException?.Message + "  -  " + e.StackTrace;
                throw new ErrorTecnicoException(mensaje);*/
                throw new RespuestaServiceApiExternaException("Error WS Rentas", e);
            }
        }

        private async Task<List<DatoRentaResultado>> ConsultarWsRentas(string cuit)
        {
            var resourceAddress = $"https://app.rentascordoba.gob.ar/WSRestTributario/secured/sujetos/getobjetos/{ cuit }";

            var httpClient = new HttpClient();
            if (!string.IsNullOrEmpty(proxyString))
            {
                httpClient = new HttpClient(new HttpClientHandler()
                {
                    Proxy = new WebProxy(proxyString),
                    UseProxy = true
                });
            }

            httpClient.DefaultRequestHeaders.Add("cuit", cuit);
            httpClient.DefaultRequestHeaders.Add("x-Authorization", CertificadoSingleton.Instance.certificado.Token);
            httpClient.DefaultRequestHeaders.Add("token", CertificadoSingleton.Instance.certificado.RefreshToken);
            httpClient.DefaultRequestHeaders.Add("timestamp", CertificadoSingleton.Instance.certificado.Timestamp);

            var response = await httpClient.GetAsync(resourceAddress);
            httpClient.Dispose();

            CertificadoSingleton.Instance.certificado = null;
            var receiveResponse = await response.Content.ReadAsStringAsync();
            var respuesta = JsonConvert.DeserializeObject<RespuestaRentas>(receiveResponse);

            if (response.IsSuccessStatusCode)
            {
                return respuesta.data.Select(x => new DatoRentaResultado
                {
                    BaseImponible = x.BaseImponible.GetValueOrDefault(),
                    Anio = x.Anio.GetValueOrDefault(),
                    Marca = x.Marca,
                    Objeto = x.Objeto,
                    Porcentaje = x.Porcentaje.GetValueOrDefault(),
                    Estado = x.Estado,
                    Superficie = x.Superficie.GetValueOrDefault(),
                    Domicilio = x.Domicilio
                })
                    .ToList();
            }
            throw new RespuestaServiceApiExternaException("Error WS Rentas", new Exception($"{respuesta.error}.- {respuesta.message}"));
            // throw new ErrorTecnicoException(respuesta.error + ". " + respuesta.message + ". ");
        }

        public async Task<List<RentasPrestamoPlano>> ConsultaRentasGrupoFamiliar(
            RespuestaAPIGrupoFamiliar grupo)
        {
            var rentasPlano = new List<RentasPrestamoPlano>();
            IList<IntegranteGrupo> integrantesGrupo;

            if (grupo.Persona == null) return rentasPlano;

            if (grupo.Grupo == null)
            {
                integrantesGrupo = new List<IntegranteGrupo>
                {
                    new IntegranteGrupo()
                    {
                        Sexo = grupo.Persona.Sexo,
                        Id_Numero = grupo.Persona.Id_Numero,
                        PaisTD = grupo.Persona.PaisTD,
                        Apellido = grupo.Persona.Apellido,
                        Nombre = grupo.Persona.Nombre,
                        NroDocumento = grupo.Persona.NroDocumento,
                        FechaNacimiento = grupo.Persona.FechaNacimiento,
                        CUIL = grupo.Persona.CUIL
                    }
                };
            }
            else
            {
                integrantesGrupo = grupo.Grupo.Integrantes;
            }

            foreach (var integrante in integrantesGrupo)
            {
                var datosRentasResultado = await ObtenerDatosRentas(integrante.CUIL ?? integrante.CalcularCuil());
                foreach (var datoRentas in datosRentasResultado)
                {
                    rentasPlano.Add(new RentasPrestamoPlano
                    {
                        GF_IdSexo = integrante.Sexo.IdSexo,
                        GF_IdNumero = integrante.Id_Numero,
                        GF_IdPais = integrante.PaisTD.IdPais,
                        GF_ApellidoNombre = integrante.Apellido + ", " + integrante.Nombre,
                        GF_NroDocumento = integrante.NroDocumento,
                        GF_Edad = integrante.Edad(),
                        GF_Objeto = datoRentas.Objeto,
                        GF_Porcentaje = datoRentas.Porcentaje,
                        GF_BaseImponible = datoRentas.BaseImponible,
                        GF_Modelo = datoRentas.Anio,
                        GF_Marca = datoRentas.Marca,
                        GF_Estado = datoRentas.Estado,
                        GF_Superficie = datoRentas.Superficie,
                        GF_Domicilio = datoRentas.Domicilio
                    });
                }
            }

            return rentasPlano;
        }

        private static RentasPrestamoPlano CrearReporteSinDatos(PersonaUnica integrante)
        {
            return new RentasPrestamoPlano
            {
                GF_IdSexo = integrante.Sexo.IdSexo,
                GF_IdNumero = integrante.Id_Numero,
                GF_IdPais = integrante.PaisTD.IdPais,
                GF_ApellidoNombre = integrante.Apellido + ", " + integrante.Nombre,
                GF_NroDocumento = integrante.NroDocumento,
                GF_Edad = integrante.Edad(),
                GF_Objeto = "",
                GF_Porcentaje = 0,
                GF_BaseImponible = 0,
                GF_Modelo = 0,
                GF_Marca = "",
                GF_Estado = "",
                GF_Superficie = 0,
                GF_Domicilio = "NO POSEE DATOS"
            };
        }

        /**
        Metodo que genera el reporte desde el menu, solo para consulta de una persona y su grupo familiar.
        */
        public async Task<string> GenerarReporteRentas(RespuestaAPIGrupoFamiliar grupo, string cookieValue)
        {
            Usuario usuario = _sesionUsuario.Usuario;
            var fechaActual = DateTime.Now;

            foreach (var familiar in grupo.Grupo.Integrantes)
            {
                var personaIntegrante = ObtenerPersona(familiar.Sexo.IdSexo, familiar.NroDocumento, familiar.PaisTD.IdPais, familiar.Id_Numero, cookieValue);
                familiar.FechaNacimiento = personaIntegrante.FechaNacimiento;
            }
            var persona = ObtenerPersona(grupo.Persona.Sexo.IdSexo, grupo.Persona.NroDocumento, grupo.Persona.PaisTD.IdPais, grupo.Persona.Id_Numero, cookieValue);

            grupo.Persona = persona;

            var grupoFamiliarDs = await ConsultaRentasGrupoFamiliar(grupo);

            if (grupoFamiliarDs.Count > 0)
            {
                grupoFamiliarDs = ObtenerDatosValidados(grupoFamiliarDs);
                grupoFamiliarDs = grupoFamiliarDs.Select(x =>
                {
                    x.ApellidoNombre = grupo.Persona?.NombreCompleto;
                    x.NroDocumento = grupo.Persona?.NroDocumento;
                    x.CondicionEdad = ObtenerCondicionEdad(grupo.Persona?.Edad());
                    x.Domicilio = grupo.Grupo.Domicilio?.DireccionCompleta + " Barrio: " + grupo.Grupo?.Domicilio?.Barrio?.Nombre;
                    x.Localidad = grupo.Grupo?.Domicilio?.Localidad?.Nombre;
                    x.Departamento = grupo.Grupo?.Domicilio?.Departamento?.Nombre;
                    return x;
                }).ToList();

                grupoFamiliarDs = ObtenerDatosValidados(grupoFamiliarDs).OrderBy(x => x.ApellidoNombre).ThenBy(x => x.GF_ApellidoNombre).ToList();
            }
            else
            {
                grupoFamiliarDs = new List<RentasPrestamoPlano>
                    {
                        new RentasPrestamoPlano
                        {
                            GF_IdSexo = grupo.Persona.Sexo.IdSexo,
                            GF_IdNumero = grupo.Persona.Id_Numero,
                            GF_IdPais = grupo.Persona.PaisTD.IdPais,
                            GF_ApellidoNombre = grupo.Persona.Apellido + ", " + grupo.Persona.Nombre,
                            GF_NroDocumento = grupo.Persona.NroDocumento,
                            GF_Edad = grupo.Persona?.Edad(),
                            ApellidoNombre = grupo.Persona?.NombreCompleto,
                            NroDocumento = grupo.Persona?.NroDocumento,
                            CondicionEdad = ObtenerCondicionEdad(grupo.Persona?.Edad()),
                            Domicilio = grupo.Grupo?.Domicilio?.DireccionCompleta + " Barrio: " + grupo.Grupo?.Domicilio?.Barrio?.Nombre,
                            Localidad = grupo.Grupo?.Domicilio?.Localidad?.Nombre,
                            Departamento = grupo.Grupo?.Domicilio?.Departamento?.Nombre
                        }
                    };
            }
            /*
            grupoFamiliarDS = grupoFamiliarDS.Select(x =>
            {
                x.ApellidoNombre = grupo.Persona?.NombreCompleto;
                x.NroDocumento = grupo.Persona?.NroDocumento;
                x.CondicionEdad = grupo.Persona?.Edad();
                x.Domicilio = grupo.Grupo.Domicilio?.DireccionCompleta + " Barrio: " + grupo.Grupo.Domicilio?.Barrio?.Nombre;
                x.Localidad = grupo.Grupo?.Domicilio?.Localidad?.Nombre;
                x.Departamento = grupo.Grupo?.Domicilio?.Departamento?.Nombre;
                return x;
            }).ToList();

            grupoFamiliarDS = ObtenerDatosValidados(grupoFamiliarDS).OrderBy(x=> x.ApellidoNombre).ThenBy(x=> x.GF_ApellidoNombre).ToList();*/
            var dateSet = new Dictionary<string, List<RentasPrestamoPlano>> { { "RentasPlanoDS", grupoFamiliarDs } };

            var parametros = new Dictionary<string, string>
            {
                ["Usuario"] = usuario.Apellido + ", " + usuario.Nombre,
                ["Fecha"] = fechaActual.ToString()
            };

            return CrearReporteRentas("RentasMenu.rdlc", "Rentas", dateSet, parametros);
        }

        /**
        Metodo que genera el reporte de Condiciones Economicas desde el Formulario.
        */
        public async Task<string> GenerarReporteRentasPorPrestamo(decimal idFormularioLinea, string cookieValue, decimal idPrestamoRequisito)
        {
            var usuario = _sesionUsuario.Usuario;
            var fechaActual = DateTime.Now;
            var datosSolicitantePlano = new List<RentasPrestamoPlano>();
            var datosGarantePlano = new List<RentasPrestamoPlano>();
            var generaAlerta = false;

            var integrantesPrestamo = _prestamoRepositorio.ConsultarIntegrantesPrestamoRentas(new Id(idFormularioLinea));
            var prestamoResultado = _prestamoRepositorio.ObtenerIdPrestamo((int)idFormularioLinea);

            //busca los grupos familiares de cada integrante de prestamo.
            foreach (var integrante in integrantesPrestamo)
            {
                var grupoFamiliar = ObtenerGrupoFamiliar(integrante, cookieValue);
                foreach (var familiar in grupoFamiliar.Grupo.Integrantes)
                {
                    var personaIntegrante = ObtenerPersona(familiar.Sexo.IdSexo, familiar.NroDocumento, familiar.PaisTD.IdPais, familiar.Id_Numero, cookieValue);
                    familiar.FechaNacimiento = personaIntegrante.FechaNacimiento;
                }
                var persona = ObtenerPersona(integrante.Sexo, integrante.NroDocumento, integrante.CodigoPais, integrante.IdNumero, cookieValue);

                grupoFamiliar.Persona = persona;
                integrante.Edad = persona.Edad();

                var grupoFamiliarDataSet =
                    await ConsultaRentasGrupoFamiliar(grupoFamiliar);

                if (grupoFamiliarDataSet.Count > 0)
                {
                    grupoFamiliarDataSet = ObtenerDatosValidados(grupoFamiliarDataSet);
                }
                else
                {
                    grupoFamiliarDataSet = new List<RentasPrestamoPlano>
                    {
                        new RentasPrestamoPlano
                        {
                            ApellidoNombre = integrante.ApellidoNombre,
                            NroDocumento = integrante.NroDocumento,
                            NroSticker = !string.IsNullOrEmpty(integrante.NroSticker)
                                ? integrante.NroSticker
                                : "S/A",
                            CondicionEdad = ObtenerCondicionEdad(persona.Edad()),
                            Domicilio = integrante.Domicilio,
                            Localidad = integrante.Localidad,
                            Departamento = integrante.Departamento,
                            EsSolicitante = integrante.EsSolicitante,
                            GF_IdSexo = grupoFamiliar.Persona.Sexo.IdSexo,
                            GF_IdNumero = grupoFamiliar.Persona.Id_Numero,
                            GF_Edad = grupoFamiliar.Persona.Edad(),
                            GF_IdPais = grupoFamiliar.Persona.PaisTD.IdPais,
                            GF_ApellidoNombre = grupoFamiliar.Persona.Apellido + ", " + grupoFamiliar.Persona.Nombre,
                            GF_NroDocumento = grupoFamiliar.Persona.NroDocumento,
                        }
                    };
                    grupoFamiliarDataSet = ObtenerDatosValidados(grupoFamiliarDataSet);
                }
                foreach (var integranteGrupo in grupoFamiliarDataSet)
                {

                    integranteGrupo.ApellidoNombre = integrante.ApellidoNombre;
                    integranteGrupo.NroDocumento = integrante.NroDocumento;
                    integranteGrupo.NroSticker = !string.IsNullOrEmpty(integrante.NroSticker)
                        ? integrante.NroSticker
                        : "S/A";
                    integranteGrupo.CondicionEdad = ObtenerCondicionEdad(integrante.Edad);
                    integranteGrupo.Domicilio = integrante.Domicilio;
                    integranteGrupo.Localidad = integrante.Localidad;
                    integranteGrupo.Departamento = integrante.Departamento;
                    integranteGrupo.EsSolicitante = integrante.EsSolicitante;
                    if (integranteGrupo.EnabledAlert)
                    {
                        generaAlerta = true;
                    }

                    _rentasRepositorio.RegistrarDatoRenta(idFormularioLinea, prestamoResultado.IdPrestamo, integranteGrupo, usuario, fechaActual, idPrestamoRequisito);

                    if (integrante.EsSolicitante)
                        datosSolicitantePlano.Add(integranteGrupo);
                    else
                        datosGarantePlano.Add(integranteGrupo);
                }
            }
            var dsDiccionario = new Dictionary<string, List<RentasPrestamoPlano>>
            {
                {"RentasSolicitantePlanoDS", datosSolicitantePlano.OrderBy(x=> x.ApellidoNombre).ThenBy(x=> x.GF_ApellidoNombre).ToList()},
                {"RentasGarantePlanoDS", datosGarantePlano.OrderBy(x=> x.ApellidoNombre).ThenBy(x=> x.GF_ApellidoNombre).ToList()}
            };

            var parametros = new Dictionary<string, string>()
            {
                ["Usuario"] = usuario.Apellido + ", " + usuario.Nombre,
                ["Fecha"] = fechaActual.ToString(),
                ["NroPrestamo"] = integrantesPrestamo[0].NroPrestamo,
                ["Linea"] = integrantesPrestamo[0].Linea,
                ["GeneraAlerta"] = generaAlerta ? "S" : "N"
            };

            return CrearReporteRentas(reporteRentasPrestamo, "RentasPrestamo", dsDiccionario, parametros);
        }

        public string ObtenerHistorial(decimal idHistorial, string cookieValue)
        {
            var datosSolicitantePlano = new List<RentasPrestamoPlano>();
            var datosGarantePlano = new List<RentasPrestamoPlano>();
            var generaAlerta = false;
            var historicoRentas = _rentasRepositorio.ObtenerHistorial(idHistorial);

            if (historicoRentas.Count <= 0) return "";

            var usuario = usuarioServicio.ObtenerUsuarioPorId(new Id(historicoRentas[0].IdUsuario));
            var fecha = historicoRentas[0].Fecha;
            var integrantesPrestamo = (List<ConsultaIntegrantesPrestamoRentasResultado>)_prestamoRepositorio
                .ConsultarIntegrantesPrestamoRentas(new Id(historicoRentas[0].IdFormularioLinea));

            foreach (var integrante in integrantesPrestamo)
            {

                var grupo = ObtenerGrupoFamiliar(integrante, cookieValue);

                foreach (var familiar in grupo.Grupo.Integrantes)
                {
                    var personaIntegrante = ObtenerPersona(familiar.Sexo.IdSexo, familiar.NroDocumento, familiar.PaisTD.IdPais, familiar.Id_Numero, cookieValue);
                    familiar.FechaNacimiento = personaIntegrante.FechaNacimiento;
                }
                var persona = ObtenerPersona(integrante.Sexo, integrante.NroDocumento, integrante.CodigoPais, integrante.IdNumero, cookieValue);

                grupo.Persona = persona;
                integrante.Edad = persona.Edad();

                IList<IntegranteGrupo> integrantes;

                if (grupo.Persona == null) continue;

                if (grupo.Grupo == null)
                {
                    integrantes = new List<IntegranteGrupo>
                    {
                        new IntegranteGrupo()
                        {
                            Sexo = grupo.Persona.Sexo,
                            Id_Numero = grupo.Persona.Id_Numero,
                            PaisTD = grupo.Persona.PaisTD,
                            Apellido = grupo.Persona.Apellido,
                            Nombre = grupo.Persona.Nombre,
                            NroDocumento = grupo.Persona.NroDocumento,
                            FechaNacimiento = grupo.Persona.FechaNacimiento,
                            CUIL = grupo.Persona.CUIL
                        }
                    };
                }
                else
                {
                    integrantes = grupo.Grupo.Integrantes;
                }

                foreach (var grupoIntegrante in integrantes)
                {

                    var historial =
                        historicoRentas.Where(x => x.IdPais == grupoIntegrante.PaisTD.IdPais &&
                                                   x.IdSexo == grupoIntegrante.Sexo.IdSexo &&
                                                   x.NroDocumento == grupoIntegrante.NroDocumento &&
                                                   x.EsSolicitante == integrante.EsSolicitante);
                    foreach (var dato in historial)
                    {
                        var datoRentas = new RentasPrestamoPlano()
                        {
                            GF_IdSexo = dato.IdSexo,
                            GF_IdNumero = dato.IdNumero,
                            GF_IdPais = dato.IdPais,
                            GF_NroDocumento = dato.NroDocumento,
                            GF_Objeto = dato.Objeto,
                            GF_Porcentaje = Convert.ToDecimal(dato.Porcentaje),
                            GF_BaseImponible = dato.BaseImponible,
                            GF_Modelo = dato.Modelo,
                            GF_Marca = dato.Marca,
                            GF_Estado = dato.Estado,
                            GF_Edad = grupoIntegrante.Edad(),
                            GF_Superficie = Convert.ToDecimal(dato.Superficie),
                            GF_Domicilio = dato.Domicilio,
                            GF_ApellidoNombre = $"{grupoIntegrante.Apellido}, {grupoIntegrante.Nombre}",
                            EsSolicitante = dato.EsSolicitante,
                            ApellidoNombre = integrante.ApellidoNombre,
                            Domicilio = integrante.Domicilio,
                            Localidad = integrante.Localidad,
                            Departamento = integrante.Departamento,
                            NroDocumento = integrante.NroDocumento,
                            NroSticker = integrante.NroSticker,
                            CondicionEdad = ObtenerCondicionEdad(grupo.Persona.Edad())
                        };

                        if (ValidarDato(datoRentas))
                        {
                            generaAlerta = true;
                        }
                        if (datoRentas.EsSolicitante)
                        {
                            datosSolicitantePlano.Add(datoRentas);
                        }
                        else
                        {
                            datosGarantePlano.Add(datoRentas);
                        }

                    }

                }
            }

            var dsDiccionario = new Dictionary<string, List<RentasPrestamoPlano>>
            {
                {"RentasSolicitantePlanoDS", datosSolicitantePlano.OrderBy(x=> x.ApellidoNombre).ThenBy(x=> x.GF_ApellidoNombre).ToList()},
                {"RentasGarantePlanoDS", datosGarantePlano.OrderBy(x=> x.ApellidoNombre).ThenBy(x=> x.GF_ApellidoNombre).ToList()}
            };

            var parametros = new Dictionary<string, string>()
            {
                ["Usuario"] = usuario.Apellido + ", " + usuario.Nombre,
                ["Fecha"] = fecha?.ToString(),
                ["NroPrestamo"] = integrantesPrestamo[0].NroPrestamo,
                ["Linea"] = integrantesPrestamo[0].Linea,
                ["GeneraAlerta"] = generaAlerta ? "S" : "N"
            };
            return CrearReporteRentas(reporteRentasPrestamo, "HistorialRentasPrestamo", dsDiccionario, parametros);
        }

        /**
        Metodo que retorna un listado con todos los historiales de rentas creados para un prestamo.
        */
        public Resultado<DocumentacionResultado> ObtenerTodosHistorialesRentas(DocumentacionConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new DocumentacionConsulta { NumeroPagina = 0 };
            }

            consulta.TamañoPagina = 5;

            var resultados = _rentasRepositorio.ObtenerTodosHistorialesRentas(consulta);

            foreach (var resultado in resultados.Elementos)
            {
                resultado.NombreArchivo = $"CONTROLAR CONDICIONES ECONÓMICAS - {resultado.FechaAlta:d}";
                resultado.Extension = "PDF";
            }

            return resultados;
        }

        /**
        Valida que la edad del solicitante o del garante cumpla las condiciones de edad.
            */
        private string ObtenerCondicionEdad(string edadIntegrante)
        {
            if (string.IsNullOrEmpty(edadIntegrante))
                return "";

            var edad = Convert.ToInt32(edadIntegrante);
            return (edad >= _edadDesde && edad <= _edadHasta) ? "Si" : "No";
        }

        /**
        Metodo que retorna un listado con todos datos validados segun los parametros.
        */
        private List<RentasPrestamoPlano> ObtenerDatosValidados(IEnumerable<RentasPrestamoPlano> grupoFamiliarDs)
        {
            var rentasPrestamoPlanos = grupoFamiliarDs.ToList();
            foreach (var dato in rentasPrestamoPlanos)
            {
                if (dato == null) continue;
                dato.EnabledAlert = ValidarDato(dato);

            }

            return rentasPrestamoPlanos;
        }

        /**
        Metodo que controla los datos, segun tipo. si es inmueble que supere su valor al del parametros, si es un rodado, que sea menor a la parametro de antiguedad.
        */
        private bool ValidarDato(RentasPrestamoPlano datoIntegrante)
        {
            if (datoIntegrante.GF_BaseImponible > 0)
            {
                return datoIntegrante.GF_BaseImponible >= _valorPesos;
            }
            return DateTime.Today.Year - datoIntegrante.GF_Modelo <= _antiguedad;

        }

        private static string CrearReporteRentas(string rdlc, string nombreReporte, Dictionary<string, List<RentasPrestamoPlano>> dataSet, Dictionary<string, string> parametros)
        {
            var reportBuilder = new ReporteBuilder(rdlc);

            var keysDataSource = dataSet.Keys;

            foreach (var key in keysDataSource)
            {
                reportBuilder.AgregarDataSource(key, dataSet[key]);
            }

            var keysParam = parametros.Keys;

            foreach (var key in keysParam)
            {
                reportBuilder.AgregarParametro(key, parametros[key]);
            }

            var reportData = reportBuilder.Generar();

            var url = ReporteBuilder.GenerarUrlReporte(reportData, nombreReporte);
            return url;
        }

        private RespuestaAPIGrupoFamiliar ObtenerGrupoFamiliar(ConsultaIntegrantesPrestamoRentasResultado integrante, string cookieValue)
        {
            return ApiGruposFamiliares.ApiConsultaGrupos(cookieValue, integrante.Sexo, integrante.NroDocumento, integrante.CodigoPais, integrante.IdNumero);
        }

        private PersonaUnica ObtenerPersona(string sexo, string documento, string pais, int numero, string cookieValue)
        {
            return ApiGruposFamiliares.ApiConsultaPersona(cookieValue, sexo, documento, pais, numero);
        }
    }
}