using System;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;

namespace Configuracion.Dominio.Modelo
{
    public class VersionChecklist : Entidad
    {
        public string Version { get; set; }
        public DateTime? FechaAlta { get; set; }
        public Usuario UsuarioAlta { get; set; }
        public MotivoBaja MotivoBaja { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
        public Usuario UsuarioUltimaModificacion { get; set; }
    }
}
