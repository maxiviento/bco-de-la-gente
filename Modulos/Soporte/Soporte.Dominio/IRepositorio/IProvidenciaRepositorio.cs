using System.Collections.Generic;
using Formulario.Aplicacion.Consultas.Resultados;
using Infraestructura.Core.Comun.Dato;
using Soporte.Dominio.Modelo;

namespace Soporte.Dominio.IRepositorio
{
    public interface IProvidenciaRepositorio : IRepositorio<DatoProvidencia>
    {
        decimal RegistrarProvidencia(decimal idFormulario);
        DatosBasicosFormularioResultado ObtenerSolicitante(decimal idFormulario);
        DatosProvidenciaResultado ObtenerDatosParaProvidencia(decimal idFormulario);
        IList<DatosProvidenciaMasivaResultado> ObtenerDatosParaProvidenciaMasiva(decimal idLote);
        decimal RegistrarProvidenciaMasiva(decimal idLote, decimal usuario);
    }
}