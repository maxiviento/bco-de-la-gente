using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class IngresoGrupo: Entidad
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public TipoConcepto Concepto { get; set; }
        public double Valor { get; set; }
    }
}
