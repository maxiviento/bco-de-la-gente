using System.Collections.Generic;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class ExportacionPrestamo
    {
        public int Dni { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Departamento { get; set; }
        public string Localidad { get; set; }
        public string NombreLinea { get; set; }
        public string Origen { get; set; }
        public int NroFormulario { get; set; }
        public string AltaFormulario { get; set; }
        public int Dia { get; set; }

        public int Mes { get; set; }

        public int Anio { get; set; }

        public int Monto { get; set; }
        public int CantidadCuotas { get; set; }
        public int CantidadPagas { get; set; }
        public int MontoPagado { get; set; }
        public int CantidadNoPagas { get; set; }
        public int MontoNoPagado { get; set; }
        public int CantidadVencidas { get; set; }
        public int MontoVencidas { get; set; }
        public string EstadoFormulario { get; set; }
        public int NroPrestamo { get; set; }
        public string EstadoPrestamo { get; set; }
        public string EsApoderado { get; set; }
    }
}