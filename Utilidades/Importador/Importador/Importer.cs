using Utilidades.Exportador.Builders;

namespace Utilidades.Importador
{
    public class Importer
    {

        public static Builders.SecuencialBuilder<T> FromSecuencial<T>()
        {
            return new Builders.SecuencialBuilder<T>();
        }

        public static SeparatedValuesBuilder<T> FromSeparatedValues<T>(string separatorValue)
        {
            return new SeparatedValuesBuilder<T>(separatorValue);
        }

        public static SeparatedValuesBuilder<T> FromSeparatedValues<T>(string separatorValue, string newLine)
        {
            return new SeparatedValuesBuilder<T>(separatorValue, newLine);
        }


        public static Builders.ExcelBuilder<T> FromExcel<T>()
        {
            return new Builders.ExcelBuilder<T>();
        }
    }
}
