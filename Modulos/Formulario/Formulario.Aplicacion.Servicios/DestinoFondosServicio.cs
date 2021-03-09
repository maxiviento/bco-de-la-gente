using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using System.Collections.Generic;
using System.Linq;
using Formulario.Dominio.Modelo;

namespace Formulario.Aplicacion.Servicios
{
    public class DestinoFondosServicio
    {
        private readonly IDestinoFondosRepositorio _destinoFondosRepositorio;

        public DestinoFondosServicio(IDestinoFondosRepositorio destinoFondosRepositorio)
        {
            _destinoFondosRepositorio = destinoFondosRepositorio;
        }

        public IList<DestinoFondoResultado> ConsultarDestinosFondos()
        {
            var destinos = OrdenarDestinoFondos(_destinoFondosRepositorio.ConsultarDestinosFondos());
            return (from df in destinos
                select new DestinoFondoResultado
                {
                    Id = df.Id.Valor,
                    Descripcion = df.Descripcion
                }).ToList();
        }


        private IList<DestinoFondos> OrdenarDestinoFondos(IList<DestinoFondos> destinos)
        {
            // Orden de elementos definido por el formulario físico.
            var ordenEspecifico = new List<int> { 0, 4, 8, 1, 5, 9, 2, 6, 10, 3, 7, 11 };
            return ordenEspecifico.Select(i => destinos[i]).ToList();
        }

        public IList<DestinoFondoResultado> ConsultarDestinosFondosPorFormulario(int idFormulario)
        {
            return (from df in _destinoFondosRepositorio.ConsultarDestinosFondosPorFormulario(idFormulario)
                    select new DestinoFondoResultado
                    {
                        Id = df.Id.Valor
                    }).ToList();
        }
    }
}