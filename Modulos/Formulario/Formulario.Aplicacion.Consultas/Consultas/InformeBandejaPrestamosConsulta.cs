using System;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;

namespace Formulario.Aplicacion.Consultas.Consultas
{
    public class InformeBandejaPrestamosConsulta : Consulta, IConsultaConDatosPersona
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cuil { get; set; }
        public string NroFormulario { get; set; }
        public string NroPrestamo { get; set; }
        public string NroSticker { get; set; }
        public string EstadoPrestamo { get; set; }
        public string OrigenFormulario { get; set; }
        public string Linea { get; set; }
        public string Usuario { get; set; }
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
        public string Localidad { get; set; }
        public string Departamento { get; set; }
        public string TipoPersonaDescripcion { get; set; }
        public string Dni { get; set; }
        public string QuiereReactivar { get; set; }
        public long? TipoPersona { get; set; }
        public string MontoOtorgado { get; set; }


        /// <summary>
        /// En caso de estar consultando por el DNI o CUIL debería no tenerse en cuenta las fechas de la consulta 
        /// </summary>
        public void RevisarInclusionDeFechas()
        {
            if (!string.IsNullOrEmpty(Dni?.Trim()) || !string.IsNullOrEmpty(Cuil?.Trim()))
            {
                FechaDesde = "";
                FechaHasta = "";
            }
        }
    }
}
