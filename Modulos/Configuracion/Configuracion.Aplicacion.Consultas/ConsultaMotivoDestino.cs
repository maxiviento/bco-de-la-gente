using Infraestructura.Core.Comun.Presentacion;

namespace Configuracion.Aplicacion.Consultas
{
    public class ConsultaMotivoDestino : Consulta
    {
        public string Nombre { get; set; }
        public bool IncluirDadosDeBaja { get; set; }
    }
}