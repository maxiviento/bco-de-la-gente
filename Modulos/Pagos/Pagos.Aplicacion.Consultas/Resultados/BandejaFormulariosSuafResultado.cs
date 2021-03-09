using System;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class BandejaFormulariosSuafResultado
    {
        public int Id { get; set; }
        public string ApellidoYNombre { get; set; }
        public int IdPrestamo { get; set; }
        public string Linea { get; set; }
        public string Departamento { get; set; }
        public string Localidad { get; set; }
        public string Origen { get; set; }
        public int NroPrestamo { get; set; }
        public int NroFormulario { get; set; }
        public DateTime FechaPedido { get; set; }
        public string Devengado { get; set; }
        public DateTime? FechaDevengado { get; set; }
    }
}
