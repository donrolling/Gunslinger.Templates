using CompositionRoot;
using Gunslinger.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Tests.Utilities
{
    public class TestBootstrapper
    {
        public static IServiceProvider Bootstrap(string pathToGenerationContext)
        {
            var generationContext = GenerationConfiguration.GetGenerationConfiguration(pathToGenerationContext);
            var serviceProvider = Bootstrapper.ConfigureServices(generationContext);
            return serviceProvider;
        }

        public static IGeneratorFacade GetGeneratorFacade(string relativePathToGenerationContext)
        {
            var serviceProvider = Bootstrap(relativePathToGenerationContext);
            return serviceProvider.GetService<IGeneratorFacade>();
        }
    }
}