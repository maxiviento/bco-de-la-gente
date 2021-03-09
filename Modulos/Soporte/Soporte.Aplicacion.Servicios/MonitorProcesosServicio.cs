using Formulario.Aplicacion.Consultas.Consultas;
using Formulario.Aplicacion.Consultas.Resultados;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Archivos;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Dominio.IRepositorio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Soporte.Aplicacion.Servicios
{
    public class MonitorProcesosServicio
    {
        private readonly IMonitorProcesosRepositorio _monitorProcesosRepositorio;
        private readonly ISesionUsuario _sesionUsuario;
        private readonly string EXCELNEW = ".XLSX";
        private readonly string PDF = ".PDF";
        private readonly string EXCEL = ".XLS";

        public MonitorProcesosServicio(IMonitorProcesosRepositorio monitorProcesosRepositorio, ISesionUsuario sesionUsuario)
        {
            _monitorProcesosRepositorio = monitorProcesosRepositorio;
            _sesionUsuario = sesionUsuario;

        }

        public IList<ClaveValorResultado<string>> ObtenerEstados()
        {
            var estados = _monitorProcesosRepositorio.ObtenerEstadosProceso();
            var estadosResultados = estados.Select(estado => new ClaveValorResultado<string>(
                    estado.Id.ToString(),
                    estado.Descripcion
                ))
                .ToList();
            return estadosResultados;
        }

        public IList<ClaveValorResultado<string>> ObtenerTipos()
        {
            var tipos = _monitorProcesosRepositorio.ObtenerTiposProceso();
            var tiposResultados = tipos.Select(tipo => new ClaveValorResultado<string>(
                    tipo.Id.ToString(),
                    tipo.Nombre
                ))
                .ToList();
            return tiposResultados;
        }

        public Resultado<BandejaMonitorProcesosResultado> ObtenerProcesoPorFiltros(BandejaMonitorProcesoConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new BandejaMonitorProcesoConsulta { NumeroPagina = 0 };
            }
            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));
            return _monitorProcesosRepositorio.ObtenerProcesosPorFiltros(consulta);
        }
        public string ObtenerTotalizadorProceso(BandejaMonitorProcesoConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new BandejaMonitorProcesoConsulta { NumeroPagina = 0 };
            }
            consulta.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));
            return _monitorProcesosRepositorio.ObtenerTotalizadorProcesos(consulta);
        }

        public ArchivoBase64 DescargarReporte(string consulta)
        {
            string[] words = consulta.Split('\\');
            string fileName = words.Last();
            var directory = string.Join("\\", words.Take(words.Length - 1));
            DirectoryInfo dirInfo = new DirectoryInfo(directory);
            FileInfo[] files;
            TipoArchivo tipo;
            string extension = fileName.Substring(fileName.IndexOf('.'));

            if (EXCELNEW.Equals(extension))
            {
                files = dirInfo.GetFiles($"*{EXCELNEW}");
                tipo = TipoArchivo.ExcelNew;
            } 
            else if (EXCEL.Equals(extension))
            {
                files = dirInfo.GetFiles($"*{EXCEL}");
                tipo = TipoArchivo.Excel;
            }
            else
            {
                files = dirInfo.GetFiles($"*{PDF}");
                tipo = TipoArchivo.PDF;
            }

            

            var reporte = files != null && files.Length > 0 ? files.FirstOrDefault(x => x.Name.ToLower() == fileName.ToLower()) : null;
            if (reporte == null) return null;

            byte[] bytes = File.ReadAllBytes(reporte.FullName);
            string archivo = Convert.ToBase64String(bytes);

            return new ArchivoBase64(archivo, tipo, fileName.Split('.').First());
        }

        public string CancelarProceso(int nroGrupoProceso, string idUsuario)
        {
            var mensajeExito = "El proceso se cancelo con exito";
            var mensajeErrorCancelacion = "Se produzco un error en la cancelacion del proceso";
            var mensajeErrorEstado = "No se puede cancelar el proceso porque ya se comenzo a ejecutar";

            //Valido que todos los procesos del grupo esten en estado 1 asi se puede cancelar
            bool puedeCancelar = _monitorProcesosRepositorio.ValidarEstadoGrupoBatch(nroGrupoProceso, 1);

            if (puedeCancelar)
            {
                 return _monitorProcesosRepositorio.CancelarProceso(nroGrupoProceso, idUsuario)
                     ? mensajeExito
                     : mensajeErrorCancelacion;
            }
            return mensajeErrorEstado;
        }
    }
}
