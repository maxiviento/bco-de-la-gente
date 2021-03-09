using System;
using System.Reflection;

namespace Utilidades.Exportador
{
    public abstract class Column
    {
       
        public PropertyInfo Property { get; set; }
        public Type PropertyType { get; set; }
    }
}
