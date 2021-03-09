using System.Configuration;

namespace Infraestructura.Core.DI
{
    public static class DiSettings
    {
        public static readonly string Repositorios = "Core:Di:Repositorios";
        public static readonly string Servicios = "Core:Di:Servicios";

        public static string RepositoriosDll()
        {
            var repositorios = ConfigurationManager.AppSettings[Repositorios];
            if (string.IsNullOrEmpty(repositorios))
            {
                repositorios = "*.Repositorios.dll";
            }
            return repositorios;
        }

        public static string ServiciosDll()
        {
            var servicios = ConfigurationManager.AppSettings[Servicios];
            if (string.IsNullOrEmpty(servicios))
            {
                servicios = "*.Servicios.dll";
            }
            return servicios;
        }
    }
}
