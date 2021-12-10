using Common.BaseClasses;
using Common.Responses;
using Gunslinger.Factories.Javascript;
using Gunslinger.Interfaces;
using Gunslinger.Models;
using Gunslinger.Models.Reflection;
using Gunslinger.Models.Settings;
using Gunslinger.Types;
using Gunslinger.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gunslinger.DataProviders
{
    public class MethodInfoDataProvider : LoggingWorker, IDataProvider
    {
        private readonly ReflectionDataProviderSettings _dataProviderSettings;

        private static readonly List<string> _ignoreTheseTypes = new List<string> {
            "short",
            "ushort",
            "int",
            "uint",
            "long",
            "ulong",
            "char",
            "decimal",
            "double",
            "single",
            "string",
            "bool",
            "sbyte",
            "byte",
            "void",
            "type",
            "ihttpactionresult",
            "object"
        };

        private static readonly List<string> _checkMethodDecoratorsWhenReturnTypeIs = new List<string> {
            "IHttpActionResult"
        };

        public MethodInfoDataProvider(ReflectionDataProviderSettings dataProviderSettings, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _dataProviderSettings = dataProviderSettings;
        }

        public OperationResult<Dictionary<string, IProviderModel>> Get(GenerationSettings settings, Template template, List<string> includeTheseEntitiesOnly, List<string> excludeTheseEntities)
        {
            Assembly assembly;
            try
            {
                assembly = Assembly.LoadFrom(_dataProviderSettings.DataSource);
            }
            catch (Exception ex)
            {
                return OperationResult.Fail<Dictionary<string, IProviderModel>>($"Could not load dll: { _dataProviderSettings.DataSource }\r\n\t{ ex.Message }");
            }
            if (assembly == null)
            {
                return OperationResult.Fail<Dictionary<string, IProviderModel>>($"Could not load dll: { _dataProviderSettings.DataSource }");
            }
            var rawTypeResults = new Dictionary<string, IProviderModel>();
            var results = new Dictionary<string, IProviderModel>();
            foreach (var ns in _dataProviderSettings.Namespaces)
            {
                // types here are maybe a controller and we're not interested in its properties, we're interested in the
                // types that its methods are returning
                var types = assembly
                   .GetExportedTypes()
                   .Where(t => String.Equals(t.Namespace, ns, StringComparison.Ordinal));

                foreach (var type in types)
                {
                    var methods = type.GetMethods().Where(t => t.IsPublic);
                    if (methods != null && methods.Any())
                    {
                        foreach (var method in methods)
                        {
                            addMethodReturnType(template, method, ns, rawTypeResults, results);
                            addMethodInputParameters(template, method, ns, rawTypeResults, results);
                        }
                    }
                }
            }
            return OperationResult<Dictionary<string, IProviderModel>>.Ok(results);
        }

        private static void addMethodInputParameters(Template template, MethodInfo method, string ns, Dictionary<string, IProviderModel> rawTypeResults, Dictionary<string, IProviderModel> results)
        {
            var parameters = method.GetParameters();
            foreach (var param in parameters)
            {
                var dataTypeInfo = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(param.ParameterType);
                addDataTypeInput(template, method.Name, ns, rawTypeResults, results, dataTypeInfo);
            }
        }

        private static void addMethodReturnType(Template template, MethodInfo methodInfo, string ns, Dictionary<string, IProviderModel> rawTypeResults, Dictionary<string, IProviderModel> results)
        {
            var dataTypeInfo = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(methodInfo.ReturnType);
            var typeName = dataTypeInfo.Name.Value;
            // some api methods use attributes instead of return types to signify the actual return type. Kinda weird, but ok.
            if (_checkMethodDecoratorsWhenReturnTypeIs.Contains(typeName))
            {
                dataTypeInfo = ReflectionUtility.GetReturnTypeInfoFromCustomAttributesWhenNeeded(methodInfo);
            }

            addDataTypeInput(template, methodInfo.Name, ns, rawTypeResults, results, dataTypeInfo);
        }

        private static void addDataTypeInput(Template template, string methodName, string ns, Dictionary<string, IProviderModel> rawTypeResults, Dictionary<string, IProviderModel> results, DataTypeInfo dataTypeInfo)
        {
            var dataTypeNameLower = dataTypeInfo.Name.Value.ToLower();
            if (
                template.ExcludeTheseTypes.Contains(dataTypeInfo.Name.Value)
                || _ignoreTheseTypes.Contains(dataTypeNameLower)
            )
            {
                return;
            }

            var name = $"{ ns }|{ methodName }|{ dataTypeInfo.Name.Value }";
            if (results.ContainsKey(name))
            {
                // this probably happens because of overloaded methods
                return;
            }

            if (rawTypeResults.ContainsKey(dataTypeInfo.Name.Value))
            {
                IProviderModel rawType;
                if (!rawTypeResults.TryGetValue(dataTypeInfo.Name.Value, out rawType))
                {
                    throw new Exception("Dictionary Get Failed. Odd.");
                }
                results.Add(name, rawType);
                return;
            }

            // probably always true
            if (!results.ContainsKey(name))
            {
                var model = convert(template, dataTypeInfo);
                rawTypeResults.Add(dataTypeInfo.Name.Value, model);
                results.Add(name, model);
            }
        }

        private static IProviderModel convert(Template template, DataTypeInfo dataTypeInfo)
        {
            var properties = new List<ReflectionProperty>();
            foreach (var propertyInfo in dataTypeInfo.Type.GetProperties())
            {
                var property = ReflectionPropertyFactory.Convert(dataTypeInfo.Name, propertyInfo, template.Language);
                properties.Add(property);
            }
            return new ReflectionModel
            {
                Imports = template.Imports,
                Properties = properties,
                Name = dataTypeInfo.Name,
                Namespace = template.Namespace,
                Type = dataTypeInfo.Name.Value
            };
        }
    }
}