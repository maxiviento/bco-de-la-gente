using System.Collections.Generic;

namespace Infraestructura.Core.Comun.Servicios
{
    public interface IConsultor<T>
    {
        T ConsultarPorId(int id);
        IEnumerable<T> ConsultarTodos();
    }
}