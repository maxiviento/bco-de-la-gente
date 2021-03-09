using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class FormularioFechaPagoResultado
    { 
        public decimal? IdFormulario { get; set; }
        public DateTime FecInicioPago { get; set; }
        public DateTime FecFinPago { get; set; }
        public bool EsAsociativa { get; set; }
        public decimal? CantMinForms { get; set; }
        public decimal? EstadoForm { get; set; }
        public decimal? CantForms { get; set; }
        public string ModPago { get; set; }
        public string ElementoPago { get; set; }
        public decimal? ConvenioPago { get; set; }
        public decimal? TipoApoderado { get; set; }
    }
}
