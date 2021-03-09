using System;

namespace Infraestructura.Core.CiDi.Model
{
    public class DocumentoCdd
    {
        /// <summary>
        /// Identificador de documento.
        /// </summary>
        public int IdDocumento { get; set; }

        /// <summary>
        /// Identificador de tipo de documento.
        /// </summary>
        public int IdCatalogo { get; set; }

        /// <summary>
        /// Dato BLOB cifrado para el almacenamiento de documentación digitalizada.
        /// </summary>
        public byte[] BlobImagen { get; set; }

        /// <summary>
        /// Extensión de la documentación digitalizada.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Vigencia de la documentación digitalizada.
        /// </summary>
        public DateTime? Vigencia { get; set; }

        /// <summary>
        /// Nombre del documento.
        /// </summary>
        public string NombreDocumento { get; set; }

        /// <summary>
        /// Descripción del documento.
        /// </summary>
        public string Descripcion { get; set; }
    }
}
