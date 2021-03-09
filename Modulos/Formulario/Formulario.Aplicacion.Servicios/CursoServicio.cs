using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using System.Collections.Generic;
using System.Linq;

namespace Formulario.Aplicacion.Servicios
{
    public class CursoServicio
    {
        private readonly ICursoRepositorio _cursoRepositorio;

        public CursoServicio(ICursoRepositorio cursoRepositorio)
        {
            _cursoRepositorio = cursoRepositorio;
        }

        public IEnumerable<ConsultarCursosResultado> Consultar()
        {
            var cursos = from curso in _cursoRepositorio.Consultar()
                select new ConsultarCursosResultado
                {
                    Id = curso.Id.Valor,
                    Nombre = curso.Nombre,
                    IdTipoCurso = curso.TipoCurso.Id.Valor,
                    NombreTipoCurso = curso.TipoCurso.Nombre
                };
            return cursos.ToList();
        }

        public IEnumerable<ConsultarCursosResultado> ConsultarPorFormulario(int idFormulario)
        {
            var cursos = from curso in _cursoRepositorio.ConsultarCursosPorFormulario(idFormulario)
                         select new ConsultarCursosResultado
                         {
                             Id = curso.Id.Valor,
                             IdTipoCurso = curso.TipoCurso.Id.Valor
                         };
            return cursos.ToList();
        }
    }
}