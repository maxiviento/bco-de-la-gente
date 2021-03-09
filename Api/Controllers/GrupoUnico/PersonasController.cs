using System.Web;
using System.Web.Http;
using AppComunicacion.ApiModels;
using Formulario.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.CiDi.Api;
using Infraestructura.Core.CiDi.Model;

namespace Api.Controllers.GrupoUnico
{
    public class PersonasController : GrupoFamiliarControllerBase
    {
        [Route("url-alta")]
        public string GetUrlAlta(string sexo, string dni, string pais)
        {
            return CallApiStringUrl(sexo, dni, pais, ApiPersonas.UrlAlta, 0);
        }

        [Route("url-consultar")]
        public string GetUrlConsultar(string sexo, string dni, string pais)
        {
            return CallApiStringUrl(sexo, dni, pais, ApiPersonas.UrlConsultarCompleto, 0);
        }

        [Route("url-consultar-basico")]
        public string GetUrlConsultarBasico(string sexo, string dni, string pais)
        {
            return CallApiStringUrl(sexo, dni, pais, ApiPersonas.UrlConsultarBasico, 0);
        }

        [Route("url-consultar-caracteristicas")]
        public string GetUrlConsultarCaracteristicas(string sexo, string dni, string pais)
        {
            return CallApiStringUrl(sexo, dni, pais, ApiPersonas.UrlConsultarCaracteristicas, 0);
        }

        [Route("url-modificar")]
        public string GetUrlModificar(string sexo, string dni, string pais)
        {
            return CallApiStringUrl(sexo, dni, pais, ApiPersonas.UrlModificar, 0);
        }

        [Route("url-modificar-caracteristicas")]
        public string GetUrlModificarCaracteristicas(string sexo, string dni, string pais)
        {
            return CallApiStringUrl(sexo, dni, pais, ApiPersonas.UrlModificarCaracteristicas, 0);
        }

        [Route("consulta-caracteristicas-persona-json")]
        public string GetApiConsultaCaracteristicasPersonaJson(string sexo, string dni, string pais)
        {
            return CallApiStringJson(sexo, dni, pais, 0, ApiPersonas.ApiConsultaCaracteristicasPersonaJson);
        }

        [Route("consulta-datos-basicos-json")]
        public string GetApiConsultaDatosBasicosJson(string sexo, string dni, string pais)
        {
            return CallApiStringJson(sexo, dni, pais, 0, ApiPersonas.ApiConsultaDatosBasicosJson);
        }

        [Route("consulta-datos-con-caracteristicas-json")]
        public string GetApiConsultaDatosConCaracteristicasJson(string sexo, string dni, string pais)
        {
            return CallApiStringJson(sexo, dni, pais, 0, ApiPersonas.ApiConsultaDatosConCaracteristicasJson);
        }

        [Route("consulta-datos-con-domicilios-json")]
        public string GetApiConsultaDatosConDomiciliosJson(string sexo, string dni, string pais)
        {
            return CallApiStringJson(sexo, dni, pais, 0, ApiPersonas.ApiConsultaDatosConDomiciliosJson);
        }

        [Route("consulta-caracteristicas-persona")]
        public object GetApiConsultaCaracteristicasPersona(string sexo, string dni, string pais)
        {
            return CallApiObject(sexo, dni, pais, 0, ApiPersonas.ApiConsultaCaracteristicasPersona);
        }

        [Route("consulta-datos-basicos")]
        public object GetApiConsultaDatosBasicos(string sexo, string dni, string pais)
        {
            return CallApiObject(sexo, dni, pais, 0, ApiPersonas.ApiConsultaDatosBasicos);
        }

        [Route("consulta-datos-completo")]
        public object GetApiConsultaDatosCompleto(string sexo, string dni, string pais)
        {
            var hash = HttpContext.Current.Request.Cookies["CiDi"].Value;
            PersonaUnica personaUnica = (PersonaUnica)CallApiObject(sexo, dni, pais, 0, ApiPersonas.ApiConsultaDatosCompleto);
            if (string.IsNullOrEmpty(personaUnica.CUIL))
                personaUnica.CUIL = CalcularCuil(sexo, dni);

            UsuarioCidi cuentaCidi = ApiCuenta.ObtenerUsuarioPorCuil(hash, personaUnica.CUIL);

            var persona = new Persona
            {
                Nombre = personaUnica.Nombre,
                Apellido = personaUnica.Apellido,
                NombreSexo = personaUnica.Sexo.Nombre,
                SexoId = personaUnica.Sexo.IdSexo,
                IdNumero = personaUnica.Id_Numero,
                CodigoPais = personaUnica.PaisTD.IdPais,
                Nacionalidad = personaUnica.Nacionalidad?.Nacionalidad ?? "",
                TipoDocumento = personaUnica.TipoDocumento?.Nombre ?? "",
                NroDocumento = personaUnica.NroDocumento,
                FechaNacimiento = personaUnica.FechaNacimiento,
                FechaDefuncion = personaUnica.FechaDefuncion,
                DomicilioIdVin = personaUnica.DomicilioGrupoFamiliar?.IdVin ?? 0,
                DomicilioGrupoFamiliar = personaUnica.DomicilioGrupoFamiliar?.DireccionCompleta ?? "",
                DomicilioGrupoFamiliarLocalidad = personaUnica.DomicilioGrupoFamiliar?.Localidad?.Nombre ?? "",
                DomicilioGrupoFamiliarDepartamento = personaUnica.DomicilioGrupoFamiliar?.Departamento?.Nombre ?? "",
                DomicilioRealIdVin = personaUnica.DomicilioReal?.IdVin ?? 0,
                DomicilioReal = personaUnica.DomicilioReal?.DireccionCompleta ?? "",
                DomicilioRealLocalidad = personaUnica.DomicilioReal?.Localidad?.Nombre ?? "",
                DomicilioRealDepartamento = personaUnica.DomicilioReal?.Departamento?.Nombre ?? "",
                Barrio = personaUnica.DomicilioReal?.Barrio?.Nombre ?? personaUnica.DomicilioGrupoFamiliar?.Barrio?.Nombre ?? "",
                Cuil = personaUnica.CUIL,
                CodigoArea = cuentaCidi.TelArea,
                Telefono = cuentaCidi.TelNro,
                CodigoAreaCelular = cuentaCidi.CelArea,
                Celular = cuentaCidi.CelNro,
                Email = cuentaCidi.Email
            };

            return persona;
        }

        [Route("consulta-datos-con-caracteristicas")]
        public object GetApiConsultaDatosConCaracteristicas(string sexo, string dni, string pais)
        {
            return CallApiObject(sexo, dni, pais, 0, ApiPersonas.ApiConsultaDatosConCaracteristicas);
        }

        [Route("consulta-datos-con-domicilios")]
        public object GetApiConsultaDatosConDomicilios(string sexo, string dni, string pais)
        {
            return CallApiObject(sexo, dni, pais, 0, ApiPersonas.ApiConsultaDatosConDomicilios);
        }
        private string CalcularCuil(string sexo, string dni)
        {
            if (string.IsNullOrEmpty(sexo) || string.IsNullOrEmpty(dni) || dni.Length > 8)
                return null;

            int verificacion;

            if (!int.TryParse(dni, out verificacion))
                return null;

            if (verificacion < 10000000)
            {
                int n = 8 - dni.Length;
                for (int i = 0; i < n; i++)
                {
                    dni = string.Concat("0", dni);
                }
            }
            string prefijo;

            switch (sexo)
            {
                case "01":
                    prefijo = "20";
                    break;
                case "02":
                    prefijo = "27";
                    break;
                default:
                    return null;
            }

            string cuil = string.Concat(prefijo, dni);

            int control = (
                              int.Parse(cuil[0].ToString()) * 5 +
                              int.Parse(cuil[1].ToString()) * 4 +
                              int.Parse(cuil[2].ToString()) * 3 +
                              int.Parse(cuil[3].ToString()) * 2 +
                              int.Parse(cuil[4].ToString()) * 7 +
                              int.Parse(cuil[5].ToString()) * 6 +
                              int.Parse(cuil[6].ToString()) * 5 +
                              int.Parse(cuil[7].ToString()) * 4 +
                              int.Parse(cuil[8].ToString()) * 3 +
                              int.Parse(cuil[9].ToString()) * 2
                          ) % 11;

            string posfijo;

            switch (control)
            {
                case 0:
                    posfijo = "0";
                    break;
                case 1:
                    prefijo = "23";
                    posfijo = sexo == "01" ? "9" : "4";
                    break;
                default:
                    posfijo = (11 - control).ToString();
                    break;
            }

            cuil = string.Concat(prefijo, dni, posfijo);

            return cuil;
        }

    }
}
