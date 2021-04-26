using CompositionRoot;
using Gunslinger.Interfaces;
using Gunslinger.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

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
            var generatorFacade = serviceProvider.GetService<IGeneratorFacade>();

            // make sure that the OutputDirectory goes to root\\Output\\whatever
            var directories = AppDomain.CurrentDomain.BaseDirectory.Split("\\bin\\")[0].Split("\\");
            var baseDirectory = string.Join('\\', directories.Take(directories.Length - 1));
            generatorFacade.Context.OutputDirectory = $"{ baseDirectory }\\Output\\{ generatorFacade.Context.OutputDirectory }";
            foreach (var dataProvider in generatorFacade.Context.DataProviders)
            {
                if (!string.IsNullOrEmpty(dataProvider["LocalDataSource"].Value))
                {
                    dataProvider.LocalDataSource = $"{ baseDirectory }\\Output\\{ dataProvider.LocalDataSource }";
                }
            }
            return generatorFacade;
        }
    }
}