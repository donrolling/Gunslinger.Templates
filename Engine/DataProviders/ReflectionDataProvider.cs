using Common.BaseClasses;
using Common.Responses;
using Gunslinger.Factories;
using Gunslinger.Factories.Javascript;
using Gunslinger.Interfaces;
using Gunslinger.Models;
using Gunslinger.Models.Reflection;
using Gunslinger.Models.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gunslinger.DataProviders
{
    public class ReflectionDataProvider : LoggingWorker, IDataProvider
    {
        private readonly ReflectionDataProviderSettings _dataProviderSettings;

        public ReflectionDataProvider(ReflectionDataProviderSettings dataProviderSettings, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _dataProviderSettings = dataProviderSettings;
        }

        public OperationResult<Dictionary<string, IProviderModel>> Get(GenerationSettings settings, Template template, List<string> includeTheseEntitiesOnly, List<string> excludeTheseEntities)
        {
            Assembly assembly;
            try
            {
                assembly = Assembly.LoadFile(_dataProviderSettings.DataSource);
            }
            catch (Exception ex)
            {
                return OperationResult.Fail<Dictionary<string, IProviderModel>>($"Could not load dll: { _dataProviderSettings.DataSource }\r\n\t{ ex.Message }");
            }
            if (assembly == null)
            {
                return OperationResult.Fail<Dictionary<string, IProviderModel>>($"Could not load dll: { _dataProviderSettings.DataSource }");
            }
            var result = new Dictionary<string, IProviderModel>();
            foreach (var ns in _dataProviderSettings.Namespaces)
            {
                var types = assembly.GetTypes().Where(t => String.Equals(t.Namespace, ns, StringComparison.Ordinal));
                if (types != null && types.Any())
                {
                    foreach (var type in types)
                    {
                        if (!result.ContainsKey(type.FullName))
                        {
                            var model = convert(template, type);
                            result.Add(type.FullName, model);
                        }
                    }
                }
            }
            return OperationResult<Dictionary<string, IProviderModel>>.Ok(result);
        }

        private static IProviderModel convert(Template template, Type type)
        {
            var modelName = NameFactory.Create(type.Name);
            var properties = new List<ReflectionProperty>();
            foreach (var propertyInfo in type.GetProperties())
            {
                var property = ReflectionPropertyFactory.Convert(modelName, propertyInfo, template.Language);
                properties.Add(property);
            }
            return new ReflectionModel
            {
                Imports = template.Imports,
                Properties = properties,
                Name = modelName,
                Namespace = template.Namespace
            };
        }

        
    }
}