using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos.Mapeos;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

namespace Infraestructura.Core.Datos
{
    public class NHibernateMapConfiguration
    {
        /*public static FluentConfiguration Configure()
        {
            var conStr = MsSqlConfiguration.MsSql2012.ConnectionString(connectionString());
            if (showLog())
            {
                conStr.ShowSql();
            }

            var config = Fluently
                .Configure()
                .Database(conStr)
                .Mappings(m => m.FluentMappings
                    .AddFromAssemblyOf<UsuarioMap>());

            return config;
        }

        private static bool showLog()
        {
            return bool.Parse(ConfigurationManager.AppSettings["Log:Hibernate"]);
        }
        private static string connectionString()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[Environment.MachineName].ConnectionString;
            return connectionString;
        }*/



    }
}
