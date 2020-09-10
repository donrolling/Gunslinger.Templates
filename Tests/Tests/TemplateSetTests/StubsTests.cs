using Gunslinger.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Tests.Utilities;

namespace Tests.Tests
{
    [TestClass]
    public class StubsTests
    {
        private readonly IGeneratorFacade _generatorFacade;

        public StubsTests()
        {
            _generatorFacade = TestBootstrapper.GetGeneratorFacade("Configurations\\GenerationContextAllStubs.json");
        }

        [TestMethod]
        public void RunAllTemplates()
        {
            var result = _generatorFacade.Generate();
            Assert.IsTrue(result.Success, result.Message);

            var di = new DirectoryInfo(_generatorFacade.Context.OutputDirectory);
            var files = di.GetFiles();
            Assert.AreEqual(0, files.Length);
            var folders = di.GetDirectories();
            Assert.AreEqual(0, folders.Length);
        }
    }
}