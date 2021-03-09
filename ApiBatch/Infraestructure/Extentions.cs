using ApiBatch.Infraestructure.Data.DSL;
using NHibernate;
using System.Configuration;

namespace ApiBatch.Infraestructure
{
    public static class Extentions
    {
        public static StoreProcedureStateless RunSP(this IStatelessSession statlessSession, string spName)
        {
            var ownerName = ConfigurationManager.AppSettings["db:owner"];
            if (!string.IsNullOrEmpty(ownerName))
            {
                ownerName = string.Format("{0}.", ownerName);
            }

            return new StoreProcedureStateless(string.Format("{0}{1}", ownerName, spName), statlessSession);
        }
    }
}