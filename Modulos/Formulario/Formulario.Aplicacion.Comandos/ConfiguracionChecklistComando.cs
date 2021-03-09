using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Formulario.Aplicacion.Consultas.Resultados;

namespace Formulario.Aplicacion.Comandos
{
    public class ConfiguracionChecklistComando
    {
        [Required(ErrorMessage = "El id de la linea es requerido.")]
        public int IdLinea { get; set; }
        public int IdEtapa { get; set; }
        public IList<RequisitoChecklistComando> Requisitos { get; set; }
        public IList<EtapaEstadoLineaResultado> EtapasEstados { get; set; }
        public IList<decimal> IdsEtapasEliminadas { get; set; }
    }
}