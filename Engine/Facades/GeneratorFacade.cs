using Common.BaseClasses;
using Gunslinger.Enum;
using Gunslinger.Interfaces;
using Gunslinger.Models;
using Gunslinger.Responses;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Gunslinger.Facades
{
    public class GeneratorFacade : LoggingWorker, IGeneratorFacade
    {
        public GenerationContext Context { get; private set; }

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
            Context = _contextFactory.Create();
        }

        public OperationResult Generate()
        {
            var errors = new List<OperationResult>();

            // initialize all data providers
            var dataProviderNames = Context.Templates.Select(a => a.DataProviderName).Distinct();
            foreach (var dataProviderName in dataProviderNames)
            {
                var dataProviderDefinition = Context.DataProviders.First(a => a.Name == dataProviderName);
                _dataProviderFactory.Create(dataProviderDefinition);
            }

            foreach (var template in Context.Templates)
            {
                switch (template.Type)
                {
                    case TemplateType.Model:
                        var generateResult = _modelGeneratorFacade.GenerateMany(Context, template);
                        if (generateResult.Failure)
                        {
                            errors.Add(generateResult);
                            this.Logger.LogError(generateResult.Message);
                        }

                        break;

                    case TemplateType.Setup:
                    default:
                        var generateOneResult = _modelGeneratorFacade.GenerateOne(Context, template);
                        if (generateOneResult.Failure)
                        {
                            errors.Add(generateOneResult);
                            this.Logger.LogError(generateOneResult.Message);
                        }

                        break;
                }
            }

            // copy resource files
            var resourseWriteResult = _resourceOutputEngine.Write(Context);
            if (resourseWriteResult.Failure)
            {
                return resourseWriteResult;
            }

            // done
            if (errors.Any())
            {
                var message = errors.Select(a => a.Message).Aggregate("Generation was not completely successful.\r\n\t", (accumulator, next) => $"{ accumulator}\r\n\t{ next }");
                return OperationResult.Fail(message);
            }
            return OperationResult.Ok();
        }
    }
}