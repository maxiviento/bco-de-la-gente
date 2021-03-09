using System.Collections.Generic;

namespace Formulario.Aplicacion.Comandos
{
    public class PrestamoComando
    {
        public long Id { get; set; }
        public long TotalFolios { get; set; }
        public decimal IdUsuario { get; set; }
        public string Observaciones { get; set; }
        public decimal IdEstado { get; set; }
        public IList<RequisitoComando> Requisitos { get; set; }
        public long? IdLinea { get; set; }
        public long IdFormularioLinea { get; set; }
    }
}
