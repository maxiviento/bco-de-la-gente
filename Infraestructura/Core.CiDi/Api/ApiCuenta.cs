using System;
using Infraestructura.Core.CiDi.Configuration;
using Infraestructura.Core.CiDi.Model;
using Infraestructura.Core.CiDi.Util;

namespace Infraestructura.Core.CiDi.Api
{
    public static class ApiCuenta
    {
        public static UsuarioCidi ObtenerUsuarioActivo(string cookieHash)
        {
            return ObtenerUsuario(cookieHash, null);
        }

        public static UsuarioCidi ObtenerUsuarioPorCuil(string cookieHash, string cuil)
        {
            return (string.IsNullOrEmpty(cuil)) ? null : ObtenerUsuario(cookieHash, cuil);
        }

        public static bool EsUsuarioNivelDos(string cookieHash, string cuil)
        {
            return (!string.IsNullOrEmpty(cuil)) && EsUsuarioNivelDos(ObtenerUsuario(cookieHash, null));
        }

        public static bool EsUsuarioNivelDos(string cookieHash)
        {
            return EsUsuarioNivelDos(ObtenerUsuario(cookieHash, null));
        }

        public static bool EsUsuarioNivelDos(UsuarioCidi usuario)
        {
            return usuario.Id_Estado.HasValue && usuario.Id_Estado.Value == 2;
        }

        public static string CerrarSesionCidi()
        {
            return CerrarSesion();
        }

        private static UsuarioCidi ObtenerUsuario(string cookieHash, string cuil)
        {
            var cidiEnvironment = CidiConfigurationManager.GetCidiEnvironment();

            var entrada = new Entrada
            {
                IdAplicacion = cidiEnvironment.IdApplication,
                Contrasenia = cidiEnvironment.ClientSecret,
                HashCookie = cookieHash,
                TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff")
            };

            if (!string.IsNullOrEmpty(cuil))
                entrada.CUIL = cuil;

            entrada.TokenValue = TokenUtil.ObtenerToken_SHA1(entrada.TimeStamp, cidiEnvironment.ClientKey);
            var optenerCuentaUrl = string.IsNullOrEmpty(cuil)
                ? GlobalVars.ApiCuenta.Usuario.ObtenerUsuarioAplicacion
                : GlobalVars.ApiCuenta.Usuario.ObtenerUsuario;
            return HttpWebRequestUtil.LlamarWebApi<Entrada, UsuarioCidi>(optenerCuentaUrl, entrada);
        }

        private static string CerrarSesion()
        {
            return CidiConfigurationManager.GetCidiEndpoint(UrlCidiEnum.CerrarSesion);
        }
    }
}
