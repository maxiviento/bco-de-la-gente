using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.IRepositorio;
using System.Collections.Generic;
using System.Linq;

namespace Configuracion.Aplicacion.Servicios
{
    public class MotivoBajaServicio
    {
        private readonly IMotivoBajaRepositorio _repositorio;

        public MotivoBajaServicio(IMotivoBajaRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<ConsultaMotivosBajaResultado> Consultar()
        {
            var listaMotivos = _repositorio.ObtenerListadoMotivoBaja();

            return listaMotivos.Select(motivoBaja => new ConsultaMotivosBajaResultado
            {
                Id = motivoBaja.Id.Valor, Descripcion = motivoBaja.Descripcion
            });
        }
    }
}
