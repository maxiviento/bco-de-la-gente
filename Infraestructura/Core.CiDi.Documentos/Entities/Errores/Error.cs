using System;

namespace Core.CiDi.Documentos.Entities.Errores
{
    public class Error
    {
        #region Propiedades

        /// <summary>
        /// Código de error representativo de la Web Api.
        /// </summary>
        public String Codigo_WA_Error { get; set; }

        /// <summary>
        /// Descripción de error representativo de la Web Api.
        /// </summary>
        public String Descripcion_WA_Error { get; set; }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por defecto.
        /// </summary>
        public Error()
        {
            Codigo_WA_Error = String.Empty;
            Descripcion_WA_Error = String.Empty;
        }

        /// <summary>
        /// Constructor parametrizado.
        /// </summary>
        public Error(String codigo_WA_Error, String descripcion_WA_Error)
        {
            Codigo_WA_Error = codigo_WA_Error;
            Descripcion_WA_Error = descripcion_WA_Error;
        }

        #endregion    
    }
}