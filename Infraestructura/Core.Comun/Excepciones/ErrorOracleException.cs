using System;

namespace Infraestructura.Core.Comun.Excepciones
{
    public class ErrorOracleException : BaseApplicationException
    {
        public ErrorOracleException(string message): base(message)
        {
        }

        public ErrorOracleException(string message, string source) : base(message, source)
        {
        }

        public ErrorOracleException(string message, Exception inner) : base (message, inner)
        { }
        public ErrorOracleException(string message, Exception inner, string source) : base (message, inner, source)
        { }
    }
}
