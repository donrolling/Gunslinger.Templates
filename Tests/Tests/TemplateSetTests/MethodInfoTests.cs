using Gunslinger.Interfaces;
using Gunslinger.Models;
using Gunslinger.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tests.Utilities;

namespace Tests.Tests
{
    [TestClass]
    public class MethodInfoTests
    {
        private readonly IGeneratorFacade _generatorFacade;
        private readonly ITemplateOutputEngine _templateOutputEngine;

        public MethodInfoTests()
        {
            _generatorFacade = TestBootstrapper.GetGeneratorFacade("Configurations\\MethodInfoConfig.json");
            _templateOutputEngine = TestBootstrapper.ServiceProvider.GetService<ITemplateOutputEngine>();
            _templateOutputEngine.CleanupOutputDirectory(TestBootstrapper.GenerationContext.OutputDirectory);
        }

        [TestMethod]
        public void RunAllTemplates()
        {
            var result = _generatorFacade.Generate();
            Assert.IsTrue(result.Success, result.Message);
        }

        [TestMethod]
        public void TypeConversion_StringType()
        {
            var type = typeof(System.String);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual(result.Name.Value, "string");
        }

        [TestMethod]
        public void TypeConversion_TaskComplexType()
        {
            var type = typeof(Task<Model>);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual("Model", result.Name.Value);
            Assert.IsTrue(result.IsTask);
        }

        [TestMethod]
        public void TypeConversion_TaskListComplexType()
        {
            var type = typeof(Task<List<Model>>);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual("Model", result.Name.Value);
            Assert.IsTrue(result.IsList);
            Assert.IsTrue(result.IsTask);
        }

        [TestMethod]
        public void TypeConversion_NullableType()
        {
            var type = typeof(int?);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual("int", result.Name.Value);
            Assert.IsTrue(result.IsNullable);
        }

        [TestMethod]
        public void TypeConversion_ListOfNullableType()
        {
            var type = typeof(List<int?>);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual("int", result.Name.Value);
            Assert.IsTrue(result.IsList);
            Assert.IsTrue(result.IsNullable);
            Assert.AreEqual("List", result.ListType);
        }

        /// <summary>
        /// The compiler doesn't seem to acknoweldge the difference between IEnumerable and List at this level.
        /// </summary>
        [TestMethod]
        public void TypeConversion_IEnumerableOfType()
        {
            var type = typeof(IEnumerable<int>);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual("int", result.Name.Value);
            Assert.IsTrue(result.IsList);
            Assert.AreEqual("IEnumerable", result.ListType);
        }

        [TestMethod]
        public void TypeConversion_IDictionaryOfType()
        {
            var type = typeof(IDictionary<string, Model>);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual("Model", result.Name.Value);
            Assert.IsTrue(result.IsDictionary);
            Assert.AreEqual("string", result.KeyType);
            Assert.AreEqual("IDictionary", result.ListType);
        }

        [TestMethod]
        public void TypeConversion_DictionaryOfType()
        {
            var type = typeof(Dictionary<string, Model>);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual("Model", result.Name.Value);
            Assert.IsTrue(result.IsDictionary);
            Assert.AreEqual("string", result.KeyType);
            Assert.AreEqual("Dictionary", result.ListType);
        }

        [TestMethod]
        public void TypeConversion_ArrayType()
        {
            var type = typeof(int[]);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual("int", result.Name.Value);
            Assert.IsTrue(result.IsList);
            Assert.AreEqual("Array", result.ListType);
        }

        [TestMethod]
        public void TypeConversion_ArrayComplexType()
        {
            var type = typeof(Model[]);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual("Model", result.Name.Value);
            Assert.IsTrue(result.IsList);
            Assert.AreEqual("Array", result.ListType);
        }

        [TestMethod]
        public void TypeConversion_ArrayNullableType()
        {
            var type = typeof(int?[]);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual("int", result.Name.Value);
            Assert.IsTrue(result.IsNullable);
            Assert.IsTrue(result.IsList);
            Assert.AreEqual("Array", result.ListType);
        }

        [TestMethod]
        public void TypeConversion_TaskArrayNullableType()
        {
            var type = typeof(Task<int?[]>);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual("int", result.Name.Value);
            Assert.IsTrue(result.IsNullable);
            Assert.IsTrue(result.IsList);
            Assert.IsTrue(result.IsTask);
            Assert.AreEqual("Array", result.ListType);
        }

        [TestMethod]
        public void TypeConversion_DoubleType()
        {
            var type = typeof(double);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual("double", result.Name.Value);
        }

        [TestMethod]
        public void TypeConversion_FloatType()
        {
            var type = typeof(float);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual("single", result.Name.Value);
        }

        [TestMethod]
        public void TypeConversion_SingleType()
        {
            var type = typeof(Single);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual("single", result.Name.Value);
        }

        [TestMethod]
        public void TypeConversion_CharType()
        {
            var type = typeof(char);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual("char", result.Name.Value);
        }

        [TestMethod]
        public void TypeConversion_BooleanType()
        {
            var type = typeof(bool);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual("bool", result.Name.Value);
        }

        [TestMethod]
        public void TypeConversion_uintType()
        {
            var type = typeof(uint);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual("uint", result.Name.Value);
        }

        [TestMethod]
        public void TypeConversion_shortType()
        {
            var type = typeof(short);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual("short", result.Name.Value);
        }

        [TestMethod]
        public void TypeConversion_ushortType()
        {
            var type = typeof(ushort);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual("ushort", result.Name.Value);
        }

        [TestMethod]
        public void TypeConversion_ulongType()
        {
            var type = typeof(ulong);
            var result = ReflectionDataTypeConversion.Convert_ReflectionDataType_to_CSDataType(type);
            Assert.AreEqual("ulong", result.Name.Value);
        }
    }
}