using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using NHibernate;

namespace Infraestructura.Core.Datos.Proxy
{
    public class CustomInterceptor : NHibernate.EmptyInterceptor
    {
        class _CustomInterceptor : Castle.DynamicProxy.IInterceptor
        {
            private Boolean finishedLoading = false;

            void Castle.DynamicProxy.IInterceptor.Intercept(IInvocation invocation)
            {
                Object target = (invocation.InvocationTarget == null) ? invocation.Proxy : invocation.InvocationTarget;

                //check if the entity has finished loading
                if ((invocation.InvocationTarget == null) && (this.finishedLoading == false))
                {
                    this.finishedLoading = true;
                }

                //TODO: do something before base method call
                if (invocation.Method.Name.StartsWith("get_") == true)
                {
                    //getter invocation
                }
                else if (invocation.Method.Name.StartsWith("set_") == true)
                {
                    //setter invocation
                }
                else
                {
                    //method invocation
                }

                if (invocation.InvocationTarget != null)
                {
                    //proceed with base implementation (base getter, setter or method call)
                    invocation.Proceed();
                }

                //TODO: do something after base method call
                if (invocation.Method.Name.StartsWith("get_") == true)
                {
                    //getter invocation
                }
                else if (invocation.Method.Name.StartsWith("set_") == true)
                {
                    //setter invocation
                }
                else
                {
                    //method invocation
                }
            }
        }

        private static readonly ProxyGenerator proxyGenerator = new ProxyGenerator();
        private ISession session = null;

        public static Object CreateProxy(Type type)
        {
            List<Type> interfaces = new List<Type>();
            //TODO: add interfaces to list

            Object instance = null;

            if ((interfaces.Count != 0) && (type.IsSealed == false))
            {
                //TODO: pass any custom parameters to the _CustomInterceptor class
                instance = proxyGenerator.CreateClassProxy(type, interfaces.ToArray(), new _CustomInterceptor());
            }
            else
            {
                instance = Activator.CreateInstance(type);
            }

            return (instance);
        }

        public static T CreateProxy<T>() where T : class, new()
        {
            Type type = typeof(T);
            return (CreateProxy(type) as T);
        }

        public override String GetEntityName(Object entity)
        {
            if (entity.GetType().Assembly.FullName.StartsWith("DynamicProxyGenAssembly2") == true)
            {
                return (entity.GetType().BaseType.FullName);
            }
            else
            {
                return (entity.GetType().FullName);
            }
        }

        public override void SetSession(ISession session)
        {
            this.session = session;
            base.SetSession(session);
        }

        public override Object Instantiate(String clazz, EntityMode entityMode, Object id)
        {
            if (entityMode == EntityMode.Poco)
            {
                Type type = Type.GetType(clazz, false);

                if (type != null)
                {
                    Object instance = CreateProxy(type);

                    this.session.SessionFactory.GetClassMetadata(clazz).SetIdentifier(instance, id, entityMode);

                    return (instance);
                }
            }

            return (base.Instantiate(clazz, entityMode, id));
        }
    }

}
