using Gunslinger.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Utilities;

namespace Tests.Tests
{
    [TestClass]
    public class GenesysTests
    {
        private readonly IGeneratorFacade _generatorFacade;

        public GenesysTests()
        {
            _generatorFacade = TestBootstrapper.GetGeneratorFacade("Configurations\\GenesysDevConfig.json");
        }

        [TestMethod]
        public void RunAllTemplates()
        {
            var result = _generatorFacade.Generate();
            Assert.IsTrue(result.Success, result.Message);
        }
    }
}