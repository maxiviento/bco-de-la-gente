using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using System;
using System.Collections.Generic;

namespace Configuracion.Dominio.Modelo
{
    public class Item : Entidad
    {
        public virtual string Nombre { get; protected set; }
        public virtual string Descripcion { get; protected set; }
        public virtual DateTime? FechaAlta { get; protected set; }
        public virtual Usuario UsuarioAlta { get; protected set; }
        public virtual MotivoBaja MotivoBaja { get; protected set; }
        public virtual DateTime FechaUltimaModificacion { get; protected set; }
        public virtual Usuario UsuarioUltimaModificacion { get; protected set; }
        public virtual IList<TipoItem> TiposItem { get; protected set; }
        public virtual Item ItemPadre { get; set; }
        public virtual Recurso Recurso { get; set; }
        public virtual bool SubeArchivo { get; protected set; }
        public virtual bool GeneraArchivo { get; protected set; }
        public TipoDocumentacion TipoDocumentacion { get; protected set; }

        public Item()
        {
        }

        public Item(Id id)
        {
            Id = id;
        }

        public Item(string nombre, string descripcion, IList<TipoItem> tipoItems) : this(nombre, descripcion, tipoItems,
            null)
        {
        }

        public Item(string nombre, string descripcion, IList<TipoItem> tiposItem, Usuario usuario) : this(nombre,
            descripcion, tiposItem, usuario, null, null)
        {
        }

        public Item(string nombre, string descripcion, IList<TipoItem> tiposItem, Usuario usuario, Recurso recurso,
            Item itemPadre) : this(nombre, descripcion, tiposItem, usuario, recurso, itemPadre, false, false, null)
        {
        }

        public Item(string nombre, string descripcion, IList<TipoItem> tiposItem, Usuario usuario,
            bool subeArchivo, bool generaArchivo, TipoDocumentacion tipoDocumentacion) : this(nombre, descripcion,
            tiposItem, usuario, null, null, subeArchivo, generaArchivo, tipoDocumentacion)
        {
        }

        public Item(string nombre, string descripcion, IList<TipoItem> tiposItem, Usuario usuario,
            Recurso recurso, Item itemPadre, bool subeArchivo, bool generaArchivo, TipoDocumentacion tipoDocumentacion)
        {
            SetearCampos(nombre, descripcion, tiposItem, usuario, recurso, itemPadre, subeArchivo, generaArchivo,
                tipoDocumentacion);
        }

        private static void ValidarIntegridadCampos(string nombre, string descripcion, IList<TipoItem> tiposItems)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new ModeloNoValidoException("El nombre del ítem es requerido.");
            if (nombre.Length > 100)
                throw new ModeloNoValidoException("El nombre del ítem no puede exceder los 100 caracteres.");
            if (string.IsNullOrEmpty(descripcion))
                throw new ModeloNoValidoException("La descripción del ítem es requerido.");
            if (descripcion.Length > 200)
                throw new ModeloNoValidoException("La descripción del ítem no puede exceder los 200 caracteres.");
            if (tiposItems == null || (tiposItems != null && tiposItems.Count == 0))
                throw new ModeloNoValidoException("El item debe tener al menos un tipo de ítem.");
        }

        public Item Modificar(string nombre, string descripcion, IList<TipoItem> tiposItem, Usuario usuario,
            Recurso recurso, Item itemPadre, bool subeArchivo, bool generaArchivo, TipoDocumentacion tipoDocumentacion)
        {
            ValidarBaja();

            SetearCampos(nombre, descripcion, tiposItem, usuario, recurso, itemPadre, subeArchivo, generaArchivo,
                tipoDocumentacion);
            return this;
        }

        private void SetearCampos(string nombre, string descripcion, IList<TipoItem> tiposItem, Usuario usuario,
            Recurso recurso, Item itemPadre, bool subeArchivo, bool generaArchivo, TipoDocumentacion tipoDocumentacion)
        {
            ValidarIntegridadCampos(nombre, descripcion, tiposItem);

            Nombre = nombre;
            Descripcion = descripcion;
            if (tiposItem != null)
                TiposItem = tiposItem;
            Recurso = recurso;
            ItemPadre = itemPadre;
            FechaUltimaModificacion = DateTime.Now;
            UsuarioUltimaModificacion = usuario;

            SubeArchivo = subeArchivo;
            GeneraArchivo = generaArchivo;
            TipoDocumentacion = tipoDocumentacion;

            if (FechaAlta != null) return;
            FechaAlta = FechaUltimaModificacion;
            UsuarioAlta = UsuarioUltimaModificacion;
        }


        public Item DarDeBaja(MotivoBaja motivo, Usuario usuario)
        {
            ValidarBaja();

            UsuarioUltimaModificacion = usuario;
            MotivoBaja = motivo;
            FechaUltimaModificacion = DateTime.Now;
            return this;
        }

        private void ValidarBaja()
        {
            if (MotivoBaja != null && !MotivoBaja.Id.IsDefault())
            {
                throw new ModeloNoValidoException("El ítem ya esta dado de baja.");
            }
        }
    }
}