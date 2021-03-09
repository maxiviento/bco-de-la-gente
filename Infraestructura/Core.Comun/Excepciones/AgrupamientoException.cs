using System.Collections.Generic;

namespace Infraestructura.Core.Comun.Excepciones
{
    public class AgrupamientoException: BaseApplicationException
    {
        public IEnumerable<string> Errores { get; private set; }

        public AgrupamientoException(string message): base(message)
        {
            Errores = new List<string>() {message};
        }

        public AgrupamientoException(IEnumerable<string> messages)
        {
            Errores = messages;
        }
    }
}