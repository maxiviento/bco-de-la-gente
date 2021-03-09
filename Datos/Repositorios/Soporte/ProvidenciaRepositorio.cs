using System.Collections.Generic;
using Formulario.Aplicacion.Consultas.Resultados;
using Infraestructura.Core.Datos;
using NHibernate;
using Soporte.Dominio.IRepositorio;
using Soporte.Dominio.Modelo;

namespace Datos.Repositorios.Soporte
{
    public class ProvidenciaRepositorio : NhRepositorio<DatoProvidencia>, IProvidenciaRepositorio
    {
        public ProvidenciaRepositorio(ISession sesion) : base(sesion)
        {
        }

        public decimal RegistrarProvidencia(decimal idFormulario)
        {
            return Execute("PR_ACTUALIZA_FORM_PROVIDENCIA")
                .AddParam(idFormulario)
                .ToSpResult().Id.Valor;
        }

        public DatosBasicosFormularioResultado ObtenerSolicitante(decimal idFormulario)
        {
            return Execute("PR_OBTENER_SOLICITANTE_X_FORM")
                .AddParam(idFormulario)
                .ToUniqueResult<DatosBasicosFormularioResultado>();
        }

        public IList<DatosProvidenciaMasivaResultado> ObtenerDatosParaProvidenciaMasiva(decimal idLote)
        {
            return Execute("PR_OBTENER_PROVIDENCIA_X_LOTE")
                .AddParam(idLote)
                .ToListResult<DatosProvidenciaMasivaResultado>();
        }

        public decimal RegistrarProvidenciaMasiva(decimal idLote, decimal usuario)
        {
            return Execute("PR_ACTUALIZA_FORM_PROV_LOTE")
                .AddParam(idLote)
                .AddParam(usuario)
                .ToSpResult().Id.Valor;
        }

        public DatosProvidenciaResultado ObtenerDatosParaProvidencia(decimal idFormulario)
        {
            return Execute("PR_OBTENER_PROVIDENCIA_X_FORM")
                .AddParam(idFormulario)
                .ToUniqueResult<DatosProvidenciaResultado>();
        }
    }
}