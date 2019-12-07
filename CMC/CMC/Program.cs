using System;
using System.IO;
using Newtonsoft.Json;

namespace CMC
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            /*
            Encoder.Testing();

            return;
            */
            File.Delete(@"combiled.TAM");
            var sourceFile = new SourceFile(@"testPudekcuf/BoolyMath.pudekcuf");
            //var scanner = new Scanner( sourceFile );

            var parser = new Parser(sourceFile);
            var program = parser.ParseProgram();
            var output = JsonConvert.SerializeObject(program, Formatting.Indented);

            Console.WriteLine(output);
            var checker = new SemanticCheckerAndDecorater();

            program.Visit( checker ); // semantic checker and tree decorater
            var encoder = new Encoder();
            encoder.Encode(program);
            encoder.SaveTargetProgram(AppContext.BaseDirectory + Encoder.FILE_NAME);
            //var scanner = new Scanner(sourceFile);
            //while( true )
            //{
            //    var token = scanner.ScanToken();
            //    Console.WriteLine( token.TheTokenType  + token.Spelling);
            //    if( Token.TokenType.END_OF_TEXT == token.TheTokenType )
            //    {
            //        break;
            //    }
            //}
            Console.WriteLine("\n\nFinished parsing file");

        }
    }
}