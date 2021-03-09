using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using System.Collections.Generic;

namespace Formulario.Dominio.IRepositorio
{
    public interface ICursoRepositorio : IRepositorio<Curso>
    {
        IEnumerable<Curso> Consultar();

        IEnumerable<Curso> ConsultarCursosPorFormulario(int idFormulario);
    }
}