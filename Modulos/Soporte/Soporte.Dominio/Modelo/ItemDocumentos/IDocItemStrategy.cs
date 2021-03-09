using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;

namespace Soporte.Dominio.Modelo.ItemDocumentos
{
    public interface IDocItemStrategy
    {
        ItemDocumentoEnum Tipo { get; }
        DocumentoDescargaResultado ObtenerDocumentoPorId(Id idDocumento, string hash);
        Resultado<DocumentacionResultado> ObtenerDocumentos(DocumentacionConsulta consulta);
    }
}