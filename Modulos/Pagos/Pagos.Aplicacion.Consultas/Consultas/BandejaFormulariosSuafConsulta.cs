using System;
using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;

namespace Pagos.Aplicacion.Consultas.Consultas
{
    public class BandejaFormulariosSuafConsulta : Consulta, IConsultaConDatosPersona
    {
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public int? IdDepartamento { get; set; }
        public int? IdLocalidad { get; set; }
        public int? IdLinea { get; set; }
        public int? IdOrigen { get; set; }
        public int? NroFormulario { get; set; }
        public int? NroPrestamoChecklist { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string NroDocumento { get; set; }
        public int? Devengado { get; set; }
        public int? IdLoteSuaf { get; set; }
        public long? TipoPersona { get; set; }
        public string Cuil { get; set; }
        public bool? EsCargaDevengado { get; set; }
        public IList<int> DepartamentoIds { get; set; } = new List<int>();
        public IList<int> LocalidadIds { get; set; } = new List<int>();
        public string Dni
        {
            get { return NroDocumento; }
            set { NroDocumento = value; }
        }

        public void RevisarInclusionDeFechas()
        {
            if (!string.IsNullOrEmpty(Dni?.Trim()) || !string.IsNullOrEmpty(Cuil?.Trim()))
            {
                FechaDesde = default(DateTime);
                FechaHasta = default(DateTime);
            }
        }
    }
}
