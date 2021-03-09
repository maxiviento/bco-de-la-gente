using System.Collections.Generic;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Formulario
{
    public class TipoInteresRepositorio : NhRepositorio<TipoInteres>, ITipoInteresRepositorio
    {
        public TipoInteresRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<TipoInteres> ConsultarTipoIntereses()
        {
            var result = Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_TIPOS_INTERES")
                .ToListResult<TipoInteres>();
            return result;
        }
    }
}