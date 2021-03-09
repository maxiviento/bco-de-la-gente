using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soporte.Aplicacion.Servicios
{
    public static class StringExtension
    {
        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, string value)
        {
            return condition ? builder.Append(value): builder;
        }
    }
}
