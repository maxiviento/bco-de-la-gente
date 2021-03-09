using System;

namespace Core.CiDi.Documentos.Entities.Imagen
{
    public class S_Imagen
    {
        /// <summary>
        /// Imagen de documento cifrada.
        /// </summary>
        public String Imagen_Documentacion { get; set; }

        /// <summary>
        /// Preview de la Imagen de documento cifrada.
        /// </summary>
        public String Preview { get; set; }

        /// <summary>
        /// Extensión de la imagen de documento.
        /// </summary>
        public String Extension { get; set; }

        /// <summary>
        /// Peso de la imagen de documento.
        /// </summary>
        public String Peso_MB { get; set; }

        /// <summary>
        /// Páginas que componen el documento.
        /// </summary>
        public Int16 Paginas { get; set; }
    }
}