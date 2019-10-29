using System;

namespace CMC
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var sourceFile = new SourceFile(@"testProgramForScanner.pudekcuf");
            //var scanner = new Scanner( sourceFile );

            var parser = new Parser(sourceFile);
            parser.ParseProgram();
            var scanner = new Scanner(sourceFile);
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


            Console.ReadKey();
        }
    }
}