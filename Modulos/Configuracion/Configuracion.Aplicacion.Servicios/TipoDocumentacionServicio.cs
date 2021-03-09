using System.Collections.Generic;
using Configuracion.Dominio.IRepositorio;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;

namespace Configuracion.Aplicacion.Servicios
{
    public class TipoDocumentacionServicio
    {
        private readonly ITipoDocumentacionRepositorio _tipoDocumentacionRepositorio;

        public TipoDocumentacionServicio(ITipoDocumentacionRepositorio tipoDocumentacionRepositorio)
        {
            _tipoDocumentacionRepositorio = tipoDocumentacionRepositorio;
        }

        public IList<TipoDocumentacion> ConsultarTiposDocumentacion()
        {
            var tiposDocumentacion = _tipoDocumentacionRepositorio.ConsultarTiposDocumentacion();

            return tiposDocumentacion;
        }
    }
}