using Common.BaseClasses;
using Common.Responses;
using Gunslinger.Factories;
using Gunslinger.Interfaces;
using Gunslinger.Models;
using Gunslinger.Models.Reflection;
using Gunslinger.Models.Settings;
using Gunslinger.Types;
using Gunslinger.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Gunslinger.DataProviders
{
    public class WebAPIDataProvider : LoggingWorker, IDataProvider
    {
        private readonly ReflectionDataProviderSettings _dataProviderSettings;

        public WebAPIDataProvider(ReflectionDataProviderSettings dataProviderSettings, ILoggerFactory loggerFactory) : base(loggerFactory)
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
                return new OperationResult<Dictionary<string, IProviderModel>>(OperationResult.Fail($"Could not load dll: { _dataProviderSettings.DataSource }\r\n\t{ ex.Message }"));
            }
            if (assembly == null)
            {
                return new OperationResult<Dictionary<string, IProviderModel>>(OperationResult.Fail($"Could not load dll: { _dataProviderSettings.DataSource }"));
            }
            var result = new Dictionary<string, IProviderModel>();
            foreach (var ns in _dataProviderSettings.Namespaces)
            {
                // types here are maybe a controller and we're not interested in its properties, we're interested in the
                // types that its methods are returning
                var types = assembly
                   .GetExportedTypes()
                   .Where(t => String.Equals(t.Namespace, ns, StringComparison.Ordinal));

                foreach (var type in types)
                {
                    var model = convert(template, type);
                    result.Add(type.FullName, model);
                }
            }
            return OperationResult<Dictionary<string, IProviderModel>>.Ok(result);
        }

        private static IProviderModel convert(Template template, Type type)
        {
            // modelName is the controller name
            // example: AgencyManagementSystemController
            //  - route name default = AgencyManagementSystem
            // properties are Name = Route
            var controllerName = NameFactory.Create(type.Name.TrimEnd("Controller".ToCharArray()));
            var routes = new List<RouteProperty>();
            foreach (var methodInfo in type.GetMethods())
            {
                var route = getRouteInfo(methodInfo, controllerName);
                var returnType = ReflectionUtility.GetReturnTypeInfoFromCustomAttributesWhenNeeded(methodInfo);
                var inputParameters = ReflectionUtility.GetInputParameters(methodInfo);
                routes.Add(new RouteProperty
                {
                    Name = NameFactory.Create(route),
                    ReturnType = returnType,
                    InputParameters = inputParameters
                });
            }
            return new RouteModel
            {
                Imports = template.Imports,
                Properties = routes,
                Name = controllerName,
                Namespace = template.Namespace
            };
        }


        private static string getRouteInfo(MethodInfo methodInfo, Name controllerName)
        {
            var customAttributes = methodInfo.GetCustomAttributes(true);
            // prefer a route that was specified
            foreach (var customAttribute in customAttributes)
            {
                var customAttributeType = customAttribute.GetType();
                if (customAttributeType.Name != "RouteAttribute")
                {
                    continue;
                }
                var responseTypePropertyInfo = customAttributeType.GetProperty("Template");
                var value = responseTypePropertyInfo.GetValue(customAttribute);
                if (value == null)
                {
                    continue;
                }
                return value.ToString();
            }
            // take what you can get
            return $"{ controllerName.Value }/{ methodInfo.Name }";
        }
    }
}