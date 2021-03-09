using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace Infraestructura.Core.CiDi.OAuth
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (AuthorizeRequest(actionContext))
            {
                return;
            }
            HandleUnauthorizedRequest(actionContext);
        }

        private bool AuthorizeRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;

            if (identity.IsAuthenticated)
            {
                //var perfilesRepositorio = (IPerfilRepositorio)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IPerfilRepositorio));
                //var perfilesParaUsuario = perfilesRepositorio.ObtenerPerfilesPorUsuarioId(new Id(identity.UsuarioId()));

                //var method = actionContext.Request.Method.ToString();
                //var url = actionContext.Request.RequestUri.LocalPath;
                return true;
            }
            //Write your code here to perform authorization

            //TODO cambiar a false para probar con la aplicacion
            return false;

        }
    }
}
