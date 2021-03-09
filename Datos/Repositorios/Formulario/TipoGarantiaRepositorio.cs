using System.Collections.Generic;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Formulario
{
    public class TipoGarantiaRepositorio : NhRepositorio<TipoGarantia>, ITipoGarantiaRepositorio
    {
        public TipoGarantiaRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<TipoGarantia> ConsultarTipoGarantias()
        {
            var result = Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_TIPOS_GARANTIA")
                .ToListResult<TipoGarantia>();
            return result;
        }
    }
}