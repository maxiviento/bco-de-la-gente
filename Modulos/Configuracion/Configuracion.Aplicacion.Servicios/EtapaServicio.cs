using System.Collections.Generic;
using System.Linq;
using Configuracion.Aplicacion.Comandos;
using Configuracion.Aplicacion.Comandos.Resultados;
using Configuracion.Aplicacion.Consultas;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.IRepositorio;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Excepciones;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Servicios;
using DarDeBajaComando = Configuracion.Aplicacion.Comandos.DarDeBajaComando;

namespace Configuracion.Aplicacion.Servicios
{
    public class EtapaServicio
    {
        private readonly IEtapaRepositorio _repositorio;
        private readonly IMotivoBajaRepositorio _motivoBajaRepositorio;
        private readonly ISesionUsuario _sesionUsuario;

        public EtapaServicio(IEtapaRepositorio repositorio, IMotivoBajaRepositorio motivoBajaRepositorio,
            ISesionUsuario sesionUsuario)
        {
            _repositorio = repositorio;
            _motivoBajaRepositorio = motivoBajaRepositorio;
            _sesionUsuario = sesionUsuario;
        }

        public NuevaEtapaResultado Registrar(RegistrarEtapaComando registrarEtapaComando)
        {
            if (_repositorio.ExisteEtapaConElMismoNombre(registrarEtapaComando.Nombre))
            {
                throw new ModeloNoValidoException("La etapa que intenta registrar ya se encuentra registrada");
            }

            Usuario usuario = _sesionUsuario.Usuario;

            var etapa = new Etapa(
                registrarEtapaComando.Nombre,
                registrarEtapaComando.Descripcion,
                usuario);

            return new NuevaEtapaResultado {Id = _repositorio.RegistrarEtapa(etapa)};
        }

        public Resultado<EtapaResultado.Consulta> ConsultarPorNombre(ConsultaEtapas consultaComando)
        {
            if (consultaComando == null)
            {
                consultaComando = new ConsultaEtapas {NumeroPagina = 0};
            }

            consultaComando.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));

            return _repositorio.ConsultarPorNombre(consultaComando);
        }

        public ConsultaEtapaPorIdResultado ConsultarPorId(decimal id)
        {
            var etapa = _repositorio.ConsultarPorId(id);
            return new ConsultaEtapaPorIdResultado
            {
                Id = etapa.Id,
                Nombre = etapa.Nombre,
                Descripcion = etapa.Descripcion,

                IdMotivoBaja = etapa.MotivoBaja?.Id,
                NombreMotivoBaja = etapa.MotivoBaja?.Descripcion,
                FechaUltimaModificacion = etapa.FechaUltimaModificacion,
                IdUsuarioUltimaModificacion = etapa.UsuarioUltimaModificacion.Id,
                CuilUsuarioUltimaModificacion = etapa.UsuarioUltimaModificacion.Cuil
            };
        }

        public void Modificar(int id, ModificarEtapaComando comando)
        {
            var etapa = _repositorio.ConsultarPorId(id);
            if (etapa.Nombre != comando.Nombre && _repositorio.ExisteEtapaConElMismoNombre(comando.Nombre))
            {
                throw new ModeloNoValidoException("Los datos que se intentan modificar ya existen para otra etapa");
            }

            Usuario usuario = _sesionUsuario.Usuario;

            etapa.Modificar(comando.Nombre, comando.Descripcion, usuario);
            _repositorio.Modificar(etapa);
        }

        public void DarDeBaja(int id, DarDeBajaComando comando)
        {
            Usuario usuario = _sesionUsuario.Usuario;
            var etapa = _repositorio.ConsultarPorId(id);
            etapa.DarDeBaja(_motivoBajaRepositorio.ConsultarPorId(new Id(comando.IdMotivoBaja)), usuario);
            _repositorio.DarDeBaja(etapa);
        }

        public IList<EtapaResultado.Consulta> ConsultarEtapas()
        {
            return _repositorio.ConsultarEtapas();
        }

        public IList<EtapaResultado.Consulta> ConsultarEtapasPorIdPrestamo(long idPrestamo)
        {
            var etapas = _repositorio.ConsultarEtapasPorPrestamo(idPrestamo);
            var etapasOrdenadas = etapas.OrderBy((etapa) => etapa.Id.Valor).ToList();
            return etapasOrdenadas;
        }
    }
}