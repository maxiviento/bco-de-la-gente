using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Utilidades.Exportador.Builders
{
    public abstract class Builder<T>
    {

        public IList<Column> Columns { get; }

        public Builder()
        {
            Columns = new List<Column>();
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
        
        public abstract byte[] Generate(IList<T> data);
        public abstract string GenerateAsString(IList<T> data);
    }
}
