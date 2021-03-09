using Infraestructura.Core.Comun.Presentacion;

namespace Soporte.Aplicacion.Consultas
{
    public class ConsultaParametro : Consulta
    {
        public ConsultaParametro()
        {
            Id = -1;
            IncluirNoVigentes = false;
        }

        public long Id { get; set; }
        public bool IncluirNoVigentes { get; set; }
    }
}