using System;

namespace Utilidades.Importador
{
    public interface IConverter
    {
        bool CanConvert(Type type);
        object Convert(Type type, object value);
    }
}
