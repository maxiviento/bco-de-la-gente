using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class NecesidadInversionResultado : ReporteFormularioResultado
    {
        public Id Id { get; set; }
        public Id IdEmprendimiento { get; set; }
        public decimal? MontoCapitalPropio { get; set; }
        public decimal? MontoMicroprestamo { get; set; }
        public decimal? MontoOtrasFuentes { get; set; }
        public Id? IdFuenteFinanciamiento { get; set; }
        public string DescripcionFuenteFinanc { get; set; }
        public List<InversionRealizadaResultado> InversionesRealizadas { get; set; }
        public decimal SumatoriaTotalPrecios { get; set; }
    }
}
