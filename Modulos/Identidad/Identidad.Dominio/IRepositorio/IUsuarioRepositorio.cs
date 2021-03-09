using System.Collections.Generic;
using Identidad.Aplicacion.Consultas.Consultas;
using Identidad.Aplicacion.Consultas.Resultados;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;

namespace Identidad.Dominio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        IEnumerable<UsuarioResultado> ConsultarUsuarios();
        IEnumerable<UsuarioResultado> ConsultarConFiltros(string cidiHash, UsuarioConFiltrosConsulta consulta);
        Resultado<UsuarioResultado> ConsultarConFiltrosPaginados(string cidiHash, UsuarioConFiltrosConsulta consulta);
        UsuarioResultado ConsultarPorId(string cidiHash, Id usuarioId);
        Usuario ConsultarUsuarioPorCuil(string cidiHash, string cuil);
        UsuarioResultado ObtenerUsuarioCidi(string cidiHash, string cuil);
        Id Registrar(Usuario usuario);
        Id Actualizar(Usuario usuario);
        IEnumerable<MotivoDeBaja> ObtenerTodosMotivos(Ambito ambito);
        MotivoDeBaja ObtenerMotivoPorId(Ambito ambito, Id id);
        IEnumerable<UsuarioComboResultado> ConsultarUsuariosCombo();
        int ObtenerValorPaginacion();
        string CerrarSesionUsuario();
    }
}
