using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulario.Dominio.Modelo;

namespace Formulario.Aplicacion.Comandos
{
    public class RegistrarRechazoReactivacionPrestamoComando
    {
        public decimal IdPrestamo { get; set; }
        public ICollection<MotivoRechazo> MotivosRechazo { get; set; }
        public string NumeroCaja { get; set; }
    }
}
