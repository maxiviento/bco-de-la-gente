using System.Collections.Generic;
using System.Linq;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Presentacion;

namespace Formulario.Aplicacion.Servicios
{
    public class EstadoFormularioServicio
    {
        private readonly IEstadoFormularioRepositorio _estadoFormularioRepositorio;

        public EstadoFormularioServicio(IEstadoFormularioRepositorio estadoFormularioRepositorio)
        {
            _estadoFormularioRepositorio = estadoFormularioRepositorio;
        }

        public IList<ClaveValorResultado<string>> ConsultarEstadosFormulario()
        {
            var estados = _estadoFormularioRepositorio.ConsultarEstadosFormulariosCombo();
            var estadosResultados = estados.Select(estado => new ClaveValorResultado<string>(
            estado.Id.ToString(),
            estado.Descripcion
            )).ToList();
            return estadosResultados;
        }

        public IList<ClaveValorResultado<string>> ConsultarEstadosParaPrestamos()
        {
            var estados = _estadoFormularioRepositorio.ConsultarEstadosPrestamosCombo();
            var estadosResultados = estados.Select(estado => new ClaveValorResultado<string>(
            estado.Id.ToString(),
            estado.Descripcion
            )).ToList();
            return estadosResultados;
        }

        public IList<ClaveValorResultado<string>> ObtenerEstadosFiltroCambioEstado()
        {
            //IList<EstadoFormulario> mock = new List<EstadoFormulario>();
            //mock.Add(EstadoFormulario.Impago);
            //mock.Add(EstadoFormulario.Pagado);
            //mock.Add(EstadoFormulario.PagoCuota);
            var estados = _estadoFormularioRepositorio.ObtenerEstadosFiltroCambioEstado();
            var estadosResultados = estados.Select(estado => new ClaveValorResultado<string>(
                estado.Id.ToString(),
                estado.Descripcion
            )).ToList();
            return estadosResultados;
            /*return mock.Select(estado => new ClaveValorResultado<string>(
                estado.Id.ToString(),
                estado.Descripcion
            )).ToList(); ;*/
        }
    }
}
