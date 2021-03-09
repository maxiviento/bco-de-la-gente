using System.Collections.Generic;
using System.Linq;
using Formulario.Dominio.IRepositorio;
using Infraestructura.Core.Comun.Presentacion;

namespace Formulario.Aplicacion.Servicios
{
    public class LocalidadServicio
    {
        private readonly ILocalidadRepositorio _localidadRepositorio;

        public LocalidadServicio(ILocalidadRepositorio localidadRepositorio)
        {
            _localidadRepositorio = localidadRepositorio;
        }

        public IList<ClaveValorResultado<string>> ConsultarLocalidades(decimal? idDepartamento)
        {
            var localidades = _localidadRepositorio.ConsultarLocalidades(idDepartamento);
            var localidadesResultados = localidades.Select(localidad => new ClaveValorResultado<string>
            (
                localidad.Id.ToString(),
                localidad.Descripcion
            )).ToList();

            return localidadesResultados;
        }
    }
}
