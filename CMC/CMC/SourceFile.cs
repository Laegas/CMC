using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CMC
{
    public class SourceFile
    {
        private StreamReader StreamReader { get; set; }
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
