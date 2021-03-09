using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class InversionRealizadaResultado : ReporteFormularioResultado
    {
        public Id Id { get; set; }
        public Id IdTipoInversion { get; set; }
        public Id IdItemInversion { get; set; }
        public Id IdInversionEmprendimiento { get; set; }
        public string Observaciones { get; set; }
        public long CantidadNuevos { get; set; }
        public long CantidadUsados { get; set; }
        public long PrecioNuevos { get; set; }
        public long PrecioUsados { get; set; }
    }
}