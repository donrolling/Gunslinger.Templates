﻿using Common.BaseClasses;
using Gunslinger.DataProviders;
using Gunslinger.Interfaces;
using Gunslinger.Models.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Gunslinger.Factories
{
    public class DataProviderFactory : LoggingWorker, IDataProviderFactory
    {
        private static readonly Dictionary<string, IDataProvider> _dataProviderDictionary;

        private readonly ISQLServerInfoFactory _sqlServerInfoFactory;
        private readonly ILoggerFactory _loggerFactory;

        static DataProviderFactory()
        {
            _dataProviderDictionary = new Dictionary<string, IDataProvider>();
        }

        public DataProviderFactory(ISQLServerInfoFactory sqlServerInfoFactory, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _sqlServerInfoFactory = sqlServerInfoFactory;
            _loggerFactory = loggerFactory;
        }

        public IDataProvider Create(dynamic dataProviderSettings)
        {
            var allSettings = JsonConvert.SerializeObject(dataProviderSettings);
            var generalSettings = JsonConvert.DeserializeObject<DataProviderSettings>(allSettings);
            if (_dataProviderDictionary.ContainsKey(generalSettings.Name))
            {
                return _dataProviderDictionary[generalSettings.Name];
            }
            switch (generalSettings.TypeName)
            {
                case "SwaggerDataProvider":
                    var swaggerDataProviderSettings = JsonConvert.DeserializeObject<SwaggerDataProviderSettings>(allSettings);
					if (swaggerDataProviderSettings.OpenDataSourceUrlInDefaultBrowser)
					{
						openDataSourceInBrowser(swaggerDataProviderSettings.DataSource);
						// wait for the browser to load the file
						Thread.Sleep(TimeSpan.FromSeconds(10));
					}
                    var swaggerDataProvider = new SwaggerDataProvider(swaggerDataProviderSettings, this._loggerFactory);
                    _dataProviderDictionary.Add(swaggerDataProviderSettings.Name, swaggerDataProvider);
                    return swaggerDataProvider;

                case "SQLModelDataProvider":
                    var sqlDataProviderSettings = JsonConvert.DeserializeObject<SQLDataProviderSettings>(allSettings);
                    var sqlModelDataProvider = new SQLModelDataProvider(_sqlServerInfoFactory, sqlDataProviderSettings, this._loggerFactory);
                    _dataProviderDictionary.Add(sqlDataProviderSettings.Name, sqlModelDataProvider);
                    return sqlModelDataProvider;

                //case "ReflectionDataProvider":
                //    var reflectionDataProviderSettings = JsonConvert.DeserializeObject<ReflectionDataProviderSettings>(allSettings);
                //    var reflectionDataProvider = new ReflectionDataProvider(reflectionDataProviderSettings, this._loggerFactory);
                //    _dataProviderDictionary.Add(settings.Name, reflectionDataProvider);
                //    return reflectionDataProvider;

                //case "ReflectionMethodInfoDataProvider":
                //    var reflectionMethodInfoDataProviderSettings = JsonConvert.DeserializeObject<ReflectionDataProviderSettings>(allSettings);
                //    var reflectionMethodInfoDataProvider = new MethodInfoDataProvider(reflectionMethodInfoDataProviderSettings, this._loggerFactory);
                //    _dataProviderDictionary.Add(settings.Name, reflectionMethodInfoDataProvider);
                //    return reflectionMethodInfoDataProvider;

                //case "WebAPIDataProvider":
                //    var webAPIDataProviderDataProviderSettings = JsonConvert.DeserializeObject<ReflectionDataProviderSettings>(allSettings);
                //    var webAPIDataProviderDataProvider = new WebAPIDataProvider(webAPIDataProviderDataProviderSettings, this._loggerFactory);
                //    _dataProviderDictionary.Add(settings.Name, webAPIDataProviderDataProvider);
                //    return webAPIDataProviderDataProvider;

                default:
                    var msg = $"Create() - Name not matched: { generalSettings.TypeName }";
                    this.Logger.LogError(msg);
                    throw new Exception(msg);
            }
        }

        private void openDataSourceInBrowser(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
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