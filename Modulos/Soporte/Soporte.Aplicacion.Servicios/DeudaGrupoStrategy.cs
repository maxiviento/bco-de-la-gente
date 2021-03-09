using System;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Dominio.Modelo.ItemDocumentos;

namespace Soporte.Aplicacion.Servicios
{
    public class DeudaGrupoStrategy : IDocItemStrategy
    {
        private readonly DeudaGrupoServicio _deudaGrupoServicio;
        public DeudaGrupoStrategy(DeudaGrupoServicio deudaGrupoServicio)
        {
            _deudaGrupoServicio = deudaGrupoServicio;
        }

        public ItemDocumentoEnum Tipo => ItemDocumentoEnum.DeudaGrupo;

        public DocumentoDescargaResultado ObtenerDocumentoPorId(Id idDocumento, string hash)
        {
            var resultado = new DocumentoDescargaResultado();
            var response = _deudaGrupoServicio.ObtenerHistorialDeudaGrupo(idDocumento.Valor, hash);
            var indiceComa = response.IndexOf(",", StringComparison.Ordinal);
            response = response.Substring(indiceComa + 1);

            resultado.Blob = Convert.FromBase64String(response);
            resultado.FileName = "";
            resultado.Extension = "application/pdf";

            return resultado;
        }

        public Resultado<DocumentacionResultado> ObtenerDocumentos(DocumentacionConsulta consulta)
        {
            return _deudaGrupoServicio.ObtenerTodosHistorialesDeudaGrupo(consulta);
        }
    }
}