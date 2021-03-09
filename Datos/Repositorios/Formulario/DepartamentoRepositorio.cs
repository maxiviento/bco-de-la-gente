using System.Collections.Generic;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Formulario
{
    public class DepartamentoRepositorio : NhRepositorio<Departamento>, IDepartamentoRepositorio
    {
        public DepartamentoRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<Departamento> Consultar()
        {
            var result = Execute("PR_OBTENER_DEPARTAMENTOS")
                .ToListResult<Departamento>();
            return result;
        }
    }
}
