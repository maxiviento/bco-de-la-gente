using System.Collections.Generic;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Formulario
{
    public class InversionEmprendimientoRepositorio : NhRepositorio<InversionEmprendimiento>,
        IInversionEmprendimientoRepositorio
    {
        public InversionEmprendimientoRepositorio(ISession sesion) : base(sesion)
        {
        }

        public IList<FuenteFinanciamientoResultado> ObtenerFuentesFinanciamiento()
        {
            var resultados = Execute("PR_OBTENER_FUEN_FINANC_COMBO")
                .ToListResult<FuenteFinanciamientoResultado>();

            return resultados;
        }

        public NecesidadInversionResultado ObtenerNecesidadInversionPorIdEmprendimiento(Id idEmprendimiento)
        {
            var resultado = Execute("PR_OBTENER_INVERSIONES_EMP")
                .AddParam(idEmprendimiento)
                .ToUniqueResult<NecesidadInversionResultado>();

            return resultado;
        }

        public void RegistrarNecesidadInversion(Id idEmprendimiento, Id? idFuenteFinanciamiento,
            decimal montoMicroprestamo,
            decimal montoCapitalPropio, decimal montoOtrasFuentes)
        {
            Execute("PR_ACTUALIZA_INVERSION_EMP")
                .AddParam(idEmprendimiento)
                .AddParam(montoMicroprestamo)
                .AddParam(montoCapitalPropio)
                .AddParam(montoOtrasFuentes)
                .AddParam(idFuenteFinanciamiento)
                .ToSpResult();
        }
    }
}