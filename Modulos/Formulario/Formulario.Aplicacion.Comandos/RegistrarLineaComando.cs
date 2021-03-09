using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Configuracion.Aplicacion.Comandos;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Formateadores.MultipartData.Infrastructure;

namespace Formulario.Aplicacion.Comandos
{
    public class RegistrarLineaComando
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "El nombre de la línea es requerido."),
         MaxLength(100, ErrorMessage = "El nombre de la línea no puede superar los 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción de la línea es requerida."),
         MaxLength(200, ErrorMessage = "La descripción de la línea no puede superar los 200 caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El objetivo de la línea es requerido"),
         MaxLength(200, ErrorMessage = "El objetivo de la línea no puede superar los 200 caracteres.")]
        public string Objetivo { get; set; }

        public string Color { get; set; }
        public bool ConOng { get; set; }
        public bool ConCurso { get; set; }
        public bool ConPrograma { get; set; }
        public bool DeptoLocalidad { get; set; }
        public string LocalidadIds { get; set; }

        [Required(ErrorMessage = "El pie de página es requerido.")]
        public HttpFile PiePagina { get; set; }

        [Required(ErrorMessage = "El logo es requerido.")]
        public HttpFile Logo { get; set; }

        public RegistrarMotivoDestinoComando MotivoDestino { get; set; }
        public RegistrarSexoDestinatarioComando SexoDestinatario { get; set; }
        public RegistrarProgramaComando Programa { get; set; }

        [Required(ErrorMessage = "Debe seleccionar al menos un requisito para la línea.")]
        public IList<RegistrarRequisitoComando> Requisitos { get; set; }

        [Required(ErrorMessage = "Debe cargar al menos un detalle para la línea.")]
        public IList<RegistrarDetalleLineaComando> DetalleLineaPrestamo { get; set; }
        public IList<OngLinea> LsOng { get; set; }
    }
}