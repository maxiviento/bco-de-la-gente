using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.IRepositorio;
using System.Collections.Generic;
using System.Linq;

namespace Configuracion.Aplicacion.Servicios
{
    public class PaisServicio
    {
        private readonly IPaisRepositorio _repositorio;

        public PaisServicio(IPaisRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<ConsultaPaisesResultado> Consultar()
        {
            var listaPaises = _repositorio.ObtenerListadoPais();

            return listaPaises.Select(pais => new ConsultaPaisesResultado
            {
                Id = pais.Id,
                Descripcion = pais.Descripcion
            });
        }
    }
}
