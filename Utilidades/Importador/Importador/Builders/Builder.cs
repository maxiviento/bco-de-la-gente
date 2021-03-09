using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Utilidades.Importador.Builders
{
    public abstract class Builder<T>
    {
        protected IList<IConverter> Converters { get; set; }
        protected IImporterInterceptor<T> Interceptor { get; set; }
        public IList<Column> Columns { get; }

        public Builder()
        {
            Columns = new List<Column>();
            Converters = new List<IConverter>();
        }

        public Builder<T> AddConverter(IConverter converter)
        {
            Converters.Add(converter);
            return this;
        }

        public Builder<T> SetInterceptor(IImporterInterceptor<T> interceptor)
        {
            Interceptor = interceptor;
            return this;
        }

        public void AddColumn<TProperty>(Column column, Expression<Func<T, TProperty>> propertyLambda)
        {
            Type type = typeof(T);

            MemberExpression member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda.ToString()));

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda.ToString()));

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format(
                    "Expresion '{0}' refers to a property that is not from type {1}.",
                    propertyLambda.ToString(),
                    type));

            column.Property = propInfo;
            column.PropertyType = propInfo.PropertyType;

            Columns.Add(column);
        }

        public abstract void Generate(byte[] data, out List<T> success, out List<ImportResult<T>> withErrors);

        public abstract void GenerateFirstRowOnly(byte[] arrayBytes, out List<T> success,
            out List<ImportResult<T>> withErrors);

        public abstract void LeerArchivoSuaf(byte[] arrayBytes, out List<T> devengados, out List<T> noProcesados,
            out List<T> conErrores);
    }
}