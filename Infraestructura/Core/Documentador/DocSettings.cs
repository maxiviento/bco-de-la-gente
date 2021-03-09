using System.Configuration;

namespace Infraestructura.Core.Documentador
{
    public static class DocSettings
    {
        public static readonly string HabilitarApiDocKey = "Core:ApiDoc:Habilitado";
        public static readonly string UrlAccesoKey = "Core:ApiDoc:Url";
        public static readonly string ApiVersionKey = "Core:ApiDoc:Version";
        public static readonly string ApiTituloKey = "Core:ApiDoc:Titulo";

        public static bool Habilitado()
        {
            var habilitado = false;
            var habilitadoValue = ConfigurationManager.AppSettings[HabilitarApiDocKey];
            if (!string.IsNullOrEmpty(habilitadoValue))
            {
                habilitado = bool.Parse(habilitadoValue);
            }

            return habilitado;
        }

        public static string TituloApi()
        {
            var titulo = ConfigurationManager.AppSettings[ApiTituloKey];
            if (string.IsNullOrEmpty(titulo))
            {
                titulo = "Api Rest";
            }
            return titulo;
        }

        public static string Url()
        {
            var url = ConfigurationManager.AppSettings[UrlAccesoKey];
            if (string.IsNullOrEmpty(url))
            {
                url = "api/docs";
            }
            return url;
        }


        public static string Version()
        {
            var version = ConfigurationManager.AppSettings[ApiVersionKey];
            if (string.IsNullOrEmpty(version))
            {
                version = "v1";
            }
            return version;
        }
    }
}
