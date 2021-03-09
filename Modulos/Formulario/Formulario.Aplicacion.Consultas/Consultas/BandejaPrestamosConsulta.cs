using System;
using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;

namespace Formulario.Aplicacion.Consultas.Consultas
{
    public class BandejaPrestamosConsulta: Consulta, IConsultaConDatosPersona
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cuil { get; set; }
        public string NroFormulario { get; set; }
        public string NroPrestamo { get; set; }
        public string NroSticker { get; set; }
        public string IdEstadoPrestamo { get; set; }
        public int? IdOrigen { get; set; }
        public string IdLinea { get; set; }
        public int? IdUsuario { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string LocalidadIds { get; set; }
        public string DepartamentoIds { get; set; }
        public long? TipoPersona { get; set; }
        public string Dni { get; set; }
        public bool QuiereReactivar { get; set; }
        public int ColumnaOrderBy { get; set; }
        public bool OrderByDes { get; set; }

        /// <summary>
        /// En caso de estar consultando por el DNI o CUIL debería no tenerse en cuenta las fechas de la consulta 
        /// </summary>
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
