using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Dominio.Modelo;

namespace Soporte.Dominio.IRepositorio
{
    public interface IDocumentacionRepositorio : IRepositorio<Documentacion>
    {
        DocumentacionResultado ObtenerDocumentacionPorId(Id documentacionId);
        Id RegistrarDocumento(Documentacion documentacion, Id idItem, Id idPrestamo);
        Resultado<DocumentacionResultado> ObtenerDocumentos(DocumentacionConsulta consulta);
        Item ObtenerItemPorId(Id idItem);
    }
}