using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.Core.Comun.Dato;

namespace Utilidades.Importador
{
    public class TypeConverter
    {
        public static object ConvertTo(Type type, object value)
        {

            if (type == typeof(string))
                return Convert.ToString(value);

            if (type == typeof(int))
                return Convert.ToInt32(value);

            if (type == typeof(int?))
                return value != null ? Convert.ToInt32(value) : value;

            if (type == typeof(long))
                return Convert.ToInt64(value);

            if (type == typeof(long?))
                return value != null ? Convert.ToInt64(value) : value;

            if (type == typeof(decimal))
                return Convert.ToDecimal(value);

            if (type == typeof(decimal?))
                return value != null ? Convert.ToDecimal(value) : value;

            if (type == typeof(DateTime))
                return Convert.ToDateTime(value);

            if (type == typeof(DateTime?))
                return value != null ? Convert.ToDateTime(value) : value;

            if (type == typeof(Id))
            {
                var stringValue = Convert.ToString(value);

                return string.IsNullOrEmpty(stringValue) ? default(Id) : new Id(stringValue);
            }

            if (type == typeof(bool))
            {
                return bool.Parse(value.ToString());
            }

            if (type.BaseType != null && type.BaseType == (typeof(Entidad)))
            {
                var instance = (Entidad)Activator.CreateInstance(type);
                instance.Id = new Id(Convert.ToInt64(value));
                return instance;
            }


            return null;
        }
    }
}
