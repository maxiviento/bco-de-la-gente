using System;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class Actividad : Entidad
    {
        protected Actividad()
        {
        }

        public Actividad(int id)
        {
            Id = new Id(id);
        }

        public Actividad(int id, string nombre, DateTime? fechaInicio, int? idRubro)
        {
            Id = new Id(id);
            FechaInicio = fechaInicio;
            Nombre = nombre;
            IdRubro = idRubro;
        }

        public virtual string Nombre { get; protected set; }
        public virtual DateTime? FechaInicio { get; protected set; }
        public virtual int? IdRubro { get; set; }
    }
}