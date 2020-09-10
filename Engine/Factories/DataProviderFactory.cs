using Common.BaseClasses;
using Gunslinger.DataProviders;
using Gunslinger.Interfaces;
using Gunslinger.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Gunslinger.Factories
{
    public class DataProviderFactory : LoggingWorker, IDataProviderFactory
    {
        private static readonly Dictionary<string, IDataProvider> _dataProviderDictionary;

        public ISQLServerInfoFactory SqlServerInfoFactory { get; }
        public ILoggerFactory LoggerFactory { get; }

        static DataProviderFactory()
        {
            _dataProviderDictionary = new Dictionary<string, IDataProvider>();
        }

        public DataProviderFactory(ISQLServerInfoFactory sqlServerInfoFactory, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            SqlServerInfoFactory = sqlServerInfoFactory;
            LoggerFactory = loggerFactory;
        }

        public IDataProvider Create(DataProvider dataProvider)
        {
            if (_dataProviderDictionary.ContainsKey(dataProvider.Name))
            {
                return _dataProviderDictionary[dataProvider.Name];
            }

            switch (dataProvider.TypeName)
            {
                case "SwaggerDataProvider":
                    var swaggerDataProvider = new SwaggerDataProvider(dataProvider, this.LoggerFactory);
                    _dataProviderDictionary.Add(dataProvider.Name, swaggerDataProvider);
                    return swaggerDataProvider;

                case "SQLModelDataProvider":
                    var SQLModelDataProvider = new SQLModelDataProvider(SqlServerInfoFactory, dataProvider, this.LoggerFactory);
                    _dataProviderDictionary.Add(dataProvider.Name, SQLModelDataProvider);
                    return SQLModelDataProvider;

                default:
                    var msg = $"Create() - Name not matched: { dataProvider.TypeName }";
                    this.Logger.LogError(msg);
                    throw new Exception(msg);
            }
        }

        public IDataProvider Get(string name)
        {
            if (_dataProviderDictionary.ContainsKey(name))
            {
                return _dataProviderDictionary[name];
            }
            else
            {
                var msg = $"Get() - Name not matched: { name }";
                this.Logger.LogError(msg);
                throw new Exception(msg);
            }
        }
    }
}