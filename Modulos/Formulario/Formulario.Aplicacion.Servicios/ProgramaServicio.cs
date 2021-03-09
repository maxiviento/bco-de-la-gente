using System.Collections.Generic;
using System.Linq;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;

namespace Formulario.Aplicacion.Servicios
{
    public class ProgramaServicio
    {
        private readonly IProgramaRepositorio _programaRepositorio;

        public ProgramaServicio(IProgramaRepositorio programaRepositorio)
        {
            _programaRepositorio = programaRepositorio;
        }

        public IList<ProgramaResultado> ConsultarProgramas()
        {
            var programas = _programaRepositorio.ConsultarProgramas();
            var programasResultado = programas.Select(
                dest => new ProgramaResultado
                {
                    Id = dest.Id,
                    Nombre = dest.Nombre,
                }).ToList();

            return programasResultado;
        }
    }
}