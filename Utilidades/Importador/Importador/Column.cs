using System;
using System.Reflection;

namespace Utilidades.Importador
{
    public abstract class Column
    {
       
        public PropertyInfo Property { get; set; }
        public Type PropertyType { get; set; }
    }
}
