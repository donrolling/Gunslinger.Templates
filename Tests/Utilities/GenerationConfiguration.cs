using CompositionRoot;
using Gunslinger.Models;
using System.IO;

namespace Tests.Utilities
{
    public static class GenerationConfiguration
    {
        public static GenerationContext GetGenerationConfiguration(string pathToGenerationContext)
        {
            if (pathToGenerationContext.Contains(":"))
            {
                return Bootstrapper.GetJSONConfigFile<GenerationContext>(pathToGenerationContext);
            }
            var currentDirectory = Directory.GetCurrentDirectory();
            return Bootstrapper.GetJSONConfigFile<GenerationContext>($"{ currentDirectory }\\{ pathToGenerationContext }");
        }
    }
}