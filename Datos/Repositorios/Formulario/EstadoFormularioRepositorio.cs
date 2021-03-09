using System.Collections.Generic;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Formulario
{
    public class EstadoFormularioRepositorio: NhRepositorio<EstadoFormulario>, IEstadoFormularioRepositorio
    {
        public EstadoFormularioRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<EstadoFormulario> ConsultarEstadosFormulariosCombo()
        {
            var result = Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_ESTADOS_FORMULARIO")
                .ToListResult<EstadoFormulario>();
            return result;
        }

        public IList<EstadoFormulario> ConsultarEstadosPrestamosCombo()
        {
            var result = Execute("PR_OBTENER_ESTADO_FORM_PREST")
                .ToListResult<EstadoFormulario>();

            return result;
        }

        public IList<EstadoFormulario> ObtenerEstadosFiltroCambioEstado()
        {
            var result = Execute("PR_OBT_ESTADOS_FORM_BANDEJAS")
                .AddParam(1)
                .ToListResult<EstadoFormulario>();

            return result;
        }
    }
}
