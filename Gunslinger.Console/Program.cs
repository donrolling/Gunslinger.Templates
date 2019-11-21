using Gunslinger.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Gunslinger.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var configPath = args.FirstOrDefault();
            if (string.IsNullOrEmpty(configPath))
            {
                System.Console.WriteLine($"Path to configuration was not found.");
            }
            else
            {
                System.Console.WriteLine($"Path to configuration: { configPath }");
            }

            var outputPath = args.Skip(1).FirstOrDefault();
            if (string.IsNullOrEmpty(outputPath))
            {
                System.Console.WriteLine($"Output path not found.");
            }
            else
            {
                System.Console.WriteLine($"Output path: { outputPath }");
            }

            var serviceProvider = ConsoleBootstrapper.Bootstrap(configPath, outputPath);
            var generatorFacade = serviceProvider.GetService<IGeneratorFacade>();
            var result = generatorFacade.Generate();

            var consoleMessage = result.Success
                ? "Model generation succeeded."
                : "Model generation failed.";
            System.Console.WriteLine(consoleMessage);
            System.Console.WriteLine(result.Message);

            System.Console.ReadLine();
        }
    }
}