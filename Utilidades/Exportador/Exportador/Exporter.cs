using Utilidades.Exportador.Builders;

namespace Utilidades.Exportador
{
    public class Exporter
    {
        public static SecuencialBuilder<T> ToSecuencial<T>()
        {
            return new SecuencialBuilder<T>();
        }

        public static ExcelBuilder<T> ToExcel<T>()
        {
            return new ExcelBuilder<T>();
        }

        public static SeparatedValuesBuilder<T> ToSeparatedValues<T>(string separatorValue)
        {
            return new SeparatedValuesBuilder<T>(separatorValue);
        }

        public static SeparatedValuesBuilder<T> ToSeparatedValues<T>(string separatorValue, string newLine)
        {
            return new SeparatedValuesBuilder<T>(separatorValue, newLine);
        }
    }
}
