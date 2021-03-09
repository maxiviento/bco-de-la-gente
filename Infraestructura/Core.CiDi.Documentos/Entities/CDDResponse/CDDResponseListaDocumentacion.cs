using System.Collections.Generic;

namespace Core.CiDi.Documentos.Entities.CDDResponse
{
    public class CDDResponseListaDocumentacion : CDDResponse
    {
        public List<MetadataDocumentacionCDD> ListaMetadataCDD { get; set; }

        public CDDResponseListaDocumentacion()
        {
            ListaMetadataCDD = new List<MetadataDocumentacionCDD>();
        }
    }
}