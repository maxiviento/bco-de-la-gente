using System.Collections.Generic;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Formulario
{
    public class LocalidadRepositorio : NhRepositorio<Localidad>, ILocalidadRepositorio
    {
        public LocalidadRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<Localidad> ConsultarLocalidades(decimal? idDepartamento)
        {
            var result = Execute("PR_OBTENER_LOCALIDADES")
                .AddParam(idDepartamento)
                .ToListResult<Localidad>();
            return result;
        }
    }
}