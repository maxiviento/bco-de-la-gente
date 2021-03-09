using System;

namespace Pagos.Aplicacion.Comandos
{
    public class ActualizaModalidadComando
    {
        public decimal IdLote { get; set; }
        public int ElementoPago { get; set; }
        public int ModalidadPago { get; set; }
        public int ConvenioPago { get; set; }
        public DateTime FechaPago { get; set; }
        public DateTime FechaFinPago { get; set; }
        public int MesesGracia { get; set; }
        public bool GeneraPlanCuotas{ get; set; }
    }
}
