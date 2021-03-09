using System;
using System.ComponentModel;
using System.Globalization;

namespace Infraestructura.Core.Comun.Dato
{
    public class IdTypeConverter : TypeConverter
    {


        public override bool CanConvertTo(
            ITypeDescriptorContext context,
            Type destinationType)
        {
            return destinationType == typeof(int) || destinationType == typeof(long) ||
                   destinationType == typeof(decimal) || destinationType == typeof(string);

        }

        //cuando viene una peticion nueva
        public override bool CanConvertFrom(
            ITypeDescriptorContext context,
            Type sourceType)
        {
            return sourceType == typeof(int) || sourceType == typeof(long) || sourceType == typeof(decimal) ||
            sourceType == typeof(string);

        }
        //cuando viene una peticion nueva
        public override object ConvertFrom(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value)
        {

            if (value != null)
            {
                decimal valueAsDecimal;
                if (decimal.TryParse(value.ToString(), out valueAsDecimal))
                {
                    return new Id(valueAsDecimal);
                }
                else
                {
                    throw new FormatException("El formato del Id no es valido para convertirse en un decimal");
                }

            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value,
            Type destinationType)
        {
            var cast = value is Id ? (Id)value : new Id();
            if (destinationType == typeof(string) && cast != null)
            {
                return ((Id)cast).Valor.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}


