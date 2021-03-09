using System.Collections.Generic;
using System.Linq;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;

namespace Formulario.Aplicacion.Servicios
{
    public class TipoFinanciamientoServicio
    {
        private readonly ITipoFinanciamientoRepositorio _tipoFinanciamientoRepositorio;

        public TipoFinanciamientoServicio(ITipoFinanciamientoRepositorio tipoFinanciamientoRepositorio)
        {
            _tipoFinanciamientoRepositorio = tipoFinanciamientoRepositorio;
        }

        public IList<TipoFinanciamientoResultado> ConsultarTiposFinanciamiento()
        {
            var tiposFinanciamiento = _tipoFinanciamientoRepositorio.ConsultarTipoFinanciamientos();
            var tiposFinanciamientoResultado = tiposFinanciamiento.Select(
                finan => new TipoFinanciamientoResultado
                {
                    Id = finan.Id,
                    Descripcion = finan.Descripcion
                }).ToList();

            return tiposFinanciamientoResultado;
        }
    }
}