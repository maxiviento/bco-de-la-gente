using Infraestructura.Core.Comun.Excepciones;

namespace Core.CiDi.Documentos.Utils
{
    public class CredencialesAutorizacion
    {
        public CredencialesAutorizacion(string idAppOrigen, string password, string key)
        {
            int idOrigen;
            var conversionValida = int.TryParse(idAppOrigen, out idOrigen);

            if (!conversionValida || string.IsNullOrEmpty(password) ||
                    string.IsNullOrEmpty(key))
                throw new ErrorTecnicoException("Falta alguna de las credenciales para autorizar el consumo de la api de documentos de CiDi.");

            IdAppOrigen = idOrigen;
            Password = password;
            Key = key;
        }

        public int IdAppOrigen { get; }
        public string Password { get; }
        public string Key { get; }

    }
}
