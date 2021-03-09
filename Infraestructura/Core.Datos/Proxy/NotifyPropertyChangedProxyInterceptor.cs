using System;
using System.ComponentModel;
using NHibernate.Proxy.DynamicProxy;

namespace Infraestructura.Core.Datos.Proxy
{
    public sealed class NotifyPropertyChangedProxyInterceptor : NHibernate.Proxy.DynamicProxy.IInterceptor
    {
        private PropertyChangedEventHandler changed = delegate { };
        private readonly Object target = null;

        public NotifyPropertyChangedProxyInterceptor(Object target)
        {
            this.target = target;
        }

        #region IInterceptor Members

        public Object Intercept(InvocationInfo info)
        {
            Object result = null;

            if (info.TargetMethod.Name == "add_PropertyChanged")
            {
                var propertyChangedEventHandler = info.Arguments[0] as PropertyChangedEventHandler;
                this.changed += propertyChangedEventHandler;
            }
            else if (info.TargetMethod.Name == "remove_PropertyChanged")
            {
                var propertyChangedEventHandler = info.Arguments[0] as PropertyChangedEventHandler;
                this.changed -= propertyChangedEventHandler;
            }
            else
            {
                result = info.TargetMethod.Invoke(this.target, info.Arguments);
            }

            if (info.TargetMethod.Name.StartsWith("set_") == true)
            {
                var propertyName = info.TargetMethod.Name.Substring("set_".Length);
                this.changed(info.Target, new PropertyChangedEventArgs(propertyName));
            }

            return (result);
        }

        #endregion
    }

}
