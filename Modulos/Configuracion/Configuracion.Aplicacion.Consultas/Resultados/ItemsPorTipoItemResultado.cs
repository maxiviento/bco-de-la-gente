using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;

namespace Configuracion.Aplicacion.Consultas.Resultados
{
    public class ItemsPorTipoItemResultado
    {
        public class Requisito
        {
            public Id Id { get; set; }
            public string Nombre { get; set; }
            public IEnumerable<ItemResultado.Requisito> TiposItem { get; set; }
        }

        public class Consulta
        {
            public Id IdTipoItem { get; set; }
            public string NombreTipoItem { get; set; }
            public Id IdItem { get; set; }
            public string NombreItem { get; set; }
        }
    }
}