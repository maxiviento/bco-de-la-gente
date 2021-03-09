using System;

namespace Soporte.Dominio.Modelo
{
    public class CabeceraHistorialDeudaGrupo
    {
        public int Id { get; set; }
        public DateTime FechaConsulta { get; set; }
        public string NroPrestamo { get; set; }
        public string NombreLinea { get; set; }
        public string NombreCompleto { get; set; }
        public string NroDocumento { get; set; }
        public string NroSticker { get; set; }
        public string Domicilio { get; set; }
        public string NroGrupoConviviente { get; set; }
        public string Localidad { get; set; }
        public string Departamento { get; set; }
        public bool EsSolicitante { get; set; }
    }
}
