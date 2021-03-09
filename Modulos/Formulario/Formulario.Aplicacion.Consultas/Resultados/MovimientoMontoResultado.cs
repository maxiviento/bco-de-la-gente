using System;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class MovimientoMontoResultado : Entidad
    {
        public string Descripcion { get; set; }
        public string TipoUso { get; set; }
        public DateTime FechaUso { get; set; }
        public decimal? ValorMovimiento { get; set; }
        public decimal? Saldo { get; set; }
        public string Usuario { get; set; }
    }
}
