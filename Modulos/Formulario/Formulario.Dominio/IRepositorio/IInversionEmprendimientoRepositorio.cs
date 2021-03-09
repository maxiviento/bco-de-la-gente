using System.Collections.Generic;
using Formulario.Aplicacion.Consultas.Resultados;
using Infraestructura.Core.Comun.Dato;

namespace Formulario.Dominio.IRepositorio
{
    public interface IInversionEmprendimientoRepositorio
    {
        IList<FuenteFinanciamientoResultado> ObtenerFuentesFinanciamiento();
        NecesidadInversionResultado ObtenerNecesidadInversionPorIdEmprendimiento(Id idEmprendimiento);
        void RegistrarNecesidadInversion(Id idEmprendimiento, Id? idFuenteFinanciamiento, decimal montoMicroprestamo,
            decimal montoCapitalPropio, decimal montoOtrasFuentes);
    }
}