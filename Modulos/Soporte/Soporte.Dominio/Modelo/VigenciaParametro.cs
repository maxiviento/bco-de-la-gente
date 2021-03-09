using System;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;

namespace Soporte.Dominio.Modelo
{
    public class VigenciaParametro: Entidad
    {
        public VigenciaParametro() { }

        public VigenciaParametro(DateTime? fechaDesde, string valor, long? idParametro, long? idVigencia)
        {
            IdParametro = idParametro;
            FechaDesde = fechaDesde;
            Valor = valor;
            IdVigenciaParametro = idVigencia;
        }

        public virtual long? IdVigenciaParametro { get; set; }
        public virtual DateTime? FechaDesde { get; set; }
        public virtual DateTime? FechaHasta { get; set; }
        public virtual string Valor { get; set; }
        public virtual DateTime? FechaAlta { get; set; }
        public virtual Usuario UsuarioAlta { get; set; }
        public virtual Usuario UsuarioModificacion { get; set; }
        public virtual long? IdParametro { get; set; }
        public virtual DateTime FechaModificacion { get; protected set; }
    }
}
