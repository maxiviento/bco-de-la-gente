using System.Collections.Generic;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;

namespace Configuracion.Dominio.IRepositorio
{
    public interface ITipoDocumentacionRepositorio
    {
        IList<TipoDocumentacion> ConsultarTiposDocumentacion();
        TipoDocumentacion ConsultarTipoDocumentacionPorId(Id idTipoDocumentacion);
    }
}