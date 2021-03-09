using Infraestructura.Core.Comun.Presentacion;

namespace Formulario.Aplicacion.Consultas.Consultas
{
    public class LineaPrestamoConsulta: Consulta
    {
        public string Nombre { get; set; }
        public bool ConOng { get; set; }
        public bool ConPrograma { get; set; }
        public bool ConDepartamento { get; set; }
        public decimal IdDestinatario { get; set; }
        public decimal IdMotivoDestino { get; set; }
        public decimal IdConvenioPago { get; set; }
        public decimal IdConvenioRecupero { get; set; }
        public bool DadosBaja { get; set; }
    }
}