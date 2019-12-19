using System;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace CMC
{
    internal class Program
    {
        const bool compileAll = true;
        private static void Main(string[] args)
        {
            var compiledPath = AppContext.BaseDirectory + "/compiled";
            Directory.CreateDirectory(compiledPath);

            var files = Directory.GetFiles(compiledPath);

            files.ToList().ForEach(item => {
                if (item.Contains(".TAM"))
                    File.Delete(item);
            });
            
            string[] pudekcufFiles = { AppContext.BaseDirectory + "testPudekcuf/Debug.pudekcuf"};
            if (compileAll)
            {
                pudekcufFiles = Directory.GetFiles("testPudekcuf");
            }

            foreach (var path in pudekcufFiles)
            {
                var sourceFile = new SourceFile(path);
                //var scanner = new Scanner( sourceFile );

                var parser = new Parser(sourceFile);
                var program = parser.ParseProgram();
                var output = JsonConvert.SerializeObject(program, Formatting.Indented);

                Console.WriteLine(output);
                var checker = new SemanticCheckerAndDecorater();

                program.Visit(checker); // semantic checker and tree decorator
                var encoder = new Encoder();
                encoder.Encode(program);
                encoder.SaveTargetProgram(AppContext.BaseDirectory + @"/compiled/" + Path.GetFileName(path) + ".TAM");
            }

           

            Console.WriteLine("\n\nFinished combilation");
        }
    }
}