using System;

namespace Soporte.Dominio.Modelo
{
    public class HistorialRentas
    {
        public string IdUsuario { get; set; }
        public DateTime? Fecha { get; set; }
        public string IdPrestamo { get; set; }
        public string NroDocumento { get; set; }
        public string Objeto { get; set; }
        public int Modelo { get; set; }
        public string Marca { get; set; }
        public int BaseImponible { get; set; }
        public string Porcentaje { get; set; }
        public string Estado { get; set; }
        public string Superficie { get; set; }
        public string Domicilio { get; set; }
        public bool EsSolicitante { get; set; }
        public int IdNumero { get; set; }
        public string IdSexo { get; set; }
        public string IdPais { get; set; }
        public string IdFormularioLinea { get; set; }
    }
}
