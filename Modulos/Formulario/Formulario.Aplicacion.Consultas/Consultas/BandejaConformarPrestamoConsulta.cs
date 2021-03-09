using System;
using System.ComponentModel.DataAnnotations;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;

namespace Formulario.Aplicacion.Consultas.Consultas
{
    public class BandejaConformarPrestamoConsulta : Consulta
    {
        [Required(ErrorMessage = "La fecha desde es requerida.")]
        public DateTime FechaDesde { get; set; }

        [Required(ErrorMessage = "La fecha hasta es requerida.")]
        public DateTime FechaHasta { get; set; }

        public string LocalidadIds { get; set; }

        public string Cuil { get; set; }

        public Id IdOrigen { get; set; }

        public string EstadoFormularioIds { get; set; }

        public string IdLinea { get; set; }
        public string DepartamentoIds { get; set; }
        public decimal NumeroFormulario { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
