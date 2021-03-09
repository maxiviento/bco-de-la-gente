using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;

namespace Formulario.Aplicacion.Consultas.Consultas
{
    public class FormularioGrillaConsulta: Consulta, IConsultaConDatosPersona
    {
        [Required(ErrorMessage = "La fecha desde es requerida.")]
        public DateTime FechaDesde { get; set; }

        [Required(ErrorMessage = "La fecha hasta es requerida.")]
        public DateTime FechaHasta { get; set; }

        public string LocalidadIds { get; set; }

        public string DepartamentoIds { get; set; }

        public Id? IdOrigen { get; set; }

        public string IdEstadoFormulario { get; set; }

        public string NumeroFormulario { get; set; }
        public string NumeroPrestamo { get; set; }
        public string NumeroSticker { get; set; }

        public string IdLinea { get; set; }
        public Id? IdLote { get; set; }

        public long? TipoPersona { get; set; }
        public string Cuil { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public bool OrderByDes { get; set; }
        public int ColumnaOrderBy { get; set; }
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
