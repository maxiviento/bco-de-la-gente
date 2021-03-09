using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class EgresoGrupoResultado
    {
        public Id Id { get; set; }
        public string Concepto { get; set; }
        public decimal? Monto { get; set; }
    }
}
