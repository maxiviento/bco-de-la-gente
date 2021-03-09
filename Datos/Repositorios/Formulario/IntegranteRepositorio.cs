using System.Collections.Generic;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Formulario
{
    public class IntegranteRepositorio : NhRepositorio<TipoIntegranteSocio>, IIntegranteRepositorio
    {
        public IntegranteRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<TipoIntegranteSocio> ConsultarIntegrantes()
        {
            var result = Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_TIPOS_INTEGRANTE")
                .ToListResult<TipoIntegranteSocio>();
            return result;
        }
    }
}