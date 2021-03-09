using System.Collections.Generic;
using Configuracion.Dominio.Modelo;

namespace Configuracion.Dominio.IRepositorio
{
    public interface IPaisRepositorio
    {
        IEnumerable<Pais> ObtenerListadoPais();
    }
}