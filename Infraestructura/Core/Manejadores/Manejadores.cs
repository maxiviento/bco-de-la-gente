using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Infraestructura.Core.Manejadores;

namespace Infraestructura.Core.Manejadores
{
    public class Manejadores
    {
        public static void Registrar(HttpConfiguration config)
        {
            config.MessageHandlers.Add(new MessageHandler());
            config.Services.Replace(typeof(IExceptionHandler), new CustomExceptionHandler());
        }
    }
}
