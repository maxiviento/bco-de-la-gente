using System;

namespace Infraestructura.Core.Comun.Excepciones
{
    public class GrupoUnicoException : BaseApplicationException
    {
        public GrupoUnicoException(string message): base(message)
        {
        }
        public GrupoUnicoException(string message, string source) : base(message, source)
        {
        }

        public GrupoUnicoException(string message, Exception inner) : base (message, inner)
        { }
        public GrupoUnicoException(string message, Exception inner, string source) : base (message, inner, source)
        { }
    }
}
