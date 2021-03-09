using System;
using Infraestructura.Core.Comun.Dato;

namespace Identidad.Aplicacion.Consultas.Resultados
{
    public class UsuarioResultado
    {
        public Id Id { get; set; }
        public string Cuil { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public Id PerfilId { get; set; }
        public string NombrePerfil { get; set; }
        public DateTime FechaAlta { get; set; }
        public DateTime? FechaBaja { get; set; }
        public string NombreMotivoBaja { get; set; }
        public bool? Activo { get; set; } = true;
        public bool? ReiniciarToken { get; set; }
        public bool? Sistema => (Cuil == "11111111111" || Cuil == "99999999999" || Cuil == "22222222222");
    }
}
