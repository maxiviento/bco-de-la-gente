using System;
using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;

namespace Pagos.Aplicacion.Consultas.Consultas
{
    public class BandejaAdendaConsulta : Consulta, IConsultaConDatosPersona
    {
        public int idLote { get; set; }
        public decimal nroDetalle { get; set; }
        public IList<int> IdsLineas { get; set; }
        public int? IdOrigen { get; set; }
        public int? NroPrestamoChecklist { get; set; }
        public long? NroFormulario { get; set; }
        public long? TipoPersona { get; set; }
        public string Cuil { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public bool SeleccionarTodos { get; set; }
        public IList<int> DepartamentoIds { get; set; } = new List<int>();
        public IList<int> LocalidadIds { get; set; } = new List<int>();

        //No consulta por fechas
        public void RevisarInclusionDeFechas()
        {
        }

    }
}