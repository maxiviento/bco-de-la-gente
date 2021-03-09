using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.IRepositorio;
using System.Collections.Generic;
using System.Linq;

namespace Configuracion.Aplicacion.Servicios
{
    public class SexoServicio
    {
        private readonly ISexoRepositorio _repositorio;

        public SexoServicio(ISexoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<ConsultaSexosResultado> Consultar()
        {
            var listadoSexos = _repositorio.ObtenerListadoSexo();

            return listadoSexos.Select(sexo => new ConsultaSexosResultado
            {
                Id = sexo.Id.Valor, Descripcion = sexo.Descripcion
            });
        }
    }
}
