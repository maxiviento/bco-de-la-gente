using System;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class BandejaMontoDisponibleResultado
    { 
        public decimal? Id { get; set; }
        public decimal? NroMonto { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaDepositoBancario { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal? Saldo { get; set; }
        public decimal? IdMotivoBaja { get; set; }
        public DateTime FechaUltimaModificacion { get; set; }
    }
}
