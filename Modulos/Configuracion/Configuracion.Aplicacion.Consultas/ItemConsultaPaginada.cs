using Infraestructura.Core.Comun.Presentacion;

namespace Configuracion.Aplicacion.Consultas
{
    public class ItemConsultaPaginada : Consulta
    {
        //public string Nombre { get; set; }
        public decimal? IdItem { get; set; }
        public long? Recurso { get; set; }
        public bool? EsSubItem { get; set; }
        public bool? IncluirHijos { get; set; }
        public bool? IncluirDadosBaja { get; set; }
    }
}
