using System.Collections.Generic;

namespace Infraestructura.Core.Comun.Excepciones
{
    public class ModeloNoValidoException: BaseApplicationException
    {
        public IEnumerable<string> Errores { get; private set; }

        public ModeloNoValidoException(string message): base(message)
        {
            Errores = new List<string>() {message};
        }

        public ModeloNoValidoException(IEnumerable<string> messages)
        {
            Errores = messages;
        }
    }
}