namespace Infraestructura.Core.Comun.Servicios
{
    public interface IRegistrador<E, S>
    {
        S Registrar(E registrarObjetivoComando);
    }
}