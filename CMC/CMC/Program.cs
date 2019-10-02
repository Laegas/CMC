using System;
using static CMC.Token;

namespace CMC
{
    class Program
    {
        public static void Main(string[] args)
        {
            var sourceFile = new SourceFile( @"testProgramForScanner.pudekcuf" );
            var scanner = new Scanner( sourceFile );

            while( true )
            {
                var token = scanner.ScanToken();
                Console.WriteLine( token.TheTokenType.ToString() + "(" + token.Spelling + ")" );
                if( token.TheTokenType.Equals(TokenType.END_OF_TEXT) )
                {
                    break;
                }
                if( token.TheTokenType.Equals( TokenType.ERROR ) )
                {
                    Console.WriteLine( token.TheTokenType.ToString() + "(" + token.Spelling + ")" );
                    //throw new Exception( "GOT A ERROR IN THE SCANNER" );
                }
            }

            Console.ReadKey();
        }
    }
}
