﻿using System;
using Infraestructura.Core.Comun.Dato;

namespace Configuracion.Aplicacion.Consultas.Resultados
{
    public class MotivoDestinoResultado
    {
        public class Combo
        {
            public Id Id { get; set; }
            public string Descripcion { get; set; }

        }
        public class Detallado
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
        }

        public class Grilla
        {
            public Id Id { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public Id? IdMotivoBaja { get; set; }
            public DateTime? FechaUltimaModificacion { get; set; }
        }
    }
}