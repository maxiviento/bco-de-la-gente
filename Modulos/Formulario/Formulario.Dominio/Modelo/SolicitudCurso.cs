using Infraestructura.Core.Comun.Excepciones;
using System.Collections.Generic;
using System.Linq;

namespace Formulario.Dominio.Modelo
{
    public sealed class SolicitudCurso
    {
        public IList<Curso> Cursos { get; private set; }
        public TipoCurso TipoCurso { get; private set; }
        public string Descripcion { get; private set; }

        private SolicitudCurso()
        {
        }

        public SolicitudCurso(IList<Curso> cursos, string descripcion) : this()
        {
            if (cursos == null || cursos.Count == 0)
                throw new ModeloNoValidoException("Una solicitud debe tener al menos un curso");
            //↑ el formulario puede tener cero solicitudes pero la solicitud debe tener al menos un curso 
            if (cursos.GroupBy(curso => curso.TipoCurso.Id).Count() != 1)
                throw new ModeloNoValidoException("Los cursos de una solicitud deben ser del mismo tipo");
            if (cursos.Any(c => c.Nombre.Equals("OTROS")) ^ descripcion != null)
                throw new ModeloNoValidoException(
                    "Una solicitud de curso \"OTROS\" debe venir acompañada de una descripción");
            Cursos = cursos;
            TipoCurso = cursos.First().TipoCurso;
            Descripcion = descripcion;
        }
    }
}