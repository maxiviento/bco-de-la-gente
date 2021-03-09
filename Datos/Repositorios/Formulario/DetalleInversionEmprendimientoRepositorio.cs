using System.Collections.Generic;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Datos;
using NHibernate;

namespace Datos.Repositorios.Formulario
{
    public class DetalleInversionEmprendimientoRepositorio : NhRepositorio<DetalleInversionEmprendimiento>,
        IDetalleInversionEmprendimientoRepositorio
    {
        public DetalleInversionEmprendimientoRepositorio(ISession sesion) : base(sesion)
        {
        }

        public List<InversionRealizadaResultado> ObtenerDetallesInversionPorIdTipoInversion(Id idEmprendimiento,
            Id idTipoInversion)
        {
            var resultados = Execute("PR_OBTENER_DET_INV_EMP")
                .AddParam(idEmprendimiento)
                .AddParam(idTipoInversion)
                .ToListResult<InversionRealizadaResultado>();

            var res = (List<InversionRealizadaResultado>)resultados;

            return res;
        }

        public decimal RegistrarDetalleInversion(Id? idDetalleInversion, Id idEmprendimiento, Id idTipoInversion,
            Id idItemInversion,
            string observaciones,
            bool esNuevo, decimal precio, long cantidad)
        {
            return Execute("PR_REGISTRA_DET_INV_EMP")
                .AddParam(idDetalleInversion ?? new Id(-1))
                .AddParam(idEmprendimiento)
                .AddParam(idItemInversion)
                .AddParam(idTipoInversion)
                .AddParam(observaciones)
                .AddParam(esNuevo)
                .AddParam(precio)
                .AddParam(cantidad)
                .ToSpResult()
                .Id.Valor;
        }

        public void EliminarDetallesInversion(Id idDetalleInversion)
        {
            Execute("PR_ELIMINA_DET_INV_EMP")
                .AddParam(idDetalleInversion)
                .ToSpResult();
        }

        public IList<ItemInversionResultado> ObtenerItemsInversion()
        {
            var resultados = Execute("PR_OBTENER_ITEMS_INVERS_COMBO")
                .ToListResult<ItemInversionResultado>();

            return resultados;
        }
    }
}