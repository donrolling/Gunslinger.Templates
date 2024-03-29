﻿using Common.BaseClasses;
using Common.Responses;
using Gunslinger.Factories;
using Gunslinger.Interfaces;
using Gunslinger.Models;
using Gunslinger.Models.Settings;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace Gunslinger.DataProviders
{
    public class SwaggerDataProvider : LoggingWorker, IDataProvider
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string RefDef = "#/definitions/";
        private readonly SwaggerDataProviderSettings _dataProviderSettings;

        public SwaggerDataProvider(SwaggerDataProviderSettings swaggerDataProviderSettings, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _dataProviderSettings = swaggerDataProviderSettings;
        }

        /// <summary>
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="template"></param>
        /// <param name="includeTheseEntitiesOnly"></param>
        /// <param name="excludeTheseEntities"></param>
        /// <returns>A dictionary of named models. The key is the model name.</returns>
        public OperationResult<Dictionary<string, IProviderModel>> Get(GenerationSettings settings, Template template, List<string> includeTheseEntitiesOnly, List<string> excludeTheseEntities)
        {
            var dataResult = getData(settings);
            if (dataResult.Failed)
            {
                return OperationResult.Fail<Dictionary<string, IProviderModel>>(dataResult.Message);
            }
            var data = dataResult.Result;
            try
            {
                var root = JToken.Parse(data);
                var definitions = root["definitions"].Value<JObject>();
                var items = new Dictionary<string, IProviderModel>();

                var whiteList = includeTheseEntitiesOnly.Any();
                var blackList = excludeTheseEntities.Any();

                foreach (var (key, value) in definitions)
                {
                    // shouldn't be both, but we're going to try to be nice
                    if (whiteList)
                    {
                        if (!includeTheseEntitiesOnly.Contains(key))
                        {
                            continue;
                        }
                    }
                    if (blackList)
                    {
                        if (excludeTheseEntities.Contains(key))
                        {
                            continue;
                        }
                    }

                    var item = parseItem(template.Namespace, key, value, template);
                    if (item == null) continue;
                    items.Add(key, item);
                }

                return OperationResult<Dictionary<string, IProviderModel>>.Ok(items);
            }
            catch (Exception ex)
            {
                return OperationResult.Fail<Dictionary<string, IProviderModel>>($"Swagger Data Provider - Failed to parse data from provider.\r\n\t{ ex.Message }\r\n\tData: { data }");
            }
        }

        private OperationResult<string> getData(GenerationSettings settings)
        {
            if (_dataProviderSettings.UseLocalDataSource)
            {
                if (string.IsNullOrEmpty(_dataProviderSettings.LocalDataSource))
                {
                    return OperationResult.Fail<string>("DataProvider is set to use a local file source, but the path is not specified.");
                }
                try
                {
                    var fixedPath = fixPath(_dataProviderSettings.LocalDataSource);
                    var result = File.ReadAllText(fixedPath);
                    return OperationResult<string>.Ok(result);
                }
                catch (Exception ex)
                {
                    return OperationResult.Fail<string>($"DataProvider failed to read file: { _dataProviderSettings.LocalDataSource }.\r\n{ ex.Message }\r\n{ ex.StackTrace }");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_dataProviderSettings.DataSource))
                {
                    return OperationResult.Fail<string>("DataProvider is set to use a network data source, but the path is not specified.");
                }
                try
                {
                    var result = _httpClient.GetAsync(_dataProviderSettings.DataSource).Result.Content.ReadAsStringAsync().Result;
                    return OperationResult<string>.Ok(result);
                }
                catch (Exception ex)
                {
                    return OperationResult.Fail<string>($"DataProvider failed to make an http call to: { _dataProviderSettings.DataSource }.\r\n{ ex.Message }\r\n{ ex.StackTrace }");
                }
            }
        }

        private string fixPath(string path)
        {
            if (Path.IsPathFullyQualified(path))
            {
                return path;
            }
            var location = Assembly.GetExecutingAssembly().Location;
            var assemblyFileName = Path.GetFileName(location);
            return location.Replace(assemblyFileName, path);
        }

        private static string fixPropertyName(string key)
        {
            return $"{char.ToUpperInvariant(key[0])}{key.Substring(1)}";
        }

        private string getDataType(JToken value, bool isNullable)
        {
            //safety. some of these didn't have a type defined.
            if (value["type"] == null)
            {
                // is there something else that we can do here?
                return "string";
            }
            var format = string.Empty;
            if (value["format"] != null)
            {
                format = value["format"].ToString(); //"format": "date-time";
            }

            var type = value["type"].ToString();
            switch (type)
            {
                case "integer":
                    return isNullable == true ? "int?" : "int";

                case "array":
                    return fixCollectionType(value);

                case "string":
                    switch (format)
                    {
                        case "date-time":
                            return isNullable ? "DateTime?" : "DateTime";

                        default:
                            return type;
                    }

                case "number":
                    if (!string.IsNullOrEmpty(format))
                    {
                        return isNullable == true ? $"{ format }?" : format;
                    }
                    // what should happen here?
                    // setting to a double for now.
                    // sometimes Swagger doesn't give us much to go on here. Can we talk to Dustin about ways to fix this?
                    return isNullable == true ? "double?" : "double";

                default:
                    return type;
            }
        }

        private string fixCollectionType(JToken value)
        {
            if (value["items"] == null) throw new Exception("Unknown parsing situation. Items node was empty.");
            if (value["items"]["type"] == null && value["items"]["$ref"] == null) throw new Exception("Unknown parsing situation. Items node had no type or $ref property.");

            if (value["items"]["type"] != null)
            {
                var type = value["items"]["type"];
                return $"List<{ type }>";
            }
            else if (value["items"]["$ref"] != null)
            {
                var referenceDefinition = value["items"]["$ref"].ToString().Replace(RefDef, string.Empty);
                return $"List<{ referenceDefinition }>";
            }

            throw new Exception("Unknown parsing situation. Items node had no type or $ref property.");
        }

        private Model parseItem(string _namespace, string className, JToken definition, Template template)
        {
            var item = new Model
            {
                Namespace = _namespace,
                Schema = "",
                Name = NameFactory.Create(className, template, true)
            };
            try
            {
                item.Description = definition["description"] != null ? definition["description"].ToString() : string.Empty;
                var requiredProperties = new List<string>();
                if (definition["required"] != null)
                {
                    requiredProperties = definition["required"].Value<JArray>().Select(a => a.ToString()).ToList();
                }
                if (definition["properties"] == null)
                {
                    //some definitions don't have properties, we're just going to skip those
                    return null;
                }
                var properties = definition["properties"].Value<JObject>();
                foreach (var (key, value) in properties)
                {
                    var prop = getProperty(requiredProperties, key, value, template);
                    item.Properties.Add(prop);
                }

                return item;
            }
            catch (Exception ex)
            {
                var msg = ex.ToString();
                return null;
            }
        }

        private Property getProperty(List<string> requiredProperties, string key, JToken value, Template template)
        {
            var isNullable = getNullableStatus(requiredProperties, key, value);
            var type = getDataType(value, isNullable);
            var propertyDescription = value["description"] != null ? value["description"].ToString() : string.Empty;
            var prop = new Property
            {
                Name = NameFactory.Create(key, template, false),
                Type = type,
                Description = propertyDescription,
                IsNullable = isNullable
            };
            return prop;
        }

        private bool getNullableStatus(List<string> requiredProperties, string key, JToken value)
        {
            // required properties could be used to determine type, but it doesn't seem like that is the intended use
            // if you wanted to do that, use this code:
            //      var isRequired = requiredProperties.Contains(key);
            // Instead, under normal circumstances we should use the nullable property
            if (value["nullable"] == null)
            {
                // I made a setting that allows the engine to interpret the lack of 
                // a nullable specification as a request for a nullable property.
                // That is odd, but here we are.
                if (_dataProviderSettings.NonSpecifiedPropertiesAreNullable)
                {
                    // special default without a nullable specification would be that the 
                    // property is nullable
                    return true;
                }
                else
                {
                    // standard default without a nullable specification would be that the 
                    // property is NOT nullable
                    return false;
                }
            }
            var nullable = value["nullable"].ToString() == "true";
            return nullable;
        }
    }
}