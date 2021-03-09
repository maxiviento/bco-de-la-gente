using System;
using Infraestructura.Core.Comun.Dato;

namespace ApiBatch.Infraestructure.Data.DSL
{
    public partial class StoreProcedureStateless
    {
        public StoreProcedureStateless AddParam(Id param)
        {
            return AddParam(param.Valor == default(decimal) ? -1 : param.Valor, typeof(decimal));
        }
        public StoreProcedureStateless AddParam(string param)
        {
            return AddParam(!string.IsNullOrEmpty(param) ? param.ToUpper() : param, typeof(string));
        }
        public StoreProcedureStateless AddParam(long param)
        {
            return AddParam(param, typeof(long));
        }
        public StoreProcedureStateless AddParam(decimal param)
        {
            return AddParam(param, typeof(decimal));
        }
        public StoreProcedureStateless AddParam(char param)
        {
            return AddParam(param, typeof(char));
        }
        public StoreProcedureStateless AddParam(DateTime param)
        {
            return AddParam(param, typeof(DateTime));
        }
        public StoreProcedureStateless AddParam(bool param)
        {
            return AddParam(param ? "S" : "N", typeof(char));
        }
        public StoreProcedureStateless AddParam(int param)
        {
            return AddParam(param, typeof(int));
        }
        

        public StoreProcedureStateless AddParam(Id? param)
        {
            return AddParam(param?.Valor ?? -1, typeof(decimal));
        }
        public StoreProcedureStateless AddParam(long? param)
        {
            return AddParam(param ?? -1, typeof(long));
        }
        public StoreProcedureStateless AddParam(decimal? param)
        {
            return AddParam(param ?? -1, typeof(decimal));
        }
        public StoreProcedureStateless AddParam(char? param)
        {
            return AddParam(param, typeof(char));
        }
        public StoreProcedureStateless AddParam(DateTime? param)
        {
            return AddParam(param ?? DateTime.MinValue, typeof(DateTime));
        }
        public StoreProcedureStateless AddParam(bool? param)
        {
            var defaultValue = (param.HasValue) ? (param.Value ? "S" : "N") : "N";
            return AddParam(defaultValue, typeof(char));
        }
        public StoreProcedureStateless AddParam(int? param)
        {
            return AddParam(param ?? -1, typeof(int));
        }
    }
}
