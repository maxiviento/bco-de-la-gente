using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Context;

namespace ApiBatch.Infraestructure.Data
{
    public class SesionFactory
    {
        private const string ConnectionStringName = "DB";
        private  static string ConnectionString { get; set; }

        static SesionFactory()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
        }
        private static ISessionFactory GetSessionFactory<T>() where T : ICurrentSessionContext
        {
            var oracleConfiguration =
                OracleDataClientConfiguration.Oracle10
                    .ConnectionString(c => c.Is(ConnectionString))
                    .Driver<NHibernate.Driver.OracleManagedDataClientDriver>()
                    .AdoNetBatchSize(250)
                    .ShowSql();

            var configuration = Fluently.Configure()
                .Database(oracleConfiguration)
                .CurrentSessionContext<T>();
            var _sessionFactory = configuration.BuildSessionFactory();
            return _sessionFactory;
        }
    }
}