using System;
using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;

namespace Pagos.Aplicacion.Consultas.Consultas
{
    public class BandejaCambioEstadoConsulta : Consulta
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public int? NroFormulario { get; set; }
        public int? NroPrestamo { get; set; }
        public decimal? IdLinea { get; set; }
        public decimal? IdEstadoFormulario { get; set; }
        public decimal? IdElementoPago { get; set; }
        public string Cuil { get; set; }
        public string Dni { get; set; }
        public decimal? IdDepartamento { get; set; }
        public decimal? IdLocalidad { get; set; }
        public string NroSticker { get; set; }
    }
}