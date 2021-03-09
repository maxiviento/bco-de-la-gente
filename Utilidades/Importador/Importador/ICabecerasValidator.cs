using System.Collections.Generic;

namespace Utilidades.Importador
{
    public interface ICabecerasValidator
    {
        bool ValidarCabecera<T>(IList<string> cabeceras);
    }
}