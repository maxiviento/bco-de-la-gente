using System;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class EmprendimientoResultado
    {
        public Id Id { get; set; }
        public string Calle { get; set; }
        public int? NroCalle { get; set; }
        public string Torre { get; set; }
        public string NroDpto { get; set; }
        public string NroPiso { get; set; }
        public string Manzana { get; set; }
        public string Casa { get; set; }
        public string Barrio { get; set; }
        public string Departamento { get; set; }
        public int IdDepartamento { get; set; }
        public string Localidad { get; set; }
        public int IdLocalidad { get; set; }
        public int? CodPostal { get; set; }
        public int? NroCodArea { get; set; }
        public int? NroTelefono { get; set; }
        public string Email { get; set; }
        public int IdTipoInmueble { get; set; }
        public string TipoInmueble { get; set; }
        public int IdActividad { get; set; }
        public int IdTipoProyecto { get; set; }
        public string TipoProyecto { get; set; }
        public DateTime? FechaActivo { get; set; }
        public int IdSectorDesarrollo { get; set; }
        public string SectorDesarrollo { get; set; }
        public bool TieneExperiencia { get; set; }
        public string TiempoExperiencia { get; set; }
        public bool HizoCursos { get; set; }
        public string CursoInteres { get; set; }
        public bool PidioCredito { get; set; }
        public bool CreditoFueOtorgado { get; set; }
        public string InstitucionSolicitante { get; set; }
        public int? IdTipoOrganizacion { get; set; }
        public int? IdRubro { get; set; }
        public string Destino { get; set; }
    }
}