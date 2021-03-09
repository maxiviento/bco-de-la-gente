using System;

namespace Infraestructura.Core.Comun.Excepciones
{
    public class BaseApplicationException : ApplicationException
    {
        public BaseApplicationException(): base() { }

        public BaseApplicationException(string message): base(message)
        {
        }
        public BaseApplicationException(string message, string source) : base(message)
        {
            Source = source;
        }

        public BaseApplicationException(string message, Exception inner): base(message, inner)
        { }

        public BaseApplicationException(string message, Exception inner, string source) : base(message, inner)
        {
            Source = source;
        }
    }
}
