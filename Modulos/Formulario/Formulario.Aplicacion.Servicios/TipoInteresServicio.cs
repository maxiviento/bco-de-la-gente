using System.Collections.Generic;
using System.Linq;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;

namespace Formulario.Aplicacion.Servicios
{
    public class TipoInteresServicio
    {
        private readonly ITipoInteresRepositorio _tipoInteresRepositorio;

        public TipoInteresServicio(ITipoInteresRepositorio tipoInteresRepositorio)
        {
            _tipoInteresRepositorio = tipoInteresRepositorio;
        }

        public IList<TipoInteresResultado> ConsultarTipoIntereses()
        {
            var tiposIntereses = _tipoInteresRepositorio.ConsultarTipoIntereses();
            var tiposInteresesResultado = tiposIntereses.Select(
                interes => new TipoInteresResultado
                {
                    Id = interes.Id,
                    Descripcion = interes.Descripcion
                }).ToList();

            return tiposInteresesResultado;
        }
    }
}