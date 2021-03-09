using Infraestructura.Core.Comun.Dato;
using System;

namespace Configuracion.Aplicacion.Consultas.Resultados
{
    public class ItemGrillaResultado
    {
        public Id Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Id? IdMotivoBaja { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
    }
}