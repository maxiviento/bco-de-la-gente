namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class ImportarArchivoRecuperoResultado
    {
        public decimal CantTotal => CantProc + CantIncons + CantEspec;

        public decimal CantProc { get; set; }
        public decimal CantIncons { get; set; }
        public decimal CantEspec { get; set; }
        public decimal MontoRecuperado { get; set; }
        public decimal MontoRechazado { get; set; }
    }
}
