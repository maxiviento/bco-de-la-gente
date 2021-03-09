using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;


namespace Infraestructura.Core.Manejadores
{
    //https://ruhul.wordpress.com/2014/09/05/how-to-handle-exceptions-globally-in-asp-net-webapi-2/
    public class CustomExceptionHandler: IExceptionHandler
    {

        public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            System.Configuration.Configuration configuration =
             WebConfigurationManager.OpenWebConfiguration("/");

            CustomErrorsSection section =
                (CustomErrorsSection)configuration.GetSection("system.web/customErrors");


            var stack = string.Empty;

            CustomErrorsMode mode = section.Mode;
            if (mode == CustomErrorsMode.Off)
            {
                stack = context.Exception.StackTrace;
            }

            if (!context.Exception.Data.Contains("App"))
            {
                context.Exception.Data.Add("App", "BGE");
            }
            if (!context.Exception.Data.Contains("Environment"))
            {
                context.Exception.Data.Add("Environment", ConfigurationManager.AppSettings["Environment"]);
            }


            var isModelException = context.Exception.GetType() == typeof(ModeloNoValidoException);
            var isEntidadException = context.Exception.GetType() == typeof(EntidadNoEncontradaException);

            /*
             * TODO
             * Es para avisarle al front cuales son las que tiene un mensaje/titulo especifico para el tipo de error.
             * Aquellos que no sean manejado tenemos que generar un mensaje/titulo generico.
             */
            var managedError = context.Exception is BaseApplicationException;

            var allErrors = new List<RespuestaError>();

            if (isModelException)
            {
                var modelException = context.Exception as ModeloNoValidoException;

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
                var exception = context.Exception;
                while (exception != null)
                {
                    allErrors.Add(new RespuestaError()
                    {
                        Titulo = exception.Message
                    });
                    exception = exception.InnerException;
                }
            }


            var respuesta = new Respuesta(allErrors) { ManagedError = managedError };


            var response  =
                context.Request.CreateResponse(
                    isModelException || isEntidadException ? 
                    HttpStatusCode.BadRequest : HttpStatusCode.InternalServerError, respuesta);

          
            context.Result = new ResponseMessageResult(response);


           return Task.FromResult<object>(null);
        }
    }
}
