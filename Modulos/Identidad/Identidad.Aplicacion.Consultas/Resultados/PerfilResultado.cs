using System;
using Infraestructura.Core.Comun.Dato;

namespace Identidad.Aplicacion.Consultas.Resultados
{
    public class PerfilResultado
    {
        public Id Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }
        public bool Activo { get; set; }
        public Id  IdMotivoBaja { get; set; }
        public string MotivoBaja { get; set; }
    }
}
