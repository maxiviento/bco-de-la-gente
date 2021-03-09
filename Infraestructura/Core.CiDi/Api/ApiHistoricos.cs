using System;
using AppComunicacion;
using Infraestructura.Core.CiDi.Util;
using Infraestructura.Core.Comun.Excepciones;

namespace Infraestructura.Core.CiDi.Api
{
    public static class ApiHistoricos
    {
        #region Urls

        public static string UrlHistoricoPersona(string cuilUsuarioLogueado, string sexo, string dni, string pais, int? idNumero)
        {
            return Url(cuilUsuarioLogueado, sexo, dni, pais, RolesHistoricos.CONSULTAR_HIST_CAR_PERSONA, idNumero);
        }

        public static string UrlHistoricoGrupo(string cuilUsuarioLogueado, string sexo, string dni, string pais, int? idNumero)
        {
            return Url(cuilUsuarioLogueado, sexo, dni, pais, RolesHistoricos.CONSULTAR_HIST_GRUPO_PERSONA, idNumero);
        }

        private static string Url(string cuilUsuarioLogueado, string sexo, string dni, string pais, RolesHistoricos rol, int? idNumero)
        {
            try
            {
                return AppComunicacionUtil.GetServicio().Historicos(cuilUsuarioLogueado,
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