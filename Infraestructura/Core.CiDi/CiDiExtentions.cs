using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Infraestructura.Core.CiDi.Configuration;
using Infraestructura.Core.CiDi.OAuth;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Infraestructura.Core.CiDi
{
    public static class CiDiExtentions
    {
        public static void UseCiDi(this IAppBuilder builder)
        {
            var path = "/autenticacion/token";

#if DEBUG
            path = "/api/autenticacion/token";
#endif
            builder.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString(path),
                Provider = new OAuthAuthorizationProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(8)
            });

            builder.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions()
            {
                Provider = new QueryStringOAuthBearerProvider("access_token")
            });

            builder.Use(async (context, next) =>
            
            {
                var cookie = context.Request.Cookies["CiDi"];

                if (cookie == null)
                {
                    var urlLogin = CidiConfigurationManager.GetCidiEndpoint(UrlCidiEnum.IniciarSesion);
                    context.Response.StatusCode = 403;
                    context.Response.Headers.Add("X-Auth-Login-Path", new[] { urlLogin });
                    return;
                }

                await next.Invoke();
            });
        }


        private static string GetInfoFromClaim(IIdentity identiy, string claim)
        {
            var identity = ((ClaimsIdentity) identiy);
            var singleOrDefault = identity.Claims.SingleOrDefault(x => x.Type == claim);
            if (singleOrDefault != null)
                return singleOrDefault.Value;
            return string.Empty;
        }


        public static string Nombre(this IIdentity identiy)
        {
            return GetInfoFromClaim(identiy, CiDiClaimTypes.Nombre);
        }

        public static string CiDiHash(this IIdentity identiy)
        {
            return GetInfoFromClaim(identiy, CiDiClaimTypes.Token);
        }

        public static string Cuil(this IIdentity identiy)
        {
            return GetInfoFromClaim(identiy, CiDiClaimTypes.Cuil);
        }

        public static string Apellido(this IIdentity identiy)
        {
            return GetInfoFromClaim(identiy, CiDiClaimTypes.Apellido);
        }

        public static string Email(this IIdentity identiy)
        {
            return GetInfoFromClaim(identiy, CiDiClaimTypes.Email);
        }

        public static string UsuarioId(this IIdentity identiy)
        {
            return GetInfoFromClaim(identiy, CiDiClaimTypes.Id);
        }
    }
}