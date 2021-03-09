using Infraestructura.Core.Comun.Dato;
using System;
using System.Collections.Generic;

namespace Configuracion.Aplicacion.Consultas.Resultados
{
    public class ItemResultado
    {
        public class Detalle
        {
            public Id Id { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public Id IdUsuarioAlta { get; set; }
            public string CuilUsuarioAlta { get; set; }
            public Id? IdMotivoBaja { get; set; }
            public string NombreMotivoBaja { get; set; }
            public DateTime? FechaUltimaModificacion { get; set; }
            public Id IdUsuarioUltimaModificacion { get; set; }
            public string CuilUsuarioUltimaModificacion { get; set; }
            public IList<TipoItemResultado> TiposItem { get; set; }
            public Id? IdItemPadre { get; set; }
            public string NombreItemPadre { get; set; }
            public Id? IdRecurso { get; set; }
            public string NombreRecurso { get; set; }
            public string UrlRecurso { get; set; }
            public bool SubeArchivo { get; set; }
            public bool GeneraArchivo { get; set; }
            public Id? IdTipoDocumentacionCdd { get; set; }
            public string DescripcionTipoDocumentacion { get; set; }
        }

        public class Grilla
        {
            public Id Id { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public Id? IdMotivoBaja { get; set; }
            public DateTime? FechaUltimaModificacion { get; set; }
            public string RequisitoChecklist { get; set; }
            public string RequisitoSolicitante { get; set; }
            public string RequisitoGarante { get; set; }
            public string TieneCDD { get; set; }
        }

        public class Requisito
        {
            public Id Id { get; set; }
            public string Nombre { get; set; }
        }

        public class Recurso
        {
            public Id IdRecurso { get; set; }
            public string NombreRecurso { get; set; }
        }

        public class BandejaConfiguracionChecklist
        {
            public Id IdItem { get; set; }
            public Id? IdItemPadre { get; set; }
            public string NombreItem { get; set; }
            public bool EsDeChecklist { get; set; }
            public bool EsDeSolicitante { get; set; }
            public bool EsDeGarante { get; set; }
            public Id? IdArea { get; set; }
            public Id? IdEtapa { get; set; }
            public Id? IdRequisito { get; set; }
            public string NombreArea { get; set; }
            public string NombreEtapa { get; set; }
            public int NroOrden { get; set; }
            public int IdTipoRequisito { get; set; }
        }
    }
}