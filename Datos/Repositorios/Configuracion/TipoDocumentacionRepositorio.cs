using System.Collections.Generic;
using Configuracion.Dominio.IRepositorio;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Configuracion
{
    public class TipoDocumentacionRepositorio : NhRepositorio<TipoDocumentacion>, ITipoDocumentacionRepositorio
    {
        public TipoDocumentacionRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<TipoDocumentacion> ConsultarTiposDocumentacion()
        {
            var tiposDocumentacion =
                Execute("PR_OBTENER_TIPOS_DOC")
                    .ToListResult<TipoDocumentacion>();

            return tiposDocumentacion;
        }

        public TipoDocumentacion ConsultarTipoDocumentacionPorId(Id idTipoDocumentacion)
        {
            var tipoDocumentacion =
                Execute("PR_OBTENER_TIPOS_DOC_X_ID")
                    .AddParam(idTipoDocumentacion)
                    .ToUniqueResult<TipoDocumentacion>();

            return tipoDocumentacion;
        }
    }
}