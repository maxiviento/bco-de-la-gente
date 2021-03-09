using System.Collections.Generic;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Formulario
{
    public class DeudaEmprendimientoRepositorio : NhRepositorio<DeudaEmprendimiento>, IDeudaEmprendimientoRepositorio

    {
        public DeudaEmprendimientoRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<TipoDeudaEmprendimiento> ObtenerTiposDeudaEmprendimiento()
        {
            var result = Execute("PR_OBTENER_TABLAS_SATELITES")
                .AddParam("T_TIPOS_DEUDA_EMP")
                .ToListResult<TipoDeudaEmprendimiento>();
            return result;
        }

        public IList<DeudaEmprendimientoResultado> ObtenerDeudasEmprendimientoPorIdEmprendimiento(Id idEmprendimiento)
        {
            var resultados = Execute("PR_OBTENER_DEUDAS_EMP")
                .AddParam(idEmprendimiento)
                .ToListResult<DeudaEmprendimientoResultado>();
            return resultados;
        }

        public void RegistrarDeudaEmprendimiento(Id idEmprendimiento, Id idUsuario, string deudas)
        {
            Execute("PR_REGISTRA_DEUDAS_EMP")
                .AddParam(idEmprendimiento)
                .AddParam(deudas)
                .AddParam(idUsuario)
                .ToSpResult();
        }
    }
}