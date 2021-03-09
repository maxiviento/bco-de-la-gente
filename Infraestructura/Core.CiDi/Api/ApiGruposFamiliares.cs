using System;
using AppComunicacion;
using AppComunicacion.ApiModels;
using Infraestructura.Core.CiDi.Util;
using Infraestructura.Core.Comun.Excepciones;

namespace Infraestructura.Core.CiDi.Api
{
    public static class ApiGruposFamiliares
    {
        #region Apis

        public static RespuestaAPIGrupoFamiliar ApiConsultaGrupos(string cookieHash, string sexo, string dni, string pais, int? idNumero)
        {
            return Model(cookieHash, sexo, dni, pais, RolesAPIGruposFamiliar.CONSULTAR_GRUPOS_CON_CARACT_DE_PERSONAS, idNumero);
        }

        public static PersonaUnica ApiConsultaPersona(string cookieHash, string sexo, string dni, string pais, int? idNumero)
        {
            return ModelPersona(cookieHash, sexo, dni, pais, RolesAPIPersonas.CONSULTAR_DATOS_COMPLETO, idNumero);
        }

        public static RespuestaAPIGrupoFamiliar ApiConsultaGruposConCaractPersonas(string cookieHash, string sexo, string dni, string pais, int? idNumero)
        {
            return Model(cookieHash, sexo, dni, pais, RolesAPIGruposFamiliar.CONSULTAR_GRUPOS_CON_CARACT_DE_PERSONAS, idNumero);
        }

        public static string ApiConsultaGruposJson(string cookieHash, string sexo, string dni, string pais, int? idNumero)
        {
            return ModelJson(cookieHash, sexo, dni, pais, RolesAPIGruposFamiliar.CONSULTAR_GRUPOS, idNumero);
        }

        public static string ApiConsultaGruposConCaractPersonasJson(string cookieHash, string sexo, string dni, string pais, int? idNumero)
        {
            return ModelJson(cookieHash, sexo, dni, pais, RolesAPIGruposFamiliar.CONSULTAR_GRUPOS_CON_CARACT_DE_PERSONAS, idNumero);
        }

        private static RespuestaAPIGrupoFamiliar Model(string cookieHash, string sexo, string dni, string pais, RolesAPIGruposFamiliar rol, int? idNumero)
        {

            try
            {
                return AppComunicacionUtil.GetServicio().ApiGruposFamiliares(cookieHash, AppComunicacionUtil.GenerarPersonaFiltro(sexo, dni, pais, idNumero), rol);
            }
            catch (Exception ex)
            {
                throw new GrupoUnicoException("Error Grupo Único.", ex, ex.Source);
            }
        }

        private static PersonaUnica ModelPersona(string cookieHash, string sexo, string dni, string pais, RolesAPIPersonas rol, int? idNumero)
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

        private static string ModelJson(string cookieHash, string sexo, string dni, string pais, RolesAPIGruposFamiliar rol, int? idNumero)
        {
            return AppComunicacionUtil.GetServicio().ApiGruposFamiliaresJSON(cookieHash, AppComunicacionUtil.GenerarPersonaFiltro(sexo, dni, pais, idNumero), rol);
        }

        #endregion

        #region Urls

        public static string UrlConsultarGrupos(string cuilUsuarioLogueado, string sexo, string dni, string pais, int? idNumero)
        {
            return Url(cuilUsuarioLogueado, sexo, dni, pais, RolesGruposFamiliar.CONSULTAR_GRUPOS, idNumero);
        }

        public static string UrlModificarAplicacionInterna(string cuilUsuarioLogueado, string sexo, string dni, string pais, int? idNumero)
        {
            return Url(cuilUsuarioLogueado, sexo, dni, pais, RolesGruposFamiliar.MODIFICAR_GRUPOS_APP_INTERNA, idNumero);
        }


        private static string Url(string cuilUsuarioLogueado, string sexo, string dni, string pais, RolesGruposFamiliar rol, int? idNumero)
        {
            try
            {
                return AppComunicacionUtil.GetServicio().GruposFamiliares(cuilUsuarioLogueado, AppComunicacionUtil.GenerarPersonaFiltro(sexo, dni, pais, idNumero), rol);
            }
            catch (Exception ex)
            {
                throw new GrupoUnicoException("Error Grupo Único.", ex, ex.Source);
            }
        }

        #endregion
    }
}
