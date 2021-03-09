using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class DeudaEmprendimiento : Entidad
    {
        public virtual Emprendimiento Emprendimiento { get; set; }
        public virtual TipoDeudaEmprendimiento TipoDeudaEmprendimiento { get; set; }
        public virtual decimal Monto { get; set; }

        public DeudaEmprendimiento(Emprendimiento emprendimiento, TipoDeudaEmprendimiento tipoDeudaEmprendimiento,
            decimal monto)
        {
            Emprendimiento = emprendimiento;
            TipoDeudaEmprendimiento = tipoDeudaEmprendimiento;
            Monto = monto;
        }
    }
}