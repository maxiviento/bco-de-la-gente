using System;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class LineaPrestamoGrillaResultado
    {
        public Id Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string NombreMotivoDestino { get; set; }
        public bool ConOng { get; set; }
        public bool ConPrograma { get; set; }
        public bool ConCurso { get; set; }
        public bool TrabajaConLocalidad { get; set; }
        public Id IdMotivoBaja { get; set; }
        public DateTime? FechaBaja { get; set; }
    }
}