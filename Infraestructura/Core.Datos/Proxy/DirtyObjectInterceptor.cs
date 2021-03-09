using System.Collections.Generic;
using Castle.DynamicProxy;
using Infraestructura.Core.Datos.DSL;

namespace Infraestructura.Core.Datos.Proxy
{
    public class DirtyObjectInterceptor<T> : IInterceptor
    {
        private T _cleanObject;
        private bool _isDirty;
        private IDictionary<string, Parameter> _parameters { get; set; }

        public DirtyObjectInterceptor(T cleanObject)
        {
            _cleanObject = cleanObject;
            _parameters = new Dictionary<string,Parameter>();
        }
        public DirtyObjectInterceptor()
        {
            _parameters = new Dictionary<string, Parameter>();
            
        }

        public void Intercept(IInvocation invocation)
        {
            
            var isSetMethod = invocation.Method.Name.StartsWith("set_");

            if (isSetMethod)
            {
                var propertyName = invocation.Method.Name.Replace("set_", "");
                var properyValue = invocation.GetArgumentValue(0);

                if (!HasEqualsValues(_cleanObject, propertyName, properyValue))
                {
                    _isDirty = true;
                    _parameters.Add(propertyName, new Parameter(propertyName, properyValue, properyValue.GetType()));
                    invocation.Proceed();
                }
            }

           
        }

        private bool HasEqualsValues(T cleanObject,string properyName, object newPropertyValue )
        {
            var propertyInfo = cleanObject.GetType().GetProperty(properyName);
            var propertyValue = propertyInfo.GetValue(cleanObject);

            return propertyValue == newPropertyValue;
        }

        public IEnumerable<Parameter> GetParameters()
        {
            return _parameters.Values;
        }


    }
}
