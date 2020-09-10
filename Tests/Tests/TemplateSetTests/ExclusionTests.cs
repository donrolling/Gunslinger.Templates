using Gunslinger.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using Tests.Utilities;

namespace Tests.Tests
{
    [TestClass]
    public class ExclusionTests
    {
        private readonly IGeneratorFacade _generatorFacade;

        public ExclusionTests()
        {
            _generatorFacade = TestBootstrapper.GetGeneratorFacade("Configurations\\GenerationContextExclusions.json");
        }

        [TestMethod]
        public void RunAllTemplates()
        {
            var result = _generatorFacade.Generate();
            Assert.IsTrue(result.Success, result.Message);
            var di = new DirectoryInfo(_generatorFacade.Context.OutputDirectory);
            var templates = _generatorFacade.Context.Templates.First();
            Assert.IsNotNull(templates);
            Assert.IsTrue(templates.ExcludeTheseTypes.Any());
            var files = di.GetFiles();
            var any = files.Any(a => templates.ExcludeTheseTypes.Contains(a.Name.Replace(".cs", "")));
            Assert.IsFalse(any);
        }
    }
}