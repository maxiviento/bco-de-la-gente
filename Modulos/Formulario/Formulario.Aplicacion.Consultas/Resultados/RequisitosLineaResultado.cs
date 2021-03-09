using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class RequisitosLineaResultado
    {
        public string NombreLinea { get; set; }
        public Id IdTipoItem { get; set; }
        public Id IdItem { get; set; }
        public string NombreTipoItem { get; set; }
        public string NombreItem { get; set; }
        public IEnumerable<TiposItem> TiposItems { get; set; }

        public class TiposItem
        {
            public string Nombre { get; set; }
            public Id Id { get; set; }
        }
    }
}