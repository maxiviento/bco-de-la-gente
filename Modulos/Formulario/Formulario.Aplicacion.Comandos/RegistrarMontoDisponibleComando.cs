using System;

namespace Formulario.Aplicacion.Comandos
{
    public class RegistrarMontoDisponibleComando
    {
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaDepositoBancario { get; set; }
        public DateTime FechaInicioPago { get; set; }
        public DateTime FechaFinPago { get; set; }
        public string IdBanco { get; set; }
        public string IdSucursal { get; set; }
    }
}
