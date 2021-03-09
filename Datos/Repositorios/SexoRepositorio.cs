using System.Collections.Generic;
using Configuracion.Dominio.IRepositorio;
using Configuracion.Dominio.Modelo;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios
{
    public class SexoRepositorio : NhRepositorio<Sexo>, ISexoRepositorio
    {
        public SexoRepositorio(ISession sesion) : base(sesion)
        {

        }

        public IEnumerable<Sexo> ObtenerListadoSexo()
        {
            return ObtenerTodos<Sexo>("VT_SEXOS");
        }
    }
}
