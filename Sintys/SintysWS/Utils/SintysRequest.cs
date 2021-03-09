using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Infraestructura.Core.Comun.Excepciones;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.Newtonsoft.Json;
using RestRequest = RestSharp.RestRequest;

namespace SintysWS.Utils
{
    public class SintysRequest
    {
        
    public class Operaciones
        {
            public const string GetPersonaFisica = "obtenerPersonaFisicaWSFiltros";
            public const string GetPersonaIdentificada = "obtenerPersonaIdentificada";
            public const string GetDomicilio = "obtenerDomicilios";
            public const string GetFallecido = "obtenerFallecidos";
            public const string GetJubilacionYPensionConMonto = "obtenerJubilacionesYPensionesConMonto";
            public const string GetPensionNoContributiva = "obtenerPensionesNoContributivas";
            public const string GetEmpleoPresunto = "obtenerEmpleoPresunto";
            public const string GetDesempleo = "obtenerDesempleo";
            public const string GetEmpleoFormalConMonto = "obtenerEmpleoFormalConMonto";
        }

        [JsonProperty("organismo")]
        public string Organismo { get; private set; }

        [JsonProperty("usuarioOrganismo")]
        public string Usuario { get; private set; }

        [JsonProperty("password")]
        public string UsuarioPassword { get; private set; }

        [JsonProperty("operacion")]
        public string Operacion { get; private set; }

        [JsonProperty("parametros")]
        public Dictionary<string, object> Parametros { get; }

        private string Certificado { get; }
        private string CertificadoPassword { get; }
        private string ApiUrl { get; }
        private readonly string proxyString = ConfigurationManager.AppSettings["ProxyUrl"];


        public SintysRequest(string operacion)
        {
            var mensaje = "";
            try
            {
                Organismo = ConfigurationManager.AppSettings["sintys_organismo"];
                Usuario = ConfigurationManager.AppSettings["sintys_usuario"];
                UsuarioPassword = ConfigurationManager.AppSettings["sintys_usuarioPassword"];
                mensaje = "Se recuperan los datos de configuracion de Organismo, Usuario, Password";
                Parametros = new Dictionary<string, object>();
                Operacion = operacion;
                mensaje = "Se agrega el tipo de operacion";
                Certificado = ConfigurationManager.AppSettings["sintys_certificado"];
                CertificadoPassword = ConfigurationManager.AppSettings["sintys_certificadoPassword"];
                mensaje = "Se recuperan los datos de Certificado y Password";
                if (Certificado.StartsWith("~"))
                    Certificado = HttpContext.Current.Server.MapPath(Certificado);
                ApiUrl = ConfigurationManager.AppSettings["sintys_apiUrl"];
                mensaje = "Se recuperan los datos de URL";
            }
            catch (Exception e)
            {
                throw new ErrorTecnicoException(mensaje, e);
            }
        }

        public SintysRequest AddParametro(string clave, string valor)
        {
            if (Parametros.ContainsKey(clave))
                Parametros[clave] = valor;
            else
                Parametros.Add(clave, valor);

            return this;
        }

        public SintysRequest AddFiltro(string campo, string valor, string operador = "=")
        {
            if (!Parametros.ContainsKey("filtros"))
                Parametros.Add("filtros", new List<Filtro>());

            var filtros = Parametros["filtros"] as List<Filtro>;
            var filtroExistente = filtros.FirstOrDefault(x => x.Campo == campo);

            if (filtroExistente != null)
            {
                filtroExistente.Valor = valor;
                filtroExistente.Operador = operador;
            }
            else
            {
                filtros.Add(new Filtro(campo, valor, operador));
            }

            return this;
        }

        public SintysResponse<TResultado> Ejecutar<TResultado>()
            where TResultado : new()
        {
            var mensaje = "";
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                var client = new RestClient(ApiUrl);
                mensaje = "Genero cliente rest";
                client.ClientCertificates = new X509Certificate2Collection();
                //client.ClientCertificates.Add(new X509Certificate2(Certificado, CertificadoPassword));
                /*var cert = new X509Certificate2(Assembly.GetExecutingAssembly().GetManifestResourceNames()[0],
                    CertificadoPassword);*/
                    
                var sintysWS = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(x => x.FullName.Contains("SintysWS"));
                if(sintysWS == null)
                    throw new ErrorTecnicoException("No se encontro el Assembly SintysWS.");

                var fullReportName = sintysWS.GetManifestResourceNames().SingleOrDefault(x => x.EndsWith("sintys.pfx"));

                if (fullReportName == null)
                    throw new ErrorTecnicoException("No se encontro el certificado.");

                mensaje = "Busco nombre del certificado";

                using (var certStream = sintysWS.GetManifestResourceStream(fullReportName))
                {
                    mensaje = "Buscó el archivo del assembly.";

                    if (certStream == null)
                        throw new ErrorTecnicoException("El certificado no posee datos.");
                    
                    var rawData = new byte[certStream.Length];
                    for (var index = 0; index < certStream.Length; index++)
                    {
                        rawData[index] = (byte) certStream.ReadByte();
                    }

                    mensaje = "Generó el array de bytes del archivo";

                    X509Certificate2 webVisorCertificate;
                    try
                    {
                        webVisorCertificate = new X509Certificate2(rawData, CertificadoPassword,
                            X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet |
                            X509KeyStorageFlags.Exportable);
                    }
                    catch (Exception e)
                    {
                        throw new RespuestaServiceApiExternaException(e.TargetSite.Name + " " + e.Message);
                    }

                    client.ClientCertificates.Add(webVisorCertificate);
                    mensaje = "Obtuvo certificado";
                }

                //client.ClientCertificates.Add(new X509Certificate2(fullReportName, CertificadoPassword));
                if (!string.IsNullOrEmpty(proxyString))
                {
                    client.Proxy = new WebProxy(proxyString) {Credentials = CredentialCache.DefaultCredentials};
                    mensaje = "Asigno el proxy";
                }
                mensaje = "Configuro cliente para su uso en una peticion";
                
                // Override with Newtonsoft JSON Handler
                client.AddHandler("application/json", JsonNetSerializer.Default);
                client.AddHandler("text/json", JsonNetSerializer.Default);
                client.AddHandler("text/x-json", JsonNetSerializer.Default);
                client.AddHandler("text/javascript", JsonNetSerializer.Default);
                client.AddHandler("*+json", JsonNetSerializer.Default);
                mensaje = "Configuro los serializadores";

                var request = new RestRequest(Method.POST);
                request.DateFormat = "dd/MM/yyyy";
                request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
                mensaje = "Genero Request";

                request.JsonSerializer = new NewtonsoftJsonSerializer();
                request.AddJsonBody(this);
                
                var response = client.Execute<SintysResponse<TResultado>>(request);
                if (response.ErrorException != null)
                {
                    var message = "Sintys :: Error de conexion con fuente de datos. " +
                                     response.ErrorException.Message;
                    //throw new ApplicationException(message, response.ErrorException);

                    response.Data = response.Data ?? new SintysResponse<TResultado>();
                    response.Data.Error = message;
                    mensaje = message;
                }

                if (response.ErrorException == null && response.StatusCode != HttpStatusCode.OK)
                {
                    var message = "Sintys :: Error al procesar la peticion. ERROR: " + response.StatusCode + ". " +
                                     response.StatusDescription;
                    //throw new ApplicationException(message, response.ErrorException);

                    response.Data = response.Data ?? new SintysResponse<TResultado>();
                    response.Data.Error += message;
                    mensaje = message;
                }

                return response.Data;
            }
            catch (Exception e)
            {
                throw new RespuestaServiceApiExternaException($"{mensaje} en SintysRequest - SintysWS", e);
            }
        }
    }
}