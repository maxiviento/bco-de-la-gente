using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class MiembroEmprendimiento: Entidad
    {
        public MiembroEmprendimiento() { }

        public MiembroEmprendimiento(Persona persona, Vinculo vinculo, string tarea, string horarioTrabajo, decimal? remuneracion, bool? antecedentesLaborales, bool esSolicitante)
        {
            Persona = persona;
            Vinculo = vinculo;
            Tarea = tarea;
            HorarioTrabajo = horarioTrabajo;
            Remuneracion = remuneracion;
            AntecedentesLaborales = antecedentesLaborales;
            EsSolicitante = esSolicitante;
        }
        
        public Persona Persona { get; set; }
        public Vinculo Vinculo { get; set; }
        public string Tarea { get; set; }
        public string HorarioTrabajo { get; set; }
        public decimal? Remuneracion { get; set; }
        public bool? AntecedentesLaborales { get; set; }
        public bool EsSolicitante { get; set; }
    }
}
