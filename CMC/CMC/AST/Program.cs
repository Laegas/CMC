using System.Collections.Generic;

namespace CMC.AST
{
    public class Program : AST
    {
        public Program(List<Declaration> declarations)
        {
            Declarations = declarations;
        }

        public List<Declaration> Declarations { get; }
    }
}