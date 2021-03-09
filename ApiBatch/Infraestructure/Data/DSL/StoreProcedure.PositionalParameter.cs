using System;
using Infraestructura.Core.Comun.Dato;

namespace ApiBatch.Infraestructure.Data.DSL
{
    public partial class StoreProcedure
    {
        public StoreProcedure AddParam(Id param)
        {
            return AddParam(param.Valor == default(decimal) ? -1 : param.Valor, typeof(decimal));
        }
        public StoreProcedure AddParam(string param)
        {
            return AddParam(!string.IsNullOrEmpty(param) ? param.ToUpper() : param, typeof(string));
        }
        public StoreProcedure AddParam(long param)
        {
            return AddParam(param, typeof(long));
        }
        public StoreProcedure AddParam(decimal param)
        {
            return AddParam(param, typeof(decimal));
        }
        public StoreProcedure AddParam(char param)
        {
            return AddParam(param, typeof(char));
        }
        public StoreProcedure AddParam(DateTime param)
        {
            return AddParam(param, typeof(DateTime));
        }
        public StoreProcedure AddParam(bool param)
        {
            return AddParam(param ? "S" : "N", typeof(char));
        }
        public StoreProcedure AddParam(int param)
        {
            return AddParam(param, typeof(int));
        }



        public StoreProcedure AddParam(Id? param)
        {
            return AddParam(param?.Valor ?? -1, typeof(decimal));
        }
        public StoreProcedure AddParam(long? param)
        {
            return AddParam(param ?? -1, typeof(long));
        }
        public StoreProcedure AddParam(decimal? param)
        {
            return AddParam(param ?? -1, typeof(decimal));
        }
        public StoreProcedure AddParam(char? param)
        {
            return AddParam(param, typeof(char));
        }
        public StoreProcedure AddParam(DateTime? param)
        {
            return AddParam(param ?? DateTime.MinValue, typeof(DateTime));
        }
        public StoreProcedure AddParam(bool? param)
        {
            var defaultValue = (param.HasValue) ? (param.Value ? "S" : "N") : "N";
            return AddParam(defaultValue, typeof(char));
        }
        public StoreProcedure AddParam(int? param)
        {
            return AddParam(param ?? -1, typeof(int));
        }
    }
}
