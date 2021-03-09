using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Comun.Presentacion;
using System;

namespace ApiBatch.Infraestructure
{
    public class InscripcionExportarLoteConsulta : Consulta
    {
        public Id Convocatoria { get; set; }
        public Id EstadoInscripcion { get; set; }
        public string NroExpediente { set; get; }
        public string NroSuac { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Genero { get; set; }
        public string NumeroDocumento { get; set; }
        public string Categoria { get; set; }
        public Id OrganizacionId { get; set; }
        public Id Departamento { get; set; }
        public Id Localidad { get; set; }
        public Id Barrio { get; set; }
        public Id LineaPobreza { get; set; }
        public Id EvaluacionFinal { get; set; }
        public Id EvaluacionSintys { get; set; }
        public Id EvaluacionSocioeconomica { get; set; }
        public decimal[] Servicios { set; get; }
        public decimal[] ServiciosOtorgados { set; get; }
        public Id Lote { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public string EntidadId { get; set; }
        public Id AccionId { get; set; }
        public bool IncluirSuspendidas { get; set; }
        public bool EsConsultaDeOrganizacion { get; set; }
        public string Cuit { get; set; }

    }
}