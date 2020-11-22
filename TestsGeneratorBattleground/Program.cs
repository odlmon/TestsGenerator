using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using TestsGenerator;

namespace TestsGeneratorBattleground
{
    class Program
    {
        private const int MaxDegreeOfParallelismRead = 3;
        private const int MaxDegreeOfParallelismGenerate = 3;
        private const int MaxDegreeOfParallelismWrite = 3;
        private const string TestsDestination = @"C:\Users\Dmitriy\Projects\CSharp\TestsGenerator\Tests\";
        private static readonly string[] FilesPaths = 
        {
            @"C:\Users\Dmitriy\Projects\CSharp\AssemblyBrowser\AssemblyReader\AssemblyReader.cs",
            @"C:\Users\Dmitriy\Projects\CSharp\Test\Test\Class1.cs",
            @"C:\Users\Dmitriy\Projects\CSharp\Test\Test\Class2.cs"
        };

        static void Main(string[] args)
        {
            var generator = new TestsGenerator.TestsGenerator();

            var loadSourceFile = new TransformBlock<string, string>(async path => 
            {
                Console.WriteLine($"Loading file ({path})...");
                using (var reader = new StreamReader(path))
                {
                    return await reader.ReadToEndAsync();
                }
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelismRead});

            var generateTests = new TransformManyBlock<string, TestClass>(async sourceCode => 
            {
                Console.WriteLine("Generating test classes...");
                return await generator.GenerateAsync(sourceCode);
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelismGenerate});

            var saveTestFile = new ActionBlock<TestClass>(async testClass => 
            {
                Console.WriteLine($"Saving {testClass.FileName} to {TestsDestination}...");
                using (StreamWriter writer = File.CreateText(TestsDestination + testClass.FileName))
                {
                    await writer.WriteAsync(testClass.SourceCode);
                }
                Console.WriteLine($"{testClass.FileName} was successfully saved.");
            }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelismWrite});

            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

            loadSourceFile.LinkTo(generateTests, linkOptions);
            generateTests.LinkTo(saveTestFile, linkOptions);
            FilesPaths.ToList().ForEach(path => loadSourceFile.Post(path));
            loadSourceFile.Complete();
            saveTestFile.Completion.Wait();
        }
    }
}