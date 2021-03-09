namespace Infraestructura.Core.Comun.Servicios
{
    public interface IEditor<T>
    {
        void Actualizar(T t);
    }
}