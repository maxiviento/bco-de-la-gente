﻿using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class EstadoProceso : Entidad
    {
        public EstadoProceso()
        {
        }

        public EstadoProceso(string nombre, string descripcion)
        {
            Nombre = nombre;
            Descripcion = descripcion;
        }

        public virtual string Nombre { get; set; }
        public virtual string Descripcion { get; set; }
    }
}
