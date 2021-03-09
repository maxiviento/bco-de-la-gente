using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Infraestructura.Core.Comun.Archivos;
using Soporte.Aplicacion.Consultas;

namespace Soporte.Aplicacion.Servicios
{
    public class ManualesServicio
    {
        public IEnumerable<string> ObtenerManuales()
        {
            List<string> manuales = new List<string>();
            string directorioRaiz = HttpContext.Current.Server.MapPath("~/Manuales/");
            DirectoryInfo dirInfo = new DirectoryInfo(directorioRaiz);
            FileInfo[] files = dirInfo.GetFiles("*.pdf");

            foreach (var file in files)
            {
                manuales.Add(GetFileName(file));
            }

            return manuales;
        }

        private static string GetFileName(FileInfo file)
        {
            int extChars = file.Extension.Length;
            string nombre = file.Name;
            nombre = nombre.Remove(file.Name.Length - extChars, extChars);
            return nombre;
        }

        public ArchivoBase64 DescargarManual(ConsultaManual consulta)
        {
            string directorioRaiz = HttpContext.Current.Server.MapPath("~/Manuales/");
            DirectoryInfo dirInfo = new DirectoryInfo(directorioRaiz);
            FileInfo[] files = dirInfo.GetFiles("*.pdf");
            string fileName = consulta.FileName + ".pdf";

            var manual = files.FirstOrDefault(x => x.Name == fileName);
            if (manual == null) return null;

            byte[] bytes = File.ReadAllBytes(manual.FullName);
            string archivo = Convert.ToBase64String(bytes);

            return new ArchivoBase64(archivo, TipoArchivo.PDF, GetFileName(manual));
        }
    }
}
