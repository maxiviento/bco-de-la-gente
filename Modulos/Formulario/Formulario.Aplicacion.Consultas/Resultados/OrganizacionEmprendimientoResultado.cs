using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class OrganizacionEmprendimientoResultado : ReporteFormularioResultado
    {
        public OrganizacionEmprendimientoResultado() { }

        public OrganizacionEmprendimientoResultado(string apellidoYNombre, string vinculo, string edad, string tarea, string horarioTrabajo, decimal? remuneracion, string antecedentesLaborales, int orden)
        {
            ApellidoYNombre = apellidoYNombre;
            Vinculo = vinculo;
            Edad = edad;
            Tarea = tarea;
            HorarioTrabajo = horarioTrabajo;
            Remuneracion = remuneracion;
            AntecedentesLaborales = antecedentesLaborales;
            Orden = orden;
        }

        public string ApellidoYNombre { get; set; }
        public string Vinculo { get; set; }
        public string Edad { get; set; }
        public string Tarea { get; set; }
        public string HorarioTrabajo { get; set; }
        public decimal? Remuneracion { get; set; }
        public string AntecedentesLaborales { get; set; }
    }
}
