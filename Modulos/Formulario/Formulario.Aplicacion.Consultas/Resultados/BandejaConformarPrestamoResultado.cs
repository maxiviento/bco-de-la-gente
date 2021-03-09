using System;
using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class BandejaConformarPrestamoResultado
    {
        public Id Id { get; set; }
        public string Localidad { get; set; }
        public string Linea { get; set; }
        public Id IdLinea { get; set; }
        public string ApellidoNombreSolicitante { get; set; }
        public string CuilSolicitante { get; set; }
        public DateTime FechaEnvio { get; set; }
        public string Origen { get; set; }
        public string Estado { get; set; }
        public DatosPersonaResultado Solicitante { get; set; }
        public IList<DatosPersonaResultado> Garantes { get; set; }
        public Id NroAgrupamiento { get; set; }
        public Id TipoIntegranteSocio { get; set; }
        public string NombreIntegranteSocio { get; set; }
        public bool PuedeGenerarPrestamo { get; set; }
        public int NroFormulario { get; set; }
        public DateTime FechaEstado { get; set; }

    }
}
