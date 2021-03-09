using System.Collections.Generic;
using System.Linq;
using Formulario.Dominio.IRepositorio;
using Infraestructura.Core.Comun.Presentacion;

namespace Formulario.Aplicacion.Servicios
{
    public class OrigenFormularioServicio
    {
        private readonly IOrigenFormularioRepositorio _origenRepositorio;

        public OrigenFormularioServicio(IOrigenFormularioRepositorio origenRepositorio)
        {
            _origenRepositorio = origenRepositorio;
        }

        public IList<ClaveValorResultado<string>> ConsultarOrigenes()
        {
            var origenes = _origenRepositorio.ConsultarOrigenes();
            var origenesResultados = origenes.Select(origen => new ClaveValorResultado<string>
            (
                origen.Id.ToString(),
                origen.Descripcion
            )).ToList();

            return origenesResultados;

        }
    }
}
