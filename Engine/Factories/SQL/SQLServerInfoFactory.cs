using Common.BaseClasses;
using Gunslinger.Interfaces;
using Gunslinger.Models;
using Gunslinger.Models.Settings;
using Gunslinger.Models.SQL;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Management.Smo;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Gunslinger.Factories.SQL
{
    public class SQLServerInfoFactory : LoggingWorker, ISQLServerInfoFactory
    {
        private static readonly Dictionary<string, SQLServerInfo> _sqlServerInfo;

        static SQLServerInfoFactory()
        {
            _sqlServerInfo = new Dictionary<string, SQLServerInfo>();
        }

        public SQLServerInfoFactory(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        public SQLServerInfo Create(DataProviderSettings dataProvider)
        {
            //don't construct this stuff twice
            if (_sqlServerInfo.ContainsKey(dataProvider.Name))
            {
                return _sqlServerInfo[dataProvider.Name];
            }

            var builder = new SqlConnectionStringBuilder(dataProvider.DataSource);
            var sqlServerInfo = new SQLServerInfo
            {
                DatabaseName = builder.InitialCatalog,
                DataProvider = dataProvider,
                Server = new Server(builder.DataSource),
                ServerName = builder.DataSource,
            };
            sqlServerInfo.Database = new Database(sqlServerInfo.Server, sqlServerInfo.DatabaseName);

            //add it to the list
            _sqlServerInfo.Add(dataProvider.Name, sqlServerInfo);

            return sqlServerInfo;
        }
    }
}