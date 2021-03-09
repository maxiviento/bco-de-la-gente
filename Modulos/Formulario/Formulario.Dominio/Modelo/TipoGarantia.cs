using System;
using Configuracion.Dominio.Modelo;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.Modelo
{
    public class TipoGarantia : Entidad
    {
        public virtual string Descripcion { get; set; }
        public virtual DateTime FechaAlta { get; set; }
        public virtual DateTime FechaUltimaModificacion { get; set; }
        public virtual MotivoBaja MotivoBaja { get; set; }
        public virtual string Nombre { get; set; }
        public virtual Usuario UsuarioAlta { get; set; }
        public virtual Usuario UsuarioUltimaModificacion { get; set; }

        public TipoGarantia() { }

        public TipoGarantia(int id, string nombre)
        {
            this.Id = new Id(id);
            this.Nombre = nombre;
        }

        public static TipoGarantia Solicitante => new TipoGarantia(1, "SOLICITANTE");
        public static TipoGarantia Terceros => new TipoGarantia(2, "TERCEROS");
        public static TipoGarantia SocioIntegrante => new TipoGarantia(3, "SOCIO INTEGRANTE");
        public static TipoGarantia SolicitanteYTercero => new TipoGarantia(4, "SOLICITANTE Y TERCERO");
    }
}
