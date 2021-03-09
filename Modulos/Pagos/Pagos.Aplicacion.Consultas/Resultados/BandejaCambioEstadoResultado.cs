using System;
using System.Collections.Generic;

namespace Pagos.Aplicacion.Consultas.Resultados
{
    public class BandejaCambioEstadoResultado
    {
        public string Linea { get; set; }
        public decimal? IdFormulario { get; set; }
        public int? NroFormulario { get; set; }
        public int? NroPrestamo { get; set; }
        public string EstadoFormulario { get; set; }
        public string EstadoPrestamo { get; set; }
        public string Cuil { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public string Localidad { get; set; }
        public DateTime FechaFormulario { get; set; }
        public string ElementoPago { get; set; }
    }
}