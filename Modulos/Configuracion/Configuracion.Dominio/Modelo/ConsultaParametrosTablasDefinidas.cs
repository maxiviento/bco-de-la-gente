using Infraestructura.Core.Comun.Presentacion;

namespace Configuracion.Dominio.Modelo
{
    public class ConsultaParametrosTablasDefinidas: Consulta
    {
        public int IdTabla { get; set; }
        public string Nombre { get; set; }
        public decimal? IdParametro { get; set; }
        public bool IncluirDadosDeBaja { get; set; }
    }
}
