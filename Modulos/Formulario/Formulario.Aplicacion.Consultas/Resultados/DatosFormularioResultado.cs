using System;
using System.Collections.Generic;
using Formulario.Aplicacion.Comandos;

namespace Formulario.Aplicacion.Consultas.Resultados
{
    public class DatosFormularioResultado
    {
        public int Id { get; set; }
        public int IdEstado { get; set; }
        public int IdOrigen { get; set; }
        public DetalleLineaParaFormularioResultado DetalleLinea { get; set; }
        public DatosPersonaResultado Solicitante { get; set; }
        public IEnumerable<SolicitudCursoResultado> SolicitudesCurso { get; set; }
        public OpcionDestinosFondoResultado DestinosFondos { get; set; }
        public CondicionesSolicitadasResultado CondicionesSolicitadas { get; set; }
        public IEnumerable<DatosPersonaResultado> Garantes { get; set; }
        public EmprendimientoResultado DatosEmprendimiento { get; set; }
        public IEnumerable<MiembroEmprendimientoFormularioResultado> MiembrosEmprendimiento { get; set; }
        public string MotivoRechazo { get; set; }
        public string Observaciones { get; set; }
        public string NroSticker { get; set; }
        public int NroFormulario { get; set; }
        public string ObservacionPrestamo { get; set; }
        public string NombreSucursalBancaria { get; set; }
        public string DomicilioSucursalBancaria { get; set; }
        public DateTime? FechaInicioPagos { get; set; }
        public DateTime? FechaPrimerVencimientoPago { get; set; }
        public DateTime? FechaVencimientoPlanPago { get; set; }
        public PatrimonioSolicitanteResultado PatrimonioSolicitante { get; set; }
        public IList<DeudaEmprendimientoResultado> DeudasEmprendimiento { get; set; }
        public IList<InversionRealizadaResultado> InversionesRealizadas { get; set; }
        public IEnumerable<IngresoGrupoResultado> IngresosGrupo { get; set; }
        public MercadoComercializacionResultado MercadoComercializacion { get; set; }
        public NecesidadInversionResultado NecesidadInversion { get; set; }
        public PrecioVentaResultado PrecioVenta { get; set; }
        public DateTime FechaForm { get; set; }
        public decimal IdAgrupamiento { get; set; }
        public IEnumerable<AgruparFormulario> Integrantes { get; set; }
        public OngFormulario DatosONG { get; set; }
        public string NumeroCaja { get; set; }
        public int? TipoApoderado { get; set; }
        public string Destino { get; set; }

        public string AgruparDestinosFondos()
        {
            string res = "";
            if (DestinosFondos == null || DestinosFondos.DestinosFondo.Count == 0) return res;

            for (int i = 0; i < DestinosFondos.DestinosFondo.Count; i++)
            {
                res += DestinosFondos.DestinosFondo[i].Descripcion;
                if (i != DestinosFondos.DestinosFondo.Count - 1) res += ", ";
            }

            return res;
        }
    }
}