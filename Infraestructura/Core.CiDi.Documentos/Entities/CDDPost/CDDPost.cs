using System;
using Core.CiDi.Documentos.Entities.CDDAutorizador;

namespace Core.CiDi.Documentos.Entities.CDDPost
{
    public class CDDPost : CDDAutorizadorData
    {
        #region DOCUMENTO

        /// <summary>
        /// Identificador de documento.
        /// </summary>
        public Int32 Id_Documento { get; set; }

        /// <summary>
        /// Identificador de tipo de documento.
        /// </summary>
        public Int32 Id_Catalogo { get; set; }

        /// <summary>
        /// Dato BLOB cifrado para el almacenamiento de documentación digitalizada.
        /// </summary>
        public byte[] Blob_Imagen { get; set; }

        /// <summary>
        /// Extensión de la documentación digitalizada.
        /// </summary>
        public String Extension { get; set; }

        /// <summary>
        /// Vigencia de la documentación digitalizada.
        /// </summary>
        public DateTime? Vigencia { get; set; }

        /// <summary>
        /// Nombre del documento.
        /// </summary>
        public String N_Documento { get; set; }

        /// <summary>
        /// Indicador de Constatacion del documento.
        /// </summary>
        public bool? N_Constatado { get; set; }

        /// <summary>
        /// Descripción del documento.
        /// </summary>
        public String N_Descripcion { get; set; }

        #endregion
    }
}