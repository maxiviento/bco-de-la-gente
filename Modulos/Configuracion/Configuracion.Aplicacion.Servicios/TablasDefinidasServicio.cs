using System;
using System.Collections.Generic;
using System.Linq;
using Configuracion.Dominio.IRepositorio;
using Configuracion.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Servicios;

namespace Configuracion.Aplicacion.Servicios
{
    public class TablasDefinidasServicio
    {
        private readonly ITablaDefinidasRepositorio _tablaDefinidasRepositorio;
        private readonly ISesionUsuario _sesionUsuario;

        public TablasDefinidasServicio(
            ITablaDefinidasRepositorio tablaDefinidasRepositorio
            , ISesionUsuario sesionUsuario)
        {
            _tablaDefinidasRepositorio = tablaDefinidasRepositorio;
            _sesionUsuario = sesionUsuario;
        }

        public IList<TablaDefinida> ObtenerTablasDefinidas()
        {
            return _tablaDefinidasRepositorio.ObtenerTablasDefinidas();
        }

        public Resultado<TablaDefinida> ObtenerTablasDefinidasPaginadas(ConsultaTablasDefinidas consulta)
        {
            if (consulta == null)
            {
                consulta = new ConsultaTablasDefinidas() { NumeroPagina = 0 };
            }
            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));

            return _tablaDefinidasRepositorio.ObtenerTablasDefinidasPaginadas(consulta);
        }

        public TablaDefinida ObtenerDatosTablaDefinida(int id)
        {
            var tabla = _tablaDefinidasRepositorio.ObtenerDatosTablaDefinida(id);
            var result = _tablaDefinidasRepositorio.ObtenerParametrosTablaDefinida(id).OrderBy(x => x.Nombre);
            var lsParametros = new List<ParametroTablaDefinida>();
            foreach (var parametro in result)
            {
                var par = new ParametroTablaDefinida
                {
                    Id = new Id(parametro.Id ?? -1),
                    Nombre = parametro.Nombre,
                    Descripcion = parametro.Descripcion,
                    FechaDesde = parametro.FechaDesde,
                    FechaHasta = parametro.FechaHasta,
                    NombreMotivoRechazo = parametro.NombreMotivoRechazo
                };
                lsParametros.Add(par);
            }
            tabla.Parametros = lsParametros;
            return tabla;
        }

        public Resultado<ParametroTablaDefinidaResult> ObtenerDatosTablaDefinida(ConsultaParametrosTablasDefinidas consulta)
        {
            if (consulta == null)
            {
                consulta = new ConsultaParametrosTablasDefinidas { NumeroPagina = 0 };
            }
            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));

            return _tablaDefinidasRepositorio.ObtenerDatosTablaDefinida(consulta);
        }

        public IList<ParametroTablaDefinida> ObtenerParametrosComboTablaDefinida(ConsultaParametrosTablasDefinidas consulta)
        {
            var result = _tablaDefinidasRepositorio.ObtenerParametrosComboTablaDefinida(consulta).OrderBy(x => x.Nombre);
            var lsParametros = new List<ParametroTablaDefinida>();
            foreach (var parametro in result)
            {
                var par = new ParametroTablaDefinida
                {
                    Id = new Id(parametro.Id ?? -1),
                    Nombre = parametro.Nombre,
                    Descripcion = parametro.Descripcion,
                    FechaDesde = parametro.FechaDesde,
                    FechaHasta = parametro.FechaHasta,
                    NombreMotivoRechazo = parametro.NombreMotivoRechazo
                };
                lsParametros.Add(par);
            }
            return lsParametros;
        }

        public decimal RegistrarRechazoParametro(RechazarParametroTablaDefinida comando)
        {
            if (comando.IdMotivoRechazo == 0)
                throw new ModeloNoValidoException("Debe seleccionar al menos un motivo de rechazo");

            var usuario = _sesionUsuario.Usuario;
            var parametro = new ParametroTablaDefinida(new Id(comando.IdParametro), comando.IdMotivoRechazo, comando.Observaciones, usuario, DateTime.Now);

            return _tablaDefinidasRepositorio.Rechazar(parametro);
        }

        public decimal RegistrarParametro(ParametroTablaDefinida parametro, int idTabla)
        {
            var usuario = _sesionUsuario.Usuario;
            var parametroNuevo = new ParametroTablaDefinida(parametro.Nombre, parametro.Descripcion, usuario, DateTime.Now);

            return _tablaDefinidasRepositorio.Registrar(parametroNuevo, idTabla);
        }

        public TablaDefinida ObtenerParametro(int id)
        {
            var result = _tablaDefinidasRepositorio.ObtenerParametroTablaDefinida(id);
            var tabla = new TablaDefinida {Id = new Id(result.IdTabla ?? -1)};

            var lsParametros = new List<ParametroTablaDefinida>();
            var parametro = new ParametroTablaDefinida
            {
                Id = new Id(result.Id ?? -1),
                Nombre = result.Nombre,
                Descripcion = result.Descripcion,
                FechaDesde = result.FechaDesde,
                FechaHasta = result.FechaHasta,
                IdMotivoRechazo = default(int),
                NombreMotivoRechazo = result.NombreMotivoRechazo
            };
            lsParametros.Add(parametro);
            tabla.Parametros = lsParametros;
            return tabla;
        }
    }
}
