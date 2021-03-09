namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class BandejaAdendaResultado
    {
        public int Id { get; set; }
        public string Linea { get; set; }
        public string Departamento { get; set; }
        public string Localidad { get; set; }
        public int NroPrestamo { get; set; }
        public decimal? MontoOtorgado { get; set; }
        public int NroFormulario { get; set; }
        public string ApellidoYNombre { get; set; }
        public bool Agregado { get; set; }
    }
}
