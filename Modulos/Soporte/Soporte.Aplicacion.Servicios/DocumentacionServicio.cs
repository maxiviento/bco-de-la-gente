using System;
using System.Collections.Generic;
using System.Linq;
using Formulario.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using Infraestructura.Core.Comun.Presentacion;
using Soporte.Aplicacion.Comandos;
using Soporte.Aplicacion.Consultas;
using Soporte.Aplicacion.Consultas.Resultados;
using Soporte.Dominio.IRepositorio;
using Soporte.Dominio.Modelo;
using Soporte.Dominio.Modelo.ItemDocumentos;

namespace Soporte.Aplicacion.Servicios
{
    public class DocumentacionServicio : IDocItemContext
    {
        private readonly IDocumentacionRepositorio _documentacionRepositorio;
        private readonly ISesionUsuario _sesionUsuario;
        private readonly DocumentacionCDDUtilServicio _documentacionCddServicio;
        private readonly List<IDocItemStrategy> _estrategias = new List<IDocItemStrategy>();


        public DocumentacionServicio(IDocumentacionRepositorio documentacionRepositorio, ISesionUsuario sesionUsuario,
            DocumentacionCDDUtilServicio documentacionServicio, SintysStrategy sintysStrategy, RentasStrategy rentasStrategy,
            DeudaGrupoStrategy deudaGrupoStrategy)
        {
            _documentacionRepositorio = documentacionRepositorio;
            _sesionUsuario = sesionUsuario;
            _documentacionCddServicio = documentacionServicio;
            _estrategias.Add(sintysStrategy);
            _estrategias.Add(rentasStrategy);
            _estrategias.Add(deudaGrupoStrategy);
        }

        public Resultado<DocumentacionResultado> ObtenerDocumentos(DocumentacionConsulta consulta)
        {
            var itemDocumento = (ItemDocumentoEnum)Enum.Parse(typeof(ItemDocumentoEnum), consulta.IdItem.ToString());
            return _estrategias.FirstOrDefault(x => x.Tipo == itemDocumento)?
                       .ObtenerDocumentos(consulta) ?? ObtenerDocumentosCdd(consulta);
        }

        public decimal RegistrarDocumentacion(RegistrarDocumentoComando comando)
        {
            var usuario = _sesionUsuario.Usuario;
            var idTipoDocumentacionCdd =
                ObtenerIdTipoDocumentacionPorIdRequisitoPrestamo(new Id(comando.IdItem));

            var extensionArchivo = AcortarExtensionArchivo(comando.Documento.MediaType);

            var documentacion = new Documentacion(
                comando.Documento.FileName,
                extensionArchivo,
                comando.Documento.Buffer,
                usuario,
                new RequisitoPrestamo())
            {
                IdTipoDocumentacionCdd = idTipoDocumentacionCdd
            };


            documentacion = _documentacionCddServicio.GuardarDocumentacionCdd(documentacion, usuario.Cuil);

            try
            {
                documentacion.Id = _documentacionRepositorio.RegistrarDocumento(documentacion, new Id(comando.IdItem),
                    new Id(comando.IdFormularioLinea));
            }
            catch (ErrorOracleException oe)
            {
                //_documentacionCddServicio.EliminarDocumentacionCdd(decimal.ToInt32(documentacion.IdDocumentoCdd.Valor),
                //    decimal.ToInt32(documentacion.IdTipoDocumentacionCdd.Valor), usuario.Cuil); // TODO Manolo: Revisar si corresponde
                throw new ModeloNoValidoException("Error de Base de Datos: " + oe.Message);
            }

            return documentacion.Id.Valor;
        }

        public DocumentoDescargaResultado ObtenerDocumentoPorId(Id idDocumento, Id idItem, string hash)
        {
            var itemDocumento = (ItemDocumentoEnum)Enum.Parse(typeof(ItemDocumentoEnum), idItem.ToString());
            return _estrategias.FirstOrDefault(x => x.Tipo == itemDocumento)?
                       .ObtenerDocumentoPorId(idDocumento, hash) ?? ObtenerDocumentoCddPorId(idDocumento, idItem);
        }

        public Id ObtenerIdTipoDocumentacionPorIdRequisitoPrestamo(Id idItem)
        {
            var idTipoDocumentacion =
                _documentacionRepositorio.ObtenerItemPorId(idItem).TipoDocumentacion.Id;

            return idTipoDocumentacion;
        }

        public string AcortarExtensionArchivo(string extension)
        {
            var indice = extension.IndexOf("/", StringComparison.Ordinal);

            if (indice == -1) return extension;
            var extensionNueva = extension.Substring(indice + 1);
            return extensionNueva;

        }

        private Resultado<DocumentacionResultado> ObtenerDocumentosCdd(DocumentacionConsulta consulta)
        {
            if (consulta == null)
            {
                consulta = new DocumentacionConsulta {NumeroPagina = 0};
            }

            consulta.TamañoPagina = 5;

            return _documentacionRepositorio.ObtenerDocumentos(consulta);
        }

        private DocumentoDescargaResultado ObtenerDocumentoCddPorId(Id idDocumento, Id idItem)
        {
            var resultado = new DocumentoDescargaResultado();
            var documentacion =
                _documentacionRepositorio.ObtenerDocumentacionPorId(idDocumento);
            var usuario = _sesionUsuario.Usuario;
            var idTipoDocumentacionCdd =
                ObtenerIdTipoDocumentacionPorIdRequisitoPrestamo(idItem);

            var response =
                _documentacionCddServicio.ObtenerDocumentacionCdd((int)documentacion.IdDocumentoCidi,
                    (int)idTipoDocumentacionCdd.Valor,
                    usuario.Cuil);

            resultado.Blob = response.Documentacion.Imagen_Documentacion;
            resultado.FileName = response.N_Documento;
            resultado.Extension = response.Documentacion.Extension.ToLower();

            return resultado;
        }
    }
}