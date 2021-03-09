using System;
using System.Collections;
using System.Collections.Generic;
using Infraestructura.Core.Comun.Presentacion;

namespace Pagos.Aplicacion.Consultas.Consultas
{
    public class BandejaLotesConsulta : Consulta
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int? NroLoteDesde { get; set; }
        public int? NroLoteHasta { get; set; }
        public int? IdMontoDisponible { get; set; }
        public int? IdLinea { get; set; }
        public int? NroPrestamo { get; set; }
        public long? TipoPersona { get; set; }
        public string Cuil { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        
        // En caso de estar consultando por el DNI o CUIL debería no tenerse en cuenta las fechas de la consulta 
        public void RevisarInclusionDeFechas()
        {
            if (string.IsNullOrEmpty(Dni?.Trim()) && string.IsNullOrEmpty(Cuil?.Trim())) return;
            FechaInicio = default(DateTime);
            FechaFin = default(DateTime);
        }
    }
}
