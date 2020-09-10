using Gunslinger.Factories.SQL;
using Gunslinger.Interfaces;
using Gunslinger.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Gunslinger.DataProviders
{
    public class SQLModelDataProvider : DataProviderBase, IDataProvider
    {
        public ISQLServerInfoFactory SqlServerInfoFactory { get; }

        public SQLModelDataProvider(ISQLServerInfoFactory sqlServerInfoFactory, DataProvider dataProvider, ILoggerFactory loggerFactory) : base(dataProvider, loggerFactory)
        {
            SqlServerInfoFactory = sqlServerInfoFactory;
        }

        public Dictionary<string, IProviderModel> Get(GenerationSettings settings, Template template, List<string> excludedTypes)
        {
            //this call won't recreate the SQLServerInfo over multiple calls
            var sqlServerInfo = SqlServerInfoFactory.Create(DataProvider);
            var tables = TableInfoFactory.Create(sqlServerInfo, settings, excludedTypes);
            var sqlTables = SQLTableFactory.Create(template.Namespace, template.Language, tables);
            var providerModels = new Dictionary<string, IProviderModel>();
            foreach (var sqlTable in sqlTables)
            {
                var sqlModel = SQLModelFactory.Create(sqlTable);
                providerModels.Add(sqlTable.UniqueName, sqlModel);
            }
            return providerModels;
        }
    }
}