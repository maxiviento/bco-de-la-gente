using System.Collections.Generic;

namespace Core.CiDi.Documentos.Entities.CDDResponse
{
    public class CDDResponseDocumentacion : CDDResponse
    {
        /// <summary>
        /// Listado de información de documentación digitalizada.
        /// </summary>
        public List<Documentacion> Lista_Documentos { get; set; }

        public CDDResponseDocumentacion()
        {
            Lista_Documentos = new List<Documentacion>();
        }
    }
}