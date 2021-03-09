using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace Infraestructura.Core.Comun.Archivos
{
    public class FileUtil
    {
        public static String GenerarRutaArchivoEnDirectorio(String fileNameWithFormat, string path)
        {
           // var tempReportsPath = HttpContext.Current.Server.MapPath(path);
           var tempReportsPath = Path.Combine(HttpRuntime.AppDomainAppPath, path);
            var fileName = fileNameWithFormat;
            GenerarDirectorio(tempReportsPath);
            var copias = 1;
            var ruta = new StringBuilder()
                .Append(tempReportsPath)
                .Append(fileName)
                .ToString();

            var archivosRuta = Directory.EnumerateFiles(tempReportsPath);

            while (archivosRuta.Contains(ruta))
            {
                fileName = $"({copias}){fileName}";
                ruta = new StringBuilder()
                    .Append(tempReportsPath)
                    .Append(fileName)
                    .ToString();
                copias++;
            }

            return ruta;
        }

        private static void GenerarDirectorio(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static String GenerarNombreArchivoConFecha(String fileName, String format)
        {
            return new StringBuilder()
                .Append(fileName)
                .Append("_")
                .Append(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .Append(".")
                .Append(format)
                .ToString();
        }

        public static String GenerarLinkDeDescarga(String fileName, String ruta)
        {
            string _baseUrl = HttpContext.Current.Request.Url.Scheme + "://" +
                              HttpContext.Current.Request.Url.Authority +
                              HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";

            return new StringBuilder()
                .Append(_baseUrl)
                .Append(ruta)
                .Append("?fileName=")
                .Append(fileName)
                .ToString();
        }

        public static bool ExisteDirectorio(string path)
        {
            var pathDirectorio = path.Substring(0, path.LastIndexOf("\\", StringComparison.Ordinal));

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            return Directory.Exists(pathDirectorio);
        }
    }
}