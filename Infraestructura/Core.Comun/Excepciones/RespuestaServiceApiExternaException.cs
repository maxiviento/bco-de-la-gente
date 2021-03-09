using System;

namespace Infraestructura.Core.Comun.Excepciones
{
    public class RespuestaServiceApiExternaException: BaseApplicationException
    {
        public RespuestaServiceApiExternaException(string message) : base(message)
        {
        }
        public RespuestaServiceApiExternaException(string message, string source) : base(message, source)
        {
        }

        public RespuestaServiceApiExternaException(string message, Exception inner) : base(message, inner)
        { }

        public RespuestaServiceApiExternaException(string message, Exception inner, string source) : base(message,
            inner, source)
        {}
    }
}
