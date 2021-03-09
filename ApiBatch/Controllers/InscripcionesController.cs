using ApiBatch.Base;
using ApiBatch.Infraestructure;
using ApiBatch.Infraestructure.Data;
using ApiBatch.Operations.Procesos;
using ApiBatch.Operations.QueueManager;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.CiDi.Api;
using Infraestructura.Core.CiDi.Model;
using Infraestructura.Core.Comun.Excepciones;
using NHibernate.Exceptions;
using NHibernate.Util;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Filters;
using ApiBatch.Base.QueueManager;

namespace ApiBatch.Controllers
{
    public class InscripcionesController : ApiController
    {
        [HttpPost]
        [Route("informe")]
        public IHttpActionResult PostInforme([FromBody] GenerarLoteTodosComando comando)
        {
            try
            {
                HostingEnvironment.QueueBackgroundWorkItem((CancellationToken ct) =>
                {
                    using (var sesion = NHibernateSessionManager.SessionFactory.OpenStatelessSession())
                    {
                        var proceso = new ProcesoInformeBanco(sesion);


                        var usuarioCidi = ObtenerUsuarioLogueado();
                        var usuario = proceso.ConsultarUsuarioPorCuil(GetCidiHash(), usuarioCidi.CUIL);
                        using (var tx = sesion.BeginTransaction())
                        {
                            try
                            {
                                     proceso.Execute(new OperacionEntrada()
                                    {
                                        Usuario = usuario,
                                        TipoProceso = TiposProcesosEnum.Prueba
                                    });
          
                                tx.Commit();
                            }
                            catch (Exception e)
                            {
                                tx.Rollback();

                                using (var tx3 = sesion.BeginTransaction())
                                {
                                    tx3.Commit();
                                }
                            }
                        }
                        sesion.Close();
                    }
                });
            }
            catch (Exception e)
            {

            }
            return Ok();
        }


        public UsuarioCidi ObtenerUsuarioLogueado()
        {
            var usuarioCidi = ApiCuenta.ObtenerUsuarioActivo(GetCidiHash());
            return usuarioCidi;
        }

        private string GetCidiHash()
        {
            var cookies = Request
                .Headers
                .GetCookies("CiDi")
                .FirstOrDefault().Cookies;

            var cidiHash = string.Empty;


            foreach (var cookie in cookies)
            {
                if (cookie.Name.Equals("CiDi"))
                {
                    cidiHash = cookie.Value;
                    break;
                }
            }

            return cidiHash;
        }
    }
}