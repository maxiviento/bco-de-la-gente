using AppComunicacion;
using AppComunicacion.ApiModels;
using Infraestructura.Core.CiDi.Configuration;
using Infraestructura.Core.Comun.Excepciones;

namespace Infraestructura.Core.CiDi.Util
{
    public static class AppComunicacionUtil
    {
        public static ServicioComunicacion GetServicio()
        {
            var cidiEnvironment = CidiConfigurationManager.GetCidiEnvironment();
            var config = new Configuracion
            {
                AppId = cidiEnvironment.IdApplication.ToString(),
                AppKey = cidiEnvironment.ClientKey,
                AppPass = cidiEnvironment.ClientSecret,
                Entorno = cidiEnvironment.EnvProd ? Entornos.PRODUCCION : Entornos.DESARROLLO
            };
            return new ServicioComunicacion(config);
        }

        public static string GenerarEntidadConsulta(string idSexo, string nroDocumento)
        {
            if (string.IsNullOrEmpty(idSexo) || string.IsNullOrEmpty(nroDocumento))
                return string.Empty;
            return idSexo + nroDocumento.PadLeft(8, '0');
        }

        public static void ValidarCadena(string sexoYDniConcatenado)
        {
            if (sexoYDniConcatenado.Length != 10)
                throw new ErrorTecnicoException("Para consular la api de grupo familiar se espera un cadena de 10 caracteres (dos para sexo y ocho para dni).");
            if (!sexoYDniConcatenado.Substring(0, 2).Equals("01")
                && !sexoYDniConcatenado.Substring(0, 2).Equals("02"))
                throw new ErrorTecnicoException("Para consular la api de grupo familiar el sexo enviado debe ser 01 o 02.");
        }

        private static void ValidarPersonaFiltro(PersonaFiltro personaFiltro)
        {
            if (!personaFiltro.IsValid())
                throw new ErrorTecnicoException("Para consular la api de grupo familiar el sexo enviado debe ser 01 o 02 y el pais enviado debe tener 3 caracteres.");
        }

        public static PersonaFiltro GenerarPersonaFiltro(string sexo, string dni, string pais, int? idNumero)
        {
            var personaFiltro = new PersonaFiltro() { Sexo = sexo, PaisTD = pais, NroDocumento = dni, Id_numero = idNumero};
            ValidarPersonaFiltro(personaFiltro);
            return personaFiltro;
        }
    }
}
