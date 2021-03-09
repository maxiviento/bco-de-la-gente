using Infraestructura.Core.Comun.Dato;

namespace Soporte.Dominio.Modelo
{
    public class DatoRenta: Entidad
    {
        public string Objeto { get; set; }
        public string Tipo { get; set; }
        public int? Anio { get; set; }
        public string Marca { get; set; }
        public decimal? BaseImponible { get; set; }
        public decimal? Porcentaje { get; set; }
        public string Estado { get; set; }
        public decimal? Superficie { get; set; }
        public string Domicilio { get; set; }
    }
}