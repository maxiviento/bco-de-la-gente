using System;

namespace Infraestructura.Core.Comun.Excepciones
{
    public class ErrorTecnicoException: BaseApplicationException
    {
        public ErrorTecnicoException(string message): base(message)
        {
        }

        public ErrorTecnicoException(string message, string source) : base(message, source)
        {
        }

        public ErrorTecnicoException(string message, Exception inner) : base(message, inner)
        {
        }

        public ErrorTecnicoException(string message, Exception inner, string source) : base(message, inner, source)
        {
        }
    }
}
