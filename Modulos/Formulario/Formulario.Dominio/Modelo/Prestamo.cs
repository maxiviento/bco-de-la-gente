using System;
using System.Collections.Generic;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Excepciones;

namespace Formulario.Dominio.Modelo
{
    public class Prestamo : Entidad
    {
        protected Prestamo()
        {
        }

        public Prestamo(long totalFolios, IList<SeguimientoPrestamo> seguimientos):this(null, totalFolios, null, null, seguimientos)
        {
        }
        public Prestamo(Id id, long totalFolios, IList<SeguimientoPrestamo> seguimientos) : this(null, totalFolios, null, null, seguimientos)
        {
            this.Id = id;
        }

        public Prestamo(Id id, MotivoRechazo motivoRechazo, Usuario usuarioModifiacion)
        {
            this.Id = id;
            this.MotivoRechazo = motivoRechazo;
            this.UsuarioModificacion = usuarioModifiacion;
        }

        public Prestamo(Id id)
        {
            this.Id = id;
         }

        public Prestamo( string version, long totalFolios, Usuario usuario, IList<DetallePrestamo> detalles, IList<SeguimientoPrestamo> seguimientos)
        {
            if (totalFolios == null || totalFolios < 0)
                throw new ModeloNoValidoException("No se puede generar un préstamo sin el total de folios");
            this.Version = version;
            this.TotalFolios = totalFolios;
            this.UsuarioAlta = usuario;
            this.Detalles = detalles;
            this.Seguimientos = seguimientos;
        }

        public Prestamo(Id id, Usuario usuarioBaja, string numeroCaja)
        {
            this.Id = id;
            UsuarioModificacion = usuarioBaja;
            NumeroCaja = numeroCaja;
        }

        public virtual string Version { get; protected set; }
        public virtual long TotalFolios { get; protected set; }
        public virtual DateTime FechaAlta { get; protected set; }
        public virtual Usuario UsuarioAlta { get; protected set; }
        public virtual int IdMotivoBaja { get; protected set; }
        public virtual IList<DetallePrestamo> Detalles { get; protected set; }
        public virtual IList<SeguimientoPrestamo> Seguimientos { get; protected set; }
        public virtual MotivoRechazo MotivoRechazo { get; protected set; }
        public virtual string NumeroCaja { get; set; }
        public virtual Usuario UsuarioModificacion { get; protected set; }

        public virtual IList<MotivoRechazo> MotivosRechazo { get; set; }
        public virtual string ObservacionesRechazo { get; set; }

        public string GetMotivosRechazoString()
        {
            List<string> lista = new List<string>();
            foreach (var motivo in MotivosRechazo)
            {
                lista.Add(motivo.Id.ToString() + "," + motivo.Observaciones);
            }
            return string.Join(";", lista);
        }
    }
}