using System;
using System.Collections.Generic;
using System.Linq;
using Configuracion.Aplicacion.Comandos;
using Configuracion.Aplicacion.Comandos.Resultados;
using Configuracion.Aplicacion.Consultas;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.IRepositorio;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Servicios;

namespace Formulario.Aplicacion.Servicios
{
    public class MotivoRechazoServicio
    {
        private readonly IMotivoRechazoRepositorio _repositorio;
        private readonly IMotivoBajaRepositorio _motivoBajaRepositorio;
        private readonly ISesionUsuario _sesionUsuario;

        public MotivoRechazoServicio(IMotivoRechazoRepositorio repositorio, IMotivoBajaRepositorio motivoBajaRepositorio, ISesionUsuario sesionUsuario)
        {
            _repositorio = repositorio;
            _motivoBajaRepositorio = motivoBajaRepositorio;
            _sesionUsuario = sesionUsuario;
        }

        public IList<MotivoRechazo> ConsultarMotivosRechazo()
        {
            var listaMotivos = _repositorio.ConsultarMotivosRechazo(Ambito.FORMULARIO);

            return listaMotivos;
        }
        public IList<MotivoRechazo> ConsultarMotivosRechazoPorAmbito(decimal idAmbito)
        {
            return _repositorio.ConsultarMotivosRechazoPorAmbito(idAmbito);
        }

        public IList<MotivoRechazo> ConsultarMotivosRechazoPrestamo()
        {
            var listaMotivos = _repositorio.ConsultarMotivosRechazo(Ambito.PRESTAMO);

            return listaMotivos;
        }

        public IList<MotivoRechazo> ConsultarMotivosRechazoTablaDefinida()
        {
            var listaMotivos = _repositorio.ConsultarMotivosRechazo(Ambito.TABLA_DEFINIDA);

            return listaMotivos;
        }

        public Resultado<ConsultaMotivoRechazoResultado.Grilla> ConsultaMotivosDestino(ConsultaMotivoRechazo consulta)
        {
            if (consulta == null)
            {
                consulta = new ConsultaMotivoRechazo() { NumeroPagina = 0 };
            }
            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));

            var resultado = _repositorio.Consultar(consulta);
            MarcarPredefinidos(resultado.Elementos);
            return resultado;
        }

        private void MarcarPredefinidos(IEnumerable<ConsultaMotivoRechazoResultado.Grilla> motivos)
        {
            int[] predefinidos = { 24, 25, 26, 27, 29, 30, 32, 34, 62 };
            foreach (var motivoRechazo in motivos)
            {
                motivoRechazo.EsPredefinido = predefinidos.Any(id => motivoRechazo.Id.Valor == id);
            }
        }

        public NuevoMotivoRechazoResultado RegistrarMotivoRechazo(MotivoRechazoComando comando)
        {
            Usuario usuario = _sesionUsuario.Usuario;
            var motivo = new MotivoRechazo(comando.Nombre, comando.Descripcion, comando.Abreviatura, comando.EsAutomatico, comando.Ambito, usuario, comando.Observaciones, comando.Codigo);
            return new NuevoMotivoRechazoResultado()
            {
                Id = _repositorio.RegistrarMotivoRechazo(motivo)
            };
        }

        public MotivoRechazo ConsultarMotivoDestinoPorId(decimal idParametro, decimal idAmbito)
        {
            var motivo = new MotivoRechazo(_repositorio.ConsultarPorIdGeneral(new Id(idParametro), new Id(idAmbito)));
            if (motivo.Ambito.Id.Valor == 1)
            {
                motivo.Ambito.Id = new Id(13);
            }
            return motivo;
        }

        public bool Modificar(ModificacionMotivoRechazoComando comando)
        {
            Usuario usuario = _sesionUsuario.Usuario;
            return _repositorio.Modificar(comando, usuario);
        }

        public void DarDeBaja(DarBajaMotivoRechazoComando comando)
        {
            Usuario usuario = _sesionUsuario.Usuario;
            var detalle = _repositorio.ConsultarPorIdGeneral(new Id(comando.Id), new Id(comando.idAmbito));
            var motivo = new MotivoRechazo(detalle);
            motivo.DarDeBaja(_motivoBajaRepositorio.ConsultarPorId(new Id(comando.IdMotivoBaja)), usuario);
            _repositorio.DarDeBaja(motivo);
        }

        public IList<Ambito> ConsultarAmbitosCombo()
        {
            return _repositorio.ConsultarAmbitosCombo();
        }

        public IList<MotivoRechazoAbreviaturaResultado> ConsultarAbreviaturas()
        {
            var resultadoDb = _repositorio.ObtenerAbreviaturas();
            return resultadoDb.Select(elemento => new MotivoRechazoAbreviaturaResultado()
                {
                    Abreviatura = elemento.Abreviatura,
                    DadoDeBaja = elemento.IdMotivoBaja != null,
                    IdAmbito = elemento.Ambito.Id.Valor,
                    Automatico = elemento.EsAutomatico
                })
                .ToList();
        }

        public bool ExisteMotivoRechazoConMismoNombre(int idAmbito, string nombre)
        {
            return _repositorio.ExisteMotivoRechazoConMismoNombre(idAmbito, nombre);
        }

        public bool ExisteMotivoRechazoConMismaAbreviatura(int idAmbito, string abreviatura)
        {
            return _repositorio.ExisteMotivoRechazoConMismaAbreviatura(idAmbito, abreviatura);
        }

        public bool ExisteMotivoRechazoConMismoCodigo(int idAmbito, string codigo)
        {
            return _repositorio.ExisteMotivoRechazoConMismoCodigo(idAmbito, codigo);
        }

    }
}