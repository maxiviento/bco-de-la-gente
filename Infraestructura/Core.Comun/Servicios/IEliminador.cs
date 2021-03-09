namespace Infraestructura.Core.Comun.Servicios
{
    public interface IEliminador<T>
    {
        void Eliminar(T t);
    }
}