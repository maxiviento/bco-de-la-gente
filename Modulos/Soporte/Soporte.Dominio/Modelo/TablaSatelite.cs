using System;
using Infraestructura.Core.Comun.Dato;

namespace Soporte.Dominio.Modelo
{
    public class TablaSatelite : Entidad
    {
        public string Nombre { get; set; }
        public DateTime FechaDesde { get; set; }
        public string Descripcion { get; set; }
    }
}
