using ApiBatch.Base;
using ApiBatch.GeneradoresArchivos;
using ApiBatch.Models;
using ApiBatch.Operations.Procesos;
using ApiBatch.Operations.QueueManager;
using NHibernate;

namespace ApiBatch.Base.QueueManager
{
    public static class QueueManagerFactory
    {
        private static ArchivoDocumentacionPago archivoDocumentacion;
        public static IQueueManagerProcess GetProcessType(ProcesoResultado process, IStatelessSession session)
        {
            switch (process.IdTipoProceso)
            {
                case 1:
                    {
                        return new ProcesoInformeBanco(session,
                            process,
                            new ArchivoInformeBanco(),
                            Variables.DIR_PRUEBA,
                            false);
                    }
                case 2:
                    {
                        return new ProcesoExportacionRecupero(session,
                            process,
                            new ReporteExportacionRecupero(),
                            Variables.DIR_PRUEBA, false);
                    }
                case 3:
                    {
                        return new ProcesoExportacionPrestamo(session,
                                process,
                                new ReporteExportacionPrestamo(),
                                Variables.DIR_PRUEBA, false);
                    }
                case 8:
                    {
                        return new ProcesoCuponera(session,
                            process,
                            new ArchivoCuponera(), 
                            Variables.DIR_PRUEBA, false);
                    }
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                    {
                        if (process.EsPrimero)
                        {
                            archivoDocumentacion = new ArchivoDocumentacionPago();
                        }
                        return new ProcesoDocumentacionPago(session,
                            process,
                            archivoDocumentacion,
                            Variables.DIR_PRUEBA,
                            false);
                    }
                default:
                    {
                        return new ProcesoInformeBanco(session,
                            process,
                            new ArchivoInformeBanco(),
                            Variables.DIR_PRUEBA, false);
                    }
            }
        }
    }
}