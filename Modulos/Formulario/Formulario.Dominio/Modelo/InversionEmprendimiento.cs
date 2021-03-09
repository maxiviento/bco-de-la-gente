using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class InversionEmprendimiento : Entidad
    {
        public virtual Id IdEmprendimiento { get; set; }
        public virtual Id IdFuenteFinanciamiento { get; set; }
        public virtual decimal MontoFinanciamientoPrestamo { get; set; }
        public virtual decimal MontoCapitalPropio { get; set; }
        public virtual decimal MontoOtraFuente { get; set; }

        public InversionEmprendimiento()
        {
        }

        public InversionEmprendimiento(Id idEmprendimiento, Id idFuenteFinanciamiento,
            decimal montoFinanciamientoPrestamo, decimal montoCapitalPropio, decimal montoOtraFuente)
        {
            IdEmprendimiento = idEmprendimiento;
            IdFuenteFinanciamiento = idFuenteFinanciamiento;
            MontoFinanciamientoPrestamo = montoFinanciamientoPrestamo;
            MontoCapitalPropio = montoCapitalPropio;
            MontoOtraFuente = montoOtraFuente;
        }
    }
}