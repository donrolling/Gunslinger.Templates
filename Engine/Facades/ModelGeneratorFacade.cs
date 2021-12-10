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
            //if (template.IsStub && !settings.ProcessTemplateStubs) {
            //    return OperationResult.Ok();
            //}

            var getResult = getDataItems(settings, template);
            if (getResult.Failure)
            {
                return getResult;
            }
            var destinationPath = prepareOutputDirectory(settings, template);
            var items = getResult.Result;
            foreach (var (entityName, value) in items)
            {
                if (template.ExcludeTheseTypes.Contains(value.Name.Value))
                {
                    // do everything but output this stuff
                    continue;
                }
                var output = _renderEngine.Render(template, value);
                var result = _templateOutputEngine.Write(destinationPath, value.Name.Value, value.Schema, output, template.IsStub, settings.ProcessTemplateStubs);
                if (result.Failure)
                {
                    return result;
                }
            }

            if (template.FileRename != null && template.FileRename.Any())
            {
                // clean name template off of the destination path for renaming
                var cleanDestinationPath = destinationPath.Substring(0, destinationPath.LastIndexOf('\\') + 1);
                foreach (var fileRename in template.FileRename)
                {
                    var renameResult = _templateOutputEngine.Rename(cleanDestinationPath, fileRename.Source, fileRename.Destination, fileRename.ClassRenameValue, fileRename.ClassReplaceValue);
                    if (renameResult.Failure)
                    {
                        return renameResult;
                    }
                }
            }

            return OperationResult.Ok();
        }

        public OperationResult GenerateOne(GenerationSettings settings, Template template)
        {
            var getResult = getDataItems(settings, template);
            if (getResult.Failure)
            {
                return getResult;
            }
            var destinationPath = prepareOutputDirectory(settings, template);
            var items = getResult.Result;
            var groupProviderModel = new GroupModel
            {
                Models = items.Select(a => a.Value),
                Namespace = template.Namespace,
                Imports = template.Imports
            };
            var output = _renderEngine.Render(template, groupProviderModel);
            var result = _templateOutputEngine.Write(destinationPath, output, template.IsStub, settings.ProcessTemplateStubs);
            if (result.Failure)
            {
                return result;
            }

            return OperationResult.Ok();
        }

        private string prepareOutputDirectory(GenerationSettings settings, Template template)
        {
            // cleanup the output directory
            var destinationPath = Path.Join(settings.OutputDirectory, template.OutputRelativePath);
            if (template.IsStub && !settings.ProcessTemplateStubs)
            {
                // don't clean the directory if we're not going to ProcessTemplateStubs
                // this way, we can be sneaky and still generate stubs that don't already exist
                return destinationPath;
            }
            if (!template.DeleteAllItemsInOutputDirectory)
            {
                // don't clean the directory if DeleteAllItemsInOutputDirectory is set to false
                // this way a directory may contain elements that shouldn't be deleted.
                // This is not preferred, but it gives us flexibility.
                return destinationPath;
            }
            var destinationDirectory = Path.GetDirectoryName(destinationPath);
            if (template.DeleteAllItemsInOutputDirectory)
            {
                var cleanupResult = _templateOutputEngine.CleanupOutputDirectory(destinationDirectory);
            }
            return destinationPath;
        }

        private OperationResult<Dictionary<string, IProviderModel>> getDataItems(GenerationSettings settings, Template template)
        {
            var dataProvider = _dataProviderFactory.Get(template.DataProviderName);
            var getResult = dataProvider.Get(settings, template, settings.IncludeTheseEntitiesOnly, settings.ExcludeTheseEntities);
            if (getResult.Failed)
            {
                return new OperationResult<Dictionary<string, IProviderModel>>(OperationResult.Fail(getResult.Message));
            }
            return OperationResult<Dictionary<string, IProviderModel>>.Ok(getResult.Result);
        }
    }
}