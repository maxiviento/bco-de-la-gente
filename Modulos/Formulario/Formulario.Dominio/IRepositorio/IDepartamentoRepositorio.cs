using Formulario.Dominio.Modelo;
using System.Collections.Generic;

namespace Formulario.Dominio.IRepositorio
{
    public interface IDepartamentoRepositorio
    {
        IList<Departamento> Consultar();
    }
}