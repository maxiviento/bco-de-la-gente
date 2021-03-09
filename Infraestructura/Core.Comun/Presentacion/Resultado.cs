using System.Collections.Generic;
using Newtonsoft.Json;

namespace Infraestructura.Core.Comun.Presentacion
{
    public sealed class Resultado<T>
    {
        public IEnumerable<T> Elementos { get; set; }
        public int NumeroPagina { get; set; }

        [JsonProperty(PropertyName = "elementosPorPagina")]
        public int TamañoDePagina { get; set; }
        public bool TieneMasResultados { get; set; }
    }
}
