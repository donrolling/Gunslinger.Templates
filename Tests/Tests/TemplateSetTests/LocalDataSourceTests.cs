using Gunslinger.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Utilities;

namespace Tests.Tests
{
    [TestClass]
    public class LocalDataSourceTests
    {
        private readonly IGeneratorFacade _generatorFacade;

        public LocalDataSourceTests()
        {
            _generatorFacade = TestBootstrapper.GetGeneratorFacade("Configurations\\SwaggerConfigLocalDataSource.json");
        }

        [TestMethod]
        public void RunAllTemplates()
        {
            var result = _generatorFacade.Generate();
            Assert.IsTrue(result.Success);
        }
    }
}