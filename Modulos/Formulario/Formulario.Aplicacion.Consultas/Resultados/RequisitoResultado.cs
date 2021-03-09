using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class RequisitoResultado
    {
        public class Detallado
        {
            public Id Id { get; set; }
            public Id IdArea { get; set; }
            public string NombreArea { get; set; }
            public Id IdEtapa { get; set; }
            public string NombreEtapa { get; set; }
            public Id IdItem { get; set; }
            public string NombreItem { get; set; }
            public Id? ItemPadre { get; set; }
            public bool EsSolicitante { get; set; }
            public bool EsGarante { get; set; }
            public bool EsSolicitanteGarante { get; set; }
            public decimal NroOrden { get; set; }
            public string UrlRecurso { get; set; }
            public bool SubeArchivo { get; set; }
            public bool GeneraArchivo { get; set; }
            public Id? IdTipoDocumentacionCdd { get; set; }
        }

        public class Cargado
        {
            public Id Id { get; set; }
            public Id IdPrestamoRequisito { get; set; }
            public Id IdEtapa { get; set; }
            public bool EsSolicitante { get; set; }
            public bool EsGarante { get; set; }
            public bool EsSolicitanteGarante { get; set; }
            public bool EsAlerta { get; set; }
        }
    }
}