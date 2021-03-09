using System;
using System.ComponentModel;
using System.Reflection;
namespace Core.CiDi.Documentos.Entities.Errores
{
    public static class AttributesHelperExtension
    {
        public static string ToDescription(this Enum value)
        {
            var da = (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return da.Length > 0 ? da[0].Description : value.ToString();
        }

        public static string GetDescription<T>(this object enumerationValue) where T : struct
        {
            Type type = enumerationValue.GetType();

            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue debe ser un tipo Enum", "enumerationValue");
            }

            // Intenta encontrar el atributo Description para el nombre de la enumeración
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    // Retorna el valor del atributo Description de la enumeración
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            // Si no esta definido el atributo Description retorna el valor ToString()
            return enumerationValue.ToString();

        }

        public static T GetValue<T>(this Enum enumeration)
        {
            T result = default(T);

            try
            {
                result = (T)Convert.ChangeType(enumeration, typeof(T));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
    }
}