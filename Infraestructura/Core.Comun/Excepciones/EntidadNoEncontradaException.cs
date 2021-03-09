namespace Infraestructura.Core.Comun.Excepciones
{
    public class EntidadNoEncontradaException : BaseApplicationException
    {
        public EntidadNoEncontradaException(string message): base(message)
        {
        }
    }
}
