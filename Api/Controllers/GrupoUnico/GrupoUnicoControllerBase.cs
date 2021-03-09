using System.Web;
using System.Web.Http;
using Infraestructura.Core.CiDi.Api;
using Infraestructura.Core.Comun.Excepciones;
using System;

namespace Api.Controllers.GrupoUnico
{
    public class GrupoFamiliarControllerBase : ApiController
    {

        protected delegate object CallApiWithObjectResultDelegate(string cookieHash, string sexo, string dni,
            string pais, int? idNumero);

        protected delegate string CallApiWithStringJsonResultDelegate(string cookieHash, string sexo, string dni,
            string pais, int? idNumero);

        protected delegate string CallApiWithStringUrlResultDelegate(string cuilUsuarioLogueado, string sexo,
            string dni, string pais, int? idNumero);

        protected delegate string CallApiDomicilioWithStringJsonResultDelegate(string cookieHash,
            string idVin);

        protected delegate string CallApiDomicilioWithStringUrlResultDelegate(string cookieHash,
            string idVin);

        protected delegate object CallApiDomicilioWithObjectResultDelegate(string cookieHash,
            string idVin);

        protected delegate string CallApiDomicilioGenWithStringJsonResultDelegate(string cookieHash,
            string sexo, string dni, string pais, int tipoDocumento, string cuilUsuario, int? idNumero);

        protected delegate string CallApiDomicilioGenWithStringUrlResultDelegate(string cuilUsuarioLogueado,
            string sexo, string dni, string pais, int tipoDocumento, int jurisdiccion, int? idNumero);

        protected delegate object CallApiDomicilioGenWithObjectResultDelegate(string cookieHash,
            string sexo, string dni, string pais, int tipoDocumento, string cuilUsuario, int? idNumero);

        protected object CallApiObject(string sexo, string dni, string pais, int? idNumero, CallApiWithObjectResultDelegate handler)
        {
            if (string.IsNullOrEmpty(sexo) || string.IsNullOrEmpty(dni) || string.IsNullOrEmpty(pais))
                throw new ModeloNoValidoException("el sexo id, el dni y el país son obligatorios.");

            var hash = HttpContext.Current.Request.Cookies["CiDi"].Value;

            var objeto = handler(hash, sexo, dni, pais, idNumero);
            return objeto;
        }

        protected string CallApiStringJson(string sexo, string dni, string pais, int? idNumero,
            CallApiWithStringJsonResultDelegate handler)
        {
            if (string.IsNullOrEmpty(sexo) || string.IsNullOrEmpty(dni) || string.IsNullOrEmpty(pais))
                throw new ModeloNoValidoException("el sexo, dni y país son obligatorios.");
            // ReSharper disable once PossibleNullReferenceException
            var hash = HttpContext.Current.Request.Cookies["CiDi"].Value;

            var objeto = handler(hash, sexo, dni, pais, idNumero);
            return objeto;
        }

        protected string CallApiStringUrl(string sexo, string dni, string pais,
            CallApiWithStringUrlResultDelegate handler, int? idNumero)
        {
            if (string.IsNullOrEmpty(sexo) || string.IsNullOrEmpty(dni) || string.IsNullOrEmpty(pais))
                throw new ModeloNoValidoException("el sexo, dni y país son obligatorios.");
            // ReSharper disable once PossibleNullReferenceException
            var hash = HttpContext.Current.Request.Cookies["CiDi"].Value;
            var usuarioLogueado = ApiCuenta.ObtenerUsuarioActivo(hash);
            var objeto = handler(usuarioLogueado.CUIL, sexo, dni, pais, idNumero);
            return objeto;
        }

        protected object CallApiObject(string idVin,
            CallApiDomicilioWithObjectResultDelegate handler)
        {
            if (string.IsNullOrEmpty(idVin))
                throw new ModeloNoValidoException("el idVin es obligatorio.");
            // ReSharper disable once PossibleNullReferenceException
            var hash = HttpContext.Current.Request.Cookies["CiDi"].Value;
            var objeto = handler(hash, idVin);
            return objeto;
        }

        protected string CallApiEntidadConsultaStringJson(string idVin,
            CallApiDomicilioWithStringJsonResultDelegate handler)
        {
            if (string.IsNullOrEmpty(idVin))
                throw new ModeloNoValidoException("el idVin es obligatorio.");
            // ReSharper disable once PossibleNullReferenceException
            var hash = HttpContext.Current.Request.Cookies["CiDi"].Value;
            var objeto = handler(hash, idVin);
            return objeto;
        }

        protected string CallApiStringUrl(string idVin,
            CallApiDomicilioWithStringUrlResultDelegate handler)
        {
            if (string.IsNullOrEmpty(idVin))
                throw new ModeloNoValidoException("el idVin es obligatorio.");
            // ReSharper disable once PossibleNullReferenceException
            var hash = HttpContext.Current.Request.Cookies["CiDi"].Value;
            var usuarioLogueado = ApiCuenta.ObtenerUsuarioActivo(hash);
            var objeto = handler(usuarioLogueado.CUIL, idVin);
            return objeto;
        }

        protected string CallApiStringUrl(string sexo, string dni, string pais, int tipoDomicilio, int jurisdiccion,
            CallApiDomicilioGenWithStringUrlResultDelegate handler, int? idNumero)
        {
            if (string.IsNullOrEmpty(sexo) || string.IsNullOrEmpty(dni) || string.IsNullOrEmpty(pais) || tipoDomicilio < 1 || jurisdiccion < 1)
                throw new ModeloNoValidoException("el sexo, dni, pais, tipoDomicilio y jurisdicción son obligatorios.");
            if (jurisdiccion > 3)
                throw new ModeloNoValidoException("la jurisdiccion puede ser PROVINCIAL(1), NACIONAL(2) ó INTERNACIONAL(3).");
            // ReSharper disable once PossibleNullReferenceException
            var hash = HttpContext.Current.Request.Cookies["CiDi"].Value;
            var usuarioLogueado = ApiCuenta.ObtenerUsuarioActivo(hash);
            var objeto = handler(usuarioLogueado.CUIL, sexo, dni, pais, tipoDomicilio, jurisdiccion, idNumero);
            return objeto;
        }
        protected object CallApiObject(string sexo, string dni, string pais, int tipoDomicilio,
            CallApiDomicilioGenWithObjectResultDelegate handler, int? idNumero)
        {
            if (string.IsNullOrEmpty(sexo) || string.IsNullOrEmpty(dni) || string.IsNullOrEmpty(pais) || tipoDomicilio < 1)
                throw new ModeloNoValidoException("el sexo, dni, pais y tipoDomicilio son obligatorios.");
            // ReSharper disable once PossibleNullReferenceException
            var hash = HttpContext.Current.Request.Cookies["CiDi"].Value;
            var usuarioLogueado = ApiCuenta.ObtenerUsuarioActivo(hash);
            var objeto = handler(hash, sexo, dni, pais, tipoDomicilio, usuarioLogueado.CUIL, idNumero);
            return objeto;
        }

        protected string CallApiEntidadConsultaStringJson(string sexo, string dni, string pais, int tipoDomicilio,
            CallApiDomicilioGenWithStringJsonResultDelegate handler, int? idNumero)
        {
            if (string.IsNullOrEmpty(sexo) || string.IsNullOrEmpty(dni) || string.IsNullOrEmpty(pais) || tipoDomicilio < 1)
                throw new ModeloNoValidoException("el sexo, dni, pais y tipoDomicilio son obligatorios.");
            // ReSharper disable once PossibleNullReferenceException
            var hash = HttpContext.Current.Request.Cookies["CiDi"].Value;
            var usuarioLogueado = ApiCuenta.ObtenerUsuarioActivo(hash);
            var objeto = handler(hash, sexo, dni, pais, tipoDomicilio, usuarioLogueado.CUIL, idNumero);
            return objeto;
        }
    }
}