using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Datos;
using NHibernate;
using System.Collections.Generic;

namespace Datos.Repositorios.Formulario
{
    public class CursoRepositorio : NhRepositorio<Curso>, ICursoRepositorio
    {
        public CursoRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IEnumerable<Curso> Consultar()
        {
            return Execute("PR_OBTENER_CURSOS")
                .ToListResult<Curso>();
        }

        public IEnumerable<Curso> ConsultarCursosPorFormulario(int idFormulario)
        {
            return Execute("PR_OBTENER_CURSOS_X_FORM")
                .AddParam(idFormulario)
                .ToListResult<Curso>();
        }
    }
}