using System.Collections.Generic;
using Core.CiDi.Documentos.Entities.Auditoria;

namespace Core.CiDi.Documentos.Entities.CDDResponse
{
    public class CDDResponseAuditoriaGeneral : CDDResponse
    {
        /// <summary>
        /// Listado de auditoria general sobre un documento digitalizado.
        /// </summary>
        public List<AuditoriaGeneral> Lista_Auditoria_General { get; set; }

        public CDDResponseAuditoriaGeneral()
        {
            Lista_Auditoria_General = new List<AuditoriaGeneral>();
        }
    }
}