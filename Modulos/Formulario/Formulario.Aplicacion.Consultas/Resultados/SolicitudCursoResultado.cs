using System.Collections.Generic;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class SolicitudCursoResultado
    {
        public IList<ConsultarCursosResultado> Cursos { get; set; }
        public string Descripcion { get; set; }
        public string NombreTipoCurso { get; set; }
    }
}