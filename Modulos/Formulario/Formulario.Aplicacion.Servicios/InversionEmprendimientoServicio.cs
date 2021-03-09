using System.Collections.Generic;
using Formulario.Aplicacion.Comandos;
using Formulario.Aplicacion.Consultas.Resultados;
using Formulario.Dominio.IRepositorio;
using Formulario.Dominio.Modelo;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Aplicacion.Servicios
{
    public class InversionEmprendimientoServicio
    {
        private readonly IInversionEmprendimientoRepositorio _inversionEmprendimientoRepositorio;
        private readonly DetalleInversionEmprendimientoServicio _detalleInversionEmprendimientoServicio;

        public InversionEmprendimientoServicio(IInversionEmprendimientoRepositorio inversionEmprendimientoRepositorio,
            DetalleInversionEmprendimientoServicio detalleInversionEmprendimientoServicio)
        {
            _inversionEmprendimientoRepositorio = inversionEmprendimientoRepositorio;
            _detalleInversionEmprendimientoServicio = detalleInversionEmprendimientoServicio;
        }

        public NecesidadInversionResultado ObtenerNecesidadInversionPorIdEmprendimiento(Id idEmprendimiento)
        {
            var necesidadInversion =
                _inversionEmprendimientoRepositorio.ObtenerNecesidadInversionPorIdEmprendimiento(idEmprendimiento);

            if (necesidadInversion != null)
            {
                necesidadInversion.InversionesRealizadas = _detalleInversionEmprendimientoServicio
                    .ObtenerDetallesNecesidadInversionPorIdEmprendimiento(idEmprendimiento);
            }

            return necesidadInversion;
        }

        public void RegistrarNecesidadInversion(Id idEmprendimiento, RegistrarNecesidadInversionComando comando)
        {
            if (comando.ValidarRegistroNecesidadInversion())
            {
                _inversionEmprendimientoRepositorio.RegistrarNecesidadInversion(idEmprendimiento,
                comando.IdFuenteFinanciamiento, comando.MontoMicroprestamo ?? 0, comando.MontoCapitalPropio ?? 0,
                comando.MontoOtrasFuentes ?? 0);

                // Registra los detalles sólo si pudo registrar la necesidad de inversión
                if (comando.InversionesRealizadas != null)
                    _detalleInversionEmprendimientoServicio.RegistrarDetalleInversionEmprendimiento(idEmprendimiento,
                    comando.InversionesRealizadas, TipoInversion.NecesidadInversion.Id);
            }
        }

        public NecesidadInversionResultado ObtenerNecesidadInversionParaReporte(Id idEmprendimiento, int orden)
        {
            var necesidadInversion = new NecesidadInversionResultado();

            if (!idEmprendimiento.IsDefault())
            {
                necesidadInversion = _inversionEmprendimientoRepositorio
                    .ObtenerNecesidadInversionPorIdEmprendimiento(idEmprendimiento);
                if (necesidadInversion != null)
                {
                    necesidadInversion.InversionesRealizadas =
                        _detalleInversionEmprendimientoServicio.ObtenerDetallesNecesidadInversionPorIdEmprendimiento(
                            idEmprendimiento);

                    var sumatoriaTotal = 0.0;
                    foreach (var inversion in necesidadInversion.InversionesRealizadas)
                    {
                        sumatoriaTotal += inversion.PrecioUsados + inversion.PrecioNuevos;
                    }

                    necesidadInversion.SumatoriaTotalPrecios = (decimal) sumatoriaTotal;
                }
                else
                {
                    necesidadInversion = new NecesidadInversionResultado();
                }
            }
            else
            {
                necesidadInversion.InversionesRealizadas = new List<InversionRealizadaResultado>();
                for (int i = 0; i < 6; i++)
                {
                    necesidadInversion.InversionesRealizadas.Add(new InversionRealizadaResultado());
                }
            }

            necesidadInversion.Orden = orden;

            necesidadInversion.DescripcionFuenteFinanc =
                necesidadInversion.DescripcionFuenteFinanc ??
                "Otra fuente de financiamiento (indique cuál)";

            return necesidadInversion;
        }

        public IList<FuenteFinanciamientoResultado> ObtenerFuentesFinanciamiento()
        {
            var resultados = _inversionEmprendimientoRepositorio.ObtenerFuentesFinanciamiento();
            return resultados;
        }
    }
}