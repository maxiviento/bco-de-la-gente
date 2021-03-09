using System;
using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Comandos
{
    public class RegistrarFormularioComando
    {
        public int? Id { get; set; }
        public int IdOrigen { get; set; }
        public DateTime FechaForm { get; set; }
        public DetalleLineaParaFormularioComando DetalleLinea { get; set; }
        public DatosPersonaComando Solicitante { get; set; }
        public IList<SolicitudCursoComando> SolicitudesCurso { get; set; }
        public OpcionDestinoFondosComando DestinosFondos { get; set; }
        public CondicionesSolicitadasComando CondicionesSolicitadas { get; set; }
        public IEnumerable<DatosPersonaComando> Garantes { get; set; }
        public IEnumerable<DatosIntegranteComando> Integrantes { get; set; }
        public Id IdAgrupamiento { get; set; }
    }
}