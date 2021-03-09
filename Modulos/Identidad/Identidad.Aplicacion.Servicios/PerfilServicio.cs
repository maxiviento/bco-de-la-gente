using System.Collections.Generic;
using System.Linq;
using Identidad.Aplicacion.Comandos;
using Identidad.Aplicacion.Consultas.Consultas;
using Identidad.Aplicacion.Consultas.Resultados;
using Identidad.Dominio.IRepositorio;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;


namespace Identidad.Aplicacion.Servicios
{
    public class PerfilServicio
    {
        private readonly IPerfilRepositorio _perfilRepositorio;
        private readonly ISesionUsuario _sesionUsuario;

        public PerfilServicio(IPerfilRepositorio perfilRepositorio, ISesionUsuario sesionUsuario)
        {
            _perfilRepositorio = perfilRepositorio;
            _sesionUsuario = sesionUsuario;
        }

        public IList<PerfilResultado> ConsultarConFiltros(PerfilConFiltrosConsulta consulta)
        {
            IList<PerfilResultado> resultado = _perfilRepositorio.ConsultarConFiltros(consulta)
                .OrderBy(perfil => perfil.Nombre)
                .ToList();

            return resultado;
        }

        public Resultado<PerfilResultado> ConsultarConFiltrosPaginados(PerfilConFiltrosConsulta consulta)
        {
            /*
             * Se agrega una consulta manual al valor de la paginación
             * ya que ParametrosSingleton no es accesible desde este srvicio.
             */

            consulta.TamañoPagina = _perfilRepositorio.ObtenerValorPaginacion();

            var resultado = _perfilRepositorio.ConsultarConFiltrosPaginados(consulta);

            resultado.Elementos = resultado.Elementos.OrderBy(perfil => perfil.Nombre).ToList();

            return resultado;
        }

        public PerfilResultado ConsultarPorId(Id id)
        {
            var perfil = _perfilRepositorio.ConsultarPorId(id);

            return perfil;
        }

        public PerfilResultado ConsultarPerfilPorIdUsuario(Id idUsuario)
        {
            var perfil = _perfilRepositorio.ConsultarPorIdUsuario(idUsuario);

            return perfil;
        }

        public IList<FuncionalidadResultado> ObtenerTodasFuncionalidades()
        {
            var funcionalidades = _perfilRepositorio.ConsultarFuncionalidades(new Id())
                .OrderBy(f => f.Nombre)
                .ToList();

            return funcionalidades;
        }

        public IList<FuncionalidadResultado> ObtenerFuncionalidadesPorPerfil(Id id)
        {
            var funcionalidades = _perfilRepositorio.ConsultarFuncionalidades(id)
                .OrderBy(f => f.Nombre)
                .ToList();

            return funcionalidades;
        }

        public IEnumerable<MotivoDeBaja> ObtenerTodosMotivos()
        {
            var ambito = Ambito.PERFIL;

            var motivos = _perfilRepositorio.ObtenerTodosMotivos(ambito);

            return motivos;
        }

        public Id Registrar(ActualizarPerfilComando comando)
        {
            Usuario usuario = _sesionUsuario.Usuario;

            IList<Funcionalidad> funcionalidades =
                comando.Funcionalidades.Select(f => new Funcionalidad() {Id = f}).ToList();

            Perfil perfil = new Perfil(comando.Nombre, usuario, funcionalidades);

            return _perfilRepositorio.RegistrarPerfil(perfil);
        }

        public Id Actualizar(Id id, ActualizarPerfilComando comando)
        {
            PerfilResultado perfilResultado = _perfilRepositorio.ConsultarPorId(id);

            if (perfilResultado == null)
                throw new ModeloNoValidoException("El perfil no existe");

            if (perfilResultado.FechaBaja.HasValue)
                throw new ModeloNoValidoException("El perfil se encuentra dado de baja");

            Usuario usuario = _sesionUsuario.Usuario;

            IList<Funcionalidad> funcionalidades =
                comando.Funcionalidades.Select(f => new Funcionalidad() {Id = f}).ToList();

            Perfil perfil = new Perfil() {Id = id};

            perfil.Modificar(comando.Nombre, usuario, funcionalidades);

            return _perfilRepositorio.ActualizarPerfil(perfil);
        }

        public Id DarDeBaja(Id id, DarDeBajaPerfilComando comando)
        {
            PerfilResultado perfilResultado = _perfilRepositorio.ConsultarPorId(id);

            if (perfilResultado == null)
                throw new ModeloNoValidoException("El perfil no existe");

            var perfilEnUso = _perfilRepositorio.PerfilEnUso(id);

            if (perfilEnUso)
                throw new ModeloNoValidoException("El perfil esta asignado a usuarios, no puede darse de baja.");
            
            if (perfilResultado.FechaBaja.HasValue)
                throw new ModeloNoValidoException("El perfil se encuentra dado de baja");

            Usuario usuario = _sesionUsuario.Usuario;

            Perfil perfil = new Perfil() {Id = id};

            MotivoDeBaja motivo = _perfilRepositorio.ObtenerMotivoPorId(Ambito.PERFIL, comando.MotivoBajaId);

            perfil.DarDeBaja(motivo, usuario);

            return _perfilRepositorio.ActualizarPerfil(perfil);
        }
    }
}