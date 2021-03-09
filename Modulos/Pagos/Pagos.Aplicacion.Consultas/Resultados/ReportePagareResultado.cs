namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class ReportePagareResultado
    {
        public string NumeroFormulario { get; set; }
        public string Fecha { get; set; }
        public string FechaVencimientoPlanPago { get; set; }
        public string SolicitanteNombre { get; set; }
        public string SolicitanteDocumento { get; set; }
        public string SolicitanteDomicilioCompleto { get; set; }
        public string ValorPrestamo { get; set; }
        public string SucursalMontoDisponible { get; set; }
    }
}
