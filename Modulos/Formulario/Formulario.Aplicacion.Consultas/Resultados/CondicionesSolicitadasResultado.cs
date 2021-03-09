namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class CondicionesSolicitadasResultado: ReporteFormularioResultado
    {
        public decimal? MontoSolicitado { get; set; }
        public decimal? CantidadCuotas { get; set; }
        public decimal? MontoEstimadoCuota { get; set; }
    }
}
