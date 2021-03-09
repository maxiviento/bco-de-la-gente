using System;

namespace Core.CiDi.Documentos.Entities.CDDAutorizador
{
    public class CDDAutorizadorData
    {
        #region APLICACION DE ORIGEN

        /// <summary>
        /// Identificador de la Aplicación de Origen.
        /// </summary>
        public Int32 Id_Aplicacion_Origen { get; set; }

        /// <summary>
        /// Password de aplicación de origen.
        /// </summary>
        public String Pwd_Aplicacion { get; set; }

        #endregion

        #region AUTORIZADOR DIFFIE-HELLMAN

        /// <summary>
        /// Clave compartida generada con el Algoritmo de Diffie-Hellman.
        /// </summary>
        public byte[] Shared_Key { get; set; }

        #endregion

        #region USUARIO

        /// <summary>
        /// Identificador o CUIL de usuario.
        /// </summary>
        public String IdUsuario { get; set; }

        #endregion
    }
}