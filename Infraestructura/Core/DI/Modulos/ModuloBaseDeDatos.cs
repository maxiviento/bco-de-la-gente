using System.Web;
using Infraestructura.Core.Datos;
using NHibernate;
using Ninject.Modules;
using Ninject.Web.Common;

namespace Infraestructura.Core.DI.Modulos
{
    public class ModuloBaseDeDatos : NinjectModule
    {
        public override void Load()
        {
            
            if (Kernel != null)
            {
                Kernel.Bind<ISession>()
                    .ToMethod(x => NHibernateSessionManager.GetCurrentSession())
                    .InRequestScope()
                    .OnActivation(OpenTransaction)
                    .OnDeactivation(EndTransaction);

                Kernel.Bind<IStatelessSession>().ToMethod(x => NHibernateSessionManager.CurrentStatelessSession()).InRequestScope();
            }
        }

        private void EndTransaction(ISession session)
        {

            try
            {
                if (session.Transaction.IsActive)
                {
                    session.Transaction.Commit();
                }
            }
            catch (ADOException ex)
            {
                session.Dispose();
            }
        }


        private void OpenTransaction(ISession session)
        {
            session.BeginTransaction();

            if (HttpContext.Current != null)
            {
                HttpContext.Current.AddOnRequestCompleted((context) =>
                {
                    EndTransaction(session);
                });

            }
        }
    }
}
