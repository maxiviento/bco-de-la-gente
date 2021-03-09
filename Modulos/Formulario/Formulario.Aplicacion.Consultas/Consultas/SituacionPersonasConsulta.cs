using Infraestructura.Core.Comun.Presentacion;

namespace Formulario.Aplicacion.Consultas.Consultas
{
    public class SituacionPersonasConsulta : Consulta
    {
        public decimal TipoPersona { get; set; }
        public string Cuil { get; set; }
        public string Apellido { get; set; }
        public string Nombre{ get; set; }
        public string Dni { get; set; }
        public decimal? NumeroSticker { get; set; }
        public decimal? NumeroFormulario { get; set; }
        public decimal? NumeroPrestamo { get; set; }
    }
}
