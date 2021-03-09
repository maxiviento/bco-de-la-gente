using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Infraestructura.Core.Comun.Presentacion;

namespace Infraestructura.Core.Manejadores
{
    public class MessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (request.RequestUri.ToString().Contains("docs") || IsMediaType(response))
            {
                return response;
            }

            return BuildApiResponse(request, response);
        }

        private static bool IsMediaType(HttpResponseMessage response)
        {
            var contentType = response.Content?.Headers?.ContentType?.MediaType;
            var result = false;
            if (contentType != null)
            {
                result = contentType.Contains("image") || contentType.Contains("application/pdf");
            }
            return result;
        }

        private static HttpResponseMessage BuildApiResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            dynamic content;

            if (response.TryGetContentValue(out content) && !response.IsSuccessStatusCode)
            {
                if (content.GetType() == typeof(Respuesta))
                {
                    ((Respuesta)content).Status = (int)response.StatusCode;
                    return response;
                }

                HttpError error = content as HttpError;

                if (error != null)
                {
                    content = null;
                    var errorMessage = error.Message;

#if DEBUG
                    errorMessage = string.Concat(errorMessage, error.ExceptionMessage, error.StackTrace);
#endif
                }
            }
            var respuesta = new Respuesta((int)response.StatusCode, content);
            var newResponse = request.CreateResponse(response.StatusCode, respuesta);

            foreach (var header in response.Headers)
            {
                newResponse.Headers.Add(header.Key, header.Value);
            }

            return newResponse;
        }
    }
}