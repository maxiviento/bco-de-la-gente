using System;

namespace Soporte.Dominio.Modelo
{
    public class CabeceraHistorialSintys
    {
        public string IdUsuario { get; set; }
        public DateTime FechaAlta { get; set; }
        public string NroPrestamo { get; set; }
        public string IdPrestamo { get; set; }
        public string NroLinea { get; set; }
        public int IdFormularioLinea { get; set; }
    }
}
