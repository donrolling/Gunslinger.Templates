using Gunslinger.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Utilities;

namespace Tests.Tests
{
    [TestClass]
    public class WebAPIControllerProviderTests
    {
        private readonly IGeneratorFacade _generatorFacade;
        private readonly ITemplateOutputEngine _templateOutputEngine;
        
        public WebAPIControllerProviderTests()
        {
            _generatorFacade = TestBootstrapper.GetGeneratorFacade("Configurations\\WebAPIControllerProviderConfig.json");
            _templateOutputEngine = TestBootstrapper.ServiceProvider.GetService<ITemplateOutputEngine>();
            _templateOutputEngine.CleanupOutputDirectory(TestBootstrapper.GenerationContext.OutputDirectory);
        }

        [TestMethod]
        public void RunAllTemplates()
        {
            var result = _generatorFacade.Generate();
            Assert.IsTrue(result.Success);
        }
    }
}