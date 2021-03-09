using System.Collections.Generic;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;

namespace Configuracion.Dominio.IRepositorio
{
    public interface IMotivoBajaRepositorio : IRepositorio<MotivoBaja>
    {
        IEnumerable<MotivoBaja> ObtenerListadoMotivoBaja();
        MotivoBaja ConsultarPorId(Id id);
    }
}