using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http.Filters;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;
using log4net;
using System.Configuration;

namespace Infraestructura.Core.Filtros
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(ExceptionFilter));
        /// <summary>
        /// Se encarga de capturar y manejar las excepciones no controladas de la aplicación.
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            System.Configuration.Configuration configuration =
                WebConfigurationManager.OpenWebConfiguration("/");

            CustomErrorsSection section =
                (CustomErrorsSection) configuration.GetSection("system.web/customErrors");

            var inner = actionExecutedContext.Exception.InnerException;
            var exceptionMessage = (inner == null) ? actionExecutedContext.Exception.Message : inner.Message;

            CustomErrorsMode mode = section.Mode;
            if (mode == CustomErrorsMode.Off)
            {
                exceptionMessage = actionExecutedContext.Exception.StackTrace;
            }

            if (!actionExecutedContext.Exception.Data.Contains("App"))
            {
                actionExecutedContext.Exception.Data.Add("App", "BG");
            }
            if (!actionExecutedContext.Exception.Data.Contains("Environment"))
            {
                actionExecutedContext.Exception.Data.Add("Environment", ConfigurationManager.AppSettings["Environment"]);
            }

            _log.Error(actionExecutedContext.Exception);

            var isModelException = actionExecutedContext.Exception.GetType() == typeof(ModeloNoValidoException);

            var allErrors = new List<RespuestaError>();

            if (isModelException)
            {
                var modelException = actionExecutedContext.Exception as ModeloNoValidoException;

                foreach (var error in modelException.Errores)
                {
                    allErrors.Add(new RespuestaError()
                    {
                        Titulo = error
                    });
                }

            }
            else
            {
                allErrors.Add(new RespuestaError()
                {
                    Titulo = exceptionMessage
                });
            }


            var respuesta = new Respuesta(allErrors);

            actionExecutedContext.Response = actionExecutedContext
                .Request
                .CreateResponse(isModelException ? HttpStatusCode.BadRequest : HttpStatusCode.InternalServerError,
                    respuesta);

            Task.FromResult<object>(null);
        }
    }
}