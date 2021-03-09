using System;
using Formulario.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;

namespace Soporte.Dominio.Modelo
{
    public class Documentacion : Entidad
    {
        protected Documentacion()
        {
        }

        public Documentacion(string nombre, string extension, byte[] archivo, Usuario usuario,
            RequisitoPrestamo requisitoPrestamo)
        {
            if (!string.IsNullOrEmpty(nombre) && nombre.Length > 100)
                throw new ModeloNoValidoException("El nombre no puede superar los 100 caracteres.");
            if (!string.IsNullOrEmpty(extension) && extension.Length > 50)
                throw new ModeloNoValidoException("La extensión no puede superar los 50 caracteres.");
            if (archivo == null)
            {
                throw new ModeloNoValidoException("El archivo es requerido.");
            }

            Nombre = nombre;
            Extension = extension;
            Archivo = archivo;
            Usuario = usuario;
            RequisitoPrestamo = requisitoPrestamo;
            FechaAlta = new DateTime();
        }

        public virtual string Nombre { get; protected set; }

        public virtual string Extension { get; protected set; }

        public virtual Id IdDocumentoCdd { get; set; }

        public virtual byte[] Archivo { get; protected set; }

        public virtual DateTime FechaAlta { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual RequisitoPrestamo RequisitoPrestamo { get; set; }

        public virtual Id IdTipoDocumentacionCdd { get; set; }
    }
}