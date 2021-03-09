using NHibernate;
using NHibernate.Proxy.DynamicProxy;

namespace Infraestructura.Core.Datos.Proxy
{
    public sealed class NotifyPropertyChangedInterceptor : EmptyInterceptor
    {
        private static readonly ProxyFactory factory = new ProxyFactory();

        private ISession session = null;

        public override void SetSession(ISession session)
        {
            this.session = session;
            base.SetSession(session);
        }

        //public override Object Instantiate(String clazz, EntityMode entityMode, Object id)
        //{
        //    var entityType = this.session.SessionFactory.GetClassMetadata(clazz).GetMappedClass(entityMode);
        //    var target = this.session.SessionFactory.GetClassMetadata(entityType).Instantiate(id, entityMode);
        //    var proxy = factory.CreateProxy(entityType, new _NotifyPropertyChangedInterceptor(target), typeof(INotifyPropertyChanged));

        //    this.session.SessionFactory.GetClassMetadata(entityType).SetIdentifier(proxy, id, entityMode);

        //    return (proxy);
        //}
    }

}
