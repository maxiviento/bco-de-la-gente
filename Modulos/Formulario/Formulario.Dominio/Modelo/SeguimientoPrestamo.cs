using System;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
   public class SeguimientoPrestamo: Entidad
    {
        public EstadoPrestamo Estado { get; protected set; }
        public Usuario Usuario { get; protected set; }
        public DateTime FechaSeguimiento { get; protected set; }
        public string Observaciones { get; protected set; }

        public SeguimientoPrestamo() { }

        public SeguimientoPrestamo(Usuario usuario, string observaciones, EstadoPrestamo estado)
        {
            this.Usuario = usuario;
            this.Observaciones = observaciones;
            this.Estado = estado;
        }
    }
}
