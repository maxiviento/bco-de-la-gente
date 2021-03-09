namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class PatrimonioSolicitanteResultado : ReporteFormularioResultado
    {
        public bool InmueblePropio { get; set; }
        public decimal? ValorInmueble { get; set; }
        public bool VehiculoPropio { get; set; }
        public decimal? CantVehiculos { get; set; } 
        public string ModeloVehiculos { get; set; }
        public decimal? ValorVehiculos { get; set; }
        public decimal? ValorDeudas { get; set; }
        public string PropiedadInmueble { get; set; }
        public string PropiedadVehiculos { get; set; }
    }
}
