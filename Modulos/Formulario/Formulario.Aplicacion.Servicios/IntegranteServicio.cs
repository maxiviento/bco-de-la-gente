using System.Collections.Generic;
using System.Linq;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;

namespace Formulario.Aplicacion.Servicios
{
    public class IntegranteServicio
    {
        private readonly IIntegranteRepositorio _integranteRepositorio;

        public IntegranteServicio(IIntegranteRepositorio integranteRepositorio)
        {
            _integranteRepositorio = integranteRepositorio;
        }

        public IList<IntegranteResultado> ConsultarIntegrantes()
        {
            var integrantes = _integranteRepositorio.ConsultarIntegrantes();
            var integrantesResultado = integrantes.Select(
                integ => new IntegranteResultado
                {
                    Id = integ.Id,
                    Descripcion = integ.Descripcion
                }).ToList();

            return integrantesResultado;
        }
    }
}