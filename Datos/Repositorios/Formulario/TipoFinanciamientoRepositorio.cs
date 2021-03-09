using System.Collections.Generic;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Formulario
{
    public class TipoFinanciamientoRepositorio : NhRepositorio<TipoFinanciamiento>, ITipoFinanciamientoRepositorio
    {
        public TipoFinanciamientoRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<TipoFinanciamiento> ConsultarTipoFinanciamientos()
        {
            var result = Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_TIPOS_FINANCIAMIENTO")
                .ToListResult<TipoFinanciamiento>();
            return result;
        }
    }
}