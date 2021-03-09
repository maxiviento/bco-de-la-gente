using System.Collections.Generic;
using Identidad.Aplicacion.Consultas.Consultas;
using Identidad.Aplicacion.Consultas.Resultados;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;

namespace Identidad.Dominio.IRepositorio
{
    public interface IPerfilRepositorio
    {
        IList<PerfilResultado> ConsultarConFiltros(PerfilConFiltrosConsulta consulta);
        Resultado<PerfilResultado> ConsultarConFiltrosPaginados(PerfilConFiltrosConsulta consulta);
        PerfilResultado ConsultarPorId(Id id);
        PerfilResultado ConsultarPorIdUsuario(Id idUsuario);
        IList<FuncionalidadResultado> ConsultarFuncionalidades(Id id);
        Id RegistrarPerfil(Perfil perfil);
        Id ActualizarPerfil(Perfil perfil);
        IEnumerable<MotivoDeBaja> ObtenerTodosMotivos(Ambito ambito);
        IList<PerfilResultado> ConsultarTodos();
        MotivoDeBaja ObtenerMotivoPorId(Ambito ambito, Id id);
        IList<Perfil> ObtenerPerfilesPorUsuarioId(Id usuarioId);
        bool PerfilEnUso(Id id);
        int ObtenerValorPaginacion();
    }
}