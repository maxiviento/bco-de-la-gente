using System.Collections.Generic;
using System.Linq;
using Configuracion.Dominio.IRepositorio;
using Formulario.Aplicacion.Comandos;
using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Servicios;
using DarDeBajaComando = Configuracion.Aplicacion.Comandos.DarDeBajaComando;

namespace Formulario.Aplicacion.Servicios
{
    public class MontoDisponibleServicio
    {
        private readonly IMontoDisponibleRepositorio _montoDisponibleRepositorio;
        private readonly ISesionUsuario _sesionUsuario;
        private readonly IMotivoBajaRepositorio _motivoBajaRepositorio;

        public MontoDisponibleServicio(
            IMontoDisponibleRepositorio montoDisponibleRepositorio,
            ISesionUsuario sesionUsuario,
            IMotivoBajaRepositorio motivoBajaRepositorio)
        {
            _montoDisponibleRepositorio = montoDisponibleRepositorio;
            _sesionUsuario = sesionUsuario;
            _motivoBajaRepositorio = motivoBajaRepositorio;
        }

        public RegistrarMontoDisponibleResultado Registrar(RegistrarMontoDisponibleComando comando)
        {
            if (_montoDisponibleRepositorio.ExisteMontoDisponibleConLaMismaDescripcion(comando.Descripcion))
            {
                throw new ModeloNoValidoException("Los descripción ya existe para otro monto disponible.");
            }

            var montoDisponible = new MontoDisponible(
                    comando.Descripcion,
                    comando.Monto,
                    comando.FechaDepositoBancario,
                    comando.FechaInicioPago,
                    comando.FechaFinPago,
                    comando.IdBanco,
                    comando.IdSucursal,
                    _sesionUsuario.Usuario
                    );

            return new RegistrarMontoDisponibleResultado(_montoDisponibleRepositorio.Registrar(montoDisponible));
        }

        public MontoDisponibleResultado ObtenerPorId(decimal id)
        {
            var montoDisponible = _montoDisponibleRepositorio.ObtenerPorId(id);
            return new MontoDisponibleResultado
            {
                Id = montoDisponible.Id.Valor,
                Descripcion = montoDisponible.Descripcion,
                IdBanco = montoDisponible.IdBanco,
                IdSucursal = montoDisponible.IdSucursal,
                Monto = montoDisponible.Monto,
                NroMonto = montoDisponible.NroMonto,
                FechaDepositoBancario = montoDisponible.FechaDepositoBancario,
                FechaInicioPago = montoDisponible.FechaInicioPago,
                FechaFinPago = montoDisponible.FechaFinPago,
                IdMotivoBaja = montoDisponible.MotivoBaja?.Id.Valor,
                NombreMotivoBaja = montoDisponible.MotivoBaja?.Descripcion,
                CuilUsuarioUltimaModificacion = montoDisponible.UsuarioUltimaModificacion.Cuil,
                FechaUltimaModificacion = montoDisponible.FechaUltimaModificacion
            };
        }

        public MontoDisponibleResultado ObtenerPorNro(decimal nroMonto)
        {
            var montoDisponible = _montoDisponibleRepositorio.ObtenerPorNro(nroMonto);
            return new MontoDisponibleResultado
            {
                Id = montoDisponible.Id.Valor,
                Descripcion = montoDisponible.Descripcion,
                IdBanco = montoDisponible.IdBanco,
                IdSucursal = montoDisponible.IdSucursal,
                Monto = montoDisponible.Monto,
                NroMonto = montoDisponible.NroMonto,
                FechaDepositoBancario = montoDisponible.FechaDepositoBancario,
                FechaInicioPago = montoDisponible.FechaInicioPago,
                FechaFinPago = montoDisponible.FechaFinPago,
                IdMotivoBaja = montoDisponible.MotivoBaja?.Id.Valor,
                NombreMotivoBaja = montoDisponible.MotivoBaja?.Descripcion,
                CuilUsuarioUltimaModificacion = montoDisponible.UsuarioUltimaModificacion.Cuil,
                FechaUltimaModificacion = montoDisponible.FechaUltimaModificacion,
                Saldo = montoDisponible.Saldo
            };
        }

        public bool DarDeBaja(decimal id, DarDeBajaComando comando)
        {
            var usuario = _sesionUsuario.Usuario;
            var montoDisponible = _montoDisponibleRepositorio.ObtenerPorId(id);
            montoDisponible.DarDeBaja(_motivoBajaRepositorio.ConsultarPorId(new Id(comando.IdMotivoBaja)), usuario);
            return _montoDisponibleRepositorio.DarDeBaja(montoDisponible);
        }

        public EditarMontoDisponibleResultado Modificar(decimal id, ModificarMontoDisponibleComando comando)
        {
            var montoDisponible = _montoDisponibleRepositorio.ObtenerPorId(id);
            if (montoDisponible.Descripcion != comando.Descripcion && _montoDisponibleRepositorio.ExisteMontoDisponibleConLaMismaDescripcion(comando.Descripcion))
            {
                throw new ModeloNoValidoException("Los descripción ya existe para otro monto disponible.");
            }
            
            var usuario = _sesionUsuario.Usuario;
            montoDisponible.Modificar(comando.Descripcion, comando.Monto, comando.FechaDepositoBancario, comando.FechaInicioPago, comando.FechaFinPago, comando.IdBanco, comando.IdSucursal, usuario);
            return _montoDisponibleRepositorio.Modificar(montoDisponible);
        }

        public Resultado<BandejaMontoDisponibleResultado> ConsultaBandeja(BandejaMontoDisponibleConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new BandejaMontoDisponibleConsulta { NumeroPagina = 0 };
            }
            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));

            return _montoDisponibleRepositorio.ObtenerMontosDisponibleBandeja(consulta);
        }

        public IList<ClaveValorResultado<string>> ConsultarCombo()
        {
            var montos = _montoDisponibleRepositorio.ConsultarMontoDisponiblesCombo();
            return montos.Select(monto => new ClaveValorResultado<string>
            (
                monto.Id.Valor.ToString(),
                monto.Descripcion
            )).ToList();
        }

        public Resultado<MovimientoMontoResultado> ConsultaMovimientosMonto(MovimientosMontoConsulta consulta)
        {
            if (consulta == null)
            {
                return new Resultado<MovimientoMontoResultado>();
            }
            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));

            return _montoDisponibleRepositorio.ObtenerMovimientosMonto(consulta);
        }
    }
}
