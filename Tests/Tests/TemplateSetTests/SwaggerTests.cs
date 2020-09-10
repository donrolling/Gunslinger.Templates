using Gunslinger.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using Tests.Utilities;

namespace Tests.Tests
{
    [TestClass]
    public class SwaggerTests
    {
        private readonly IGeneratorFacade _generatorFacade;

        public SwaggerTests()
        {
            _generatorFacade = TestBootstrapper.GetGeneratorFacade("Configurations\\SwaggerConfig.json");
        }

        [TestMethod]
        public void RunAllTemplates()
        {
            var result = _generatorFacade.Generate();
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void RunAllTemplates_04_22_2020()
        {
            var generatorFacade = TestBootstrapper.GetGeneratorFacade("Configurations\\SwaggerConfig.04.22.2020.json");
            var result = generatorFacade.Generate();
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void RunAllTemplates_05_21_2020()
        {
            var generatorFacade = TestBootstrapper.GetGeneratorFacade("Configurations\\SwaggerConfig.05.21.2020.json");
            var result = generatorFacade.Generate();
            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void RunAllTemplates_05_21_2020_DoNotCleanDirectoryPriorToOutput()
        {
            var generatorFacade = TestBootstrapper.GetGeneratorFacade("Configurations\\SwaggerConfig.05.21.2020.json");
            var template = generatorFacade.Context.Templates.First();
            var relativeOutputPath = template.OutputRelativePath.Replace("{entityName}", "RunAllTemplates_05_21_2020_DoNotCleanDirectoryPriorToOutput");
            var path = $"{ generatorFacade.Context.OutputDirectory }{ relativeOutputPath }";
            File.WriteAllText(path, "Test");
            try
            {
                var result = generatorFacade.Generate();
                Assert.IsTrue(result.Success);
                var exists = File.Exists(path);
                Assert.IsTrue(exists, $"File should still exist: { path }");
            }
            finally
            {
                File.Delete(path);
            }
        }

        [TestMethod]
        public void RunAllTemplates_05_21_2020_RenameOneFile()
        {
            var generatorFacade = TestBootstrapper.GetGeneratorFacade("Configurations\\SwaggerConfig.05.21.2020.Rename.json");
            var result = generatorFacade.Generate();
            var template = generatorFacade.Context.Templates.First();
            var badFilePath = Path.Join(generatorFacade.Context.OutputDirectory, template.OutputRelativePath.Replace("{entityName}", "Item"));
            var goodFilePath = Path.Join(generatorFacade.Context.OutputDirectory, template.OutputRelativePath.Replace("{entityName}", "PolicyEntityGraph"));
            var badFile = new FileInfo(badFilePath);
            var goodFile = new FileInfo(goodFilePath);
            Assert.IsTrue(result.Success, $"This process should have succeeded. Error: { result.Message }");
            Assert.IsFalse(badFile.Exists, $"{ badFilePath } shouldn't exist.");
            Assert.IsTrue(goodFile.Exists, $"{ goodFilePath } should exist.");
            var contents = File.ReadAllText(goodFilePath);
            Assert.IsFalse(contents.Contains("public class Item"), "File should have had its contents replaced to reflect the new name.");
            Assert.IsTrue(contents.Contains("public class PolicyEntityGraph"), "File should have had its contents replaced to reflect the new name.");
        }
    }
}