using System;
using System.Collections.Generic;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class PrestamoResultado
    {
        public class Detallado
        {
            public decimal Id { get; set; }
            public string Version { get; set; }
            public int TotalFolios { get; set; }
            public DateTime FechaAlta { get; set; }
            public IList<RequisitoResultado> Requisitos { get; set; }
        }

        public class Integrante
        {
            public string ApellidoNombre { get; set; }
            public string SexoId { get; set; }
            public string CodigoPais { get; set; }
            public string NroDocumento { get; set; }
            public string Cuil { get; set; }
            public string EstadoFormulario { get; set; }
            public string NroSticker { get; set; }
            public int NroAgrupamiento { get; set; }
            public string Departamento { get; set; }
            public string Localidad { get; set; }
            public string OrigenFormulario { get; set; }
            public string NroLinea { get; set; }
            public string IdFormulario { get; set; }
            public DateTime? FechaNacimiento { get; set; }
            public string NombreBanco { get; set; }
            public string NombreSucursal { get; set; }
            public string NroFormulario { get; set; }
            public string MotivoRechazo { get; set; }
            public string NumeroCaja { get; set; }
            public int TipoIntegrante { get; set; }
            public string NumDevengado { get; set; }
        }

        public class Datos
        {
            public string Id { get; set; }
            public string NumeroPrestamo { get; set; }
            public string NumDevengado { get; set; }
            public string TotalFolios { get; set; }
            public string IdEstado { get; set; }
            public bool EsSolicGarante { get; set; }
            public string Observaciones { get; set; }
            public DateTime FechaAlta { get; set; }
            public string Color { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public string MotivosRechazo { get; set; }
            public int IdTipoIntegrante { get; set; }
            public int IdEtapaEstadoLinea { get; set; }
            public int IdLinea { get; set; }
        }

        public class Seguimiento
        {
            public string EstadoNombre { get; set; }
            public string UsuarioCuil { get; set; }
            public DateTime Fecha { get; set; }
            public string Observaciones { get; set; }
            public int NroFormulario { get; set; }
        }

        public class EncabezadoArchivos
        {
            public int NroPrestamo { get; set; }
            public string NombreLinea { get; set; }
            public string NroSticker { get; set; }
            public string NombreEstadoPrestamo { get; set; }
            public string NombreOrigenPrestamo { get; set; }
        }

        public class Garante
        {
            public string SexoId { get; set; }
            public string CodigoPais { get; set; }
            public string NroDocumento { get; set; }
            public DateTime? FechaNacimiento { get; set; }
            public int? Numero { get; set; }
            public string Apellido { get; set; }
            public string Nombre { get; set; }
            public string Cuil { get; set; }
            public string NroFormulario { get; set; }
            public string Departamento { get; set; }
            public string Localidad { get; set; }
        }
    }
}