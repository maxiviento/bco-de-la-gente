using System;
using System.Text;
using AppComunicacion;
using AppComunicacion.ApiModels;
using Infraestructura.Core.CiDi.Configuration;
using Infraestructura.Core.CiDi.Util;
using Infraestructura.Core.Comun.Excepciones;

namespace Infraestructura.Core.CiDi.Api
{
    public static class ApiDomicilios
    {
        #region Apis Domicilio

        public static Domicilio ApiConsultaDatosBasicos(string cookieHash, string idVin)
        {
            return Model(cookieHash, idVin, RolesAPIDomicilios.CONSULTAR_DATOS_BASICOS);
        }

        public static Domicilio ApiConsultaConCaracteristicas(string cookieHash, string idVin)
        {
            return Model(cookieHash, idVin, RolesAPIDomicilios.CONSULTAR_CON_CARACTERISTICAS);
        }

        public static Domicilio ApiConsultaDomicilioGenerado(string cookieHash, string sexo, string dni, string pais,
            int tipoDomicilio, string cuilUsuario, int? idNumero)
        {
            return Model(cookieHash, sexo, dni, pais, tipoDomicilio, cuilUsuario, idNumero,
                RolesAPIDomicilios.CONSULTAR_DOMICILIO_GEN);
        }

        public static string ApiConsultaDatosBasicosJson(string cookieHash, string idVin)
        {
            return ModelJson(cookieHash, idVin, RolesAPIDomicilios.CONSULTAR_DATOS_BASICOS);
        }

        public static string ApiConsultaConCaracteristicasJson(string cookieHash, string idVin)
        {
            return ModelJson(cookieHash, idVin, RolesAPIDomicilios.CONSULTAR_CON_CARACTERISTICAS);
        }

        public static string ApiConsultaDomicilioGeneradoJson(string cookieHash, string sexo, string dni, string pais,
            int tipoDomicilio, string cuilUsuario, int? idNumero)
        {
            return ModelJson(cookieHash, sexo, dni, pais, tipoDomicilio, cuilUsuario, idNumero,
                RolesAPIDomicilios.CONSULTAR_DOMICILIO_GEN);
        }

        private static Domicilio Model(string cookieHash, string sexo, string dni, string pais, int tipoDomicilio,
            string cuilUsuario, int? idNumero, RolesAPIDomicilios rol)
        {
            try
            {
                var cidiEnvironment = CidiConfigurationManager.GetCidiEnvironment();
                var idApp = cidiEnvironment.IdApplication;
                return AppComunicacionUtil.GetServicio().ApiDomicilios(cookieHash,
                    GenerateIdDomEntidad(sexo, dni, pais, idNumero, idApp), rol, idApp, tipoDomicilio, cuilUsuario);
            }
            catch (Exception ex)
            {
                throw new GrupoUnicoException("Error Grupo Único.", ex, ex.Source);
            }
        }

        private static string ModelJson(string cookieHash, string sexo, string dni, string pais, int tipoDomicilio,
            string cuilUsuario, int? idNumero, RolesAPIDomicilios rol)
        {
            var cidiEnvironment = CidiConfigurationManager.GetCidiEnvironment();
            var idApp = cidiEnvironment.IdApplication;
            return AppComunicacionUtil.GetServicio().ApiDomiciliosJSON(cookieHash,
                GenerateIdDomEntidad(sexo, dni, pais, idNumero, idApp), rol, idApp, tipoDomicilio, cuilUsuario);
        }

        private static Domicilio Model(string cookieHash, string idVin, RolesAPIDomicilios rol)
        {
            try
            {
                var cidiEnvironment = CidiConfigurationManager.GetCidiEnvironment();
                var idApp = cidiEnvironment.IdApplication;
                return AppComunicacionUtil.GetServicio().ApiDomicilios(cookieHash, idVin, rol);
            }
            catch (Exception ex)
            {
                throw new GrupoUnicoException("Error Grupo Único.", ex, ex.Source);
            }
        }

        private static string ModelJson(string cookieHash, string idVin, RolesAPIDomicilios rol)
        {
            var cidiEnvironment = CidiConfigurationManager.GetCidiEnvironment();
            var idApp = cidiEnvironment.IdApplication;
            return AppComunicacionUtil.GetServicio().ApiDomiciliosJSON(cookieHash, idVin, rol);
        }

        #endregion


        #region Apis Caracteristicas Domicilio

        public static CaracteristicasDomicilio ApiConsultaCaracteristicasDomicilio(string cookieHash,
            string sexoYDniConcatenado)
        {
            return Model(cookieHash, sexoYDniConcatenado, RolesAPICaracteristicasDomicilio.CONSULTAR);
        }

        public static string ApiConsultaCaracteristicasDomicilioJson(string cookieHash, string sexoYDniConcatenado)
        {
            return ModelJson(cookieHash, sexoYDniConcatenado, RolesAPICaracteristicasDomicilio.CONSULTAR);
        }

        private static CaracteristicasDomicilio Model(string cookieHash, string idVin,
            RolesAPICaracteristicasDomicilio rol)
        {
            try
            {
                return AppComunicacionUtil.GetServicio().ApiCaracteristicasDomicilio(cookieHash, idVin, rol);
            }
            catch (Exception ex)
            {
                throw new GrupoUnicoException("Error Grupo Único.", ex, ex.Source);
            }
        }

        private static string ModelJson(string cookieHash, string idVin, RolesAPICaracteristicasDomicilio rol)
        {
            return AppComunicacionUtil.GetServicio().ApiCaracteristicasDomicilioJSON(cookieHash, idVin, rol);
        }

        #endregion


        #region Urls

        public static string UrlConsultar(string cuilUsuarioLogueado, string idVin)
        {
            return Url(cuilUsuarioLogueado, idVin, RolesDomicilio.CONSULTAR_DOMICILIO);
        }

        public static string UrlModificarCaracteristicas(string cuilUsuarioLogueado, string idVin)
        {
            return Url(cuilUsuarioLogueado, idVin, RolesDomicilio.MODIFICAR_CARACTERISTICAS);
        }

        public static string UrlConsultarCaracteristicas(string cuilUsuarioLogueado, string idVin)
        {
            return Url(cuilUsuarioLogueado, idVin, RolesDomicilio.CONSULTAR_CARACTERISTICAS);
        }

        public static string UrlAlta(string cuilUsuarioLogueado, string sexo, string dni, string pais,
            int tipoDomicilio, int jurisdiccion, int? idNumero)
        {
            JurisdiccionDomicilio jurisdiccionDom = (JurisdiccionDomicilio)jurisdiccion;

            return Url(cuilUsuarioLogueado, sexo, dni, pais, tipoDomicilio, jurisdiccionDom,
                RolesDomicilio.ALTA_DOMICILIO_GENERICO, idNumero);
        }

        private static string Url(string cuilUsuarioLogueado, string sexo, string dni, string pais, int tipoDomicilio,
            JurisdiccionDomicilio jurisdiccion, RolesDomicilio rol, int? idNumero)
        {
            try
            {
                var cidiEnvironment = CidiConfigurationManager.GetCidiEnvironment();
                var idApp = cidiEnvironment.IdApplication;
                return AppComunicacionUtil.GetServicio().Domicilios(cuilUsuarioLogueado,
                    GenerateIdDomEntidad(sexo, dni, pais, idNumero, idApp), rol, tipoDomicilio, jurisdiccion);
            }
            catch (Exception ex)
            {
                throw new GrupoUnicoException("Error Grupo Único.", ex, ex.Source);
            }
        }

        private static string Url(string cuilUsuarioLogueado, string idVin, RolesDomicilio rol)
        {
            try
            {
                var cidiEnvironment = CidiConfigurationManager.GetCidiEnvironment();
                var idApp = cidiEnvironment.IdApplication;
                return AppComunicacionUtil.GetServicio().Domicilios(cuilUsuarioLogueado, idVin, rol);
            }
            catch (Exception ex)
            {
                throw new GrupoUnicoException("Error Grupo Único.", ex, ex.Source);
            }
        }

        private static string GenerateIdDomEntidad(string sexo, string dni, string pais, int? idNumero, int idApp)
        {
            StringBuilder sb = new StringBuilder(sexo);
            sb.Append(dni)
                .Append(idApp)
                .Append(pais)
                .Append(idNumero);
            return sb.ToString();
        }

        #endregion
    }
}