using Formulario.Dominio.Modelo;
using System.Collections.Generic;
using Formulario.Aplicacion.Consultas.Resultados;

namespace Formulario.Dominio.IRepositorio
{
    public interface IDestinoFondosRepositorio
    {
        IList<DestinoFondos> ConsultarDestinosFondos();

        IList<DestinoFondoSeleccionadoResultado> ConsultarDestinosFondosPorFormulario(int idFormulario);
    }
}