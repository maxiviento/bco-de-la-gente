using System.ComponentModel;

namespace Core.CiDi.Documentos.Entities.Errores
{
    public enum EnumCDDError
    {
        #region ACCESO A DATOS

        [Description("Finalización correcta del proceso.")]
        SUCCESS_END_OPERATION = 1,

        [Description("No se encontraron datos.")]
        NO_DATA_FOUND_EXCEPTION = 2,

        [Description("Finalización incorrecta del proceso.")]
        FATAL_END_OPERATION_EXCEPTION = 3,

        [Description("El resultado devuelto por BD no está tipificado.")]
        CODE_RESULT_EXCEPTION = 4,

        #endregion

        #region HELPERS

        [Description("La solicitud a procesar no está permitida para la aplicación de origen y/o el catálogo especificado.")]
        INVALID_OPERATION_EXCEPTION = 5,

        [Description("Error al intentar calcular el peso de la imagen.")]
        SIZE_OF_EXCEPTION = 6,

        [Description("Error al intentar obtener la cantidad de páginas de la imagen.")]
        NUMBER_PAGES_EXCEPTION = 7,

        [Description("La aplicación de origen no posee el permiso sobre el catálogo especificado.")]
        PERMISSION_APPLICATION_CATALOG_EXCEPTION = 8,

        [Description("El valor de la shared key es incorrecto.")]
        ECCDH_VALUE_SHARED_KEY_EXCEPTION = 9,

        [Description("El identificador de aplicación de origen es incorrecto o no se encuentra.")]
        INVALID_SOURCE_APP_ID_EXCEPTION = 10,

        [Description("El identificador de la documentación es incorrecto o no se encuentra.")]
        INVALID_DOCUMENT_ID_EXCEPTION = 11,

        [Description("El identificador del catálogo es incorrecto o no se encuentra.")]
        INVALID_CATALOG_ID_EXCEPTION = 12,

        [Description("El identificador proporcionado está fuera del rango de valores permitidos.")]
        INVALID_RANGE_EXCEPTION = 13,

        [Description("El identificador del usuario es incorrecto o no se encuentra.")]
        INVALID_USER_ID_EXCEPTION = 14,

        [Description("El password de la aplicación de origen no ha sido provisto.")]
        NULL_EMPTY_PASSWORD_EXCEPTION = 15,

        [Description("La extensión de la imagen de documentación no ha sido provista.")]
        NULL_EMPTY_EXTENSION_EXCEPTION = 16,

        [Description("La longitud de la cadena de password excede el límite permitido.")]
        LENGHT_PASSWORD_EXCEPTION = 17,

        [Description("La longitud de la cadena de extensión excede el límite permitido.")]
        LENGHT_EXTENSION_EXCEPTION = 18,

        [Description("El password de la aplicación de origen es incorrecto.")]
        INVALID_PASSWORD_EXCEPTION = 19,

        [Description("La Key de la aplicación de origen no ha sido provista.")]
        NULL_EMPTY_KEY_EXCEPTION = 20,

        [Description("La longitud de la cadena de Key excede el límite permitido.")]
        LENGHT_KEY_EXCEPTION = 21,

        [Description("La Key de la aplicación de origen es incorrecta.")]
        INVALID_KEY_EXCEPTION = 22,

        [Description("La fecha de vigencia es incorrecta.")]
        DATE_VALIDITY_INVALID_EXCEPTION = 23,

        [Description("El token es incorrecto.")]
        INVALID_TOKEN_EXCEPTION = 24,

        [Description("El valor de la cadena de Token no ha sido provisto.")]
        NULL_EMPTY_TOKEN_EXCEPTION = 25,

        [Description("La longitud de la cadena de Token excede el límite permitido.")]
        LENGHT_TOKEN_EXCEPTION = 26,

        [Description("El formato del time stamp es incorrecto.")]
        TIME_STAMP_FORMAT_EXCEPTION = 27,

        [Description("El valor de la cadena del time stamp es incorrecto.")]
        NULL_EMPTY_TIME_STAMP_EXCEPTION = 28,

        [Description("La longitud de la cadena de time stamp excede el límite permitido.")]
        LENGHT_TIME_STAMP_EXCEPTION = 29,

        [Description("La imagen provista es incorrecta.")]
        INVALID_IMAGE_EXCEPTION = 30,

        [Description("La extensión de la imagen es incorrecta o no está permitida.")]
        INVALID_EXTENSION_EXCEPTION = 31,

        [Description("El valor de la clave BLOB pública es nula.")]
        PUBLIC_KEY_NULL_VALUE_EXCEPTION = 32,

        #endregion

        #region CONTROLLERS

        [Description("El modelo de datos es incorrecto.")]
        INVALID_MODEL_EXCEPTION = 33,

        [Description("El sistema se encuentra temporalemente inactivo.")]
        INACTIVE_WA_EXCEPTION = 34,

        #endregion

        #region VALIDATORS

        [Description("La aplicación de origen ha alcanzado el tope máximo de documentos a insertar en BD.")]
        DOCUMENT_MAX_INSERT = 35,

        [Description("El Id de Usuario especificado es incorrecto o no se encuentra.")]
        USER_ID_EXCEPTION = 36,

        [Description("El expediente solicitado se constituye por mas de un documento.")]
        TOO_MANY_ROWS_DOC_EXCEPTION = 37,

        [Description("No se autoriza el uso de la Web Api.")]
        AUTHORIZE_DENEGATE_EXCEPTION = 38

        #endregion
    }
}