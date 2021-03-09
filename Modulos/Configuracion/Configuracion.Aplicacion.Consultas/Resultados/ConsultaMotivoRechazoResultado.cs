using System;
using Identidad.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;

namespace Configuracion.Aplicacion.Consultas.Resultados
{
    public class ConsultaMotivoRechazoResultado
    {
        public class Grilla
        {
            public Id Id { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public Id? IdMotivoBaja { get; set; }
            public DateTime? FechaUltimaModificacion { get; set; }
            public Ambito Ambito { get; set; }
            public string Abreviatura { get; set; }
            public bool EsAutomatico { get; set; }
            public bool EsPredefinido { get; set; }
            public string Codigo { get; set; }
        }

        public class GrillaBD
        {
            public Id Id { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public Id? IdMotivoBaja { get; set; }
            public DateTime? FechaUltimaModificacion { get; set; }
            public string NombreAmbito { get; set; }
            public decimal? IdAmbito { get; set; }
            public string Abreviatura { get; set; }
            public string EsAutomatico { get; set; }
            public string Codigo { get; set; }
        }

        public class Detallado
        {
            public Id Id { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public Id IdUsuarioAlta { get; set; }
            public string CuilUsuarioAlta { get; set; }
            public Id? MotivoBajaId { get; set; }
            public string MotivoBajaDescripcion { get; set; }
            public DateTime? FechaUltimaModificacion { get; set; }
            public Id UsuarioUltimaModificacionId { get; set; }
            public string UsuarioUltimaModificacionCuil { get; set; }
            public string Abreviatura { get; set; }
            public string EsAutomatico { get; set; }
            public Id AmbitoId { get; set; }
            public string AmbitoNombre { get; set; }
            public string Codigo { get; set; }
        }
    }
}
