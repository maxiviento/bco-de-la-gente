using System;
using Configuracion.Aplicacion.Consultas.Resultados;
using Configuracion.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;

namespace Formulario.Dominio.Modelo
{
    public class MotivoRechazo : Entidad
    {
        protected MotivoRechazo()
        {
        }

        public MotivoRechazo(int id)
        {
            Id = new Id(id);
        }

        public MotivoRechazo(string nombre, string descripcion)
        {
            Nombre = nombre;
            Descripcion = descripcion;
        }

        public MotivoRechazo(string nombre, string descripcion, string abreviatura, bool esAutomatico, Ambito ambito,
            Usuario usuarioAlta, string observaciones, string codigo)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Abreviatura = abreviatura;
            EsAutomatico = esAutomatico;
            Ambito = ambito;
            UsuarioAlta = usuarioAlta;
            EsPredefinido = false;
            Observaciones = observaciones;
            Codigo = codigo;
        }

        public MotivoRechazo(ConsultaMotivoRechazoResultado.Detallado detalle)
        {
            Id = detalle.Id;
            Nombre = detalle.Nombre;
            Descripcion = detalle.Descripcion;
            UsuarioAlta = new Usuario {Id = detalle.IdUsuarioAlta, Cuil = detalle.CuilUsuarioAlta};
            if (detalle.MotivoBajaId != null)
                MotivoBaja = new MotivoBaja
                {
                    Id = detalle.MotivoBajaId.Value,
                    Descripcion = detalle.MotivoBajaDescripcion
                };
            FechaUltimaModificacion = detalle.FechaUltimaModificacion.Value;
            UsuarioUltimaModificacion = new Usuario
            {
                Id = detalle.UsuarioUltimaModificacionId,
                Cuil = detalle.UsuarioUltimaModificacionCuil
            };
            Abreviatura = detalle.Abreviatura;
            EsAutomatico = detalle.EsAutomatico == "S";
            Ambito = new Ambito((int) detalle.AmbitoId.Valor, detalle.AmbitoNombre);
            Codigo = detalle.Codigo;
        }

        public MotivoRechazo DarDeBaja(MotivoBaja motivo, Usuario usuario)
        {
            ValidarBaja();

            UsuarioUltimaModificacion = usuario;
            MotivoBaja = motivo;
            FechaUltimaModificacion = DateTime.Now;
            return this;
        }


        public string Nombre { get; protected set; }
        public string Descripcion { get; protected set; }
        public string Abreviatura { get; protected set; }
        public Ambito Ambito { get; protected set; }

        public virtual DateTime? FechaAlta { get; protected set; }
        public Usuario UsuarioAlta { get; protected set; }
        public virtual MotivoBaja MotivoBaja { get; protected set; }
        public virtual DateTime FechaUltimaModificacion { get; protected set; }
        public virtual Usuario UsuarioUltimaModificacion { get; protected set; }
        public bool EsAutomatico { get; set; }
        public bool EsPredefinido { get; set; }
        public string Observaciones { get; set; }
        public string Codigo { get; set; }

        private void ValidarBaja()
        {
            if (MotivoBaja != null && !MotivoBaja.Id.IsDefault())
            {
                throw new ModeloNoValidoException("El motivo rechazo ya está dado de baja.");
            }
        }

        public MotivoRechazo Modificar(string nombre, string descripcion, string abreviatura, bool esAutomatico,
            Ambito ambito, Usuario usuario, string codigo)
        {
            ValidarBaja();

            SetearCampos(nombre, descripcion, abreviatura, esAutomatico, ambito, usuario, codigo);
            return this;
        }

        private void SetearCampos(string nombre, string descripcion, string abreviatura, bool esAutomatico,
            Ambito ambito, Usuario usuario, string codigo)
        {
            ValidarIntegridadCampos(nombre, descripcion);

            Nombre = nombre;
            Descripcion = descripcion;
            FechaUltimaModificacion = DateTime.Now;
            UsuarioUltimaModificacion = usuario;
            Abreviatura = abreviatura;
            EsAutomatico = esAutomatico;
            Ambito = ambito;
            if (FechaAlta != null) return;
            FechaAlta = FechaUltimaModificacion;
            UsuarioAlta = UsuarioUltimaModificacion;
            Codigo = codigo;
        }

        private static void ValidarIntegridadCampos(string nombre, string descripcion)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                throw new ModeloNoValidoException("El Nombre del motivo destino es requerido.");
            }

            if (string.IsNullOrEmpty(descripcion))
            {
                throw new ModeloNoValidoException("La descripción del motivo destino es requerida.");
            }

            if (nombre.Length > 100)
            {
                throw new ModeloNoValidoException("El nombre no debe superar los 100 caracteres.");
            }

            if (descripcion.Length > 200)
            {
                throw new ModeloNoValidoException("La descripción no debe superar los 200 caracteres.");
            }
        }
    }
}