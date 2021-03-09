using System.Collections.Generic;
using Formulario.Dominio.Modelo;

namespace Formulario.Aplicacion.Comandos
{
    public class RechazarPrestamoComando
    {
        public int IdPrestamo { get; set; }
        public int? IdMotivoRechazo { get; set; }

        public IList<MotivoRechazo> MotivosRechazo { get; set; }
        public string Observaciones { get; set; }
        public virtual string NumeroCaja { get; set; }
    }
}
