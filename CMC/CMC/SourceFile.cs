using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CMC
{
    class SourceFile
    {
        private StreamReader StreamReader { get; set; }
        public SourceFile(string path)
        {
            StreamReader = File.OpenText(path);
        }

        public char GetChar()
        {
            return (char)StreamReader.Read();
        }
    }
}
