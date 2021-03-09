using System;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class DetalleLineaResultado
    {
        public Id Id { get; set; }
        public Id LineaId { get; set; }
        public string NombreLinea { get; set; }
        public string NombreSocioIntegrante { get; set; }
        public int MontoTope { get; set; }
        public int? MontoPrestable { get; set; }
        public int CantidadMaximaIntegrantes { get; set; }
        public int CantidadMinimaIntegrantes { get; set; }
        public string NombreTipoFinanciamiento { get; set; }
        public string NombreTipoInteres { get; set; }
        public int PlazoDevolucion { get; set; }
        public string ValorCuotaSolidaria { get; set; }
        public string NombreTipoGarantia { get; set; }
        public DateTime? FechaBaja { get; set; }
        public Id IdMotivoBaja { get; set; }
        public string NombreMotivoBaja { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
        public string NombreUsuario { get; set; }
        public string IdSocioIntegrante { get; set; }
        public string IdTipoFinanciamiento { get; set; }
        public string IdTipoGarantia { get; set; }
        public string IdTipoInteres { get; set; }
        public string Visualizacion { get; set; }
        public bool Apoderado{ get; set; }
        public decimal? CodConvenioPag { get; set; }
        public decimal? CodConvenioRec { get; set; } //Tiene q ser obligatorio
        public string NombreConvPag { get; set; }
        public string NombreConvRec { get; set; }


    }
}