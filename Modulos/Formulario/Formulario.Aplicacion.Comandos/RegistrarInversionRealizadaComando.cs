using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Comandos
{
    public class RegistrarInversionRealizadaComando
    {
        public Id? Id { get; set; }
        public Id IdItemInversion { get; set; }
        public string Observaciones { get; set; }
        public long? CantidadNuevos { get; set; }
        public long? CantidadUsados { get; set; }
        public decimal? PrecioNuevos { get; set; }
        public decimal? PrecioUsados { get; set; }
    }
}