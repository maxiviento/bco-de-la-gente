using System;
using System.ComponentModel.DataAnnotations;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;

namespace Formulario.Aplicacion.Consultas.Consultas
{
    public class FormularioGrillaChequeConsulta: Consulta
    {
        [Required(ErrorMessage = "La fecha desde es requerida.")]
        public DateTime FechaDesde { get; set; }

        [Required(ErrorMessage = "La fecha hasta es requerida.")]
        public DateTime FechaHasta { get; set; }

        public Id? IdLocalidad { get; set; }

        public Id? idDepartamento { get; set; }

        public Id? IdOrigen { get; set; }

        public Id IdEstadoFormulario { get; set; }

        public long? NumeroFormulario { get; set; }
        public long? NumeroPrestamo { get; set; }

        public Id? IdLinea { get; set; }
        public Id? IdLote { get; set; }

        public string Cuil { get; set; }
        public string Dni { get; set; }
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
