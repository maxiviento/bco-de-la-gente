using System;
using System.Collections.Generic;
using AppComunicacion.ApiModels;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class LineaPrestamoResultado
    {
        public Id Id { get; set; }
        public bool ConOng { get; set; }
        public bool ConPrograma { get; set; }
        public bool ConCurso { get; set; }
        public string IdPrograma { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string NombreSexoDestinatario { get; set; }
        public string IdSexoDestinatario { get; set; }
        public string NombreMotivoDestino { get; set; }
        public string IdMotivoDestino { get; set; }
        public string Objetivo { get; set; }
        public string Color { get; set; }
        public DateTime? FechaBaja { get; set; }
        public string NombreMotivoBaja { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
        public string NombreUsuario { get; set; }
        public string Logo { get; set; }
        public string PiePagina { get; set; }
        public string NombrePrograma { get; set; }
        public bool DeptoLocalidad { get; set; }
    }
}