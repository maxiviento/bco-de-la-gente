using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace Infraestructura.Core.CiDi.Util
{
    public static class HttpWebRequestUtil
    {
        /// <summary>
        /// Realiza la llamada a la WebAPI de Ciudadano Digital, serializa la Entrada y deserializa la Respuesta.
        /// </summary>
        /// <typeparam name="TEntrada">Declarar el objeto de Entrada al método.</typeparam>
        /// <typeparam name="TRespuesta">Declarar el objeto de Respuesta al método.</typeparam>
        /// <param name="accion">Recibe la acción específica del controlador de la WebAPI.</param>
        /// <param name="tEntrada">Objeto de entrada de la WebAPI , especificado en TEntrada.</param>
        /// <returns>Objeto de salida de la WebAPI, especificado en TRespuesta.</returns>
        public static TRespuesta LlamarWebApi<TEntrada, TRespuesta>(string accion, TEntrada tEntrada)
        {
            var uri = new Uri(accion);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.ContentType = "application/json; charset=utf-8";

            var rawjson = JsonConvert.SerializeObject(tEntrada);
            httpWebRequest.Method = "POST";

            var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());

            streamWriter.Write(rawjson);
            streamWriter.Flush();
            streamWriter.Close();

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            var streamReader = new StreamReader(httpResponse.GetResponseStream());
            var result = streamReader.ReadToEnd();

            return JsonConvert.DeserializeObject<TRespuesta>(result);
        }
    }
}
