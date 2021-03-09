using System;
using System.Collections.Generic;
using System.IO;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using GrupoUnico.Aplicacion.Servicios;
using ICSharpCode.SharpZipLib.Zip;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Archivos;
using Infraestructura.Core.Formateadores.MultipartData.Infrastructure;
using Infraestructura.Core.Reportes;
using Pagos.Aplicacion.Comandos;
using Soporte.Dominio.IRepositorio;

namespace Soporte.Aplicacion.Servicios
{
    public class ProvidenciaServicio
    {
        private readonly IProvidenciaRepositorio _providenciaRepositorio;
        private readonly ILineaPrestamoRepositorio _lineaPrestamoRepositorio;
        private readonly ISesionUsuario _sesionUsuario;
        private readonly GrupoUnicoServicio _grupoUnicoServicio;
        private readonly DocumentacionBGEUtilServicio _documentacionBgeUtilServicio;

        public ProvidenciaServicio(IProvidenciaRepositorio providenciaRepositorio,
            GrupoUnicoServicio grupoUnicoServicio, ILineaPrestamoRepositorio lineaPrestamoRepositorio, ISesionUsuario sesionUsuario,
            DocumentacionBGEUtilServicio documentacionBgeUtilServicio)
        {
            _providenciaRepositorio = providenciaRepositorio;
            _documentacionBgeUtilServicio = documentacionBgeUtilServicio;
            _grupoUnicoServicio = grupoUnicoServicio;
            _sesionUsuario = sesionUsuario;
            _lineaPrestamoRepositorio = lineaPrestamoRepositorio;
        }
        
        public ReporteResultado ObtenerReporteProvidencia(ProvidenciaComando comando)
        {
            var datosBasicos = _providenciaRepositorio.ObtenerSolicitante(comando.IdFormulario);

            var datosReporte = new DatosFormularioResultado()
            {
                NroSticker = datosBasicos.NroSticker,
                DetalleLinea = _lineaPrestamoRepositorio.DetalleLineaParaFormulario(datosBasicos.Id),
                Id = datosBasicos.IdNumero,
                NroFormulario = datosBasicos.NroFormulario
            };

            var datosProvidencia = _providenciaRepositorio.ObtenerDatosParaProvidencia(comando.IdFormulario);

            datosProvidencia.ImporteLetras = ConvertidorNumeroLetras.enletras(datosProvidencia.Importe).ToLower();
                       
            var fechaConsulta = comando.Fecha;

            var ds = new List<DatosProvidenciaResultado>();

            var datosSolicitante = ObtenerDatosPersonalesCompleto(datosBasicos);
            
            var reportData = GenerarReporteProvidencia(ds, datosReporte, datosSolicitante, datosProvidencia, fechaConsulta);

            _providenciaRepositorio.RegistrarProvidencia(comando.IdFormulario);

            var nombreReporte = datosReporte.NroFormulario + "-" + datosProvidencia.Departamento + "-" +
                                datosProvidencia.Localidad + "-" + datosSolicitante.Apellido + " " +
                                datosSolicitante.Nombre;

            return new ReporteResultado(
                _documentacionBgeUtilServicio.GenerarArchivoReporteBGE(reportData.Content, TipoArchivo.PDF, nombreReporte, -1 /*Para no poner identificacion*/));
        }

        private Reporte GenerarReporteProvidencia(List<DatosProvidenciaResultado> ds, DatosFormularioResultado datosFormulario, 
            DatosPersonalesResultado datos, DatosProvidenciaResultado datosProvidencia, DateTime fechaConsulta)
        {
            try
            {
                return new ReporteBuilder("ReporteProvidencia.rdlc")
                    .AgregarDataSource("DSDatosProvidencia", ds)
                    .AgregarParametro("Sticker", datosFormulario.NroSticker)
                    .AgregarParametro("NombreCompleto", $"{datos.Apellido} {datos.Nombre}")
                    .AgregarParametro("Cuil", datos.Cuil)
                    .AgregarParametro("NombreLinea", datosFormulario.DetalleLinea.Nombre)
                    .AgregarParametro("DetalleLinea", datosFormulario.DetalleLinea.Descripcion)
                    .AgregarParametro("Fecha", fechaConsulta.ToString("dd MMMM yyyy"))
                    .AgregarParametro("ImporteLetras", datosProvidencia.ImporteLetras)
                    .AgregarParametro("Importe", datosProvidencia.Importe)
                    .AgregarParametro("Destino", datosProvidencia.Destino)
                    .AgregarParametro("NroFormulario", datosFormulario.NroFormulario)
                    .Generar();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public ReporteResultado ObtenerReporteProvidenciaMasivo(ProvidenciaComando comando)
        {
            var archivos = new List<HttpFile>();

            var datosFormulariosLote = _providenciaRepositorio.ObtenerDatosParaProvidenciaMasiva(comando.IdLote);

            foreach (var datosFormulario in datosFormulariosLote)
            {
                ConcatenadorPDF concatenador = new ConcatenadorPDF();

                if (!comando.FechaAprovacionMasiva)
                {
                    datosFormulario.FechaAprobacion = comando.Fecha;
                }

                datosFormulario.ImporteLetras = ConvertidorNumeroLetras.enletras(datosFormulario.Importe).ToLower();

                Reporte reporte = new ReporteBuilder("ReporteProvidencia.rdlc")
                    .AgregarDataSource("DSDatosProvidencia", new List<DatosProvidenciaResultado>())
                    .AgregarParametro("Sticker", datosFormulario.NroSticker)
                    .AgregarParametro("NombreCompleto", $"{datosFormulario.Apellido} {datosFormulario.Nombre}")
                    .AgregarParametro("Cuil", datosFormulario.Cuil)
                    .AgregarParametro("NombreLinea", datosFormulario.NombreLinea)
                    .AgregarParametro("DetalleLinea", datosFormulario.DescripcionLinea)
                    .AgregarParametro("Fecha", datosFormulario.FechaAprobacion.ToString("dd MMMM yyyy"))
                    .AgregarParametro("ImporteLetras", datosFormulario.ImporteLetras)
                    .AgregarParametro("Importe", datosFormulario.Importe)
                    .AgregarParametro("Destino", datosFormulario.Destino)
                    .AgregarParametro("NroFormulario", datosFormulario.NroFormulario)
                    .Generar();

                concatenador.AgregarReporte(reporte, $"Providencia");
                var arrayBytesConcatenado = concatenador.ObtenerReporteConcatenadoEnPDF();

                if (arrayBytesConcatenado == null)
                {
                    return new ReporteResultado(concatenador.Errores);
                }

                var nombreReporte = datosFormulario.NroFormulario + "-" + datosFormulario.Departamento + "-" +
                                    datosFormulario.Localidad + "-" + datosFormulario.Apellido + " " +
                                    datosFormulario.Nombre;
                
                archivos.Add(new HttpFile()
                {
                    Buffer = arrayBytesConcatenado,
                    FileName = nombreReporte + ".pdf",
                    MediaType = "application/pdf"
                });
            }

            var zipEnBytes = GuardarArchivosZip(archivos);

            _providenciaRepositorio.RegistrarProvidenciaMasiva(comando.IdLote, _sesionUsuario.Usuario.Id.Valor);

            return new ReporteResultado(_documentacionBgeUtilServicio.GenerarArchivoReporteBGE(zipEnBytes, TipoArchivo.Zip, "ProvidenciasMasiva" , -1));
        }
      
        private DatosPersonalesResultado ObtenerDatosPersonalesCompleto(DatosBasicosFormularioResultado persona)
        {
            var resultado =
                _grupoUnicoServicio.GetApiConsultaDatosPersonales(persona.SexoId, persona.NroDocumento,
                    persona.CodigoPais, persona.IdNumero);

            return resultado;
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
    }
}