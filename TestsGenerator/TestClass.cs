namespace TestsGenerator
{
    public class TestClass
    {
        public string FileName { get; private set; }
        public string SourceCode { get; private set; }

        public TestClass(string fileName, string code)
        {
            FileName = fileName;
            SourceCode = code;
        }
    }
}