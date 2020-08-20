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
    public class SQLModelDataProvider : LoggingWorker, IDataProvider
    {
        private readonly DataProviderSettings _dataProviderSettings;
        private readonly ISQLServerInfoFactory _sqlServerInfoFactory;

        public SQLModelDataProvider(ISQLServerInfoFactory sqlServerInfoFactory, DataProviderSettings dataProvider, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _dataProviderSettings = dataProvider;
            _sqlServerInfoFactory = sqlServerInfoFactory;
        }

        public OperationResult<Dictionary<string, IProviderModel>> Get(GenerationSettings settings, Template template, List<string> includeTheseEntitiesOnly, List<string> excludeTheseEntities)
        {
            try
            {
                //this call won't recreate the SQLServerInfo over multiple calls
                var sqlServerInfo = _sqlServerInfoFactory.Create(_dataProviderSettings);
                var tables = TableInfoFactory.Create(sqlServerInfo, settings, includeTheseEntitiesOnly, excludeTheseEntities);
                var sqlTables = SQLTableFactory.Create(template.Namespace, template.Language, tables);
                var providerModels = new Dictionary<string, IProviderModel>();
                foreach (var sqlTable in sqlTables)
                {
                    var sqlModel = SQLModelFactory.Create(sqlTable);
                    providerModels.Add(sqlTable.UniqueName, sqlModel);
                }
                return OperationResult<Dictionary<string, IProviderModel>>.Ok(providerModels);
            }
            catch (System.Exception ex)
            {
                return new OperationResult<Dictionary<string, IProviderModel>>
                {
                    Failure = true,
                    Message = $"SQL Database Data Provider had a failure: { ex.Message }\r\n{ ex.StackTrace }",
                };
            }
        }
    }
}