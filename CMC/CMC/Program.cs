﻿using System;
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
            var sourceFile = new SourceFile(@"testPudekcuf/PrintVariable.pudekcuf");
            //var scanner = new Scanner( sourceFile );

            var parser = new Parser(sourceFile);
            var program = parser.ParseProgram();
            var output = JsonConvert.SerializeObject(program, Formatting.Indented);

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
            Console.WriteLine(output);
            Console.WriteLine("\n\nFinished parsing file");


            Console.ReadKey();
        }
    }
}