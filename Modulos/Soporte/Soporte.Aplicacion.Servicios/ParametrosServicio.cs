using System;
using System.Collections.Generic;
using System.Linq;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Comandos;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Aplicacion.Consultas.TablasSatelite;
using Soporte.Dominio.IRepositorio;
using Soporte.Dominio.Modelo;

namespace Soporte.Aplicacion.Servicios
{
    public class ParametrosServicio
    {
        private readonly IParametrosRepositorio _parametrosRepositorio;
        private readonly ISesionUsuario _sesionUsuario;

        public ParametrosServicio(IParametrosRepositorio parametrosRepositorio, ISesionUsuario sesionUsuario)
        {
            _parametrosRepositorio = parametrosRepositorio;
            _sesionUsuario = sesionUsuario;
        }
        public VigenciaParametroResultado ObtenerValorVigenciaParametroPorFecha(Id idParametro, DateTime? fecha)
        {
            var vigenciaParametro = _parametrosRepositorio.ObtenerValorVigenciaParametroPorFecha(idParametro, fecha);
            var vigenciaParametroResultado = new VigenciaParametroResultado { Valor = vigenciaParametro.Valor };
            return vigenciaParametroResultado;
        }

        public Resultado<ConsultarParametrosResultado> ConsultarParametrosPorFiltros(ConsultaParametro consulta)
        {
            if (consulta == null)
            {
                consulta = new ConsultaParametro();
            }
            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));

            var resultado = _parametrosRepositorio.ConsultarPorFiltros(consulta);
            //foreach (var elemento in resultado.Elementos)
            //{
            //    if (elemento.FechaDesde == new DateTime(1, 1, 1))
            //    {
            //        elemento.FechaDesde = null;
            //    }
            //}
            resultado.Elementos = resultado.Elementos.OrderBy(x => x.Nombre).ThenBy(x=> x.FechaDesde);
            return resultado;
        }

        public VigenciaParametroIdResultado RegistrarVigenciaParametro(ActualizarParametroComando comando)
        {
            var listaDeErrores = new List<string>();

            if (string.IsNullOrEmpty(comando.Valor)) { listaDeErrores.Add("Debe ingresar un valor del parámetro");}
            if (comando.Id == null) { listaDeErrores.Add("Debe seleccionar un parámetro");}

            //Comprobacion de existencia de errores
            if (listaDeErrores.Count > 0) { throw new ModeloNoValidoException(listaDeErrores); }

            VigenciaParametro vigenciaParametro = new VigenciaParametro(comando.FechaDesde, comando.Valor, comando.Id, comando.IdVigencia);

            vigenciaParametro.UsuarioModificacion = _sesionUsuario.Usuario;

            vigenciaParametro = _parametrosRepositorio.RegistrarVigenciaParametro(vigenciaParametro);
            
            var resultado = new VigenciaParametroIdResultado(vigenciaParametro.IdVigenciaParametro == null ? -1 : (decimal)vigenciaParametro.IdVigenciaParametro);
            return resultado;
        }

        public VigenciaParametroIdResultado ActualizarVigenciaExistente(ActualizarParametroComando comando)
        {
            var listaDeErrores = new List<string>();

            if (string.IsNullOrEmpty(comando.Valor)) { listaDeErrores.Add("Debe ingresar un valor del parámetro"); }
            if (comando.Id == null) { listaDeErrores.Add("Debe seleccionar un parámetro"); }

            //Comprobacion de existencia de errores
            if (listaDeErrores.Count > 0) { throw new ModeloNoValidoException(listaDeErrores); }

            VigenciaParametro vigenciaParametro = new VigenciaParametro(comando.FechaDesde, comando.Valor, comando.Id, comando.IdVigencia);

            vigenciaParametro.UsuarioModificacion = _sesionUsuario.Usuario;

            vigenciaParametro = _parametrosRepositorio.ActualizarVigenciaExistente(vigenciaParametro);

            var resultado = new VigenciaParametroIdResultado(vigenciaParametro.IdVigenciaParametro == null ? -1 : (decimal)vigenciaParametro.IdVigenciaParametro);
            return resultado;
        }

        public ConsultarParametrosResultado ExisteVigenciaEnFecha(Id idParametro, DateTime? fechaDesde)
        {
            return _parametrosRepositorio.ExisteVigenciaEnFecha(idParametro, fechaDesde);
        }

        #region Tablas Satelite

        public IEnumerable<ConsultaParametrosSatelite> ObtenerParametros()
        {
            var listParametros = _parametrosRepositorio.ObtenerParametros();
            return listParametros.Select(parametro => new ConsultaParametrosSatelite()
            {
                Id = (long) parametro.Id.Valor,
                Nombre = parametro.Descripcion
            });
        }

        #endregion


    }
}

