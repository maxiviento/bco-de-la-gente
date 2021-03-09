using System;
using System.Collections.Generic;
using System.Configuration;
using Core.CiDi.Documentos.Entities.CDDResponse;
using Core.CiDi.Documentos.Utils;
using Infraestructura.Core.CiDi.Api;
using Infraestructura.Core.CiDi.Configuration;
using Infraestructura.Core.CiDi.Model;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using Soporte.Dominio.Modelo;

namespace Soporte.Aplicacion.Servicios
{
    public class DocumentacionCDDUtilServicio
    {
        #region Inicializacion

        public static IList<string> FormatosPermitidos { get; set; }

        private static readonly string CidiCuenta =
            CidiConfigurationManager.GetCidiEnvironment().IdApplication.ToString();

        public DocumentacionCDDUtilServicio(IList<string> formatosPermitidos)
        {
            FormatosPermitidos = formatosPermitidos;
        }

        public DocumentacionCDDUtilServicio()
        {
            FormatosPermitidos = new List<string> {"pdf", "jpeg", "jpg"};
        }

        private static CredencialesAutorizacion CredencialesCdd(string idAppOrigen = null, string password = null,
            string key = null)
        {
            string credencialesIdAppOrigen = idAppOrigen ??
                                             ConfigurationManager.AppSettings[
                                                 "credenciales-api-documentos-cdd:id-app-origen"];
            string credencialesPassword =
                password ?? ConfigurationManager.AppSettings["credenciales-api-documentos-cidi:password"];
            string credencialesKey = key ?? ConfigurationManager.AppSettings["credenciales-api-documentos-cidi:key"];

            return new CredencialesAutorizacion(credencialesIdAppOrigen, credencialesPassword, credencialesKey);
        }

        #endregion

        #region GuardarDocumentacion

        public Documentacion GuardarDocumentacionCdd(Documentacion documentacion, string cuilUsuario)
        {
            return GuardarDocumentacion(documentacion, cuilUsuario);
        }

        public Documentacion GuardarDocumentacionCdd(Documentacion documentacion, string cuilUsuario,
            string destino, string password, string key)
        {
            return GuardarDocumentacion(documentacion, cuilUsuario, destino, password, key);
        }

        public Documentacion GuardarDocumentacionCidi(Documentacion documentacion, string cuilUsuario)
        {
            return GuardarDocumentacion(documentacion, cuilUsuario, CidiCuenta);
        }

        private static Documentacion GuardarDocumentacion(Documentacion documentacion, string cuilUsuario,
            string destino = null, string password = null, string key = null)
        {
            var documentoCdd = new DocumentoCdd
            {
                NombreDocumento = documentacion.Nombre,
                BlobImagen = documentacion.Archivo,
                IdCatalogo = decimal.ToInt32(documentacion.IdTipoDocumentacionCdd.Valor),
                Extension = documentacion.Extension
            };

            var credenciales = CredencialesCdd(destino, password, key);

            var apiDocumentos = new ApiDocumentos(documentoCdd, cuilUsuario, FormatosPermitidos, credenciales);
            try
            {
                var documentoResponse = apiDocumentos.GuardarNuevoDocumentoCdd();
                documentacion.IdDocumentoCdd = new Id(documentoResponse.Id_Documento);
                return documentacion;
            }

            catch (Exception e)
            {
                throw new ModeloNoValidoException(
                    "Error en la comunicación con CIDI para la almacenar de documentación." +
                    e.Message);
            }
        }

        #endregion

        #region ObtenerDocumentacion

        public CDDResponseConsulta ObtenerDocumentacionCdd(int idDocumento, int idCatalogo, string cuilUsuario)
        {
            return ObtenerDocumentacion(idDocumento, idCatalogo, cuilUsuario);
        }

        public CDDResponseConsulta ObtenerDocumentacionCdd(int idDocumento, int idCatalogo, string cuilUsuario,
            string destino, string password, string key)
        {
            return ObtenerDocumentacion(idDocumento, idCatalogo, cuilUsuario, destino, password, key);
        }

        public CDDResponseConsulta ObtenerDocumentacionCidi(int idDocumento, int idCatalogo, string cuilUsuario)
        {
            return ObtenerDocumentacion(idDocumento, idCatalogo, cuilUsuario, CidiCuenta);
        }

        private static CDDResponseConsulta ObtenerDocumentacion(int idDocumento, int idCatalogo, string cuilUsuario,
            string destino = null, string password = null, string key = null)
        {
            var documentoCdd = new DocumentoCdd
            {
                IdDocumento = idDocumento,
                IdCatalogo = idCatalogo
            };

            var credenciales = CredencialesCdd(destino, password, key);

            var apiDocumentos = new ApiDocumentos(documentoCdd, cuilUsuario, FormatosPermitidos, credenciales);

            try
            {
                var response = apiDocumentos.BuscarDocumentoCdd();

                return response;
            }
            catch (Exception e)
            {
                throw new ModeloNoValidoException(
                    "Error en la comunicación con CIDI para la descarga de documentación." +
                    e.Message);
            }
        }

        #endregion

        #region EliminarDocumentacion

        public bool EliminarDocumentacionCdd(int idDocumento, int idCatalogo, string cuilUsuario)
        {
            return EliminarDocumentacion(idDocumento, idCatalogo, cuilUsuario);
        }

        public static bool EliminarDocumentacionCdd(int idDocumento, int idCatalogo, string cuilUsuario,
            string destino, string password, string key)
        {
            return EliminarDocumentacion(idDocumento, idCatalogo, cuilUsuario,
                destino, password, key);
        }

        public bool EliminarDocumentacionCidi(int idDocumento, int idCatalogo, string cuilUsuario)
        {
            return EliminarDocumentacion(idDocumento, idCatalogo, cuilUsuario, CidiCuenta);
        }

        private static bool EliminarDocumentacion(int idDocumento, int idCatalogo, string cuilUsuario,
            string destino = null, string password = null, string key = null)
        {
            var documentoCdd = new DocumentoCdd()
            {
                IdDocumento = idDocumento,
                IdCatalogo = idCatalogo
            };

            var credenciales = CredencialesCdd(destino, password, key);

            var apiDocumentos = new ApiDocumentos(documentoCdd, cuilUsuario, null, credenciales);

            try
            {
                apiDocumentos.EliminarDocumentoCdd();

                return true;
            }
            catch (Exception e)
            {
                throw new ModeloNoValidoException(
                    "Error en la comunicación con CIDI para la eliminación de documentación." +
                    e.Message);
            }
        }

        #endregion
    }
}