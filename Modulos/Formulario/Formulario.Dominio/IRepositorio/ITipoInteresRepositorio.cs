using System.Collections.Generic;
using Formulario.Dominio.Modelo;

namespace Formulario.Dominio.IRepositorio
{
    public interface ITipoInteresRepositorio
    {
        IList<TipoInteres> ConsultarTipoIntereses();
    }
}