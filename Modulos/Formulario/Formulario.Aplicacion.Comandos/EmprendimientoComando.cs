using System;
using System.ComponentModel.DataAnnotations;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Comandos
{
    public class EmprendimientoComando
    {
        //[Required(ErrorMessage = "El id de vínculo del domicilio del emprendimiento es requerido.")]
        public int IdVinculo { get; set; }

        public int Id { get; set; }
        public int NroCodArea { get; set; }
        public long NroTelefono { get; set; }
        public string Email { get; set; }
        public int IdTipoInmueble { get; set; }
        public int IdActividad { get; set; }
        public DateTime? FechaInicioActividad { get; set; }
        public long IdTipoProyecto { get; set; }
        public DateTime? FechaActivo { get; set; }
        public int IdSectorDesarrollo { get; set; }
        public bool? TieneExperiencia { get; set; }
        public string TiempoExperiencia { get; set; }
        public bool? HizoCursos { get; set; }
        public string CursoInteres { get; set; }
        public bool? PidioCredito { get; set; }
        public bool? CreditoFueOtorgado { get; set; }
        public string InstitucionSolicitante { get; set; }
        public int? IdTipoOrganizacion { get; set; }
    }
}