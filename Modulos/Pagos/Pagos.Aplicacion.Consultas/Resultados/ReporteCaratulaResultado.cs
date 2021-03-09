namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class ReporteCaratulaResultado
    {
        public string NumeroFormulario { get; set; }
        public string Fecha { get; set; }
        public string SolicitanteNombre { get; set; }
        public string SolicitanteDocumento { get; set; }
        public string ValorPrestamo { get; set; }
        public string LineaPrestamo { get; set; }
        public string SolicitanteDomicilioCompleto { get; set; }
        public string GaranteNombre { get; set; }
        public string TelefonoTitular { get; set; }
        public string TelefonoGarante { get; set; }
        public string SucursalBancaria { get; set; }
        public string CodigoSucursal { get; set; }
        public string Departamento { get; set; }
        public string Localidad { get; set; }
    }
}
