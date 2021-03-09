using System;
using System.Configuration;
using Infraestructura.Core.CiDi.Configuration;

namespace Infraestructura.Core.CiDi.Util
{
    public class GlobalVars
    {
        #region Variables Ciudadano Digital

        public static string CiDiUrl => ConfigurationManager.AppSettings["CiDiUrl"];

        public static string UrlIniciarSesion => CidiConfigurationManager.GetCidiEndpoint(UrlCidiEnum.IniciarSesion) + "?app=" + CidiConfigurationManager.GetCidiEnvironment().IdApplication;

        public static string UrlCerrarSesion => CidiConfigurationManager.GetCidiEndpoint(UrlCidiEnum.CerrarSesion);

        public static string UrlApiCuenta => CidiConfigurationManager.GetCidiEndpoint(UrlCidiEnum.CidiUrlApiCuenta);

        public static string UrlApiComunicacion => CidiConfigurationManager.GetCidiEndpoint(UrlCidiEnum.CidiUrlApiComunicacion);

        public static string UrlApiDocumentacion => CidiConfigurationManager.GetCidiEndpoint(UrlCidiEnum.CidiUrlApiDocumentacion);

        public static string UrlApiMobile => CidiConfigurationManager.GetCidiEndpoint(UrlCidiEnum.CidiUrlApiMobile);

        public static string UrlRelacion => CidiConfigurationManager.GetCidiEndpoint(UrlCidiEnum.CidiUrlRelacion);

        public static string UrlDocumentosCdd=> CidiConfigurationManager.GetCidiEndpoint(UrlCidiEnum.CidiUrlDocumentosCdd);

        public static string CiDiOk => "OK";

        #endregion

        #region Endpoints apis

        public class ApiCuenta
        {
            public class Usuario
            {
                /// <summary>
                /// Obtención del usuario Ciudadano Digital a través de la cookie
                /// </summary>
                /// <param>
                /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
                /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff").
                /// </param>
                /// <returns>Usuario</returns>
                public static string ObtenerUsuarioAplicacion => UrlApiCuenta + "api/Usuario/Obtener_Usuario_Aplicacion";

                /// <summary>
                /// Obtención de un usuario Ciudadano Digital a través del CUIL
                /// </summary>
                /// <param>
                /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff").
                /// </param>
                /// <returns>Usuario</returns>
                public static string ObtenerUsuario => UrlApiCuenta + "api/Usuario/Obtener_Usuario";

                /// <summary>
                /// Obtención del usuario Ciudadano Digital con datos básicos a través de la cookie
                /// </summary>
                /// <param>
                /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
                /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff").
                /// </param>
                /// <returns>Usuario</returns>
                public static string ObtenerUsuarioBasicos => UrlApiCuenta + "api/Usuario/Obtener_Usuario_Basicos";

                /// <summary>
                /// Obtención del usuario Ciudadano Digital con datos básicos a través del CUIL
                /// </summary>
                /// <param>
                /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.CUIL" tipo="Alfanumérico" long="30">CUIL del usuario.
                /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff").
                /// </param>
                /// <returns>Usuario</returns>
                public static string ObtenerUsuarioBasicosCuil => UrlApiCuenta + "api/Usuario/Obtener_Usuario_Basicos_CUIL";

                /// <summary>
                /// Obtención del usuario Ciudadano Digital con datos básicos y el domicilo a través de la cookie
                /// </summary>
                /// <param>
                /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
                /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff").
                /// </param>
                /// <returns>Usuario</returns>
                public static string ObtenerUsuarioBasicosDomicilio => UrlApiCuenta + "api/Usuario/Obtener_Usuario_Basicos_Domicilio";

                /// <summary>
                /// Obtención del usuario Ciudadano Digital con datos básicos y el domicilio a través del CUIL
                /// </summary>
                /// <param>
                /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.CUIL" tipo="Alfanumérico" long="30">CUIL del usuario.
                /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
                /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff").
                /// </param>
                /// <returns>Usuario</returns>
                public static string ObtenerUsuarioBasicosDomicilioCuil => UrlApiCuenta + "api/Usuario/Obtener_Usuario_Basicos_Domicilio_CUIL";

                /// <summary>
                /// Obtención del usuario Ciudadano Digital con datos básicos y el representado a través de la cookie
                /// </summary>
                /// <param>
                /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
                /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff").
                /// </param>
                /// <returns>Usuario</returns>
                public static string ObtenerUsuarioBasicosRepresentado => UrlApiCuenta + "api/Usuario/Obtener_Usuario_Basicos_Representado";

                /// <summary>
                /// Obtención de los datos del representado a través de la cookie
                /// </summary>
                /// <param>
                /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
                /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff").
                /// </param>
                /// <returns>Usuario</returns>
                public static string ObtenerRepresentado => UrlApiCuenta + "api/Usuario/Obtener_Representado";

                /// <summary>
                /// Cierre de sesión a través de la cookie
                /// </summary>
                /// <param>
                /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
                /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff").
                /// </param>
                /// <returns>Usuario</returns>
                public static string CerrarSesionUsuarioAplicacion => UrlApiCuenta + "api/Usuario/Cerrar_Sesion_Usuario_Aplicacion";
            }

            public class Representado
            {
                /// <summary>
                /// Obtención de un usuario y sus representados a través de la cookie
                /// </summary>
                /// <param>
                /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene del query string de la cookie encriptada por 
                /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff").
                /// </param>
                /// <returns>Usuario</returns>
                public static string ObtenerUsuarioYRepresentados => UrlApiCuenta + "api/Representado/Obtener_Usuario_Y_Representados";

                /// <summary>
                /// Obtención de un usuario y sus representados a través de la cookie
                /// </summary>
                /// <param>
                /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
                /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff").
                /// </param>
                /// <returns>Usuario</returns>
                public static string ObtenerUsuarioListaRepresentados => UrlApiCuenta + "api/Representado/Obtener_Usuario_Lista_Representados";

                /// <summary>
                /// Obtención del representado y un listado de representantes a través del CUIL/CUIT
                /// </summary>
                /// <param>
                /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.CuitCuil" tipo="Alfanumérico" long="30">CUIL del usuario o CUIT de la organización.
                /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff").
                /// </param>
                /// <returns>Usuario</returns>
                public static string ObtenerRepresentadoListaRepresentantes => UrlApiCuenta + "api/Representado/Obtener_Representado_Lista_Representantes";

                /// <summary>
                /// Obtención del usuario Ciudadano Digital con datos básicos y el representado a través de la cookie
                /// </summary>
                /// <param>
                /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
                /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff").
                /// </param>
                /// <returns>Usuario</returns>
                public static string ObtenerUsuarioBasicosRepresentado => UrlApiCuenta + "api/Representado/Obtener_Usuario_Basicos_Representado";

                /// <summary>
                /// Obtención de los datos del representado a través de la cookie
                /// </summary>
                /// <param>
                /// name="Entrada.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="Entrada.HashCookie" tipo="Alfanumérico" long="255">Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
                /// name="Entrada.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA1 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// name="Entrada.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff").
                /// </param>
                /// <returns>Usuario</returns>
                public static string ObtenerRepresentado => UrlApiCuenta + "api/Representado/Obtener_Representado";
            }

            public class Aplicacion
            {
                /// <summary>
                /// Cambio de contraseña de la aplicación. Para uso interno del desarrollador
                /// </summary>
                /// <param> name="IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación (provisto por la gestión del proyecto Ciudadano Digital).</param>
                /// <param> name="ContraseniaAnterior">Contraseña actual de la aplicación.</param>
                /// <param> name="ContraseniaNueva">Nueva contraseña a establecer para la aplicación.</param>
                /// <param> name="TokenValue" tipo="Alfanumérico" long="40">Contraseña de Ciudadano Digital. Consiste en un hash SHA1 del timestamp+SECRET_KEY para validar la integridad y autenticidad de los parámetros utilizados. La SECRET_KEY será una clave acordada entre la Aplicación y el portal.</param>
                /// <param> name="TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del mensaje. El formato debe ser yyyyMMddHHmmssfff. Ej: DateTime.Now.Tostring("yyyyMMddHHmmssfff");</param>
                /// <returns></returns>
                public static string CambiarContraseniaAplicacion => UrlApiCuenta + "api/Representado/Cambiar_Contrasenia_Aplicacion";

                /// <summary>
                /// Login de la aplicación y obtención del SesioHash
                /// </summary>
                /// <param> name="IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación (provisto por la gestión del proyecto Ciudadano Digital).</param>
                /// <param> name="Contrasenia">Contraseña de la aplicación.</param>
                /// <param> name="TokenValue" tipo="Alfanumérico" long="40">Contraseña de Ciudadano Digital. Consiste en un hash SHA1 del timestamp+SECRET_KEY para validar la integridad y autenticidad de los parámetros utilizados. La SECRET_KEY será una clave acordada entre la Aplicación y el portal.</param>
                /// <param> name="TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del mensaje. El formato debe ser yyyyMMddHHmmssfff. Ej: DateTime.Now.Tostring("yyyyMMddHHmmssfff");</param>
                /// <returns></returns>
                public static string RegistrarAplicacion => UrlApiCuenta + "api/Representado/Registrar_Aplicacion";
            }
        }

        public class ApiComunicacion
        {
            public class Notificacion
            {
                /// <summary>
                /// Envía notificaciones a ciudadanos a través del CUIL, que se muestran en el portal de Ciudadano Digital.
                /// </summary>
                /// <param>EntradaNotificacion</param>
                /// <returns>Respuesta</returns>
                public static string EnviarAlerta => UrlApiComunicacion + "api/Notificacion/Enviar_Alerta";
            }

            public class Sms
            {
                /// <summary>
                /// Envia un SMS a un usuario Ciudadano Digital a través del Cuil
                /// </summary>
                /// <param>Objecto SMS</param>
                /// <returns>ResultadoSMS</returns>
                public static string Enviar => UrlApiComunicacion + "api/SMS/Enviar";

                /// <summary>
                /// Envia un SMS a un usuario Ciudadano Digital a través de la cookie
                /// </summary>
                /// <param>Objecto SMS</param>
                /// <returns>ResultadoSMS</returns>
                public static string EnviarAplicacion => UrlApiComunicacion + "api/SMS/Enviar_Aplicacion";
            }

            public class Email
            {
                /// <summary>
                /// Envia un email a un usuario Ciudadano Digital a través del Cuil
                /// </summary>
                /// <param>Objecto Email</param>
                /// <returns>ResultadoEmail</returns>
                public static string Enviar => UrlApiComunicacion + "api/Email/Enviar";

                /// <summary>
                /// Envia un email a un usuario Ciudadano Digital a través de la cookie
                /// </summary>
                /// <param>Objecto Email</param>
                /// <returns>ResultadoEmail</returns>
                public static string EnviarAplicacion => UrlApiComunicacion + "api/Email/Enviar_Aplicacion";
            }
        }

        public class ApiDocumentacion
        {
            public class Documentacion
            {
                /// <summary>
                /// Obtención de los documentos del Ciudadano Digital a través del Cuil
                /// </summary>
                /// <param>
                /// Entrada.IdAplicacion tipo="Numérico" El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// Entrada.Contrasenia tipo="Alfanumérico" Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// Entrada.CUIL tipo="Alfanumérico" CUIL del usuario.
                /// Entrada.TokenValue tipo="Alfanumérico" Token. Consiste en un hash SHA512 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// Entrada.TimeStamp tipo="Alfanumérico" TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff").
                /// </param>
                /// <returns>RespuestaList</returns>
                public static string ObtenerDocumentacionUsuario => UrlApiDocumentacion + "api/Documentacion/Obtener_Documentacion_Usuario";

                /// <summary>
                /// Obtención de los documentos del Ciudadano Digital a través de la cookie
                /// </summary>
                /// <param>
                /// Entrada.IdAplicacion tipo="Numérico" El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// Entrada.Contrasenia tipo="Alfanumérico" Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// Entrada.HashCookie tipo="Alfanumérico" Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
                /// Entrada.TokenValue tipo="Alfanumérico" Token. Consiste en un hash SHA512 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// Entrada.TimeStamp tipo="Alfanumérico" TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff").
                /// </param>
                /// <returns>RespuestaList</returns>
                public static string ObtenerDocumentacionSesion => UrlApiDocumentacion + "api/Documentacion/Obtener_Documentacion_Sesion";

                /// <summary>
                /// Obtención de un listado de tipos de documentos permitidos en la plataforma Ciudadano Digital
                /// </summary>
                /// <param>
                /// Entrada.IdAplicacion tipo="Numérico" El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// Entrada.Contrasenia tipo="Alfanumérico" Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// Entrada.TokenValue tipo="Alfanumérico" Token. Consiste en un hash SHA512 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// Entrada.TimeStamp tipo="Alfanumérico" TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff").
                /// </param>
                /// <returns>RespuestaTipoDoc</returns>
                public static string ObtenerTipoDocumentos => UrlApiDocumentacion + "api/Documentacion/Obtener_Tipo_Documentos";

                /// <summary>
                /// Obtención de una vista previa del listado de documentos del Ciudadano Digital
                /// </summary>
                /// <param>
                /// Entrada.IdAplicacion tipo="Numérico" El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital
                /// Entrada.Contrasenia tipo="Alfanumérico" Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital
                /// Entrada.HashCookie tipo="Alfanumérico" Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
                /// Entrada.TokenValue tipo="Alfanumérico" Token. Consiste en un hash SHA512 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// Entrada.TimeStamp tipo="Alfanumérico" TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff")
                /// Entrada.Cuil tipo="Alfanumérico" CUIL del usuario al que se requiere obtener el documento.
                /// Entrada.DiccionarioDocumentos tipo="Numérico" Diccionario de datos con el identificador del documento y el tipo de documento IdDocumento;IdTipoDocumento
                /// </param>
                /// <returns>RespuestaVistaPrevia</returns>
                public static string ObtenerVistaPrevia => UrlApiDocumentacion + "api/Documentacion/Obtener_Vista_Previa";

                /// <summary>
                /// Obtención de un documento específico del Ciudadano Digital
                /// </summary>
                /// <param>
                /// Entrada.IdAplicacion tipo="Numérico" El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital
                /// Entrada.Contrasenia tipo="Alfanumérico" Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital
                /// Entrada.HashCookie tipo="Alfanumérico" Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
                /// Entrada.TokenValue tipo="Alfanumérico" Token. Consiste en un hash SHA512 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// Entrada.TimeStamp tipo="Alfanumérico" TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff")
                /// Entrada.Documentacion.IdDocumento tipo="Numérico" Identificador del documento que se quiere obtener.
                /// Entrada.Cuil tipo="Alfanumérico" CUIL del usuario al que se requiere obtener el documento.
                /// </param>
                /// <returns>RespuestaDoc</returns>
                public static string ObtenerDocumento => UrlApiDocumentacion + "api/Documentacion/Obtener_Documento";

                /// <summary>
                /// Obtención de la foto de perfil del Ciudadano Digital (Nivel 2)
                /// </summary>
                /// <param>
                /// Entrada.IdAplicacion tipo="Numérico" El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// Entrada.Contrasenia tipo="Alfanumérico" Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// Entrada.HashCookieOperador tipo="Alfanumérico" Valor que se obtiene de la cookie. Nombre de la cookie "CiDi".
                /// Entrada.TokenValue tipo="Alfanumérico" Token. Consiste en un hash SHA512 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// Entrada.TimeStamp tipo="Alfanumérico" TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff").
                /// Entrada.CUIL tipo="Alfanumérico" CUIL del usuario.
                /// </param>
                /// <returns>RespuestaList</returns>
                public static string ObtenerFotoPerfil => UrlApiDocumentacion + "api/Documentacion/Obtener_Foto_Perfil";

                /// <summary>
                /// Incorporar un documento en la plataforma al Ciudadano Digital
                /// </summary>
                /// <param>
                /// Entrada.IdAplicacion El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital
                /// Entrada.Contrasenia Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital
                /// Entrada.TokenValue Token. Consiste en un hash SHA512 del TimeStamp + SECRET_KEY sin guiones. Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// Entrada.TimeStamp TimeStamp del token. El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff");
                /// Entrada.CUIL CUIL del usuario al que se le asocia el documento.
                /// Entrada.HashCookie Valor de la cookie del operador.
                /// Entrada.Documentacion.Imagen Array de byte del documento, cifrado por la librería CryptoManager.
                /// Entrada.Documentacion.Extension Extensión del documento.
                /// Entrada.Documentacion.FechaVencimiento Fecha vencimiento del documento.
                /// Entrada.Documentacion.IdTipo Identificador del tipo documento.
                /// Entrada.Documentacion.Descripcion Descripción del documento.
                /// </param>
                /// <returns>RespuestaDocInsercion</returns>
                public static string GuardarDocumento => UrlApiDocumentacion + "api/Documentacion/Guardar_Documento";
            }
        }

        public class ApiMobile
        {
            public class Mobile
            {
                /// <summary>
                /// Inicio de sesión y obtención del usuario de la cuanta de Ciudadano Digital
                /// </summary>
                /// <param>
                /// name="EntradaLogin.IdAplicacion" tipo="Numérico" long="2">El identificador de aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="EntradaLogin.Contrasenia" tipo="Alfanumérico" long="30">Contraseña de la Aplicación. Provisto por la gestión del proyecto Ciudadano Digital.
                /// name="EntradaLogin.CUIL" tipo="Alfanumérico" long="30">CUIL del usuario.
                /// name="EntradaLogin.ContraseniaUsuario" tipo="Alfanumérico" long="30">Contraseña del usuario
                /// name="EntradaLogin.TokenValue" tipo="Alfanumérico" long="40">Token. Consiste en un hash SHA512 del TimeStamp + SECRET_KEY sin guiones. 
                /// Funciona como validador de la integridad y autenticidad de los parámetros utilizados.
                /// name="EntradaLogin.TimeStamp" tipo="Alfanumérico" long="17">TimeStamp del token. 
                /// El formato debe ser yyyyMMddHHmmssfff. Ej .NET C#: DateTime.Now.Tostring("yyyyMMddHHmmssfff").
                /// </param>
                /// <returns>Usuario</returns>
                public static string LoginUsuario => UrlApiMobile + "api/Mobile/Login_Usuario";
            }
        }

        public class ApiCdd
        {
            public class Documentos
            {

                public static string AutorizarSolicitud => UrlDocumentosCdd + "Authorize/Autorizar_Solicitud";

                public static string AutorizarSSolicitud => UrlDocumentosCdd + "Authorize/Autorizar_Solicitud_S_Key";

                public static string ObtenerCatalogos => UrlDocumentosCdd + "Documentacion/Obtener_Catalogos";

                public static string ObtenerListadoDocumentos => UrlDocumentosCdd + "Documentacion/Obtener_Listado_Documentos";

                public static string ObtenerListadoDocumentosConPreview => UrlDocumentosCdd + "Documentacion/Obtener_Listado_Documentos_Con_Preview";

                public static string ObtenerDocumento => UrlDocumentosCdd + "Documentacion/Obtener_Documento";

                public static string ObtenerSDocumento => UrlDocumentosCdd + "Documentacion/Obtener_S_Documento";

                public static string ObtenerDocumentoSBlob => UrlDocumentosCdd + "Documentacion/Obtener_Documento_S_Blob";

                public static string ObtenerDocumentoLargeFileData => UrlDocumentosCdd + "Documentacion/Obtener_Large_Data_File_Documento";

                public static string GuardarDocumento => UrlDocumentosCdd + "Documentacion/Guardar_Documento";

                public static string GuardarSDocumento => UrlDocumentosCdd + "Documentacion/Guardar_S_Documento";

                public static string ObtenerDocumentosExpedienteSuac => UrlDocumentosCdd + "Documentacion/Obtener_Documentos_Expediente_Suac";

                public static string GuardarExpedienteSuac => UrlDocumentosCdd + "Documentacion/Guardar_Expediente_Suac";

                public static string EliminarDocumento => UrlDocumentosCdd + "Documentacion/Eliminar_Documento"; 
            }
        }

        #endregion
    }
}
