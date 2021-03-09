using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;

namespace Soporte.Aplicacion.Consultas
{
    public class DocumentacionConsulta : Consulta
    {
        public Id IdItem { get; set; }
        public Id IdFormularioLinea { get; set; }
    }
}