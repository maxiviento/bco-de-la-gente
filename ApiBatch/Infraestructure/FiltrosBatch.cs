using Infraestructura.Core.Filtros;
using System.Web.Http;

namespace ApiBatch.Infraestructure
{
    public class FiltrosBatch
    {
        public static void Registrar(HttpConfiguration config)
        {
            config.Filters.Add(new ExceptionFilter());
            config.Filters.Add(new ValidateModelFilter());
            //config.Filters.Add(new CustomAuthorizeAttribute());            
        }
    }
}