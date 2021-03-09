using System.Web.Http;
using Infraestructura.Core.CiDi.OAuth;

namespace Infraestructura.Core.Filtros
{
    public class Filtros
    {
        public static void Registrar(HttpConfiguration config)
        {
            if (FiltrosSettings.HabilitarValidacionModelo())
            {
                config.Filters.Add(new ValidateModelFilter());
            }

            if (FiltrosSettings.HabilitarAutorizacion())
            {
                config.Filters.Add(new CustomAuthorizeAttribute());
            }
        }
    }
}