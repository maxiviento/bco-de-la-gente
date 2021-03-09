using ApiBatch.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiBatch.GeneradoresArchivos
{
    public class GeneradorArchivos<T> : IGeneradorArchivos<T> where T : class
    {      
        public IList<T> Datos { get; set; }

        public GeneradorArchivos() { }

        public GeneradorArchivos(IList<T> datos)
        {
            Datos = datos;
        }

        public virtual IList<HttpFile> DefinirArchivo(string processName) { throw new NotImplementedException(); }

        public void AgregarDatos(IList<T> datos)
        {
            if (Datos == null)
                Datos = datos;
            else
                Datos = Datos.Concat(datos).ToList();
        }
    }
}