using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class RegistrarPrestamoResultado
    {
        public decimal IdPrestamo { get; set; }
        public decimal NumeroPrestamo { get; set; }

        public RegistrarPrestamoResultado()
        {
        }

        public RegistrarPrestamoResultado(decimal idPrestamo, decimal numeroPrestamo)
        {
            IdPrestamo = idPrestamo;
            NumeroPrestamo = numeroPrestamo;
        }
    }
}
