using System;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class DetalleLineaGrillaResultado
    {
        public Id Id { get; set; }
        public string NombreSocioIntegrante { get; set; }
        public int IdSocioIntegrante { get; set; }
        public int? MontoTope { get; set; }
        public int MontoPrestable { get; set; }
        public int CantidadMaximaIntegrantes { get; set; }
        public int CantidadMinimaIntegrantes { get; set; }
        public string NombreTipoFinanciamiento { get; set; }
        public string IdTipoFinanciamiento { get; set; }
        public int PlazoDevolucion { get; set; }
        public string ValorCuotaSolidaria { get; set; }
        public string NombreTipoGarantia { get; set; }
        public int IdTipoGarantia { get; set; }
        public DateTime? FechaBaja { get; set; }
        public Id IdMotivoBaja { get; set; }
        public int IdTipoInteres { get; set; }
        public string NombreMotivoBaja { get; set; }
        public bool Apoderado { get; set; }
        public string NombreConvPag { get; set; }
        public string NombreConvRec { get; set; }
        public decimal CodConvenioPag { get; set; }
        public decimal CodConvenioRec { get; set; }
    }
}