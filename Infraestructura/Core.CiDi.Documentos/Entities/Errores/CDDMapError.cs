using System;

namespace Core.CiDi.Documentos.Entities.Errores
{
    public class CDDMapError : Error
    {
        #region Constructores

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public CDDMapError()
            : base()
        {
        }

        /// <summary>
        /// Constructor parametrizado.
        /// </summary>
        /// <param name="codigo_WA_Error">Código de error.</param>
        /// <param name="descripcion_WA_Error">Descripción de error.</param>
        public CDDMapError(String codigo_WA_Error, String descripcion_WA_Error)
            : base(codigo_WA_Error, descripcion_WA_Error)
        {
        }

        /// <summary>
        /// Constructor parametrizado.
        /// </summary>
        /// <param name="codigo_WA_Error">Código de error.</param>
        public CDDMapError(int codigo_WA_Error)
        {
            // Seteo los atributos del error
            Set_WADocumentacion_Map_Error(codigo_WA_Error.ToString());
        }

        #endregion

        #region Metodos

        /// <summary>
        /// Seteo los valores del error generado.
        /// </summary>
        /// <param name="codigo_WA_Error">Código de error.</param>
        internal void Set_WADocumentacion_Map_Error(String codigo_WA_Error)
        {
            switch (codigo_WA_Error)
            {
                case "1":
                    base.Codigo_WA_Error = "SEO";
                    base.Descripcion_WA_Error = EnumCDDError.SUCCESS_END_OPERATION.ToDescription();
                    break;
                case "2":
                    base.Codigo_WA_Error = "NDFEXC";
                    base.Descripcion_WA_Error = EnumCDDError.NO_DATA_FOUND_EXCEPTION.ToDescription();
                    break;
                case "3":
                    base.Codigo_WA_Error = "FEOEXC";
                    base.Descripcion_WA_Error = EnumCDDError.FATAL_END_OPERATION_EXCEPTION.ToDescription();
                    break;
                case "4":
                    base.Codigo_WA_Error = "CODEXC";
                    base.Descripcion_WA_Error = EnumCDDError.CODE_RESULT_EXCEPTION.ToDescription();
                    break;
                case "5":
                    base.Codigo_WA_Error = "IOPEXC";
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_OPERATION_EXCEPTION.ToDescription();
                    break;
                case "6":
                    base.Codigo_WA_Error = "SIZEXC";
                    base.Descripcion_WA_Error = EnumCDDError.SIZE_OF_EXCEPTION.ToDescription();
                    break;
                case "7":
                    base.Codigo_WA_Error = "NPAEXC";
                    base.Descripcion_WA_Error = EnumCDDError.NUMBER_PAGES_EXCEPTION.ToDescription();
                    break;
                case "8":
                    base.Codigo_WA_Error = "PACEXC";
                    base.Descripcion_WA_Error = EnumCDDError.PERMISSION_APPLICATION_CATALOG_EXCEPTION.ToDescription();
                    break;
                case "9":
                    base.Codigo_WA_Error = "ESKEXC";
                    base.Descripcion_WA_Error = EnumCDDError.ECCDH_VALUE_SHARED_KEY_EXCEPTION.ToDescription();
                    break;
                case "10":
                    base.Codigo_WA_Error = "IIAEXC";
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_SOURCE_APP_ID_EXCEPTION.ToDescription();
                    break;
                case "11":
                    base.Codigo_WA_Error = "IIDEXC";
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_DOCUMENT_ID_EXCEPTION.ToDescription();
                    break;
                case "12":
                    base.Codigo_WA_Error = "IICEXC";
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_CATALOG_ID_EXCEPTION.ToDescription();
                    break;
                case "13":
                    base.Codigo_WA_Error = "RANEXC";
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_RANGE_EXCEPTION.ToDescription();
                    break;
                case "14":
                    base.Codigo_WA_Error = "IUCEXC";
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_USER_ID_EXCEPTION.ToDescription();
                    break;
                case "15":
                    base.Codigo_WA_Error = "NEPEXC";
                    base.Descripcion_WA_Error = EnumCDDError.NULL_EMPTY_PASSWORD_EXCEPTION.ToDescription();
                    break;
                case "16":
                    base.Codigo_WA_Error = "NEEEXC";
                    base.Descripcion_WA_Error = EnumCDDError.NULL_EMPTY_EXTENSION_EXCEPTION.ToDescription();
                    break;
                case "17":
                    base.Codigo_WA_Error = "LPAEXC";
                    base.Descripcion_WA_Error = EnumCDDError.LENGHT_PASSWORD_EXCEPTION.ToDescription();
                    break;
                case "18":
                    base.Codigo_WA_Error = "LEXEXC";
                    base.Descripcion_WA_Error = EnumCDDError.LENGHT_EXTENSION_EXCEPTION.ToDescription();
                    break;
                case "19":
                    base.Codigo_WA_Error = "IPAEXC";
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_PASSWORD_EXCEPTION.ToDescription();
                    break;
                case "20":
                    base.Descripcion_WA_Error = EnumCDDError.NULL_EMPTY_KEY_EXCEPTION.ToDescription();
                    break;
                case "21":
                    base.Descripcion_WA_Error = EnumCDDError.LENGHT_KEY_EXCEPTION.ToDescription();
                    break;
                case "22":
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_KEY_EXCEPTION.ToDescription();
                    break;
                case "23":
                    base.Codigo_WA_Error = "DVIEXC";
                    base.Descripcion_WA_Error = EnumCDDError.DATE_VALIDITY_INVALID_EXCEPTION.ToDescription();
                    break;
                case "24":
                    base.Codigo_WA_Error = "ITOEXC";
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_TOKEN_EXCEPTION.ToDescription();
                    break;
                case "25":
                    base.Codigo_WA_Error = "NETEXC";
                    base.Descripcion_WA_Error = EnumCDDError.NULL_EMPTY_TOKEN_EXCEPTION.ToDescription();
                    break;
                case "26":
                    base.Codigo_WA_Error = "LTOEXC";
                    base.Descripcion_WA_Error = EnumCDDError.LENGHT_TOKEN_EXCEPTION.ToDescription();
                    break;
                case "27":
                    base.Codigo_WA_Error = "TSFEXC";
                    base.Descripcion_WA_Error = EnumCDDError.TIME_STAMP_FORMAT_EXCEPTION.ToDescription();
                    break;
                case "28":
                    base.Codigo_WA_Error = "ETSEXC";
                    base.Descripcion_WA_Error = EnumCDDError.NULL_EMPTY_TIME_STAMP_EXCEPTION.ToDescription();
                    break;
                case "29":
                    base.Codigo_WA_Error = "LTSEXC";
                    base.Descripcion_WA_Error = EnumCDDError.LENGHT_TIME_STAMP_EXCEPTION.ToDescription();
                    break;
                case "30":
                    base.Codigo_WA_Error = "IIMEXC";
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_IMAGE_EXCEPTION.ToDescription();
                    break;
                case "31":
                    base.Codigo_WA_Error = "IEXEXC";
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_EXTENSION_EXCEPTION.ToDescription();
                    break;
                case "32":
                    base.Codigo_WA_Error = "PKVEXC";
                    base.Descripcion_WA_Error = EnumCDDError.PUBLIC_KEY_NULL_VALUE_EXCEPTION.ToDescription();
                    break;
                case "33":
                    base.Codigo_WA_Error = "IMOEXC";
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_MODEL_EXCEPTION.ToDescription();
                    break;
                case "34":
                    base.Codigo_WA_Error = "IWAEXC";
                    base.Descripcion_WA_Error = EnumCDDError.INACTIVE_WA_EXCEPTION.ToDescription();
                    break;
                case "35":
                    base.Codigo_WA_Error = "DMIEXC";
                    base.Descripcion_WA_Error = EnumCDDError.DOCUMENT_MAX_INSERT.ToDescription();
                    break;
                case "36":
                    base.Codigo_WA_Error = "INUEXC";
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_USER_ID_EXCEPTION.ToDescription();
                    break;
                case "37":
                    base.Codigo_WA_Error = "TMREXC";
                    base.Descripcion_WA_Error = EnumCDDError.TOO_MANY_ROWS_DOC_EXCEPTION.ToDescription();
                    break;
                case "38":
                    base.Codigo_WA_Error = "AUTEXC";
                    base.Descripcion_WA_Error = EnumCDDError.AUTHORIZE_DENEGATE_EXCEPTION.ToDescription();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Seteo los valores del error generado.
        /// </summary>
        /// <param name="codigo_WA_Error">Código de error.</param>
        internal void Set_WADocumentacion_Map_Error_Descripcion(String codigo_WA_Error)
        {
            switch (codigo_WA_Error)
            {
                case "SEO":
                    base.Descripcion_WA_Error = EnumCDDError.SUCCESS_END_OPERATION.ToDescription();
                    break;
                case "NDFEXC":
                    base.Descripcion_WA_Error = EnumCDDError.NO_DATA_FOUND_EXCEPTION.ToDescription();
                    break;
                case "FEOEXC":
                    base.Descripcion_WA_Error = EnumCDDError.FATAL_END_OPERATION_EXCEPTION.ToDescription();
                    break;
                case "CODEXC":
                    base.Descripcion_WA_Error = EnumCDDError.CODE_RESULT_EXCEPTION.ToDescription();
                    break;
                case "IOPEXC":
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_OPERATION_EXCEPTION.ToDescription();
                    break;
                case "SIZEXC":
                    base.Descripcion_WA_Error = EnumCDDError.SIZE_OF_EXCEPTION.ToDescription();
                    break;
                case "NPAEXC":
                    base.Descripcion_WA_Error = EnumCDDError.NUMBER_PAGES_EXCEPTION.ToDescription();
                    break;
                case "PACEXC":
                    base.Descripcion_WA_Error = EnumCDDError.PERMISSION_APPLICATION_CATALOG_EXCEPTION.ToDescription();
                    break;
                case "ESKEXC":
                    base.Descripcion_WA_Error = EnumCDDError.ECCDH_VALUE_SHARED_KEY_EXCEPTION.ToDescription();
                    break;
                case "IIAEXC":
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_SOURCE_APP_ID_EXCEPTION.ToDescription();
                    break;
                case "IIDEXC":
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_DOCUMENT_ID_EXCEPTION.ToDescription();
                    break;
                case "IICEXC":
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_CATALOG_ID_EXCEPTION.ToDescription();
                    break;
                case "RANEXC":
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_RANGE_EXCEPTION.ToDescription();
                    break;
                case "IUCEXC":
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_USER_ID_EXCEPTION.ToDescription();
                    break;
                case "NEPEXC":
                    base.Descripcion_WA_Error = EnumCDDError.NULL_EMPTY_PASSWORD_EXCEPTION.ToDescription();
                    break;
                case "NEEEXC":
                    base.Descripcion_WA_Error = EnumCDDError.NULL_EMPTY_EXTENSION_EXCEPTION.ToDescription();
                    break;
                case "LPAEXC":
                    base.Descripcion_WA_Error = EnumCDDError.LENGHT_PASSWORD_EXCEPTION.ToDescription();
                    break;
                case "LEXEXC":
                    base.Descripcion_WA_Error = EnumCDDError.LENGHT_EXTENSION_EXCEPTION.ToDescription();
                    break;
                case "IPAEXC":
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_PASSWORD_EXCEPTION.ToDescription();
                    break;
                case "NEKEXC":
                    base.Descripcion_WA_Error = EnumCDDError.NULL_EMPTY_KEY_EXCEPTION.ToDescription();
                    break;
                case "LKEEXC":
                    base.Descripcion_WA_Error = EnumCDDError.LENGHT_KEY_EXCEPTION.ToDescription();
                    break;
                case "IKEEXC":
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_KEY_EXCEPTION.ToDescription();
                    break;
                case "DVIEXC":
                    base.Descripcion_WA_Error = EnumCDDError.DATE_VALIDITY_INVALID_EXCEPTION.ToDescription();
                    break;
                case "ITOEXC":
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_TOKEN_EXCEPTION.ToDescription();
                    break;
                case "NETEXC":
                    base.Descripcion_WA_Error = EnumCDDError.NULL_EMPTY_TOKEN_EXCEPTION.ToDescription();
                    break;
                case "LTOEXC":
                    base.Descripcion_WA_Error = EnumCDDError.LENGHT_TOKEN_EXCEPTION.ToDescription();
                    break;
                case "TSFEXC":
                    base.Descripcion_WA_Error = EnumCDDError.TIME_STAMP_FORMAT_EXCEPTION.ToDescription();
                    break;
                case "ETSEXC":
                    base.Descripcion_WA_Error = EnumCDDError.NULL_EMPTY_TIME_STAMP_EXCEPTION.ToDescription();
                    break;
                case "LTSEXC":
                    base.Descripcion_WA_Error = EnumCDDError.LENGHT_TIME_STAMP_EXCEPTION.ToDescription();
                    break;
                case "IIMEXC":
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_IMAGE_EXCEPTION.ToDescription();
                    break;
                case "IEXEXC":
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_EXTENSION_EXCEPTION.ToDescription();
                    break;
                case "PKVEXC":
                    base.Descripcion_WA_Error = EnumCDDError.PUBLIC_KEY_NULL_VALUE_EXCEPTION.ToDescription();
                    break;
                case "IMOEXC":
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_MODEL_EXCEPTION.ToDescription();
                    break;
                case "IWAEXC":
                    base.Descripcion_WA_Error = EnumCDDError.INACTIVE_WA_EXCEPTION.ToDescription();
                    break;
                case "INUEXC":
                    base.Descripcion_WA_Error = EnumCDDError.INVALID_USER_ID_EXCEPTION.ToDescription();
                    break;
                case "TMREXC":
                    base.Descripcion_WA_Error = EnumCDDError.TOO_MANY_ROWS_DOC_EXCEPTION.ToDescription();
                    break;
                case "AUTEXC":
                    base.Descripcion_WA_Error = EnumCDDError.AUTHORIZE_DENEGATE_EXCEPTION.ToDescription();
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}