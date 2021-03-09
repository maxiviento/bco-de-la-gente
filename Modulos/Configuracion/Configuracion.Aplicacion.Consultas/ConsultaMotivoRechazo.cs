using Infraestructura.Core.Comun.Presentacion;

namespace Configuracion.Aplicacion.Consultas
{
    public class ConsultaMotivoRechazo: Consulta
    {
        public string Abreviatura { get; set; }
        public long? AmbitoId { get; set; }
        public bool? Automatico { get; set; }
        public string Codigo { get; set; }
        public bool IncluirDadosDeBaja { get; set; }
    }
}
