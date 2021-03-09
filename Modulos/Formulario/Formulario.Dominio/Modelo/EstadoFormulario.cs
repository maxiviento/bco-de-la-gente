using System;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class EstadoFormulario: Entidad
    {
        public static EstadoFormulario Borrador => new EstadoFormulario(1, "BORRADOR");
        public static EstadoFormulario Completado => new EstadoFormulario(2, "COMPLETADO");
        public static EstadoFormulario Iniciado => new EstadoFormulario(3, "INICIADO");
        public static EstadoFormulario Rechazado => new EstadoFormulario(4, "RECHAZADO");
        public static EstadoFormulario Prestamo => new EstadoFormulario(5, "PRESTAMO");
        public static EstadoFormulario Eliminado => new EstadoFormulario(6, "ELIMINADO");
        public static EstadoFormulario PagoCuota => new EstadoFormulario(8, "PAGO CUOTA");
        public static EstadoFormulario Finalizado => new EstadoFormulario(9, "FINALIZADO");
        public static EstadoFormulario Pagado => new EstadoFormulario(10, "PAGADO");
        public static EstadoFormulario Impago => new EstadoFormulario(11, "IMPAGO");
        public static EstadoFormulario Inconsistencia => new EstadoFormulario(12, "INCONSISTENCIA");
        public static EstadoFormulario Reprogramado => new EstadoFormulario(13, "REPROGRAMADO");
        public static EstadoFormulario Moroso3Y4 => new EstadoFormulario(15, "MOROSO ENTRE 3 Y 4 MESES");
        public static EstadoFormulario MorosoMas5 => new EstadoFormulario(16, "MOROSO >= 5 MESES");
        public static EstadoFormulario Refinanciado => new EstadoFormulario(17, "REFINANCIADO");

        public virtual string Descripcion { get; set; }

        protected EstadoFormulario()
        {
        }

        protected EstadoFormulario(int id, string descripcion)
        {
            Id = new Id(id);
            Descripcion = descripcion;
        }
        public static EstadoFormulario ConId(int id)
        {
            switch (id)
            {
                case 1:
                    return Borrador;
                case 2:
                    return Completado;
                case 3:
                    return Iniciado;
                case 4:
                    return Rechazado;
                case 5:
                    return Prestamo;
                case 6:
                    return Eliminado;
                case 8:
                    return PagoCuota;
                case 9:
                    return Finalizado;
                case 10:
                    return Pagado;
                case 11:
                    return Impago;
                case 12:
                    return Inconsistencia;
                case 13:
                    return Reprogramado;
                case 15:
                    return Moroso3Y4;
                case 16:
                    return MorosoMas5;
                case 17:
                    return Refinanciado;
                default:
                    throw new ArgumentOutOfRangeException(nameof(id),
                        "No existe estado formulario para el ID solicitado");
            }
        }
    }
}
