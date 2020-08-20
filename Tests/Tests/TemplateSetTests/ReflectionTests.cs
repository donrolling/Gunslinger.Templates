using Gunslinger.Interfaces;
using Gunslinger.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Utilities;

namespace Tests.Tests
{
    [TestClass]
    public class ReflectionTests
    {
        private readonly IGeneratorFacade _generatorFacade;

        public ReflectionTests()
        {
            _generatorFacade = TestBootstrapper.GetGeneratorFacade("Configurations\\ReflectionConfig.json");
        }

        [TestMethod]
        public void RunAllTemplates()
        {
            var result = _generatorFacade.Generate();
            Assert.IsTrue(result.Success);
        }
    }
}