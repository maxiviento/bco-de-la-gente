using Infraestructura.Core.Comun.Presentacion;

namespace Infraestructura.Core.Comun.Dato
{
    public interface IRepositorio<TEntity>
    {
        /*TEntity ConsultarPorId(int id);
        IEnumerable<TEntity> ConsultarTodos();
        TEntity Guardar(TEntity tEntity);*/

        //TResultado EjecutarStoreProcedure<TResultado>(string spName, List<object> parameters) where TResultado : Dto;

        //TEntity EjecutarStoreProcedure<TEntity>(object obj);

        Resultado<R> ConsultarPor<C,R>(string spName, C c) where C : Consulta where R : class;
    }
}
