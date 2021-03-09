using System.Collections.Generic;
using Formulario.Dominio.Modelo;

namespace Formulario.Dominio.IRepositorio
{
    public interface IEstadoFormularioRepositorio
    {
        IList<EstadoFormulario> ConsultarEstadosFormulariosCombo();
        IList<EstadoFormulario> ConsultarEstadosPrestamosCombo();
        IList<EstadoFormulario> ObtenerEstadosFiltroCambioEstado();
    }
}
