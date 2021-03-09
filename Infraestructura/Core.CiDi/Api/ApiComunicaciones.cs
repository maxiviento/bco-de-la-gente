using System;
using Infraestructura.Core.CiDi.Configuration;
using Infraestructura.Core.CiDi.Model;
using Infraestructura.Core.CiDi.Util;

namespace Infraestructura.Core.CiDi.Api
{
    public static class ApiComunicaciones
    {
        public static ResultadoEmail EnviarMailPorCuil(DatosEmail datosEmail)
        {
            var cidiEnvironment = CidiConfigurationManager.GetCidiEnvironment();

            var email = new Email
            {
                Cuil = datosEmail.Cuil,
                Asunto = datosEmail.Asunto,
                Subtitulo = string.Empty,
                Mensaje = datosEmail.Asunto,
                InfoDesc = datosEmail.InfoDesc,
                InfoDato = datosEmail.InfoDato,
                InfoLink = datosEmail.InfoLink,
                Firma = datosEmail.Firma,
                Ente = datosEmail.Ente,
                Id_App = cidiEnvironment.IdApplication,
                Pass_App = cidiEnvironment.ClientSecret,
                TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssfff")
            };
            email.TokenValue = TokenUtil.ObtenerToken_SHA1(email.TimeStamp, cidiEnvironment.ClientKey);

            return HttpWebRequestUtil.LlamarWebApi<Email, ResultadoEmail>(GlobalVars.ApiComunicacion.Email.Enviar, email);
        }
    }
}
