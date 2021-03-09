using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class SolicitudCursoReporteResultado: ReporteFormularioResultado
    {
        public SolicitudCursoReporteResultado() { }

        public SolicitudCursoReporteResultado(string nombre, decimal idTipoCurso, string nombreTipoCurso, bool seleccionado)
        {
            Nombre = nombre;
            IdTipoCurso = idTipoCurso;
            NombreTipoCurso = nombreTipoCurso;
            Seleccionado = seleccionado;
        }

        public string Nombre { get; set; }
        public decimal IdTipoCurso { get; set; }
        public string NombreTipoCurso { get; set; }
        public bool Seleccionado { get; set; }
    }
}
