using System.Collections.Generic;
using Configuracion.Dominio.IRepositorio;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios
{
    public class PaisRepositorio : NhRepositorio<Entidad>, IPaisRepositorio
    {
        public PaisRepositorio(ISession sesion) : base(sesion)
        {

        }
        
        public IEnumerable<Pais> ObtenerListadoPais()
        {
            return ObtenerTodos<Pais>("VT_PAISES");
        }
    }
}
