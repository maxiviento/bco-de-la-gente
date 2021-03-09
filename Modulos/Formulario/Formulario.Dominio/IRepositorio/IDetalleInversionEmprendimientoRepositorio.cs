using System.Collections.Generic;
using Formulario.Aplicacion.Consultas.Resultados;
using Infraestructura.Core.Comun.Dato;
using Formulario.Dominio.Modelo;

namespace Formulario.Dominio.IRepositorio
{
    public interface IDetalleInversionEmprendimientoRepositorio : IRepositorio<DetalleInversionEmprendimiento>
    {
        List<InversionRealizadaResultado> ObtenerDetallesInversionPorIdTipoInversion(Id idEmprendimiento,
            Id idTipoInversion);

        decimal RegistrarDetalleInversion(Id? idDetalleInversion, Id idEmprendimiento, Id idTipoInversion,
            Id idItemInversion,
            string observaciones, bool esNuevo, decimal precio, long cantidad);

        void EliminarDetallesInversion(Id idDetalleInversion);

        IList<ItemInversionResultado> ObtenerItemsInversion();
    }
}