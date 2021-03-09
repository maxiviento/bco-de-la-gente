using System;
using System.Collections.Generic;

namespace Core.CiDi.Documentos.Entities.CDDResponse
{
    public class CDDResponseCatalogos: CDDResponse
    {
        /// <summary>
        /// Lista de catálogos.
        /// </summary>
        public List<CatalogosPermiso> Lista_Catalogos { get; set; }
    }

    public class CatalogosPermiso
    {
        /// <summary>
        /// Identificador de catálogo.
        /// </summary>
        public int Id_Catalogo { get; set; }

        /// <summary>
        /// Nombre de catálogo.
        /// </summary>
        public string N_Catalogo { get; set; }

        /// <summary>
        /// Nombre de permiso.
        /// </summary>
        public String N_Permiso { get; set; }
    }
}