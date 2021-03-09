using System;
using Infraestructura.Core.Comun.Excepciones;

namespace Formulario.Dominio.Modelo
{
    public sealed class CondicionesPrestamo
    {
        public decimal MontoSolicitado { get; private set; }
        public decimal CantidadCuotas { get; private set; }
        public decimal MontoEstimadoCuota { get; private set; }

        private CondicionesPrestamo()
        {
        }

        public CondicionesPrestamo(decimal montoSolicitado, decimal cantidadDeCuotas, decimal montoEstimadoCuota)
            : this()
        {
            var montoParcial = Math.Round(cantidadDeCuotas * montoEstimadoCuota, 2);

            if (montoParcial < Math.Round(montoSolicitado)) montoParcial = montoParcial + (decimal) 0.01;

            if (Math.Ceiling(montoParcial) != Math.Round(montoSolicitado))
                throw new ModeloNoValidoException(
                    "La cantidad y monto estimado de cuotas no coinciden con el monto solicitado para el préstamo");
            MontoSolicitado = montoSolicitado;
            CantidadCuotas = cantidadDeCuotas;
            MontoEstimadoCuota = montoEstimadoCuota;
        }

        public CondicionesPrestamo(decimal montoSolicitado, decimal cantidadDeCuotas)
            : this()
        {
            MontoSolicitado = montoSolicitado;
            CantidadCuotas = cantidadDeCuotas;
        }
    }
}