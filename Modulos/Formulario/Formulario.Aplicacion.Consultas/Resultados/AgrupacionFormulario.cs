using System;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class AgruparFormulario
    {
        public Id Id { get; set; }
        public Id IdLinea { get; set; }
        public string NombreLinea { get; set; }
        public DateTime FechaBGE { get; set; }
        public string Origen { get; set; }
        public Id IdFormulario { get; set; }
        public string Pais { get; set; }
        public string NroDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public int? IdNumero { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string CodigoArea { get; set; }
        public string TelFijo { get; set; }
        public string CodigoAreaCelular { get; set; }
        public string TelCelular { get; set; }
        public string Mail { get; set; }
        public int? IdEstado { get; set; }
        public string Sexo { get; set; }
        public decimal? MinIntegrantes { get; set; }
        public decimal? MaxIntegrantes { get; set; }
        public int EsApoderado { get; set; }
        public bool TitularSolicitante { get; set; }
        public string Estado { get; set; }
        public string ConvPago { get; set; }
        public string ConvRecupero { get; set; }
        public string NroFormulario { get; set; }
        public string Cuil { get; set; }
    }
}
