using Infraestructura.Core.Comun.Dato;
using System;
using System.Collections.Generic;
using Infraestructura.Core.Comun.Presentacion;

namespace Configuracion.Aplicacion.Consultas.Resultados
{
    public class ItemDetalleResultado
    {
        public Id Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Id IdUsuarioAlta { get; set; }
        public string CuilUsuarioAlta { get; set; }
        public Id? IdMotivoBaja { get; set; }
        public string NombreMotivoBaja { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
        public Id IdUsuarioUltimaModificacion { get; set; }
        public string CuilUsuarioUltimaModificacion { get; set; }
        public IList<TipoItemResultado> TiposItem { get; set; }
    }
}