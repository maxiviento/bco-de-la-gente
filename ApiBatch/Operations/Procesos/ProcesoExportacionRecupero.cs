using ApiBatch.Base.QueueManager;
using ApiBatch.GeneradoresArchivos;
using ApiBatch.Models;
using ApiBatch.Operations.QueueManager;
using Formulario.Aplicacion.Consultas.Resultados;
using NHibernate;

namespace ApiBatch.Operations.Procesos
{
    public class ProcesoExportacionRecupero: QueueManagerProcess<ExportacionRecupero>
    {

        public ProcesoExportacionRecupero(IStatelessSession _statelessSession) : base(_statelessSession)
        {
        }
        public ProcesoExportacionRecupero(IStatelessSession _statelessSession, ProcesoResultado process,
            ReporteExportacionRecupero generador, string dir, bool mismoArchivo) : base(_statelessSession, process, generador, dir, mismoArchivo)
        {            
        }        

        public override string GetStoreProcedureCall(OperacionEntrada cmd)
        {
            var spStringCall = "";
            //var filtrosInforme = (InscripcionConFiltrosSuspendidaConsulta) cmd.Filtros;
            //var spStringCall = StatelessSession.RunSP("PR_GENERAR_PROCESO_BENEF")
            //    .AddParam(filtrosInforme.ProgramaId)
            //    .AddParam(filtrosInforme.LoteId)
            //    .AddParam(filtrosInforme.EstadoInscripcionId == null ? null : ConcatenarEstadosInscripcion(filtrosInforme.EstadoInscripcionId))
            //    .AddParam(filtrosInforme.FechaDesde)
            //    .AddParam(filtrosInforme.FechaHasta)
            //    .AddParam(filtrosInforme.TipoInformeId)
            //    .AddParam(filtrosInforme.DocumentoBeneficiario)
            //    .AddParam(filtrosInforme.GeneroBeneficiarioId)
            //    .AddParam(filtrosInforme.ApellidoBeneficiario)
            //    .AddParam(filtrosInforme.NombreBeneficiario)
            //    .AddParam(filtrosInforme.DepartamentoId)
            //    .AddParam(filtrosInforme.LocalidadId)
            //    .AddParam(filtrosInforme.BarrioId)
            //    .AddParam(filtrosInforme.CategoriaId)
            //    .AddParam(filtrosInforme.LineaPobrezaId)
            //    .AddParam(filtrosInforme.OtorgadoAgua)
            //    .AddParam(filtrosInforme.OtorgadoLuz)
            //    .AddParam(filtrosInforme.OtorgadoRentas)
            //    .AddParam(filtrosInforme.DivisionId)
            //    .AddParam(filtrosInforme.SolicitadoAgua)
            //    .AddParam(filtrosInforme.SolicitadoLuz)
            //    .AddParam(filtrosInforme.SolicitadoRentas)
            //    .AddParam(!string.IsNullOrEmpty(filtrosInforme.NroResolucion) ? new Id(filtrosInforme.NroResolucion) : new Id())
            //    .AddParam(filtrosInforme.IncluirSuspendidas)
            //    .GetStoreProcedureCall();
            return spStringCall;
        }
    }
}