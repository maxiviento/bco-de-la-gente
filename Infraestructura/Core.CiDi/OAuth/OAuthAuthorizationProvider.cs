using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Identidad.Dominio.IRepositorio;
using Infraestructura.Core.CiDi.Api;
using Infraestructura.Core.CiDi.Configuration;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace Infraestructura.Core.CiDi.OAuth
{
    public class OAuthAuthorizationProvider : OAuthAuthorizationServerProvider
    {

        private readonly string CustomGrantTypeName = "cidi";

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            var authorizedClientId = CidiConfigurationManager.GetCidiEnvironment().ClientKey;
            var clientId = string.Empty;
            var clientSecret = string.Empty;

            if (context.TryGetBasicCredentials(out clientId, out clientSecret) ||
                context.TryGetFormCredentials(out clientId, out clientSecret))
            {

                if (!string.IsNullOrEmpty(clientId))
                {
                    if (authorizedClientId.Equals(clientId))
                    {
                        context.Validated(clientId);
                    }
                }
            }
        }

        public override async Task GrantCustomExtension(OAuthGrantCustomExtensionContext context)
        {
            if (context.GrantType.Equals(CustomGrantTypeName))
            {

                var tokenRequest = TokenRequest.From(context.Request);

                var usuario = ApiCuenta.ObtenerUsuarioActivo(tokenRequest.CidiHash);

                if (usuario != null && !string.IsNullOrEmpty(usuario.CUIL))
                {
                    var usuarioReporitorio =
                        (IUsuarioRepositorio)
                        GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IUsuarioRepositorio));

                    var usuarioGuardado = usuarioReporitorio.ConsultarUsuarioPorCuil(tokenRequest.CidiHash, usuario.CUIL);

                    var identity = new ClaimsIdentity(new List<Claim>()
                    {
                        new Claim(CiDiClaimTypes.Id, usuarioGuardado.Id.ToString()),
                        new Claim(CiDiClaimTypes.Cuil, usuario.CUIL),
                        new Claim(CiDiClaimTypes.Nombre, usuario.Nombre),
                        new Claim(CiDiClaimTypes.Apellido, usuario.Apellido),
                        new Claim(CiDiClaimTypes.Email, usuario.Email),
                        new Claim(CiDiClaimTypes.Genero, usuario.Id_Sexo),
                        new Claim(CiDiClaimTypes.NumeroMovil, usuario.TelFormateado),
                        new Claim(CiDiClaimTypes.Token, tokenRequest.CidiHash)
                    }, "ExternalBearer");

                    var authProps = new AuthenticationProperties();

                    authProps.Dictionary.Add("client_id", tokenRequest.ClientId);
                    //authProps.IssuedUtc = DateTime.UtcNow;
                    //authProps.ExpiresUtc = DateTime.UtcNow.AddHours(2);
                    authProps.AllowRefresh = true;

                    var ticket = new AuthenticationTicket(identity, authProps);
                    
                    context.Validated(ticket);
                }
                else
                {

                    var urlLogin = CidiConfigurationManager.GetCidiEndpoint(UrlCidiEnum.IniciarSesion);
                    context.Response.Headers.Add("X-Auth-Login-Path", new[] {urlLogin});

                    context.SetError("Iniciar sesion en CiDi");

                }

            }
        }
    }
}