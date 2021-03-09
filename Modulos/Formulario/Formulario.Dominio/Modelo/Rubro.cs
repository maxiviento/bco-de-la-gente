using System;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class Rubro : Entidad
    {
        protected Rubro()
        {
        }

        public Rubro(int id)
        {
            Id = new Id(id);
        }

        public Rubro(int id, string nombre)
        {
            Id = new Id(id);
            Nombre = nombre;
        }

        public virtual string Nombre { get; protected set; }
    }
}