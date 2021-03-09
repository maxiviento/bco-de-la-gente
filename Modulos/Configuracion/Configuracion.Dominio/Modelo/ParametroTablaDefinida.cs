using System;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;

namespace Configuracion.Dominio.Modelo
{
    public class ParametroTablaDefinida : Entidad
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaDesde { get; set; } 
        public DateTime? FechaHasta { get; set; }
        public int IdMotivoRechazo { get; set; }
        public string NombreMotivoRechazo{ get; set; }

    public virtual Usuario UsuarioModificacion { get; protected set; }
        public virtual string ObservacionesRechazo { get; set; }


        public ParametroTablaDefinida(Id id, int motivo, string observacionesRechazo, Usuario usuarioBaja, DateTime fechaHasta)
        {
            Id = id;
            IdMotivoRechazo = motivo;
            ObservacionesRechazo = observacionesRechazo;
            UsuarioModificacion = usuarioBaja;
            FechaHasta = fechaHasta;
        }

        public ParametroTablaDefinida(string nombre, string descripcion, DateTime fechaDesde, DateTime fechaHasta, int motivoRechazo)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            FechaDesde = fechaDesde;
            FechaHasta = fechaHasta;
            IdMotivoRechazo = motivoRechazo;
        }

        public ParametroTablaDefinida()
        {
        }

        public ParametroTablaDefinida(string nombre, string descripcion, Usuario usuarioModificacion, DateTime fechaDesde)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            UsuarioModificacion = usuarioModificacion;
            FechaDesde = fechaDesde;
        }
    }
}
