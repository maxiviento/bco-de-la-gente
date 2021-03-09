using System;
using Core.CiDi.Documentos.Entities.CDDAutorizador;

namespace Core.CiDi.Documentos.Entities.CDDPost
{
    public class CDDPostExpediente : CDDAutorizadorData
    {
        #region METADATA DOCUMENTO CDD

        /// <summary>
        /// Identificador de documento.
        /// </summary>
        public Int32 Id_Documento { get; set; }

        /// <summary>
        /// Nombre del documento.
        /// </summary>
        public String N_Documento { get; set; }

        /// <summary>
        /// Identificador de catálogo.
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

        #endregion

        #region METADATA EXPEDIENTE CDD

        /// <summary>
        /// Identificador de expediente (Número de sticker).
        /// </summary>
        public String Id_Documentacion { get; set; }

        /// <summary>
        /// Descripción del documento por expediente.
        /// </summary>
        public String N_Descripcion { get; set; }

        /// <summary>
        /// Numero de trámite del expediente.
        /// </summary>
        public String Id_Tramite { get; set; }

        /// <summary>
        /// Identificador de expediente.
        /// </summary>
        public String Id_Expediente { get; set; }

        /// <summary>
        /// Identificador de Pase.
        /// </summary>
        public Int32 Id_Pase { get; set; }

        /// <summary>
        /// Cantidad de cuerpos que componenen el expediente.
        /// </summary>
        public Int32 Id_Cuerpo { get; set; }

        /// <summary>
        /// Cantidad de fojas que componenen el expediente.
        /// </summary>
        public Int32 Id_Foja { get; set; }

        /// <summary>
        /// Cuil / Cuit de usuario.
        /// </summary>
        public String N_Usuario { get; set; }

        #endregion

        #region METADATA DOCUMENTO CIDI

        public String HashCookie { get; set; }
        public String TokenValue { get; set; }
        public String TimeStamp { get; set; }
        public Core.CiDi.Documentos.Entities.Ciudadano_Digital.Documentacion Documentacion { get; set; }
        public int CantidadRegistros { get; set; }
        public String CuilCuitCiDi { get; set; }

        #endregion

        public CDDPostExpediente()
        {
            Documentacion = new Core.CiDi.Documentos.Entities.Ciudadano_Digital.Documentacion();
        }
    }
}