using Gunslinger.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
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
                System.Console.WriteLine($"Configuration path was not found.");
            }
            else
            {
                System.Console.WriteLine($"Configuration path: { configPath }");
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

            var templateDirectory = args.Skip(2).FirstOrDefault();
            if (string.IsNullOrEmpty(templateDirectory))
            {
                System.Console.WriteLine($"Template path not found.");
            }
            else
            {
                System.Console.WriteLine($"Template path: { templateDirectory }");
            }

            var processTemplateStubsVal = args.Skip(3).FirstOrDefault();
            var processTemplateStubs = true;
            if (string.IsNullOrEmpty(templateDirectory))
            {
                System.Console.WriteLine($"Process Template Stubs: true.");
            }
            else
            {
                processTemplateStubs = bool.Parse(processTemplateStubsVal);
                System.Console.WriteLine($"Process Template Stubs: { processTemplateStubsVal }");
            }

            var serviceProvider = ConsoleBootstrapper.Bootstrap(configPath, outputPath, templateDirectory, processTemplateStubs);
            var generatorFacade = serviceProvider.GetService<IGeneratorFacade>();
            var result = generatorFacade.Generate();

            var consoleMessage = result.Success
                ? "Model generation succeeded."
                : "Model generation failed.";
            if (!result.Success)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
            }
            System.Console.WriteLine(result.Message);
            System.Console.WriteLine(consoleMessage);
            System.Console.ForegroundColor = ConsoleColor.White;
        }
    }
}