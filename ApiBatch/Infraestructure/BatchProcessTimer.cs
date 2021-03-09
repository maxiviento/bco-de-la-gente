using ApiBatch.Infraestructure.Data;
using ApiBatch.Operations.Procesos;
using ApiBatch.Operations.QueueManager;
using Infraestructura.Core.Comun.Dato;
using System;
using System.Threading;
using System.Timers;
using System.Web.Hosting;

namespace ApiBatch.Infraestructure
{
    public class BatchProcessTimer
    {
        private static System.Timers.Timer timer;
        public static void Start()
        {
            timer = new System.Timers.Timer(60000);
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
                        try
                        {                            
                            var process = ProcesoInformeBanco.FindNext(sesion);
                            if (process != null)
                            {
                                ProcesoInformeBanco.ProcessSp(sesion, process);
                            }
                            tx.Commit();
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                }
            });
        }
    }
}