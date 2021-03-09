using System;

namespace Soporte.Aplicacion.Consultas.Resultados
{
    public class SintysResultado
    {
        public string ApellidoNombre { get; set; }
        public string NroDocumento { get; set; }
        public string Activo_PeridoBase { get; set; }
        public string Activo_MontoIngreso { get; set; }
        public string Activo_Empleador { get; set; }
        public string Activo_Cuil { get; set; }
        public string Pasivo_PeriodoBase { get; set; }
        public string Pasivo_LeyAplicada { get; set; }
        public string Pasivo_MontoIngreso { get; set; }
        public bool EmpleadoPublico { get; set; }
        public DateTime? FechaDefuncion { get; set; }
    }
}