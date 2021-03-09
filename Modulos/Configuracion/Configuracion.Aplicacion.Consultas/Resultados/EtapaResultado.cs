using System;
using Infraestructura.Core.Comun.Dato;

namespace Configuracion.Aplicacion.Consultas.Resultados
{
    public class EtapaResultado
    {
        public class Consulta
        {
            public Id Id { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public Id? IdMotivoBaja { get; set; }
            public DateTime? FechaUltimaModificacion { get; set; }
        }
    }
}