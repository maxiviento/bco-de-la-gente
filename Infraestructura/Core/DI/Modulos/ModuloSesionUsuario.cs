using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Ninject.Modules;
using Ninject.Web.Common;

namespace Infraestructura.Core.DI.Modulos
{
    public class ModuloSesionUsuario : NinjectModule
    {
        public static readonly string Id = "Id";
        public static readonly string Nombre = "Nombre";
        public static readonly string Cuil = "Cuil";
        public static readonly string Apellido = "Apellido";
        public static readonly string Email = "Email";
        public static readonly string Token = "Token";

        public override void Load()
        {
            Kernel.Bind<ISesionUsuario>()
                .ToMethod(x => ResolverSesionUsuario())
                .InRequestScope();
        }

        public ISesionUsuario ResolverSesionUsuario()
        {
            
            var identity =(ClaimsIdentity) HttpContext.Current.User.Identity;
            var claimId = GetInfoFromClaim(identity, Id);
            ISesionUsuario  sesionUsuario = new SesionUsuarioImpl();

            if (!string.IsNullOrEmpty(claimId))
            {
                var usuario = new Usuario()
                {
                    Apellido = GetInfoFromClaim(identity, Apellido),
                    Cuil = GetInfoFromClaim(identity, Cuil),
                    Email = GetInfoFromClaim(identity, Email),
                    Nombre = GetInfoFromClaim(identity, Nombre),
                    Id = new Id(claimId)
                };

                sesionUsuario = new SesionUsuarioImpl()
                {
                    Usuario = usuario,
                    CiDiHash = GetInfoFromClaim(identity, Token)
                };
            }

            return sesionUsuario;
        }

        private static string GetInfoFromClaim(IIdentity identiy, string claim)
        {
            var identity = ((ClaimsIdentity)identiy);
            var singleOrDefault = identity.Claims.SingleOrDefault(x => x.Type == claim);
            if (singleOrDefault != null)
                return singleOrDefault.Value;
            return string.Empty;
        }

        public class SesionUsuarioImpl : ISesionUsuario
        {
            public Usuario Usuario { get; internal set; }
            public string CiDiHash { get; internal set; }
            public bool EstaCerrada { get; internal set; }
        }
    }
}
