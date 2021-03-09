using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class TipoConcepto: Entidad
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public enum Enum
        {
            Ingreso = 1,
            Gasto = 2
        }
    }
}
