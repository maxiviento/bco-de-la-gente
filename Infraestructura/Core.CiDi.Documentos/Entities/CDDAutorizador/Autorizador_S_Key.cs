using System;

namespace Core.CiDi.Documentos.Entities.CDDAutorizador
{
    public class Autorizador_S_Key
    {
        /// <summary>
        /// Identificador de la Aplicación generadora de la solicitud.
        /// </summary>
        public Int32 Id_Aplicacion_Origen { get; set; }

        /// <summary>
        /// Identificador de la Aplicación Padre contenedora.
        /// </summary>
        public Int32? Id_Aplicacion { get; set; }

        /// <summary>
        /// Password de aplicación de origen.
        /// </summary>
        public String Pwd_Aplicacion { get; set; }

        /// <summary>
        /// Key de aplicación de origen.
        /// </summary>
        public String Key_Aplicacion { get; set; }

        /// <summary>
        /// Operación a realizar en la solicitud.
        /// </summary>
        public String Operacion { get; set; }

        /// <summary>
        /// Identificador de usuario.
        /// </summary>
        public String Id_Usuario { get; set; }

        /// <summary>
        /// Cadena que representa el TimeStamp de la Shared Key en formato yyyyMMddHHmmssfff.
        /// </summary>
        public String Time_Stamp { get; set; }

        /// <summary>
        /// Token.
        /// </summary>
        public String Token { get; set; }

        /// <summary>
        /// Clave BLOB pública cifrada generada con el Algoritmo de Diffie-Hellman.
        /// </summary>
        public String Public_Blob_Key { get; set; }

        /// <summary>
        /// Clave compartida cifrada generada con el Algoritmo de Diffie-Hellman.
        /// </summary>
        public String Shared_Key { get; set; }
    }
}