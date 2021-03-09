using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class SituacionDomicilioIntegranteResultado
    {
        public Id Id { get; set; }
        public Id IdGrupoUnico { get; set; }
        public string NumeroDocumento { get; set; }
        public string ApellidoNombre { get; set; }
        public string Sexo { get; set; }
        public bool Vigente { get; set; }
        public bool HayRechazo { get; set; }
        public decimal? MontoCuotasVencidas { get; set; }
        public decimal? MontoCuotasAVencer { get; set; }
        public decimal? MontoCuotasVencidasAsociativa { get; set; }
        public decimal? MontoCuotasAVencerAsociativa { get; set; }
    }
}