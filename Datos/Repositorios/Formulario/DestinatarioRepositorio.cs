using System.Collections.Generic;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Formulario
{
    public class DestinatarioRepositorio : NhRepositorio<Destinatario>, IDestinatarioRepositorio
    {
        public DestinatarioRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<Destinatario> ConsultarDestinatarios()
        {
            var result = Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_SEXOS_DESTINATARIO")
                .ToListResult<Destinatario>();
            return result;
        }
    }
}