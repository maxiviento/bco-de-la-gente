using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Configuracion.Aplicacion.Comandos
{
    public class ItemComando
    {
        public long? Id { get; set; }

        [Required(ErrorMessage = "El nombre del ítem es requerido."),
         MaxLength(100, ErrorMessage = "El nombre del ítem tiene un largo máximo de 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción del ítem es requerido."),
         MaxLength(200, ErrorMessage = "La descripción del ítem tiene un largo máximo de 200 caracteres.")]
        public string Descripcion { get; set; }

        public long? IdUsuarioUltimaModificacion { get; set; }

        public DateTime FechaUltimaModificacion { get; set; }

        public DateTime? FechaBaja { get; set; }

        public long? IdMotivoBaja { get; set; }

        public IList<TipoItemComando> TiposItem { get; set; }

        public long? IdRecurso { get; set; }

        public long? IdItemPadre { get; set; }

        public bool SubeArchivo { get; set; }

        public bool GeneraArchivo { get; set; }

        public long? IdTipoDocumentacionCdd { get; set; }
    }
}