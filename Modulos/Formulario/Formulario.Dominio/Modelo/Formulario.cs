using Configuracion.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Formulario.Dominio.Modelo
{
    public class Formulario : Entidad
    {
        //Campos propios del formulario
        public virtual DetalleLineaPrestamo DetalleLineaPrestamo { get; protected set; }

        public virtual Persona Solicitante { get; protected set; }
        public virtual IList<Persona> Garantes { get; protected set; }
        public virtual OrigenFormulario OrigenFormulario { get; protected set; }
        public virtual DateTime FechaAlta { get; protected set; }
        public virtual Usuario UsuarioAlta { get; protected set; }
        public virtual MotivoBaja MotivoBaja { get; protected set; }
        public virtual MotivoRechazo MotivoRechazo { get; protected set; }
        public virtual string NumeroCaja { get; set; }
        //public virtual decimal IdAgrupamiento { get; set; } REVISAR

        //Campos correspondientes a cuadrantes del formulario
        public virtual CondicionesPrestamo CondicionesPrestamo { get; protected set; }

        public virtual IList<SolicitudCurso> CursosSolicitados { get; protected set; }
        public virtual OpcionDestinoFondos OpcionDestinoFondos { get; protected set; }

        public virtual IList<MotivoRechazo> MotivosRechazo { get; set; }
        public virtual string ObservacionesRechazo { get; set; }
        public virtual string IdBanco { get; protected set; }
        public virtual string IdSucursal { get; protected set; }
        public virtual PatrimonioSolicitante PatrimonioSolicitante { get; protected set; }
        public virtual IList<IngresoGrupo> IngresosGrupo { get; protected set; }
        public virtual Id IdAgrupamiento { get; set; }
        public virtual int? EsApoderado { get; set; }
        public virtual bool TitularSolicitante { get; set; }
        public virtual DateTime FechaForm { get; protected set; }

        protected Formulario()
        {
        }

        public Formulario(Persona solicitante)
        {
            RegistrarDatosIniciales(solicitante);
        }

        public Formulario(Id id, DetalleLineaPrestamo detalleLineaPrestamo, Persona solicitante,
            OrigenFormulario origenFormulario, Usuario usuarioAlta, DateTime fechaForm)
            : this(solicitante)
        {
            Id = id;
            DetalleLineaPrestamo = detalleLineaPrestamo;
            OrigenFormulario = origenFormulario;
            UsuarioAlta = usuarioAlta;
            FechaForm = fechaForm;
        }

        public Formulario(Id id, Usuario usuarioBaja)
        {
            Id = id;
            UsuarioAlta = usuarioBaja;
        }

        public Formulario(Id id, string idBanco, string idSucursal)
        {
            Id = id;
            IdBanco = idBanco;
            IdSucursal = idSucursal;
        }


        public Formulario(Id id, MotivoRechazo motivoRechazo, Usuario usuarioBaja)
        {
            Id = id;
            MotivoRechazo = motivoRechazo;
            UsuarioAlta = usuarioBaja;
        }

        public Formulario(Id id, string numeroCaja, Usuario usuarioBaja)
        {
            Id = id;
            NumeroCaja = numeroCaja;
            UsuarioAlta = usuarioBaja;
        }

        public Formulario(Id id, MotivoBaja motivoBaja, Usuario usuarioBaja)
        {
            Id = id;
            MotivoBaja = motivoBaja;
            UsuarioAlta = usuarioBaja;
        }

        public Formulario(Id id, int esApoderado, Usuario usuarioAlta)
        {
            Id = id;
            EsApoderado = esApoderado;
            UsuarioAlta = usuarioAlta;
        }

        private void RegistrarDatosIniciales(Persona solicitante)
        {
            //resharper no entiende esto por alguna razon
            if (solicitante == null)
                throw new ModeloNoValidoException("No se puede generar un formulario sin persona asociada");
            Solicitante = solicitante;
        }

        private void RegistrarCursosSolicitaos(IList<SolicitudCurso> cursosSolicitados)
        {
            if (cursosSolicitados?[0]?.Cursos?[0] == null)
                throw new ModeloNoValidoException("Debe seleccionar al menos un curso de capacitación");
            CursosSolicitados = cursosSolicitados;
        }

        public string GetMotivosRechazoString()
        {
            if (MotivosRechazo == null) return null;

            var lista = MotivosRechazo.Select((motivo) => (motivo.Id.ToString() + " , " + motivo.Observaciones)).ToList();
            return string.Join(";", lista);
        }

        public Formulario(PatrimonioSolicitante patrimonio)
        {
            PatrimonioSolicitante = patrimonio;
        }
    }
}
