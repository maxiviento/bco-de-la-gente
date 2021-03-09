using System;
using Infraestructura.Core.Comun.Dato;

namespace ApiBatch.Infraestructure.Data.DSL
{
    public partial class StoreProcedure
    {

        public StoreProcedure AddParamUndefined(string name, object param, Type valueType)
        {
            if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                valueType = valueType.GetGenericArguments()[0];
                if (TypeChecker.IsId(valueType))
                    return AddParam(name, (Id?)param);
                if (TypeChecker.IsInteger(valueType))
                {
                    if (param == null)
                    {
                        decimal? nullValue = null;
                        return AddParam(name, nullValue);
                    }
                    return AddParam(name, (decimal?)Convert.ToDecimal(param));
                }
                    
                if (TypeChecker.IsDate(valueType))
                    return AddParam(name, (DateTime?)param);
                if (TypeChecker.IsDecimal(valueType))
                {
                    if (param == null)
                    {
                        decimal? nullValue = null;
                        return AddParam(name, nullValue);
                    }
                    return AddParam(name, (decimal?)Convert.ToDecimal(param));
                }
                    
                if (TypeChecker.IsBoolean(valueType))
                    return AddParam(name, (bool?)param);                
            }
            else
            {
                if (TypeChecker.IsId(valueType))
                    return AddParam(name, (Id)param);
                if (TypeChecker.IsInteger(valueType))
                    return AddParam(name, Convert.ToDecimal(param));
                if (TypeChecker.IsDate(valueType))
                    return AddParam(name, (DateTime)param);
                if (TypeChecker.IsDecimal(valueType))
                    return AddParam(name, Convert.ToDecimal(param));
                if (TypeChecker.IsBoolean(valueType))
                    return AddParam(name, (bool)param);
                if (TypeChecker.IsText(valueType))
                    return AddParam(name, (string)param);
            }
            return null;
        }

        public StoreProcedure AddParam(string name, Id param)
        {
            return AddParam(name, param.Valor == default(decimal) ? -1 : param.Valor, typeof(decimal));
        }
        public StoreProcedure AddParam(string name, string param)
        {
            return AddParam(name, !string.IsNullOrEmpty(param) ? param.ToUpper() : param, typeof(string));
        }
        public StoreProcedure AddParam(string name, long param)
        {
            return AddParam(name, param, typeof(long));
        }
        public StoreProcedure AddParam(string name, decimal param)
        {
            return AddParam(name, param, typeof(decimal));
        }
        public StoreProcedure AddParam(string name, char param)
        {
            return AddParam(name, param, typeof(char));
        }
        public StoreProcedure AddParam(string name, DateTime param)
        {
            return AddParam(name, param, typeof(DateTime));
        }
        public StoreProcedure AddParam(string name, bool param)
        {
            return AddParam(name, param ? "S" : "N", typeof(char));
        }
        public StoreProcedure AddParam(string name, int param)
        {
            return AddParam(name, param, typeof(int));
        }


        public StoreProcedure AddParam(string name, Id? param)
        {
            return AddParam(name, param?.Valor ?? -1, typeof(decimal));
        }
        public StoreProcedure AddParam(string name, long? param)
        {
            return AddParam(name, param ?? -1, typeof(long));
        }
        public StoreProcedure AddParam(string name, decimal? param)
        {
            return AddParam(name, param ?? -1, typeof(decimal));
        }
        public StoreProcedure AddParam(string name, char? param)
        {
            return AddParam(name, param, typeof(char));
        }
        public StoreProcedure AddParam(string name, DateTime? param)
        {
            return AddParam(name, param ?? DateTime.MinValue, typeof(DateTime));
        }
        public StoreProcedure AddParam(string name, bool? param)
        {
            var defaultValue = (param.HasValue) ? (param.Value ? "S" : "N") : "N";
            return AddParam(name, defaultValue, typeof(char));
        }
        public StoreProcedure AddParam(string name, int? param)
        {
            return AddParam(name, param ?? -1, typeof(int));
        }
    }
}
