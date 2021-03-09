using ApiBatch.Utilidades;
using System.Collections.Generic;

namespace ApiBatch.GeneradoresArchivos
{
    public interface IGeneradorArchivos<T>
    {
        IList<HttpFile> DefinirArchivo(string processName);

        IList<T> Datos
        {
            set;
            get;
        }

        void AgregarDatos(IList<T> datos);
    }
}
