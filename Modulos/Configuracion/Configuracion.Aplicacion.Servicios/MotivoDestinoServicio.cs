using System.Collections.Generic;
using System.Linq;
using Configuracion.Aplicacion.Comandos;
using Configuracion.Aplicacion.Comandos.Resultados;
using Configuracion.Aplicacion.Consultas;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.IRepositorio;
using Configuracion.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Servicios;
using DarDeBajaComando = Configuracion.Aplicacion.Comandos.DarDeBajaComando;

namespace Configuracion.Aplicacion.Servicios
{
    public class MotivoDestinoServicio
    {
        private readonly IMotivoDestinoRepositorio _motivoDestinoRepositorio;
        private readonly IMotivoBajaRepositorio _motivoBajaRepositorio;
        private readonly ISesionUsuario _sesionUsuario;

        public MotivoDestinoServicio(IMotivoDestinoRepositorio motivoDestinoRepositorio,
            IMotivoBajaRepositorio motivoBajaRepositorio,
            ISesionUsuario sesionUsuario)
        {
            _motivoDestinoRepositorio = motivoDestinoRepositorio;
            _motivoBajaRepositorio = motivoBajaRepositorio;
            _sesionUsuario = sesionUsuario;
        }

        public IList<MotivoDestinoResultado.Combo> ConsultarMotivosDestino()
        {
            var motivosDestino = _motivoDestinoRepositorio.ConsultarMotivosDestino();
            var motivosDestinoResultado = motivosDestino.Select(
                    motDest => new MotivoDestinoResultado.Combo
                    {
                        Id = motDest.Id,
                        Descripcion = motDest.Descripcion
                    })
                .ToList();

            return motivosDestinoResultado;
        }

        public NuevoMotivoDestinoResultado RegistrarMotivoDestino(MotivoDestinoComando comando)
        {
            if (_motivoDestinoRepositorio.ExisteMotivoDestinoConMismoNombre(comando.Nombre))
            {
                throw new ModeloNoValidoException(
                    "El motivo de destino que intenta registrar ya se encuentra registrado.");
            }
            Usuario usuario = _sesionUsuario.Usuario;
            var motivo = new MotivoDestino(comando.Nombre, comando.Descripcion, usuario);
            return new NuevoMotivoDestinoResultado
            {
                Id = _motivoDestinoRepositorio.RegistrarMotivoDestino(motivo)
            };
        }

        public MotivoDestinoResultado.Detallado ConsultarMotivoDestinoPorId(decimal id)
        {
            var motivo = _motivoDestinoRepositorio.ConsultarPorId(new Id(id));
            return new MotivoDestinoResultado.Detallado
            {
                Id = motivo.Id,
                Nombre = motivo.Nombre,
                Descripcion = motivo.Descripcion,
                IdUsuarioAlta = motivo.UsuarioAlta.Id,
                CuilUsuarioAlta = motivo.UsuarioAlta.Cuil,
                IdMotivoBaja = motivo.MotivoBaja?.Id,
                NombreMotivoBaja = motivo.MotivoBaja?.Descripcion,
                FechaUltimaModificacion = motivo.FechaUltimaModificacion,
                IdUsuarioUltimaModificacion = motivo.UsuarioUltimaModificacion.Id,
                CuilUsuarioUltimaModificacion = motivo.UsuarioUltimaModificacion.Cuil
            };
        }

        public Resultado<MotivoDestinoResultado.Grilla> ConsultaMotivosDestino(ConsultaMotivoDestino consulta)
        {
            if (consulta == null)
            {
                consulta = new ConsultaMotivoDestino() {NumeroPagina = 0};
            }
            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));

            return _motivoDestinoRepositorio.Consultar(consulta);
        }

        public void Modificar(int id, MotivoDestinoComando comando)
        {
            var motivo = _motivoDestinoRepositorio.ConsultarPorId(new Id(id));
            if (motivo.Nombre != comando.Nombre &&
                _motivoDestinoRepositorio.ExisteMotivoDestinoConMismoNombre(comando.Nombre))
            {
                throw new ModeloNoValidoException(
                    "Los datos que se intentan modificar ya existen para otro motivo de destino.");
            }
            Usuario usuario = _sesionUsuario.Usuario;
            motivo.Modificar(comando.Nombre, comando.Descripcion, usuario);
            _motivoDestinoRepositorio.Modificar(motivo);
        }

        public void DarDeBaja(int id, DarDeBajaComando comando)
        {
            Usuario usuario = _sesionUsuario.Usuario;
            var motivo = _motivoDestinoRepositorio.ConsultarPorId(new Id(id));
            motivo.DarDeBaja(_motivoBajaRepositorio.ConsultarPorId(new Id(comando.IdMotivoBaja)), usuario);
            _motivoDestinoRepositorio.DarDeBaja(motivo);
        }
    }
}