using System;
using System.Collections;
using System.Collections.Generic;
using Infraestructura.Core.Comun.Dato;
using Infraestructura.Core.Datos.DSL;
using Infraestructura.Core.Datos.Proxy;
using NHibernate;
using NHibernate.Proxy;
using NHibernate.Transform;

namespace Infraestructura.Core.Datos
{
    public class OracleResultTransformer<T> : IResultTransformer where T : class
    {
        private IConventionDefinition _conventionDefinition;
        private ISession _sesion;

        public OracleResultTransformer(IConventionDefinition conventionDefinition, ISession sesion)
        {
            this._conventionDefinition = conventionDefinition;
            this._sesion = sesion;
        }

        public OracleResultTransformer()
        {
        }


        public object TransformTuple(object[] tuple, string[] aliases)
        {
            T instance = null;

            for (int i = 0; i < aliases.Length; i++)
            {
                Generate(aliases[i], tuple[i], ref instance);
            }
            return instance;
        }


        private void Generate<T>(string path, object value, ref T root) where T : class
        {
            //generar una cola, 
            var queue = new Queue<KeyValuePair<string, object>>();

            var properties = path.Split('_');
            foreach (var property in properties)
            {
                KeyValuePair<string, object> keyValue;
                keyValue = new KeyValuePair<string, object>(property, value);
                queue.Enqueue(keyValue);
            }

            GenerarObjeto(queue, ref root);
        }

        private object GenerarObjeto<T>(Queue<KeyValuePair<string, object>> queue, ref T root) where T : class
        {
            if (queue.Count == 0)
            {
                return null;
            }

            if (root == null)
            {
                root = (T) Activator.CreateInstance(typeof(T), true);
            }
            var keyValue = queue.Dequeue();

            var properyName = keyValue.Key;
            var property = root.GetType().GetProperty(properyName);
            if (property == null) return null;
            var propertyType = property.PropertyType;

            object propertyValue = property.GetValue(root);

            var typeIsNullable = (propertyType.IsGenericType &&
                                  propertyType.GetGenericTypeDefinition() == typeof(Nullable<>));

            if (propertyType.IsPrimitive || propertyType == typeof(string)
                || propertyType == typeof(Id) || propertyType == typeof(DateTime)
                || typeIsNullable)
            {
                if (propertyValue is Id && ((Id) propertyValue).IsDefault()
                    || propertyValue is DateTime && ((DateTime) propertyValue).Equals(DateTime.MinValue)
                    || propertyType.IsPrimitive
                    || typeIsNullable)
                {
                    propertyValue = keyValue.Value;
                }
                else
                {
                    propertyValue = propertyValue ?? keyValue.Value;
                }
            }
            else
            {
                propertyValue = propertyValue ?? Activator.CreateInstance(propertyType, true);
            }

            GenerarObjeto(queue, ref propertyValue);
            property.SetValue(root, TypeChecker.Converter(property.PropertyType, propertyValue));

            return propertyValue;
        }

        private object TransformToNHProxy(object[] tuple, string[] aliases)
        {
            var instance = Activator.CreateInstance(typeof(T));

            for (int i = 0; i < aliases.Length; i++)
            {
                var properyName = GetProperyName(aliases[i]);
                var property = instance.GetType().GetProperty(properyName);
                property.SetValue(instance, TypeChecker.Converter(property.PropertyType, tuple[i]));
            }

            var proxy = NhProxyGenerator.CreateNHProxy(instance);
            var entity = CastEntity<T>(proxy);


            return entity;
        }

        public static T CastEntity<T>(object entity) where T : class
        {
            var proxy = entity as INHibernateProxy;
            if (proxy != null)
            {
                return proxy.HibernateLazyInitializer.GetImplementation() as T;
            }
            else
            {
                return entity as T;
            }
        }

        private object TransformToProxy(object[] tuple, string[] aliases)
        {
            var instance = Activator.CreateInstance(typeof(T));

            for (int i = 0; i < aliases.Length; i++)
            {
                var properyName = GetProperyName(aliases[i]);
                var property = instance.GetType().GetProperty(properyName);
                property.SetValue(instance, TypeChecker.Converter(property.PropertyType, tuple[i]));
            }

            var proxy = NhProxyGenerator.CreateProxy(instance);

            return proxy;
        }

        private object TransformToProxyLinFU(object[] tuple, string[] aliases)
        {
            var instance = Activator.CreateInstance(typeof(T));

            for (int i = 0; i < aliases.Length; i++)
            {
                var properyName = GetProperyName(aliases[i]);
                var property = instance.GetType().GetProperty(properyName);
                property.SetValue(instance, TypeChecker.Converter(property.PropertyType, tuple[i]));
            }

            var proxy = NhProxyGenerator.CreateProxyLinFu(instance);
            return proxy;
        }

        private string GetProperyName(string column)
        {
            return column;
        }

        public IList TransformList(IList collection)
        {
            return collection;
        }
    }
}