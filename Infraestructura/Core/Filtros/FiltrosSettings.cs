using System.Configuration;

namespace Infraestructura.Core.Filtros
{
    public static class FiltrosSettings
    {
        public static readonly string ValidarModelo = "Core:Filtros:ValidarModelo:Habilitar";
        public static readonly string Autorizacion = "Core:Filtros:Autorizacion:Habilitar";

        public static bool HabilitarValidacionModelo()
        {
            var habilitar = true;
            var habilitarValue = ConfigurationManager.AppSettings[ValidarModelo];
            if (!string.IsNullOrEmpty(habilitarValue))
            {
                habilitar = bool.Parse(habilitarValue);
            }

            return habilitar;
        }

        public static bool HabilitarAutorizacion()
        {
            var habilitar = true;
            var habilitarValue = ConfigurationManager.AppSettings[Autorizacion];
            if (!string.IsNullOrEmpty(habilitarValue))
            {
                habilitar = bool.Parse(habilitarValue);
            }

            return habilitar;
        }
    }
}
