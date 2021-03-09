using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class DetalleInversionEmprendimiento : Entidad
    {
        public virtual Id IdItemInversion { get; set; }
        public virtual Id IdTipoInversion { get; set; }
        public virtual Id IdInversionEmprendimiento { get; set; }
        public virtual string Observaciones { get; set; }
        public virtual bool EsNuevo { get; set; }
        public virtual long Precio { get; set; }
        public virtual long Cantidad { get; set; }

        public DetalleInversionEmprendimiento()
        {
        }

        public DetalleInversionEmprendimiento(Id idItemInversion, Id idTipoInversion, Id idInversionEmprendimiento,
            string observaciones, bool esNuevo, long precio, long cantidad)
        {
            IdItemInversion = idItemInversion;
            IdTipoInversion = idTipoInversion;
            IdInversionEmprendimiento = idInversionEmprendimiento;
            Observaciones = observaciones;
            EsNuevo = esNuevo;
            Precio = precio;
            Cantidad = cantidad;
        }
    }
}