using System.Collections.Generic;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class ItemsMercadoComerResultado
    {
        public List<int> Items { get; set; }
        public string Descripcion { get; set; }
        public decimal? TipoItem { get; set; }
    }
}
