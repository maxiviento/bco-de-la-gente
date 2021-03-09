using System.Configuration;
using System.IO;
using System.Text;

namespace ApiBatch.Base
{
    public static  class Variables
    {
        public readonly static string DIR_TEMPORAL = ConfigurationManager.AppSettings["DirTemp"];
        public readonly static StringBuilder DIR_ARCHIVOS = new StringBuilder("Archivos\\");

        public static string GetDirectoryPath(string processDirectory)
        {
            return Path.Combine(DIR_ARCHIVOS.ToString(), $"{processDirectory}\\");
        }

        public readonly static string DIR_PRUEBA = "PRUEBA";
    }
}