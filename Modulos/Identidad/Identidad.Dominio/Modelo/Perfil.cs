using System;
using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;

namespace Identidad.Dominio.Modelo
{
    public class Perfil : Entidad
    {
        public Perfil()
        {
            Funcionalidades = new List<Funcionalidad>();
        }

        public Perfil(string nombre, Usuario usuario, IList<Funcionalidad> funcionalidades)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new ModeloNoValidoException("El nombre del perfil es requerido");
            if (nombre.Length > 200)
                throw new ModeloNoValidoException("El nombre no puede tener más de 200 caracteres");

            if (usuario == null)
                throw new ModeloNoValidoException("El usuario no puede ser nulo");

            if (funcionalidades == null || funcionalidades.Count.Equals(0))
                throw new ModeloNoValidoException("Se debe seleccionar al menos una funcionalidad");

            this.Nombre = nombre;
            this.FechaAlta = DateTime.Today;
            this.UsuarioAlta = usuario;
            this.Funcionalidades = funcionalidades;
        }

        public void Modificar(string nombre, Usuario usuario, IList<Funcionalidad> funcionalidades)
        {
            if (usuario == null)
                throw new ModeloNoValidoException("El usuario no puede ser nulo");

            if (!string.IsNullOrEmpty(nombre))
            {
                if (nombre.Length > 200)
                    throw new ModeloNoValidoException("El nombre no puede tener más de 200 caracteres");

                this.Nombre = nombre;
            }

            if (funcionalidades != null && funcionalidades.Count > 0)
            {
                this.Funcionalidades = funcionalidades;
            }

            this.FechaModificacion = DateTime.Today;
            this.UsuarioModificacion = usuario;
        }

        public void DarDeBaja(MotivoDeBaja motivoBaja, Usuario usuario)
        {
            if (usuario == null)
                throw new ModeloNoValidoException("El usuario no puede ser nulo");

            if (motivoBaja == null)
                throw new ModeloNoValidoException("El motivo es requerido");

            this.MotivoBaja = motivoBaja;
            this.FechaBaja = DateTime.Today;
            this.UsuarioModificacion = usuario;
            this.Funcionalidades = new List<Funcionalidad>();
        }

        public virtual string Nombre { get; set; }
        public virtual DateTime FechaAlta { get; protected set; }
        public virtual Usuario UsuarioAlta { get; protected set; }
        public virtual DateTime? FechaModificacion { get; protected set; }
        public virtual Usuario UsuarioModificacion { get; protected set; }
        public virtual DateTime? FechaBaja { get; protected set; }
        public virtual MotivoDeBaja MotivoBaja { get; protected set; }
        public virtual IList<Funcionalidad> Funcionalidades { get; set; }
    }
}
