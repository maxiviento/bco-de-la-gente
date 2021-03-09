using System;
using System.Collections.Generic;

namespace Pagos.Aplicacion.Consultas.Consultas
{
    public class InformePagosConsulta
    {
        public int[] IdsInformes { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public bool CreditosHabilitado { get; set; }
        public bool PagadosHabilitado { get; set; }
        public bool HistoricoHabilitado { get; set; }
        public bool ProyectadosPagosHabilitado { get; set; }
        public bool ProyectadosDtosHabilitado { get; set; }
        public bool ExportacionPrestamosHabilitado { get; set; }
        public bool ExportacionRecuperoHabilitado { get; set; }

    }
}
