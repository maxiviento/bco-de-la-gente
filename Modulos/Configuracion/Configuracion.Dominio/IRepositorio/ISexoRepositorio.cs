using System.Collections.Generic;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;

namespace Configuracion.Dominio.IRepositorio
{
    public interface ISexoRepositorio : IRepositorio<Sexo>
    {
        IEnumerable<Sexo> ObtenerListadoSexo();
    }
}