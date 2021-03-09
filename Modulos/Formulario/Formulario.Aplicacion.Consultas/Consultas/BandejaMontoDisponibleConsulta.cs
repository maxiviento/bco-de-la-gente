using System;
using System.ComponentModel.DataAnnotations;
using Infraestructura.Core.Comun.Presentacion;

namespace Formulario.Aplicacion.Consultas.Consultas
{
    public class BandejaMontoDisponibleConsulta : Consulta
    {
        [Required(ErrorMessage = "La fecha inicio es requerida.")]
        public DateTime FechaDesde { get; set; }

        [Required(ErrorMessage = "La fecha fin es requerida.")]
        public DateTime FechaHasta { get; set; }

        public decimal? NroMonto { get; set; }

        public bool IncluirBaja { get; set; }
    }
}
