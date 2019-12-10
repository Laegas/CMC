using System;
using System.IO;
using Newtonsoft.Json;

namespace CMC
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            File.Delete(@"combiled.TAM");
            var sourceFile = new SourceFile(@"testPudekcuf/Functions.pudekcuf");
            //var scanner = new Scanner( sourceFile );

            var parser = new Parser(sourceFile);
            var program = parser.ParseProgram();
            var output = JsonConvert.SerializeObject(program, Formatting.Indented);

            Console.WriteLine(output);
            var checker = new SemanticCheckerAndDecorater();

            program.Visit( checker ); // semantic checker and tree decorator
            var encoder = new Encoder();
            encoder.Encode(program);
            encoder.SaveTargetProgram(AppContext.BaseDirectory + Encoder.FILE_NAME);

            Console.WriteLine("\n\nFinished combilation");
        }
    }
}