namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class CabeceraResultadoBanco
    {
        public decimal ImporteTotal { get; set; }
        public string Periodo { get; set; }
        public string FormaPago{ get; set; }
        public string TipoPago { get; set; }
        public string IdBanco { get; set; }
        public string IdSucursal { get; set; }
    }
}
