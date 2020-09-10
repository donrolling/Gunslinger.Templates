using Gunslinger.Factories;
using Gunslinger.Interfaces;
using Gunslinger.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace Gunslinger.DataProviders
{
    public class SwaggerDataProvider : DataProviderBase, IDataProvider
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string RefDef = "#/definitions/";

        public SwaggerDataProvider(DataProvider dataProvider, ILoggerFactory loggerFactory) : base(dataProvider, loggerFactory)
        {
        }

        public Dictionary<string, IProviderModel> Get(GenerationSettings settings, Template template, List<string> excludedTypes)
        {
            var data = getData(settings);
            var root = JToken.Parse(data);
            var definitions = root["definitions"].Value<JObject>();
            var items = new Dictionary<string, IProviderModel>();
            foreach (var (key, value) in definitions)
            {
                var item = parseItem(template.Namespace, key, value);
                if (item == null) continue;
                items.Add(key, item);
            }

            return items;
        }

        private string getData(GenerationSettings settings)
        {
            if (this.DataProvider.UseLocalDataSource)
            {
                if (string.IsNullOrEmpty(this.DataProvider.LocalDataSource))
                {
                    throw new Exception("DataProvider is set to use a local file source, but the path is not specified.");
                }
                return File.ReadAllText(this.DataProvider.LocalDataSource);
            }
            else
            {
                if (string.IsNullOrEmpty(this.DataProvider.DataSource))
                {
                    throw new Exception("DataProvider is set to use a network data source, but the path is not specified.");
                }
                return _httpClient.GetAsync(this.DataProvider.DataSource).Result.Content.ReadAsStringAsync().Result;
            }
        }

        private static string fixPropertyName(string key)
        {
            return $"{char.ToUpperInvariant(key[0])}{key.Substring(1)}";
        }

        private static string fixType(JToken value)
        {
            //safety. some of these didn't have a type defined.
            var type = value["type"] == null ? "string" : value["type"].ToString();
            switch (type)
            {
                case "integer":
                    return "int";

                case "array":
                    return fixCollectionType(value);

                default:
                    return type;
            }
        }

        private static string fixCollectionType(JToken value)
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

        private static Model parseItem(string _namespace, string className, JToken definition)
        {
            var item = new Model
            {
                Namespace = _namespace,
                Schema = "",
                Name = NameFactory.Create(className),
                Properties = new List<Property>()
            };
            try
            {
                item.Description = definition["description"] != null ? definition["description"].ToString() : string.Empty;
                var properties = definition["properties"].Value<JObject>();
                foreach (var (key, value) in properties)
                {
                    var type = fixType(value);
                    var propertyName = fixPropertyName(key);
                    var propertyDescription = value["description"] != null ? value["description"].ToString() : string.Empty;
                    item.Properties.Add(new Property { Name = NameFactory.Create(propertyName), Type = type, Description = propertyDescription });
                }

                return item;
            }
            catch (Exception)
            {
                //some definitions don't have properties, we're just going to skip those
                return null;
            }
        }
    }
}