using System;
using System.Collections.Generic;
using Configuracion.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;

namespace Formulario.Dominio.Modelo
{
    public class LineaPrestamo : Entidad
    {
        public virtual DateTime FechaAlta { get; protected set; }
        public virtual bool ConOng { get; protected set; }
        public virtual bool ConCurso { get; protected set; }
        public virtual bool ConPrograma { get; protected set; }
        public virtual bool DeptoLocalidad { get; protected set; }
        public virtual Programa Programa { get; protected set; }
        public virtual string Nombre { get; protected set; }
        public virtual string Descripcion { get; protected set; }
        public virtual string Objetivo { get; protected set; }
        public virtual DateTime FechaBaja { get; protected set; }
        public virtual DateTime FechaUltimaModificacion { get; protected set; }
        public virtual Usuario UsuarioUltimaModificacion { get; protected set; }
        public virtual IList<RequisitoPrestamo> Configuracion { get; protected set; }
        public virtual MotivoBaja MotivoBaja { get; protected set; }
        public virtual SexoDestinatario SexoDestinatario { get; protected set; }
        public virtual Formulario Formulario { get; protected set; }
        public virtual string Color { get; protected set; }
        public virtual string PathLogo { get; protected set; }
        public virtual string PathPieDePagina { get; protected set; }
        public virtual IList<DetalleLineaPrestamo> DetalleLineaPrestamo { get; protected set; }
        public virtual MotivoDestino MotivoDestino { get; protected set; }
        public virtual IList<OrdenCuadrante> OrdenCuadrantes { get; protected set; }
        public virtual string LocalidadIds { get; set; }


        public LineaPrestamo()
        {
        }

        public LineaPrestamo(Id id, MotivoBaja motivoBaja, Usuario usuario)
        {
            if (usuario == null)
                throw new ModeloNoValidoException("No se puede eliminar línea sin usuario asociado");
            if (motivoBaja == null)
                throw new ModeloNoValidoException("No se puede eliminar línea sin motivo de baja");
            Id = id;
            MotivoBaja = motivoBaja;
            UsuarioUltimaModificacion = usuario;
            FechaUltimaModificacion = DateTime.Now;
        }

        public LineaPrestamo(
            bool conOng,
            bool conCurso,
            bool conPrograma,
            Programa programa,
            string nombre,
            string descripcion,
            string objetivo,
            IList<RequisitoPrestamo> configuracion,
            SexoDestinatario sexoDestinatario,
            string color,
            string pathLogo,
            string pathPieDePagina,
            MotivoDestino motivoDestino, Usuario usuario, string localidadIds, bool deptoLocalidad) : this(conOng, conCurso, conPrograma, programa, nombre, descripcion,
            objetivo,
            configuracion, sexoDestinatario, color, pathLogo, pathPieDePagina, null, motivoDestino, usuario, localidadIds, deptoLocalidad)
        {
        }

        public LineaPrestamo(
            bool conOng,
            bool conCurso,
            bool conPrograma, 
            Programa programa,
            string nombre,
            string descripcion,
            string objetivo,
            IList<RequisitoPrestamo> configuracion,
            SexoDestinatario sexoDestinatario,
            string color,
            string pathLogo,
            string pathPieDePagina,
            IList<DetalleLineaPrestamo> detalleLineaPrestamo,
            MotivoDestino motivoDestino, Usuario usuario,
            string localidadIds,
            bool deptoLocalidad)
        {
            ValidarDatos(conOng, conCurso, conPrograma, programa, nombre, descripcion,
                objetivo, configuracion, sexoDestinatario, color,
                pathLogo, pathPieDePagina, motivoDestino, usuario);

            if (detalleLineaPrestamo == null)
                throw new ModeloNoValidoException("Se debe agregar al menos un detalle de línea.");

            ConOng = conOng;
            ConCurso = conCurso;
            ConPrograma = conPrograma;
            Programa = programa;
            Nombre = nombre;
            Descripcion = descripcion;
            Objetivo = objetivo;
            Configuracion = configuracion;
            SexoDestinatario = sexoDestinatario;
            Color = color;
            PathLogo = pathLogo;
            PathPieDePagina = pathPieDePagina;
            DetalleLineaPrestamo = detalleLineaPrestamo;
            MotivoDestino = motivoDestino;
            FechaAlta = DateTime.Now;
            UsuarioUltimaModificacion = usuario;
            LocalidadIds = localidadIds;
            DeptoLocalidad = deptoLocalidad;
        }

        public LineaPrestamo ModificarLineaPrestamo(Id id, bool conOng, bool conCurso, bool conPrograma, Programa programa, string nombre,
            string descripcion, string objetivo, IList<RequisitoPrestamo> configuracion,
            SexoDestinatario sexoDestinatario, string color,
            string pathLogo, string pathPieDePagina,
            MotivoDestino motivoDestino, Usuario usuario,
            string localidadIds,
            bool deptoLocalidad)
        {
            ValidarDatos(conOng, conCurso, conPrograma, programa,nombre, descripcion,
                objetivo, configuracion, sexoDestinatario, color,
                pathLogo, pathPieDePagina, motivoDestino, usuario);
            Id = id;
            ConOng = conOng;
            ConCurso = conCurso;
            ConPrograma = conPrograma;
            Programa = programa;
            Nombre = nombre;
            Descripcion = descripcion;
            Objetivo = objetivo;
            Configuracion = configuracion;
            SexoDestinatario = sexoDestinatario;
            Color = color;
            PathLogo = pathLogo;
            PathPieDePagina = pathPieDePagina;
            MotivoDestino = motivoDestino;
            UsuarioUltimaModificacion = usuario;
            FechaUltimaModificacion = DateTime.Now;
            DeptoLocalidad = deptoLocalidad;
            LocalidadIds = localidadIds;
            return this;
        }

        public void ValidarDatos(bool conOng, bool conCurso, bool conPrograma, Programa programa, string nombre, string descripcion,
            string objetivo, IList<RequisitoPrestamo> configuracion, SexoDestinatario sexoDestinatario, string color,
            string pathLogo, string pathPieDePagina,
            MotivoDestino motivoDestino, Usuario usuario)
        {
            if (string.IsNullOrEmpty(nombre))
                throw new ModeloNoValidoException("El nombre es requerido.");

            if (nombre.Length > 100)
                throw new ModeloNoValidoException("El nombre no puede superar los 100 caracteres.");

            if (string.IsNullOrEmpty(descripcion))
                throw new ModeloNoValidoException("La descripción es requerida.");

            if (descripcion.Length > 200)
                throw new ModeloNoValidoException("La descripción no puede superar los 200 caracteres.");


            if (string.IsNullOrEmpty(objetivo))
                throw new ModeloNoValidoException("El objetivo es requerido.");

            if (descripcion.Length > 200)
                throw new ModeloNoValidoException("El objetivo no puede superar los 200 caracteres.");

            if (string.IsNullOrEmpty(pathLogo))
                throw new ModeloNoValidoException("El logo es requerido.");

            if (string.IsNullOrEmpty(pathPieDePagina))
                throw new ModeloNoValidoException("El pie de página es requerido.");

            if (descripcion.Length > 2048)
                throw new ModeloNoValidoException("El logo no puede superar los 2048 caracteres.");

            if (string.IsNullOrEmpty(color))
                Color = "#FFFFFF";

            if (!string.IsNullOrEmpty(color) && color.Length > 8)
                throw new ModeloNoValidoException("El color no puede superar los 8 caracteres.");

            if (configuracion == null)
                throw new ModeloNoValidoException("Se debe asignar al menos un requisito.");

            if (usuario == null)
                throw new ModeloNoValidoException("El usuario es requerido.");

            if (conPrograma && programa.Id.Valor == -1)
            {
                throw new ModeloNoValidoException("El programa es requerido");
            }
        }

        public LineaPrestamo DarDeBaja(MotivoBaja motivo, Usuario usuario)
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
                throw new ModeloNoValidoException("La línea ya esta dada de baja.");
            }
        }
    }
}