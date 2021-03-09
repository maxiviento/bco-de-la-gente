using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Comandos
{
    public class RegistrarOrdenFormularioComando
    {
        public Id IdLinea { get; set; }
        public IList<OrdenFormularioComando> Cuadrantes { get; set; }
        public decimal IdDetalleLinea { get; set; }
    }
}