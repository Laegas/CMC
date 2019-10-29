using System.IO;

namespace CMC
{
    public class SourceFile
    {
        public SourceFile(string path)
        {
            StreamReader = File.OpenText(path);
        }

        private StreamReader StreamReader { get; }

        public char GetChar()
        {
            return (char) StreamReader.Read();
        }
    }
}