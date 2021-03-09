using System.Collections.Generic;

namespace Formulario.Aplicacion.Comandos
{
    public class NumeroCajaComando
    {
        public long IdFormularioLinea { get; set; }
        public decimal IdUsuario { get; set; }
        public virtual string NumeroCaja { get; set; }
    }
}
