using CompositionRoot;
using Gunslinger.Interfaces;
using Gunslinger.Models;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Tests.Utilities
{
    public static class TestBootstrapper
    {
        public static GenerationContext GenerationContext { get; private set; }
        public static ServiceProvider ServiceProvider { get; private set; }

        public static IServiceProvider Bootstrap(string pathToGenerationContext)
        {
            GenerationContext = GenerationConfiguration.GetGenerationConfiguration(pathToGenerationContext);
            ServiceProvider = Bootstrapper.ConfigureServices(GenerationContext);
            return ServiceProvider;
        }

        public static IGeneratorFacade GetGeneratorFacade(string relativePathToGenerationContext)
        {
            var serviceProvider = Bootstrap(relativePathToGenerationContext);
            return serviceProvider.GetService<IGeneratorFacade>();
        }
    }
}