using System;
using Infraestructura.Core.CiDi.Util;
using System.Collections.Generic;
using AppComunicacion;
using AppComunicacion.ApiModels;
using Infraestructura.Core.Comun.Excepciones;

namespace Infraestructura.Core.CiDi.Api
{
    public static class ApiPersonas
    {
        #region Apis Persona

        public static PersonaUnica ApiConsultaDatosBasicos(string cookieHash, string sexo, string dni, string pais, int? idNumero)
        {            
            return Model(cookieHash, sexo, dni, pais, RolesAPIPersonas.CONSULTAR_DATOS_BASICOS, idNumero);
        }
        
        public static PersonaUnica ApiConsultaDatosCompleto(string cookieHash, string sexo, string dni, string pais, int? idNumero)
        {
            return Model(cookieHash, sexo, dni, pais, RolesAPIPersonas.CONSULTAR_DATOS_COMPLETO, idNumero);
        }

        public static PersonaUnica ApiConsultaDatosConCaracteristicas(string cookieHash, string sexo, string dni, string pais, int? idNumero)
        {
            return Model(cookieHash, sexo, dni, pais, RolesAPIPersonas.CONSULTAR_DATOS_CON_CARACTERISTICAS, idNumero);
        }

        public static PersonaUnica ApiConsultaDatosConDomicilios(string cookieHash, string sexo, string dni, string pais, int? idNumero)
        {
            return Model(cookieHash, sexo, dni, pais, RolesAPIPersonas.CONSULTAR_DATOS_CON_DOMICILIOS, idNumero);
        }

        public static string ApiConsultaDatosBasicosJson(string cookieHash, string sexo, string dni, string pais, int? idNumero)
        {
            return ModelJson(cookieHash, sexo, dni, pais, RolesAPIPersonas.CONSULTAR_DATOS_BASICOS, idNumero);
        }

        public static string ApiConsultaDatosCompletoJson(string cookieHash, string sexo, string dni, string pais, int? idNumero)
        {
            return ModelJson(cookieHash, sexo, dni, pais, RolesAPIPersonas.CONSULTAR_DATOS_COMPLETO, idNumero);
        }

        public static string ApiConsultaDatosConCaracteristicasJson(string cookieHash, string sexo, string dni, string pais, int? idNumero)
        {
            return ModelJson(cookieHash, sexo, dni, pais, RolesAPIPersonas.CONSULTAR_DATOS_CON_CARACTERISTICAS, idNumero);
        }

        public static string ApiConsultaDatosConDomiciliosJson(string cookieHash, string sexo, string dni, string pais, int? idNumero)
        {
            return ModelJson(cookieHash, sexo, dni, pais, RolesAPIPersonas.CONSULTAR_DATOS_CON_DOMICILIOS, idNumero);
        }

        private static PersonaUnica Model(string cookieHash, string sexo, string dni, string pais, RolesAPIPersonas rol, int? idNumero)
        {
            try
            {
                return AppComunicacionUtil.GetServicio().ApiPersonas(cookieHash, AppComunicacionUtil.GenerarPersonaFiltro(sexo, dni, pais, idNumero), rol);
            }
            catch (Exception ex)
            {
                throw new GrupoUnicoException("Error Grupo Único.", ex, ex.Source);
            }
        }

        private static string ModelJson(string cookieHash, string sexo, string dni, string pais, RolesAPIPersonas rol, int? idNumero)
        {
            return AppComunicacionUtil.GetServicio().ApiPersonasJSON(cookieHash, AppComunicacionUtil.GenerarPersonaFiltro(sexo, dni, pais, idNumero), rol);
        }

        #endregion

        #region Apis Caracteristicas Persona

        public static List<Caracteristica> ApiConsultaCaracteristicasPersona(string cookieHash, string sexo, string dni, string pais, int? idNumero)
        {
            return Model(cookieHash, sexo, dni, pais, idNumero, RolesAPICaracteristicasPersona.CONSULTAR);
        }
      
        public static string ApiConsultaCaracteristicasPersonaJson(string cookieHash, string sexo, string dni, string pais, int? idNumero)
        {
            return ModelJson(cookieHash, sexo, dni, pais, idNumero, RolesAPICaracteristicasPersona.CONSULTAR);
        }

        private static List<Caracteristica> Model(string cookieHash, string sexo, string dni, string pais, int? idNumero, RolesAPICaracteristicasPersona rol)
        {
            try
            {
                return AppComunicacionUtil.GetServicio().ApiCaracteristicasPersona(cookieHash, AppComunicacionUtil.GenerarPersonaFiltro(sexo, dni, pais, idNumero), rol);
            }
            catch (Exception ex)
            {
                throw new GrupoUnicoException("Error Grupo Único.", ex);
            }
        }

        private static string ModelJson(string cookieHash, string sexo, string pais, string documento, int? idNumero, RolesAPICaracteristicasPersona rol)
        {
            var persona = new PersonaFiltro() { Sexo = sexo, PaisTD = pais, NroDocumento = documento, Id_numero = idNumero};
            return  AppComunicacionUtil.GetServicio().ApiCaracteristicasPersonaJSON(cookieHash, persona, rol);
        }

        #endregion

        #region Urls

        public static string UrlAlta(string cuilUsuarioLogueado, string sexo, string dni, string pais, int? idNumero)
        {
            return Url(cuilUsuarioLogueado, sexo, dni, pais, RolesPersonas.ALTA_PERSONA, idNumero);
        }

        public static string UrlConsultarCompleto(string cuilUsuarioLogueado, string sexo, string dni, string pais, int? idNumero)
        {
            return Url(cuilUsuarioLogueado, sexo, dni, pais, RolesPersonas.CONSULTAR_PERSONA_COMPLETO, idNumero);
        }

        public static string UrlConsultarBasico(string cuilUsuarioLogueado, string sexo, string dni, string pais, int? idNumero)
        {
            return Url(cuilUsuarioLogueado, sexo, dni, pais, RolesPersonas.CONSULTAR_PERSONA_BASICO, idNumero);
        }
        
        public static string UrlModificar(string cuilUsuarioLogueado, string sexo, string dni, string pais, int? idNumero)
        {
            return Url(cuilUsuarioLogueado, sexo, dni, pais, RolesPersonas.MODIFICAR_PERSONA, idNumero);
        }

        public static string UrlConsultarCaracteristicas(string cuilUsuarioLogueado, string sexo, string dni, string pais, int? idNumero)
        {
            return Url(cuilUsuarioLogueado, sexo, dni, pais, RolesPersonas.CONSULTAR_CARACTERISTICAS, idNumero);
        }

        public static string UrlModificarCaracteristicas(string cuilUsuarioLogueado, string sexo, string dni, string pais, int? idNumero)
        {
            return Url(cuilUsuarioLogueado, sexo, dni, pais, RolesPersonas.MODIFICAR_CARACTERISTICAS, idNumero);
        }

        private static string Url(string cuilUsuarioLogueado, string sexo, string dni, string pais, RolesPersonas rol, int? idNumero)
        {
            try
            {
                return AppComunicacionUtil.GetServicio().Personas(cuilUsuarioLogueado,
                    AppComunicacionUtil.GenerarPersonaFiltro(sexo, dni, pais, idNumero), rol);
            }
            catch (Exception ex)
            {
                throw new GrupoUnicoException("Error Grupo Único.", ex);
            }
        }

        #endregion
    }
}
