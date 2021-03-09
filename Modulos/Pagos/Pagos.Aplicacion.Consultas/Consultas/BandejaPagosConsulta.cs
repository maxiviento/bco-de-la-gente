using System;
using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;

namespace Pagos.Aplicacion.Consultas.Consultas
{
    public class BandejaPagosConsulta : Consulta, IConsultaConDatosPersona
    {
        public DateTime FechaInicioTramite { get; set; }
        public DateTime FechaFinTramite { get; set; }
        public int? IdLocalidad { get; set; }
        public IList<int> IdsLineas { get; set; }
        public int? IdOrigen { get; set; }
        public int? IdLugarOrigen { get; set; }
        public int? NroPrestamoChecklist { get; set; }
        public long? NroFormulario { get; set; }

        public long? TipoPersona { get; set; }
        public string Cuil { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }

        public IList<int> DepartamentoIds { get; set; } = new List<int>();
        public IList<int> LocalidadIds { get; set; } = new List<int>();
        public bool OrderByDes { get; set; }
        public int ColumnaOrderBy { get; set; }

        /// <summary>
        /// En caso de estar consultando por el DNI o CUIL debería no tenerse en cuenta las fechas de la consulta 
        /// </summary>
        public void RevisarInclusionDeFechas()
        {
            if (!string.IsNullOrEmpty(Dni?.Trim()) || !string.IsNullOrEmpty(Cuil?.Trim()))
            {
                FechaInicioTramite = default(DateTime);
                FechaFinTramite = default(DateTime);
            }
        }
    }
}