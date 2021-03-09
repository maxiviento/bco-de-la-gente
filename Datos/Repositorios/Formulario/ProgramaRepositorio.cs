using System.Collections.Generic;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Formulario
{
    public class ProgramaRepositorio : NhRepositorio<Programa>, IProgramaRepositorio
    {
        public ProgramaRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<Programa> ConsultarProgramas()
        {
            var result = Execute("PR_OBTENER_PROGRAMAS")
            .ToListResult<Programa>();
            return result;

        }
    }
}