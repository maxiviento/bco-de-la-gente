using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Aplicacion.Comandos
{
    public class ModificacionOngLineaComando
    {
        public decimal IdLinea { get; set; }
        public IList<OngLinea> LsOngAgregadas { get; set; }
        public IList<OngLinea> LsOngQuitadas { get; set; }
    }
}
