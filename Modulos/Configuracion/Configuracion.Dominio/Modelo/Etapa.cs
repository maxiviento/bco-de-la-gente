using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using System;

namespace Configuracion.Dominio.Modelo
{
    public class Etapa : Entidad
    {
        public virtual string Nombre { get; protected set; }
        public virtual string Descripcion { get; protected set; }
        public virtual DateTime? FechaAlta { get; protected set; }
        public virtual Usuario UsuarioAlta { get; protected set; }
        public virtual MotivoBaja MotivoBaja { get; protected set; }
        public virtual DateTime FechaUltimaModificacion { get; protected set; }
        public virtual Usuario UsuarioUltimaModificacion { get; protected set; }

        protected Etapa()
        {
        }

        public Etapa(Id id)
        {
            Id = id;
        }

        public Etapa(string nombre, string descripcion) : this(nombre, descripcion, null)
        {
        }

        public Etapa(string nombre, string descripcion, Usuario usuario)
        {
            SetearCampos(nombre, descripcion, usuario);
        }

        public Etapa DarDeBaja(MotivoBaja motivo, Usuario usuario)
        {
            ValidarBaja();

            UsuarioUltimaModificacion = usuario;
            MotivoBaja = motivo;
            FechaUltimaModificacion = DateTime.Now;
            return this;
        }

        public Etapa Modificar(string nombre, string descripcion, Usuario usuario)
        {
            ValidarBaja();

            SetearCampos(nombre, descripcion, usuario);
            return this;
        }

        private void SetearCampos(string nombre, string descripcion, Usuario usuario)
        {
            ValidarIntegridadCampos(nombre, descripcion);

            Nombre = nombre;
            Descripcion = descripcion;
            FechaUltimaModificacion = DateTime.Now;
            UsuarioUltimaModificacion = usuario;
            if (FechaAlta != null) return;
            FechaAlta = FechaUltimaModificacion;
            UsuarioAlta = UsuarioUltimaModificacion;
        }

        private static void ValidarIntegridadCampos(string nombre, string descripcion)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                throw new ModeloNoValidoException("El Nombre de la etapa es requerido.");
            }

            if (string.IsNullOrEmpty(descripcion))
            {
                throw new ModeloNoValidoException("La descripción de la etapa es requerida.");
            }

            if (nombre.Length > 100)
            {
                throw new ModeloNoValidoException("El Nombre no debe superar los 100 caracteres.");
            }

            if (descripcion.Length > 200)
            {
                throw new ModeloNoValidoException("La descripción no debe superar los 200 caracteres.");
            }
        }

        private void ValidarBaja()
        {
            if (MotivoBaja != null && !MotivoBaja.Id.IsDefault())
            {
                throw new ModeloNoValidoException("La etapa ya esta dada de baja.");
            }
        }
    }
}