using System;
using System.Collections.Generic;
using System.IO;
using Configuracion.Aplicacion.Comandos;
using Configuracion.Aplicacion.Comandos.Resultados;
using Configuracion.Aplicacion.Consultas;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.IRepositorio;
using Configuracion.Dominio.Modelo;
using Formulario.Aplicacion.Consultas.Resultados;
using ICSharpCode.SharpZipLib.Zip;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Archivos;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;
using Infraestructura.Core.Formateadores.MultipartData.Infrastructure;
using Infraestructura.Core.Reportes;
using Soporte.Aplicacion.Servicios;
using DarDeBajaComando = Configuracion.Aplicacion.Comandos.DarDeBajaComando;

namespace Configuracion.Aplicacion.Servicios
{
    public class AreaServicio
    {
        private readonly IAreaRepositorio _areaRepositorio;
        private readonly IMotivoBajaRepositorio _motivoBajaRepositorio;
        private readonly ISesionUsuario _sesionUsuario;
        private readonly DocumentacionBGEUtilServicio _documentacionBgeUtilServicio;


        public AreaServicio(IAreaRepositorio areaRepositorio, IMotivoBajaRepositorio motivoBajaRepositorio,
            ISesionUsuario sesionUsuario, DocumentacionBGEUtilServicio documentacionBgeUtilServicio)
        {
            _areaRepositorio = areaRepositorio;
            _motivoBajaRepositorio = motivoBajaRepositorio;
            _sesionUsuario = sesionUsuario;
            _documentacionBgeUtilServicio = documentacionBgeUtilServicio;
        }

        public NuevaAreaResultado RegistrarArea(RegistrarAreaComando registrarAreaComando)
        {
            if (_areaRepositorio.ExisteAreaConMismoNombre(registrarAreaComando.Nombre))
            {
                throw new ModeloNoValidoException("El area que intenta registrar ya se encuentra registrada");
            }
            Usuario usuario = _sesionUsuario.Usuario;
            var area = new Area(registrarAreaComando.Nombre,
                registrarAreaComando.Descripcion, usuario);

            return new NuevaAreaResultado
            {
                Id = _areaRepositorio.RegistrarArea(area)
            };
        }

        public Resultado<AreaResultado.Consulta> ConsultarAreas(ConsultarAreas consultaAreas)
        {
            if (consultaAreas == null)
            {
                consultaAreas = new ConsultarAreas {NumeroPagina = 0};
            }
            consultaAreas.TamañoPagina = int.Parse(ParametrosSingleton.Instance.GetValue("4"));

            return _areaRepositorio.Consultar(consultaAreas);
        }

        public ConsultarAreaPorIdResultado ConsultarAreaPorId(decimal id)
        {
            var area = _areaRepositorio.ConsultarPorId(id);
            return new ConsultarAreaPorIdResultado
            {
                Id = area.Id,
                Nombre = area.Nombre,
                Descripcion = area.Descripcion,
                IdUsuarioAlta = area.UsuarioAlta.Id,
                CuilUsuarioAlta = area.UsuarioAlta.Cuil,
                IdMotivoBaja = area.MotivoBaja?.Id,
                NombreMotivoBaja = area.MotivoBaja?.Descripcion,
                FechaUltimaModificacion = area.FechaUltimaModificacion,
                IdUsuarioUltimaModificacion = area.UsuarioUltimaModificacion.Id,
                CuilUsuarioUltimaModificacion = area.UsuarioUltimaModificacion.Cuil
            };
        }

        public void Modificar(int id, ModificarEtapaComando comando)
        {
            var area = _areaRepositorio.ConsultarPorId(id);

            if (area.Nombre != comando.Nombre && _areaRepositorio.ExisteAreaConMismoNombre(comando.Nombre))
            {
                throw new ModeloNoValidoException("Los datos que se intentan modificar ya existen para otra area");
            }
            Usuario usuario = _sesionUsuario.Usuario;
            area.Modificar(comando.Nombre, comando.Descripcion, usuario);
            _areaRepositorio.Modificar(area);
        }

        public void DarDeBaja(int id, DarDeBajaComando comando)
        {
            Usuario usuario = _sesionUsuario.Usuario;
            var area = _areaRepositorio.ConsultarPorId(id);
            area.DarDeBaja(_motivoBajaRepositorio.ConsultarPorId(new Id(comando.IdMotivoBaja)), usuario);
            _areaRepositorio.DarDeBaja(area);
        }

        public IList<AreaResultado.Consulta> ConsultarAreasCombo()
        {
            return _areaRepositorio.ConsultarAreasCombo();
        }

        public ReporteResultado ObtenerReporteDeudaGrupoConviviente(int cantidad)
        {
            var archivos = new List<HttpFile>();

            int cont = 1;

            var ds = new List<DatosProvidenciaResultado>();


            for (int i = 0; i < cantidad; i++)
            {
                ConcatenadorPDF concatenador = new ConcatenadorPDF();

                var reportData = GenerarReporteDeudaGrupoConviviente(ds);

                concatenador.AgregarReporte(reportData, $"DeudaGrupoConviviente__SOLICITANTE=");
                var arrayBytesConcatenado = concatenador.ObtenerReporteConcatenadoEnPDF();

                if (arrayBytesConcatenado == null)
                {
                    return new ReporteResultado(concatenador.Errores);
                }

                archivos.Add(new HttpFile()
                {
                    Buffer = arrayBytesConcatenado,
                    FileName = "ProvidenciaTest.pdf",
                    MediaType = "application/pdf"
                });
            }

            var zipEnBytes = GuardarArchivosZip(archivos);

            return new ReporteResultado(_documentacionBgeUtilServicio.GenerarArchivoReporteBGE(zipEnBytes, TipoArchivo.Zip, "Providencias"));
        }

        private byte[] GuardarArchivosZip(List<HttpFile> archivos)
        {

            using (var compressedStream = new MemoryStream())
            using (var zipStream = new ZipOutputStream(compressedStream))
            {
                foreach (var archivo in archivos)
                {
                    var fileEntry = new ZipEntry(archivo.FileName)
                    {
                        Size = archivo.Buffer.Length
                    };

                    zipStream.PutNextEntry(fileEntry);
                    zipStream.Write(archivo.Buffer, 0, archivo.Buffer.Length);
                }

                zipStream.Flush();
                zipStream.Close();

                Console.Write(Path.GetExtension(compressedStream.ToString()));

                return compressedStream.ToArray();
            }
        }

        private Reporte GenerarReporteDeudaGrupoConviviente(List<DatosProvidenciaResultado> ds)
        {
            try
            {
                return new ReporteBuilder("ReporteProvidencia.rdlc")
                    .AgregarDataSource("DSDatosProvidencia", ds)
                    .Generar();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}