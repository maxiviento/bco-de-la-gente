using System;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class FormularioFiltradoResultado
    {
        public Id IdFormulario { get; set; }
        public int? NroFormulario { get; set; }
        public int? NroPrestamo { get; set; }
        public string ApellidoNombreSolicitante { get; set; }
        public string Localidad { get; set; }
        public string Departamento { get; set; }
        public string Linea { get; set; }
        public string CuilSolicitante { get; set; }
        public string Banco { get; set; }
        public string Sucursal { get; set; }
        public int? IdSucursal { get; set; }
        public int? IdBanco { get; set; }
        public int? IdEstadoFormulario { get; set; }
        public DateTime? FechFinPago { get; set; }
        public DateTime? FechaUltimoMovimiento { get; set; }
        public decimal? MonPres { get; set; }
        public int? CanCuot { get; set; }
        public int? TipoApoderado { get; set; }
    }
}