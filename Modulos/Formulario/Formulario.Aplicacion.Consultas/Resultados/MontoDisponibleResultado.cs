using System;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class MontoDisponibleResultado
    {
        public decimal Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaDepositoBancario { get; set; }
        public DateTime FechaInicioPago { get; set; }
        public DateTime FechaFinPago { get; set; }
        public string IdBanco { get; set; }
        public string IdSucursal { get; set; }
        public decimal? Monto { get;  set; }
        public decimal? NroMonto { get;  set; }
        public DateTime FechaAlta { get; set; }
        public decimal? IdMotivoBaja { get; set; }
        public string NombreMotivoBaja { get; set; }
        public DateTime FechaUltimaModificacion { get; set; }
        public string CuilUsuarioUltimaModificacion { get; set; }
        public decimal? Saldo { get; set; }
    }
}
