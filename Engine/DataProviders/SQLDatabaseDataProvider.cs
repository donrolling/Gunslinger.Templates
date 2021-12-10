using Common.BaseClasses;
using Common.Responses;
using Gunslinger.Factories.SQL;
using Gunslinger.Interfaces;
using Gunslinger.Models;
using Gunslinger.Models.Settings;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Gunslinger.DataProviders
{
    public class SQLDatabaseDataProvider : LoggingWorker, IDataProvider
    {
        private readonly ISQLServerInfoFactory _sqlServerInfoFactory;
        private readonly DataProviderSettings _dataProviderSettings;

        public SQLDatabaseDataProvider(ISQLServerInfoFactory sqlServerInfoFactory, DataProviderSettings dataProvider, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _sqlServerInfoFactory = sqlServerInfoFactory;
            _dataProviderSettings = dataProvider;
        }

        public OperationResult<Dictionary<string, IProviderModel>> Get(GenerationSettings settings, Template template, List<string> includeTheseEntitiesOnly, List<string> excludeTheseEntities)
        {
            //this call won't recreate the SQLServerInfo over multiple calls
            try
            {
                var sqlServerInfo = _sqlServerInfoFactory.Create(_dataProviderSettings);
                var tables = TableInfoFactory.Create(sqlServerInfo, settings, includeTheseEntitiesOnly, excludeTheseEntities);
                var sqlTables = SQLTableFactory.Create(template.Namespace, template.Language, tables);
                var providerModels = new Dictionary<string, IProviderModel>();
                foreach (var sqlTable in sqlTables)
                {
                    var sqlModel = SQLModelFactory.Create(sqlTable, settings);
                    providerModels.Add(sqlTable.UniqueName, sqlModel);
                }
                return OperationResult.Ok(providerModels);
            }
            catch (System.Exception ex)
            {
                return OperationResult.Fail<Dictionary<string, IProviderModel>>($"SQL Database Data Provider had a failure: { ex.Message }\r\n{ ex.StackTrace }");
            }
        }
    }
}