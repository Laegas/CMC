using System;
using static CMC.Token;

namespace CMC
{
    class Program
    {
        static void Main(string[] args)
        {
            var sourceFile = new SourceFile( @"testProgramForScanner.pudekcuf" );
            //var scanner = new Scanner( sourceFile );

            var parser = new Parser(sourceFile);
            parser.ParseProgram();
            Console.WriteLine( "\n\nFinished parsing file" );
            //while( true )
            //{
            //    parser.

            //    //var token = scanner.ScanToken();
            //    //Console.WriteLine( token.TheTokenType.ToString() + "(" + token.Spelling + ")" );
            //    //if( token.TheTokenType.Equals(TokenType.END_OF_TEXT) )
            //    //{
            //    //    break;
            //    //}else if( token.TheTokenType.Equals( TokenType.ERROR ) )
            //    //{
            //    //    Console.WriteLine( token.TheTokenType.ToString() + "(" + token.Spelling + ")" );
            //    //    //throw new Exception( "GOT A ERROR IN THE SCANNER" );
            //    //}
            //}

            Console.ReadKey();
        }
    }
}
