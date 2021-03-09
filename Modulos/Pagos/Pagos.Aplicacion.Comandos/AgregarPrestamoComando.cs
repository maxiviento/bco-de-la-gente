using System;
using System.Collections.Generic;
using Pagos.Aplicacion.Consultas.Consultas;

namespace Pagos.Aplicacion.Comandos
{
    public class AgregarPrestamoComando
    {
        public int IdMonto { get; set; }
        public string IdsPrestamo { get; set; }
        public int IdLote { get; set; }
        public int Monto { get; set; }

    }
}
