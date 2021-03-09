using System.ComponentModel.DataAnnotations;
using Formulario.Dominio.Modelo;

namespace Formulario.Aplicacion.Comandos
{
    public class DetalleLineaPrestamoComando
    {
        public int? Id { get; set; }

        public int? LineaId { get; set; }

        [Required(ErrorMessage = "El monto prestable es requerido para el detalle de la línea."),
         MaxLength(8, ErrorMessage = "El monto prestable del detalle no puede superar los 8 caracteres.")]
        public string MontoPrestable { get; set; }

        [MaxLength(8, ErrorMessage = "El monto por socio integrante del detalle no puede superar los 8 caracteres.")]
        public string MontoTope { get; set; }

        [Required(ErrorMessage = "La cantidad mínima de integrantes es requerida para el detalle de la línea."),
         MaxLength(3, ErrorMessage =
             "La cantidad mínima de integrantes del detalle no puede superar los 3 caracteres.")]
        public string CantidadMinimaIntegrantes { get; set; }

        [Required(ErrorMessage = "La cantidad máxima de integrantes es requerida para el detalle de la línea."),
         MaxLength(3, ErrorMessage =
             "La cantidad máxima de integrantes del detalle no puede superar los 3 caracteres.")]
        public string CantidadMaximaIntegrantes { get; set; }

        [Required(ErrorMessage = "El plazo de devolución máximo es requerido para el detalle de la línea."),
         MaxLength(3, ErrorMessage = "El plazo de devolución máximo del detalle no puede superar los 3 caracteres.")]
        public string PlazoDevolucion { get; set; }

        [Required(ErrorMessage = "El valor estimado de la cuota solidaria es requerido para el detalle de la línea."),
         MaxLength(8, ErrorMessage =
             "El valor estimado del detalle de la cuota solidaria no puede superar los 8 caracteres.")]
        public string ValorCuotaSolidaria { get; set; }

        [Required(ErrorMessage = "La visualización de la línea es requerida."),
         MaxLength(500, ErrorMessage = "La visualización del detalle de la línea no puede superar los 500 caracteres.")]
        public string Visualizacion { get; set; }

        public bool Apoderado { get; set; }

        public RegistrarItegranteSocioComando IntegranteSocio { get; set; }

        public RegistrarTipoFinanciamientoComando TipoFinanciamiento { get; set; }

        public RegistrarTipoInteresComando TipoInteres { get; set; }

        public RegistrarTipoGarantiaComando TipoGarantia { get; set; }
        [Required(ErrorMessage = "El convenio de pago de la línea es requerido.")]
        public Convenio ConvenioPago { get; set; }
        [Required(ErrorMessage = "El convenio de recupero de la línea es requerido.")]
        public Convenio ConvenioRecupero { get; set; }

    }
}