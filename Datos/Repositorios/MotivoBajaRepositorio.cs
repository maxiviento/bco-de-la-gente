
using System.Collections.Generic;
using System.Linq;
using Configuracion.Dominio.IRepositorio;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios
{
    public class MotivoBajaRepositorio : NhRepositorio<MotivoBaja>, IMotivoBajaRepositorio
    {
        public MotivoBajaRepositorio(ISession sesion) : base(sesion)
        {

        }

        public IEnumerable<MotivoBaja> ObtenerListadoMotivoBaja()
        {
            return Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_MOTIVOS_BAJA")
                .ToListResult<MotivoBaja>();
        }

        public MotivoBaja ConsultarPorId(Id id)
        {
            var lista = Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_MOTIVOS_BAJA")
                .ToListResult<MotivoBaja>();

            return lista.First(x => x.Id.Valor == id.Valor);
        }
    }
}
