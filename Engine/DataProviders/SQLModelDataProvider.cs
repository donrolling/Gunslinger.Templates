using Common.BaseClasses;
using Common.Responses;
using Gunslinger.Factories.SQL;
using Gunslinger.Interfaces;
using Gunslinger.Models;
using Gunslinger.Models.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Management.Smo;
using System.Collections.Generic;

namespace Gunslinger.DataProviders
{
	public class SQLModelDataProvider : LoggingWorker, IDataProvider
	{
		private readonly SQLDataProviderSettings _dataProviderSettings;
		private readonly ISQLServerInfoFactory _sqlServerInfoFactory;

		public SQLModelDataProvider(ISQLServerInfoFactory sqlServerInfoFactory, SQLDataProviderSettings dataProvider, ILoggerFactory loggerFactory) : base(loggerFactory)
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
				sqlServerInfo.Database.Refresh();
				var smoTables = TableInfoFactory.Create(sqlServerInfo, settings, includeTheseEntitiesOnly, excludeTheseEntities);
				var gunslingerTables = SQLTableFactory.Create(template.Namespace, template.Language, smoTables);
				var smoViews = _dataProviderSettings.GenerateViews
					? ViewInfoFactory.Create(sqlServerInfo, settings, includeTheseEntitiesOnly, excludeTheseEntities)
					: new List<View>();
				var gunslingerViews = _dataProviderSettings.GenerateViews 
					? SQLViewFactory.Create(template.Namespace, template.Language, smoViews)
					: new List<SQLView>();
				var providerModels = new Dictionary<string, IProviderModel>();
				foreach (var gunslingerTable in gunslingerTables)
				{
					var sqlModel = SQLModelFactory.Create(gunslingerTable, settings);
					providerModels.Add(gunslingerTable.UniqueName, sqlModel);
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