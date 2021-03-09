using System.Collections.Generic;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Formulario
{
    public class OrigenRepositorio : NhRepositorio<OrigenFormulario>, IOrigenFormularioRepositorio
    {
        public OrigenRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<OrigenFormulario> ConsultarOrigenes()
        {
            var result = Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_ORIGENES_FORMULARIO")
                .ToListResult<OrigenFormulario>();
            return result;
        }
    }
}
