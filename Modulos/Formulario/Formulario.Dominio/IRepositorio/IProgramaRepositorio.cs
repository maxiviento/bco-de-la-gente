using System.Collections.Generic;
using Formulario.Dominio.Modelo;

namespace Formulario.Dominio.IRepositorio
{
    public interface IProgramaRepositorio
    {
        IList<Programa> ConsultarProgramas();
    }
}
