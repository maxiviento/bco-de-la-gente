using Infraestructura.Core.Comun.Dato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Aplicacion.Comandos
{
    public class OngLinea : Entidad
    {
        public decimal IdLineaOng { get; set; }
        public string Nombre { get; set; }
        public decimal PorcentajeRecupero { get; set; }
        public decimal PorcentajePago { get; set; }
    }
}
