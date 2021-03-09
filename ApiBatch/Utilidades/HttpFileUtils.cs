using Infraestructura.Core.Comun.Archivos;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ApiBatch.Utilidades
{
    public class HttpFileUtils
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        protected HttpFileUtils() { }
        public static void GuardarArchivoTmp(HttpFile archivo, string path)
        {
            try
            {

                var directorio = $"{path}\\";
                var name = archivo.FileName + $".{MimeTypeUtils.Obtener_Extenxion(archivo.MediaType)}";

                Logger.Info("Directorio: " + directorio + "Name: " + name);
                var rutaArchivo = FileUtil.GenerarRutaArchivoEnDirectorio(name, directorio);



                byte[] fileBuffer = archivo.BufferArray != null && archivo.BufferArray.Length != 0 ? archivo.BufferArray : Convert.FromBase64String(archivo.Buffer);

                //if (archivo.MediaType.Equals(MimeTypeUtils.Obtener_MimeType(MimeTypeUtils.FileExtension.Txt)))
                //{
                //    fileBuffer = Encoding.ASCII.GetBytes(archivo.Buffer);
                //    rutaArchivo += $".{MimeTypeUtils.FileExtension.Txt}";
                //}
                //else
                //{
                //    fileBuffer = Convert.FromBase64String(archivo.Buffer);
                //    rutaArchivo += $".{MimeTypeUtils.FileExtension.Xlsx}";
                //}

                var fs = new FileStream(rutaArchivo, FileMode.Create);
                fs.Write(fileBuffer, 0, fileBuffer.Length);
                fs.Close();
                archivo.Path = rutaArchivo;
            }
            catch(Exception e) {
                Logger.Error(e.Message, "Error en HTTP files");
            }
        }

        public static string GuardarArchivo(HttpFile archivo, string path)
        {
            var directorio = $"{path}";
            var name = archivo.FileName + $".{MimeTypeUtils.Obtener_Extenxion(archivo.MediaType)}";
            var rutaArchivo = FileUtil.GenerarRutaArchivoEnDirectorio(name, directorio);
            byte[] fileBuffer = archivo.BufferArray != null && archivo.BufferArray.Length != 0 ? archivo.BufferArray : Convert.FromBase64String(archivo.Buffer);


            var fs = new FileStream(rutaArchivo, FileMode.Create);
            fs.Write(fileBuffer, 0, fileBuffer.Length);
            fs.Close();
            archivo.Path = rutaArchivo;

            return archivo.Path;
        }
    }
}