using System;
using System.Collections.Generic;
using Core.CiDi.Documentos.Entities.CDDAutorizador;

namespace Core.CiDi.Documentos.Entities.CDDPost
{
    public class CDDPostListaDocumentacion : CDDAutorizadorData
    {
        /// <summary>
        /// Cantidad de documentación a ser gestionada en la lista.
        /// </summary>
        public int CantidadDocumentacion { get; set; }

        /// <summary>
        /// Lista de identificadores de documento.
        /// </summary>
        public List<Int32> ListaIdDocumentos { get; set; }

        public CDDPostListaDocumentacion()
        {
            CantidadDocumentacion = 0;
            ListaIdDocumentos = new List<Int32>();
        }
    }
}