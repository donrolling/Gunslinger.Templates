using Gunslinger.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Utilities;

namespace Tests.Tests
{
    [TestClass]
    public class RunAllTemplatesTests
    {
        private readonly IGeneratorFacade _generatorFacade;

        public RunAllTemplatesTests()
        {
            _generatorFacade = TestBootstrapper.GetGeneratorFacade("Configurations\\GenerationContext.json");
        }

        [TestMethod]
        public void RunAllTemplates()
        {
            var result = _generatorFacade.Generate();
            Assert.IsTrue(result.Success);
        }
    }
}