using Common.BaseClasses;
using Gunslinger.Interfaces;
using Gunslinger.Models;
using Gunslinger.Responses;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gunslinger.Facades
{
    public class ModelGeneratorFacade : LoggingWorker, IModelGeneratorFacade
    {
        private readonly IDataProviderFactory _dataProviderFactory;
        private readonly ITemplateOutputEngine _templateOutputEngine;
        private readonly IRenderEngine _renderEngine;

        public ModelGeneratorFacade(
            ITemplateOutputEngine templateOutputEngine,
            IRenderEngine renderEngine,
            IDataProviderFactory dataProviderFactory,
            ILoggerFactory loggerFactory
        ) : base(loggerFactory)
        {
            _dataProviderFactory = dataProviderFactory;
            _templateOutputEngine = templateOutputEngine;
            _renderEngine = renderEngine;
        }

        public OperationResult Generate(GenerationSettings settings, IEnumerable<Template> templates)
        {
            foreach (var template in templates)
            {
                var result = GenerateMany(settings, template);
                if (result.Failure)
                {
                    this.Logger.LogError(result.Message);
                }
            }

            return OperationResult.Ok();
        }

        public OperationResult GenerateMany(GenerationSettings settings, Template template)
        {
            var items = getDataItems(settings, template);
            var destinationPath = prepareOutputDirectory(settings, template);

            foreach (var (entityName, value) in items)
            {
                if (settings.ExcludeTheseEntities.Contains(value.Name.Value))
                {
                    continue;
                }

                var output = _renderEngine.Render(template, value);
                var result = _templateOutputEngine.Write(destinationPath, value.Name.Value, value.Schema, output);
                if (result.Failure)
                {
                    return result;
                }
            }

            return OperationResult.Ok();
        }

        public OperationResult GenerateOne(GenerationSettings settings, Template template)
        {
            var items = getDataItems(settings, template);
            var destinationPath = prepareOutputDirectory(settings, template);

            var groupProviderModel = new GroupModel
            {
                Models = items.Select(a => a.Value),
                Namespace = template.Namespace,
                Imports = template.Imports
            };
            var output = _renderEngine.Render(template, groupProviderModel);
            var result = _templateOutputEngine.Write(destinationPath, output);
            if (result.Failure)
            {
                return result;
            }

            return OperationResult.Ok();
        }

        private string prepareOutputDirectory(GenerationSettings settings, Template template)
        {
            // cleanup the output directory
            var destinationPath = $"{ settings.OutputDirectory }\\{ template.OutputRelativePath }";
            var destinationDirectory = Path.GetDirectoryName(destinationPath);
            var cleanupResult = _templateOutputEngine.CleanupOutputDirectory(destinationDirectory);
            return destinationPath;
        }

        private Dictionary<string, IProviderModel> getDataItems(GenerationSettings settings, Template template)
        {
            var dataProvider = _dataProviderFactory.Get(template.DataProviderName);
            var items = dataProvider.Get(settings, template, settings.ExcludeTheseEntities);
            return items;
        }
    }
}