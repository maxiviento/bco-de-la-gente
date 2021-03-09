using Infraestructura.Core.Comun.Presentacion;

namespace Formulario.Aplicacion.Consultas.Consultas
{
    public class FiltrosFormularioConsulta : Consulta
    {
        public int? IdLote { get; set; }
        public int? IdPrestamo { get; set; }
        public int? IdFormularioLinea { get; set; }
        public int? IdPrestamoItem { get; set; }
        public int? IdDepartamento { get; set; }
        public int? IdLocalidad { get; set; }
        public long? NroPrestamo { get; set; }
        public long? NroFormulario { get; set; }
        public string Dni { get; set; }
        public string Cuil { get; set; }
    }
}