using System.Configuration;
using CiDi.SDK.Common;
using Infraestructura.Core.CiDi.Configuration.Sections;

namespace Infraestructura.Core.CiDi.Configuration
{
    public static class CidiConfigurationManager
    {
        public static void RegisterApplication()
        {            
            var cidiEnvironment = GetCidiEnvironment();
            var appCidi = new App
            {
                ID = cidiEnvironment.IdApplication,
                EntornoEjecucion = cidiEnvironment.EnvProd ? Entorno.Produccion : Entorno.Desarrollo,
                Password = cidiEnvironment.ClientSecret,
                Key = cidiEnvironment.ClientKey
            };
            appCidi.Registrar();
        }

        public static CidiEnvironment GetCidiEnvironment()
        {
            var config = GetCidiSection();
            if (config?.Environments == null || config.Environments.Count < 1)
                return null;
            return config.Environments[GetCidiSection().Env];
        }

        public static string GetCidiEndpoint(UrlCidiEnum url)
        {
            var config = GetCidiSection();
            if (config?.Environments == null || config.Environments.Count < 1)
                return null;
            var env = config.Environments[GetCidiSection().Env];
            if (env == null || env.Endpoints.Count < 1)
                return null;
            return env.Endpoints[url] == null ? null : env.Endpoints[url].Value;
        }

        public static CidiSection GetCidiSection()
        {
            return (CidiSection) ConfigurationManager.GetSection("cidi");
        }

    }
}
