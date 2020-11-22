using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestsGeneratorTester
{
    [TestClass]
    public class UnitTest
    {
        private TestsGenerator.TestsGenerator _generator = new TestsGenerator.TestsGenerator();

        private const string EmptyClassPath =
            @"C:\Users\Dmitriy\Projects\CSharp\TestsGenerator\TestsGeneratorTester\EmptyClass.cs";

        private const string ClassWithoutPublicMethodsPath =
            @"C:\Users\Dmitriy\Projects\CSharp\TestsGenerator\TestsGeneratorTester\ClassWithoutPublicMethods.cs";
        
        private const string TwoClassInOneFilePath =
            @"C:\Users\Dmitriy\Projects\CSharp\TestsGenerator\TestsGeneratorTester\TwoClassInOneFile.cs";

        private const string OuterClassPath = @"C:\Users\Dmitriy\Projects\CSharp\Test\Test\Class1.cs";

            [TestMethod]
        public void EmptyClassTest()
        {
            var sourceCode = File.ReadAllText(EmptyClassPath);
            var tests = _generator.Generate(sourceCode);
            Assert.AreEqual(0, tests.Count);
        }

        [TestMethod]
        public void ClassWithoutPublicMethodsTest()
        {
            var sourceCode = File.ReadAllText(ClassWithoutPublicMethodsPath);
            var tests = _generator.Generate(sourceCode);
            Assert.AreEqual(0, tests.Count);
        }

        [TestMethod]
        public void TwoClassInOneFileTest()
        {
            var sourceCode = File.ReadAllText(TwoClassInOneFilePath);
            var tests = _generator.Generate(sourceCode);
            Assert.AreEqual(2, tests.Count);
        }

        [TestMethod]
        public void OuterClassTest()
        {
            var sourceCode = File.ReadAllText(OuterClassPath);
            var tests = _generator.Generate(sourceCode);
            Assert.AreEqual(1, tests.Count);
            Assert.AreEqual("Class1UnitTest.cs", tests[0].FileName);
        }
    }
}