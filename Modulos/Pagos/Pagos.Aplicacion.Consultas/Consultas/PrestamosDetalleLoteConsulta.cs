using Infraestructura.Core.Comun.Presentacion;

namespace Pagos.Aplicacion.Consultas.Consultas
{
    public class PrestamosDetalleLoteConsulta : Consulta
    {
        public int? IdLote { get; set; }
        public bool? EsVer { get; set; }
    }
}
