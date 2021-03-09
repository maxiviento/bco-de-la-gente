using Infraestructura.Core.Comun.Dato;
using System;

namespace Infraestructura.Core.Datos.DSL
{
    static class TypeChecker
    {
        public static bool IsText(Type type)
        {
            return Type.GetTypeCode(type) == TypeCode.String;
        }

        public static bool IsBoolean(Type type)
        {
            return Type.GetTypeCode(type) == TypeCode.Boolean;
        }

        public static bool IsDecimal(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Decimal:
                case TypeCode.Double:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsInteger(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsDate(Type type)
        {
            return Type.GetTypeCode(type) == TypeCode.DateTime;
        }

        public static bool IsCharacter(Type type)
        {
            return Type.GetTypeCode(type) == TypeCode.Char;
        }

        public static bool IsId(Type type)
        {
            return type == typeof(Id);
        }

        public static object Converter(Type type, object value)
        {
            if (type == typeof(string))
                return Convert.ToString(value);
/*

            if (type == typeof(double))
                return Convert.ToDouble(value);*/

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
                return new Id(Convert.ToInt64(value));

            if (type == typeof(Id?))
                return value == null ? default(Id?) : new Id(Convert.ToInt64(value));

            if (type == typeof(bool))
            {
                var hasCorrectValue = value.ToString().Equals("S") || value.ToString().Equals("N");

                if (hasCorrectValue)
                {
                    return value.ToString().Equals("S");
                }
                else
                {
                    throw new ArgumentException("Para convertir desde un valor booleando, este debe contener N o S");
                }
            }/*

            if (type == typeof(bool?))
            {
                var hasCorrectValue = value == null || value.ToString().Equals("S") || value.ToString().Equals("N");

                if (hasCorrectValue)
                {
                    return value?.ToString().Equals("S");
                }
                else
                {
                    throw new ArgumentException("Para convertir desde un valor booleano, este debe contener N o S");
                }

            }
*/

            if (type.BaseType == null || type.BaseType != (typeof(Entidad))) return null;
            if (value.GetType() == type) return ((Entidad) value).Id.IsDefault() ? null : value;
            var instance = (Entidad) Activator.CreateInstance(type);
            instance.Id = new Id(Convert.ToInt64(value));
            return instance;
        }
    }
}