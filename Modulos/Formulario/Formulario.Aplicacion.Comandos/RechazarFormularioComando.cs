using System.Collections.Generic;
using Formulario.Dominio.Modelo;

namespace Formulario.Aplicacion.Comandos
{
    public class RechazarFormularioComando
    {
        public int IdFormulario { get; set; }
        public IList<int> IdFormularios { get; set; }
        public int? IdMotivoBaja { get; set; }
        public int? IdMotivoRechazo { get; set; }
        public IList<MotivoRechazo> MotivosRechazo { get; set; }
        public string NumeroCaja { get; set; }
        public bool EsAsociativa { get; set; }
    }
}
