using System;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class SituacionPersonasResultadoVista
    {
        public decimal? IdNumero { get; set; }
        public string NombreApellido { get; set; }
        public string Sexo { get; set; }
        public string NumeroDocumento { get; set; }
        public string NumeroSolicitud { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal? IdProducto { get; set; }
        public string Producto { get; set; }
        public string Condicion { get; set; }
        public decimal? MontoAdeudado { get; set; }
        public decimal? MontoCredito { get; set; }
        public decimal? MontoAbonado { get; set; }
    }
}
