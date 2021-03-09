using System.Collections.Generic;
using Formulario.Dominio.Modelo;

namespace Formulario.Dominio.IRepositorio
{
    public interface ILocalidadRepositorio
    {
        IList<Localidad> ConsultarLocalidades(decimal? idDepartamento);
    }
}
