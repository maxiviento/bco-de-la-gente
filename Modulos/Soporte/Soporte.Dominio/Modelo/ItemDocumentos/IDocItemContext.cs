using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;

namespace Soporte.Dominio.Modelo.ItemDocumentos
{
    public interface IDocItemContext
    {
        Resultado<DocumentacionResultado> ObtenerDocumentos(DocumentacionConsulta consulta);
        DocumentoDescargaResultado ObtenerDocumentoPorId(Id idDocumento, Id idItem, string hash);
    }
}