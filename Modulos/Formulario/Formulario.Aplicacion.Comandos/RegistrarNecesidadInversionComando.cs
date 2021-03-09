using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Comandos
{
    public class RegistrarNecesidadInversionComando
    {
        public Id? Id { get; set; }
        public decimal? MontoMicroprestamo { get; set; }
        public decimal? MontoCapitalPropio { get; set; }
        public decimal? MontoOtrasFuentes { get; set; }
        public Id? IdFuenteFinanciamiento { get; set; }
        public List<RegistrarInversionRealizadaComando> InversionesRealizadas { get; set; }

        public bool ValidarRegistroNecesidadInversion()
        {
            if (!(MontoMicroprestamo.HasValue || MontoCapitalPropio.HasValue || MontoOtrasFuentes.HasValue ||
                IdFuenteFinanciamiento.HasValue))
                return false;
            return true;
        }
    }
}