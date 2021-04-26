using Gunslinger.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using Tests.Utilities;

namespace Tests.Tests
{
    [TestClass]
    public class EntityTests
    {
        private readonly IGeneratorFacade _generatorFacade;

        public EntityTests()
        {
            _generatorFacade = TestBootstrapper.GetGeneratorFacade("Configurations\\GenerationContext.Entities.json");
        }

        [TestMethod]
        public void RunAllTemplates()
        {
            var result = _generatorFacade.Generate();
            Assert.IsTrue(result.Success, result.Message);
        }
    }
}