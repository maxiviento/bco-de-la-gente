using System.Collections.Generic;
using System.Linq;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;

namespace Formulario.Aplicacion.Servicios
{
    public class DestinatarioServicio
    {
        private readonly IDestinatarioRepositorio _destinatarioRepositorio;

        public DestinatarioServicio(IDestinatarioRepositorio destinatarioRepositorio)
        {
            _destinatarioRepositorio = destinatarioRepositorio;
        }

        public IList<DestinatarioResultado> ConsultarDestinatarios()
        {
            var destinatarios = _destinatarioRepositorio.ConsultarDestinatarios();
            var destinatariosResultado = destinatarios.Select(
                dest => new DestinatarioResultado
                {
                    Id = dest.Id,
                    Descripcion = dest.Descripcion
                }).ToList();

            return destinatariosResultado;
        }
    }
}