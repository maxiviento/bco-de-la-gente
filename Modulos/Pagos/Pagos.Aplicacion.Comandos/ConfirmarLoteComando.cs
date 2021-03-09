using System;
using System.Collections.Generic;
using Pagos.Aplicacion.Consultas.Consultas;

namespace Pagos.Aplicacion.Comandos
{
    public class ConfirmarLoteComando
    {
        public int IdMontoDisponible { get; set; }
        public string NombreLote { get; set; }
        public BandejaPagosConsulta Consulta { get; set; }
        public IList<int> IdPrestamosSeleccionados { get; set; }
        public IList<int> IdAgrupamientosSeleccionados { get; set; }
        public decimal Monto { get; set; }
        public decimal Comision { get; set; }
        public decimal Iva { get; set; }
        public DateTime FechaPago { get; set; }
        public DateTime FechaFinPago { get; set; }
        public int Modalidad { get; set; }
        public int Elemento { get; set; }
        public int Convenio { get; set; }
        public int IdLoteSuaf { get; set; }
        public decimal MesesGracia { get; set; }
        public decimal IdTipoLote { get; set; }
    }
}
