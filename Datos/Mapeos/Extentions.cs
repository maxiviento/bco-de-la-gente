using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Datos.Mapeos
{
    public static class Extentions
    {
        public static OneToOnePart<T> WithStoreProcedure<T>(this OneToOnePart<T> part)
        {

            return part;
        }

        public static ManyToOnePart<T> WithStoreProcedure<T>(this ManyToOnePart<T> part, string procedureName, Expression<Func<T>> extr )
        {

            return part;
        }
    }
}

