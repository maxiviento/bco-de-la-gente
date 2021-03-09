using System;
using System.Collections.Generic;
using System.Linq;
using Core.CiDi.Documentos.Entities.CDDAutorizador;
using Core.CiDi.Documentos.Entities.CDDPost;
using Core.CiDi.Documentos.Entities.CDDResponse;
using Core.CiDi.Documentos.Utils;
using CryptoManagerV4._0.Clases;
using CryptoManagerV4._0.General;
using Infraestructura.Core.CiDi.Model;
using Infraestructura.Core.CiDi.Util;
using Infraestructura.Core.Comun.Excepciones;

namespace Infraestructura.Core.CiDi.Api
{
    public class ApiDocumentos
    {

        private CryptoDiffieHellman ObjDiffieHellman { get; set; }
        private Autorizador ObjAutorizador { get; set; }
        private CDDPost RequestPost { get; set; }
        private DocumentoCdd DocumentoCdd { get; }
        private string CuilUsuarioLogueado { get; }
        private IList<string> FormatosPermitidos { get; }
        private int IdDocumento { get; set; }
        private int IdAplicacionOrigen { get; }
        private string Password { get; }
        private string Key { get; }
        private string Operacion { get; set; }

        private const string OperacionConsultar = "1";
        private const string OperacionAdjuntar = "1";//3??
        private const string OperacionEliminar = "1";

        /// <summary>
        /// Inicializa los datos necesarios para consultar la api de documentos.
        /// </summary>
        /// <param name="documento">Datos del documento a registrar/consultar/eliminar</param>
        /// <param name="cuilUsuarioLogueado">usuario logueado en cidi</param>
        /// <param name="formatosPermitidos">Attay de string de formatos permitidos. Ej ["pdf","jpg"]</param>
        /// <param name="credenciales">Credenciales para autorizar el consumo de la api de documentos de CiDi. Se debe solicitar a gobierno.</param>
        public ApiDocumentos(DocumentoCdd documento, string cuilUsuarioLogueado, IList<string> formatosPermitidos,
            CredencialesAutorizacion credenciales)
        {
            IdAplicacionOrigen = credenciales.IdAppOrigen;
            Password = credenciales.Password;
            Key = credenciales.Key;
            DocumentoCdd = documento;
            CuilUsuarioLogueado = cuilUsuarioLogueado;
            FormatosPermitidos = formatosPermitidos;
            ValidarDatosRequeridos();
        }

        #region public Methods

        public CDDResponseConsulta BuscarDocumentoCdd()
        {
            ValidarDatosRequeridosBusquedaDocumento();

            IdDocumento = DocumentoCdd.IdDocumento;

            Operacion = OperacionConsultar;

            var respLogin = GetAuthorizeWebApiCdd();

            if (respLogin != null && respLogin.Codigo_Resultado.Equals("SEO"))
            {
                SetParametersRequestPost(respLogin.Llave_BLOB_Login);

                var response = GetDocumentWebApiCdd();

                var objCryptoHash = new CryptoHash();
                var mensaje = String.Empty;

                if (response != null && response.Codigo_Resultado.Equals("SEO"))
                {
                    response.Documentacion.Imagen_Documentacion =
                        objCryptoHash.Descifrar_Datos(response.Documentacion.Imagen_Documentacion, out mensaje);
                    return response;
                }

                if (response != null)
                    throw new ErrorTecnicoException("Codigo Error: " + response.Codigo_Resultado + " Descripcion: " +
                                                    response.Detalle_Resultado);
            }
            else
            {
                if (respLogin != null)
                    throw new ErrorTecnicoException("Codigo Error: " + respLogin.Codigo_Resultado + " Descripcion: " +
                                                    respLogin.Detalle_Resultado);
            }

            return null;
        }

        public CDDResponseInsercion GuardarNuevoDocumentoCdd()
        {
            IdDocumento = 0;

            ValidarDatosRequeridosNuevoDocumento();

            Operacion = OperacionAdjuntar;

            var respLogin = GetAuthorizeWebApiCdd();

            if (respLogin != null && respLogin.Codigo_Resultado.Equals("SEO"))
            {
                SetParametersRequestPost(respLogin.Llave_BLOB_Login);

                var response = SaveDocumentWebApiCdd();

                if (response != null && response.Codigo_Resultado.Equals("SEO"))
                    return response;

                if (response != null)
                    throw new ErrorTecnicoException("Codigo Error: " + response.Codigo_Resultado + " Descripcion: " +
                                                    response.Detalle_Resultado);
            }
            else
            {
                if (respLogin != null)
                    throw new ErrorTecnicoException("Codigo Error: " + respLogin.Codigo_Resultado + " Descripcion: " +
                                                    respLogin.Detalle_Resultado);
            }

            return null;
        }

        public CDDResponse EliminarDocumentoCdd()
        {
            ValidarDatosRequeridosEliminacionDocumento();

            Operacion = OperacionEliminar;

            var respLogin = GetAuthorizeWebApiCdd();

            if (respLogin != null && respLogin.Codigo_Resultado.Equals("SEO"))
            {
                var respCdd = SetearDataPostEliminar(respLogin.Llave_BLOB_Login);

                var cddRespEliminacion = EliminarDocumentWebApiCdd(respCdd);

                if (cddRespEliminacion != null && cddRespEliminacion.Codigo_Resultado.Equals("SEO"))

                    return cddRespEliminacion;

                if (cddRespEliminacion != null)
                    throw new ErrorTecnicoException("Codigo Error: " + cddRespEliminacion.Codigo_Resultado + " Descripcion: " +
                                                    cddRespEliminacion.Detalle_Resultado);
            }
            else
            {
                if (respLogin != null)
                    throw new ErrorTecnicoException("Codigo Error: " + respLogin.Codigo_Resultado + " Descripcion: " +
                                                    respLogin.Detalle_Resultado);
            }

            return null;
        }

        #endregion

        #region private Methods

        private void ValidarDatosRequeridos()
        {
            if (IdAplicacionOrigen == 0)
                throw new ModeloNoValidoException("El id de aplicación origen es requerido.");
            if (Password == null)
                throw new ModeloNoValidoException("La password es requerida.");
            if (Key == null)
                throw new ModeloNoValidoException("La key es requerida.");
            if (DocumentoCdd == null)
                throw new ModeloNoValidoException("El documento es requerido");
            if (string.IsNullOrEmpty(CuilUsuarioLogueado))
                throw new ModeloNoValidoException("El cuil del usuario logueado es requerido.");
            if (DocumentoCdd.IdCatalogo == 0)
                throw new ModeloNoValidoException("El tipo de documento (id de catálogo) es requerido.");
        }

        private void ValidarDatosRequeridosNuevoDocumento()
        {
            if (DocumentoCdd.BlobImagen == null)
                throw new ModeloNoValidoException("El contenido del documento es requerido.");
            if (FormatosPermitidos == null)
                throw new ModeloNoValidoException("La lista de formatos permitidos es requerida.");
            if (FormatosPermitidos.Count == 0)
                throw new ModeloNoValidoException("La lista de formatos permitidos debe poseer como mínimo un formato.");
            if (string.IsNullOrEmpty(DocumentoCdd.NombreDocumento))
                throw new ModeloNoValidoException("El nombre del documento es requerido.");
            if (string.IsNullOrEmpty(DocumentoCdd.Extension))
                throw new ModeloNoValidoException("La extensión del documento es requerida.");
        }

        private void ValidarDatosRequeridosBusquedaDocumento()
        {
            if (DocumentoCdd.IdDocumento == 0)
                throw new ModeloNoValidoException("El id del documento es requerido.");
        }

        private void ValidarDatosRequeridosEliminacionDocumento()
        {
            if (DocumentoCdd.IdDocumento == 0)
                throw new ModeloNoValidoException("El id del documento es requerido.");
        }

        private void SetParametersRequestPost(byte[] pBlobKey)
        {
            RequestPost = new CDDPost
            {
                Id_Aplicacion_Origen = ObjAutorizador.Id_Aplicacion_Origen,
                Pwd_Aplicacion = ObjAutorizador.Pwd_Aplicacion,
                IdUsuario = ObjAutorizador.Id_Usuario,
                Shared_Key = pBlobKey,
                Id_Documento = IdDocumento,
                Id_Catalogo = DocumentoCdd.IdCatalogo
            };

            if (IdDocumento == 0)
                AdjuntarDocumento();
        }

        private CDDPostExpediente SetearDataPostEliminar(byte[] llaveBlobLogin)
        {
            var cddPostExpediente = new CDDPostExpediente
            {
                Id_Aplicacion_Origen = IdAplicacionOrigen,
                Pwd_Aplicacion = Password,
                Shared_Key = null,
                IdUsuario = CuilUsuarioLogueado,
                N_Documento = string.Empty,
                Vigencia = null,
                Blob_Imagen = null,
                Extension = string.Empty,
                Id_Catalogo = DocumentoCdd.IdCatalogo
            };
            cddPostExpediente.Vigencia = null;
            cddPostExpediente.Id_Documentacion = string.Empty;
            cddPostExpediente.Id_Documento = DocumentoCdd.IdDocumento;
            cddPostExpediente.Id_Tramite = string.Empty;
            cddPostExpediente.Id_Expediente = string.Empty;
            cddPostExpediente.Id_Pase = 0;
            cddPostExpediente.Id_Cuerpo = 0;
            cddPostExpediente.Id_Foja = 0;
            cddPostExpediente.N_Usuario = CuilUsuarioLogueado; // Colocar el Cuil del Operador
            cddPostExpediente.N_Descripcion = string.Empty;
            cddPostExpediente.Documentacion.Descripcion = string.Empty;
            cddPostExpediente.Shared_Key = llaveBlobLogin;
            return cddPostExpediente;
        }

        private void AdjuntarDocumento()
        {
            var objCryptoHash = new CryptoHash();
            string mensaje;
            ValidarFormatosUploadValidos();
            RequestPost.N_Documento = DocumentoCdd.NombreDocumento;
            RequestPost.Extension = DocumentoCdd.Extension;
            RequestPost.Blob_Imagen = objCryptoHash.Cifrar_Datos(DocumentoCdd.BlobImagen, out mensaje);
            RequestPost.Vigencia = DocumentoCdd.Vigencia;
            RequestPost.N_Constatado = true;
            RequestPost.N_Descripcion = DocumentoCdd.Descripcion;
        }

        private CDDResponseInsercion SaveDocumentWebApiCdd()
        {
            return HttpWebRequestUtil.LlamarWebApi<CDDPost, CDDResponseInsercion>(GlobalVars.ApiCdd.Documentos.GuardarDocumento, RequestPost);
        }

        private CDDResponseEliminacion EliminarDocumentWebApiCdd(CDDPostExpediente pCddPostExpediente)
        {
            return HttpWebRequestUtil.LlamarWebApi<CDDPostExpediente, CDDResponseEliminacion>(GlobalVars.ApiCdd.Documentos.EliminarDocumento, pCddPostExpediente);
        }

        private void ValidarFormatosUploadValidos()
        {
            if (!FormatosPermitidos.Any(formatoPermitido => DocumentoCdd.Extension.ToUpper().Equals(formatoPermitido.ToUpper())))
                throw new ModeloNoValidoException("Formato no permitido para el documento que se intenta almacenar.");
        }

        private CDDResponseLogin GetAuthorizeWebApiCdd()
        {
            InitializeAuthorize();

            var cddRespLogin = HttpWebRequestUtil.LlamarWebApi<Autorizador, CDDResponseLogin>(GlobalVars.ApiCdd.Documentos.AutorizarSolicitud, ObjAutorizador);

            if (cddRespLogin != null && cddRespLogin.Codigo_Resultado.Equals("SEO"))
                CompleteAuthorize(cddRespLogin.Llave_BLOB_Login);

            return cddRespLogin;
        }

        private CDDResponseConsulta GetDocumentWebApiCdd()
        {
            return HttpWebRequestUtil.LlamarWebApi<CDDPost, CDDResponseConsulta>(GlobalVars.ApiCdd.Documentos.ObtenerDocumento, RequestPost);
        }

        private void InitializeAuthorize()
        {
            ObjDiffieHellman = new CryptoDiffieHellman();
            ObjAutorizador = new Autorizador();

            var helper = new Helper();
            var cmd = new CryptoManagerData();

            var mensaje = string.Empty;

            ObjAutorizador.Id_Aplicacion_Origen = IdAplicacionOrigen;
            ObjAutorizador.Pwd_Aplicacion = helper.Encriptar_Password(Password); ;
            ObjAutorizador.Key_Aplicacion = Key;
            ObjAutorizador.Operacion = Operacion; // 1 = Consulta ; 3 = Adjuntar
            ObjAutorizador.Id_Usuario = CuilUsuarioLogueado;
            ObjAutorizador.Time_Stamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            try
            {
                ObjAutorizador.Token = cmd.Get_Token(ObjAutorizador.Time_Stamp, ObjAutorizador.Key_Aplicacion);

                if (!string.IsNullOrEmpty(mensaje)) throw new ErrorTecnicoException(mensaje);
            }
            catch (Exception ex)
            {
                throw new ErrorTecnicoException(ex.Message);
            }

            // Creo la llave CNG
            ObjDiffieHellman.Create_Key_ECCDHP521();

            // Creo la llave Blob pública
            ObjAutorizador.Public_Blob_Key = ObjDiffieHellman.Export_Key_Material();

            // Llave compartida
            ObjAutorizador.Shared_Key = null;

        }

        private void CompleteAuthorize(byte[] pPublicBlobKey)
        {
            try
            {
                // Creo la llave compartida
                ObjAutorizador.Shared_Key = ObjDiffieHellman.Share_Key_Generate(pPublicBlobKey);
            }
            catch (Exception ex)
            {
                throw new ErrorTecnicoException(ex.Message);
            }
        }

        #endregion
    }
}
