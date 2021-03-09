using Infraestructura.Core.Comun.Presentacion;

namespace Formulario.Aplicacion.Consultas.Consultas
{
    public class DetalleLineaConsulta : Consulta
    {
        public decimal LineaId { get; set; }
        public bool DadosBaja { get; set; }
    }
}