using Common.BaseClasses;
using Gunslinger.Enum;
using Gunslinger.Interfaces;
using Gunslinger.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Gunslinger.Facades
{
    public class GeneratorFacade : LoggingWorker, IGeneratorFacade
    {
        private readonly IContextFactory _contextFactory;
        private readonly IDataProviderFactory _dataProviderFactory;
        private readonly IModelGeneratorFacade _modelGeneratorFacade;
        private readonly IResourceOutputEngine _resourceOutputEngine;

        public GeneratorFacade(
            IContextFactory contextFactory, 
            IDataProviderFactory dataProviderFactory,
            IModelGeneratorFacade modelGeneratorFacade, 
            IResourceOutputEngine resourceOutputEngine,
            ILoggerFactory loggerFactory
        ) : base(loggerFactory)
        {
            _contextFactory = contextFactory;
            _dataProviderFactory = dataProviderFactory;
            _modelGeneratorFacade = modelGeneratorFacade;
            _resourceOutputEngine = resourceOutputEngine;
        }

        public OperationResult Generate()
        {
            var context = _contextFactory.Create();

            // initialize all data providers
            var dataProviderNames = context.Templates.Select(a => a.DataProviderName).Distinct();
            foreach (var dataProviderName in dataProviderNames)
            {
                var dataProviderDefinition = context.DataProviders.First(a => a.Name == dataProviderName);
                _dataProviderFactory.Create(dataProviderDefinition);
            }

            foreach (var template in context.Templates)
            {
                switch (template.Type)
                {
                    case TemplateType.Model:
                        var generateResult = _modelGeneratorFacade.GenerateMany(context, template);
                        if (generateResult.Failure)
                        {
                            this.Logger.LogError(generateResult.Message);
                        }

                        break;
                    case TemplateType.Setup:
                    default:
                        var generateOneResult = _modelGeneratorFacade.GenerateOne(context, template);
                        if (generateOneResult.Failure)
                        {
                            this.Logger.LogError(generateOneResult.Message);
                        }

                        break;
                }
            }

            // copy resource files
            var resourseWriteResult = _resourceOutputEngine.Write(context);
            if (resourseWriteResult.Failure)
            {
                return resourseWriteResult;
            }

            // done
            return OperationResult.Ok();
        }
    }
}