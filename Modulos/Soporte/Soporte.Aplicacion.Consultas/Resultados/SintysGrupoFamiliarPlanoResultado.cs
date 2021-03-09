using System;

namespace Soporte.Aplicacion.Consultas.Resultados
{
    class SintysPrestamoPlanoResultado
    {
        public string ApellidoNombre { get; set; }
        public string NroDocumento { get; set; }
        public string Sexo { get; set; }
        public string CodigoPais { get; set; }
        public string NroSticker { get; set; }
        public string CondicionEdad { get; set; }
        public string Domicilio { get; set; }
        public string Localidad { get; set; }
        public string Departamento { get; set; }
        public string Linea { get; set; }
        public string NroPrestamo { get; set; }
        public bool EsSolicitante { get; set; }
        public string GF_ApellidoNombre { get; set; }
        public string GF_NroDocumento { get; set; }
        public string GF_Activo_PeridoBase { get; set; }
        public string GF_Activo_MontoIngreso { get; set; }
        public string GF_Activo_Empleador { get; set; }
        public string GF_Activo_Cuil { get; set; }
        public string GF_Pasivo_PeriodoBase { get; set; }
        public string GF_Pasivo_LeyAplicada { get; set; }
        public string GF_Pasivo_MontoIngreso { get; set; }
        public bool GF_EmpleadoPublico { get; set; }
        public DateTime GF_FechaDefuncion { get; set; }
    }
}