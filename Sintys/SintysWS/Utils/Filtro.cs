using Newtonsoft.Json;

namespace SintysWS.Utils
{
    public class Filtro
    {
        public class Operadores
        {
            public const string Igual = "=";
        }

        [JsonProperty("campo")]
        public string Campo { get; set; }

        [JsonProperty("operador")]
        public string Operador { get; set; }

        [JsonProperty("valor")]
        public string Valor { get; set; }

        public Filtro()
        {
            Operador = "=";
        }

        public Filtro(string campo, string valor, string operador = "=")
        {
            Campo = campo;
            Operador = operador;
            Valor = valor;
        }
    }
}