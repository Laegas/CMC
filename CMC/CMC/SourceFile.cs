using System.IO;

namespace CMC
{
    public class SourceFile
    {
        private StreamReader StreamReader { get; }
        public SourceFile( string path )
        {
            StreamReader = File.OpenText( path );
        }

        public char GetChar()
        {
            return (char)StreamReader.Read();
        }
    }
}
