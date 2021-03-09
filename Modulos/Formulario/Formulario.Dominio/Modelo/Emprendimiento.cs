using System;
using System.Collections;
using System.Collections.Generic;
using Formulario.Aplicacion.Consultas.Resultados;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class Emprendimiento : Entidad
    {
        protected Emprendimiento()
        {
        }

        public Emprendimiento(int idVinculo, string email, int nroCodArea, long nroTelefono,
            int idTipoInmueble, int idTipoProyecto, int idSectorDesarrollo,
            DateTime? fechaAtivacion, bool? tieneExperiencia, string tiempoExperiencia,
            Actividad actividad, bool? hizoCursos, string cursosInteres,
            bool? pidioCredito, bool? creditoFueOtorgado, string institucionSolicitante, IList<MiembroEmprendimiento> miembros, int idTipoOrganizacion)
        {
            IdVinculo = idVinculo != 0 ? idVinculo : -1;
            Email = email;
            NroCodArea = nroCodArea;
            NroTelefono = nroTelefono;
            TipoInmueble = new TipoInmueble(idTipoInmueble != 0 ? idTipoInmueble : -1);
            TipoProyecto = new TipoProyecto(idTipoProyecto != 0 ? idTipoProyecto : -1);
            SectorDesarrollo = new SectorDesarrollo(idSectorDesarrollo != 0 ? idSectorDesarrollo : -1);
            FechaActivacion = fechaAtivacion ?? default(DateTime);
            TieneExperiencia = tieneExperiencia;
            TiempoExperiencia = tiempoExperiencia;
            Actividad = actividad;
            HizoCursos = hizoCursos;
            CursosInteres = cursosInteres;
            PidioCredito = pidioCredito;
            CreditoFueOtorgado = creditoFueOtorgado;
            InstitucionSolicitante = institucionSolicitante;
            Miembros = miembros;
            TipoOrganizacion = new TipoOrganizacion(idTipoOrganizacion != 0 ? idTipoOrganizacion : -1);
        }

        public Emprendimiento(EmprendimientoResultado resultado, int idTipoOrganizacion = -1)
        {
            //IdVinculo = null;
            Id = resultado.Id;
            Email = resultado.Email;
            NroCodArea = resultado.NroCodArea ?? 0;
            NroTelefono = resultado.NroTelefono ?? 0;
            TipoInmueble = new TipoInmueble(resultado.IdTipoInmueble);
            TipoProyecto = new TipoProyecto(resultado.IdTipoProyecto);
            SectorDesarrollo = new SectorDesarrollo(resultado.IdSectorDesarrollo);
            FechaActivacion = resultado.FechaActivo ?? default(DateTime);
            TieneExperiencia = resultado.TieneExperiencia;
            TiempoExperiencia = resultado.TiempoExperiencia;
            Actividad = new Actividad(resultado.IdActividad);
            HizoCursos = resultado.HizoCursos;
            CursosInteres = resultado.CursoInteres;
            PidioCredito = resultado.PidioCredito;
            CreditoFueOtorgado = resultado.CreditoFueOtorgado;
            InstitucionSolicitante = resultado.InstitucionSolicitante;
            //Miembros = null;
            TipoOrganizacion = new TipoOrganizacion(idTipoOrganizacion != 0 ? idTipoOrganizacion : -1);
        }

        public Emprendimiento(int idEmprendimiento, int idTipoOrganizacion)
        {
            Id = new Id(idEmprendimiento);
            IdVinculo = -1;
            Email = null;
            NroCodArea = -1;
            NroTelefono = -1;
            TipoInmueble = new TipoInmueble(-1);
            TipoProyecto = new TipoProyecto(-1);
            SectorDesarrollo = new SectorDesarrollo(-1);
            FechaActivacion = default(DateTime);
            TieneExperiencia = null;
            TiempoExperiencia = null;
            Actividad = new Actividad(-1, null, null, null);
            HizoCursos = null;
            CursosInteres = null;
            PidioCredito = null;
            CreditoFueOtorgado = null;
            InstitucionSolicitante = null;
            Miembros = null;
            TipoOrganizacion = new TipoOrganizacion(idTipoOrganizacion != 0 ? idTipoOrganizacion : -1);
        }

        public virtual int IdVinculo { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual int NroCodArea { get; protected set; }
        public virtual long NroTelefono { get; protected set; }
        public virtual TipoInmueble TipoInmueble { get; protected set; }
        public virtual TipoProyecto TipoProyecto { get; protected set; }
        public virtual SectorDesarrollo SectorDesarrollo { get; protected set; }
        public virtual DateTime? FechaActivacion { get; protected set; }
        public virtual bool? TieneExperiencia { get; protected set; }
        public virtual string TiempoExperiencia { get; protected set; }
        public virtual Actividad Actividad { get; protected set; }
        public virtual Rubro Rubro { get; protected set; }
        public virtual bool? HizoCursos { get; protected set; }
        public virtual string CursosInteres { get; protected set; }
        public virtual bool? PidioCredito { get; protected set; }
        public virtual bool? CreditoFueOtorgado { get; protected set; }
        public virtual string InstitucionSolicitante { get; protected set; }
        public virtual IList<MiembroEmprendimiento> Miembros { get; protected set; }
        public virtual TipoOrganizacion TipoOrganizacion { get; protected set; }
    }
}