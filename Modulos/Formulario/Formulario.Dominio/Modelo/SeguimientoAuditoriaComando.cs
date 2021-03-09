using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Comandos
{
    public class SeguimientoAuditoriaComando
    {
        public Id Id { get; set; }
        public decimal? IdFormularioLinea { get; set; }
        public decimal? IdSeguimientoFormulario { get; set; }
        public decimal? IdAccion { get; set; }
        public decimal? IdPrestamoItem { get; set; }
        public decimal? IdUsuario { get; set; }
        public string Observaciones { get; set; }
    }
}
