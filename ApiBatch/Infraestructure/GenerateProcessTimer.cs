using ApiBatch.Base.QueueManager;
using ApiBatch.Infraestructure.Data;
using ApiBatch.Operations.Procesos;
using Infraestructura.Core.Comun.Dato;
using NLog;
using System;
using System.Threading;
using System.Timers;
using System.Web.Hosting;

namespace ApiBatch.Infraestructure
{
    public class GenerateProcessTimer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static System.Timers.Timer timer;
        public static void Start()
        {
            timer = new System.Timers.Timer(90000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public static void Stop()
        {
            timer.Stop();
            timer.Dispose();
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            HostingEnvironment.QueueBackgroundWorkItem((CancellationToken ct) =>
            {
                using (var sesion = NHibernateSessionManager.SessionFactory.OpenStatelessSession())
                {
                    using (var tx = sesion.BeginTransaction())
                    {
                        Id idProceso = new Id();
                        try
                        {

                            var groupProcess = ProcesoInformeBanco.VerifyGroupProcessToGenerate(sesion);

                            if (groupProcess != null && groupProcess.NroGrupoProceso.Valor != 0)
                            {
                                var listProcess = ProcesoInformeBanco.VerifyProcessToGenerate(sesion, groupProcess);
                                if (listProcess != null && listProcess.Count > 0)
                                {
                                    for (var i = 0; i < listProcess.Count; i++)
                                    {
                                        idProceso = listProcess[i].IdProceso;
                                        // Si hay alguno para generar
                                        listProcess[i].IdEstado = new Id(4);
                                        listProcess[i].IdUsuario = new Id(-1);
                                        listProcess[i].TxPath = null;
                                        listProcess[i].EsUltimo = i == listProcess.Count - 1;
                                        listProcess[i].EsPrimero = i == 0;
                                        listProcess[i].AcumuloResultados = listProcess.Count > 1;
                                        ProcesoInformeBanco.UpdateProcess(sesion, listProcess[i]);
                                        var manager = QueueManagerFactory.GetProcessType(listProcess[i], sesion);
                                        var filePath = manager.GenerateFile(listProcess[i].Abreviatura);
                                        manager.EndProcess(filePath);
                                    }
                                    tx.Commit();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (idProceso.Valor > 0)
                            {
                                ProcesoInformeBanco.Error(sesion, ex.Message, idProceso);
                                Logger.Error(ex.Message, "Error en GenerateProcessTimer");
                                throw;
                            }

                        }
                    }
                }
            });
        }
    }
}