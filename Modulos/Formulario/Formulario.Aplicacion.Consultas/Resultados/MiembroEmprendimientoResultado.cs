using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class MiembroEmprendimientoResultado
    {
        public Id Id { get; set; }
        public string CodigoPais { get; set; }
        public string NroDocumento { get; set; }
        public string IdSexo { get; set; }
        public string IdVinculo { get; set; }
        public string Tarea { get; set; }
        public string HorarioTrabajo { get; set; }
        public decimal? Remuneracion { get; set; }
    }

    public class MiembroEmprendimientoFormularioResultado
    {
        public MiembroEmprendimientoFormularioResultado() { }

        public MiembroEmprendimientoFormularioResultado(DatosPersonalesResultado persona, string idVinculo, string vinculo, string tarea, string horarioTrabajo, decimal? remuneracion, bool? antecedentesLaborales, bool esSolicitante)
        {
            Persona = persona;
            IdVinculo = idVinculo;
            Vinculo = vinculo;
            Tarea = tarea;
            HorarioTrabajo = horarioTrabajo;
            Remuneracion = remuneracion;
            AntecedentesLaborales = antecedentesLaborales;
            EsSolicitante = esSolicitante;
        }

        public Id Id { get; set; }
        public DatosPersonalesResultado Persona { get; set; }
        public string IdVinculo { get; set; }
        public string Vinculo { get; set; }
        public string Tarea { get; set; }
        public string HorarioTrabajo { get; set; }
        public decimal? Remuneracion { get; set; }
        public bool? AntecedentesLaborales { get; set; }
        public bool EsSolicitante { get; set; }
    }
}
