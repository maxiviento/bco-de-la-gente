using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class DeudaEmprendimientoResultado: ReporteFormularioResultado
    {
        public Id Id { get; set; }
        public Id IdEmprendimiento { get; set; }
        public Id IdTipoDeudaEmprendimiento { get; set; }
        public long Monto { get; set; }
        public string Descripcion { get; set; }
    }
}