using Infraestructura.Core.Comun.Dato;

namespace Infraestructura.Core.Comun.Presentacion
{
    public abstract class Consulta
    {
        [SkepParameter]
        public  int NumeroPagina { get; set; }
        [SkepParameter]
        public  int TamañoPagina { get; set; }

        public virtual int PaginaDesde => NumeroPagina * TamañoPagina;

        public virtual int PaginaHasta => NumeroPagina * TamañoPagina + TamañoPagina;
    }
}

