using System.Collections.Generic;

namespace CMC.AST
{
    public class Program : AST
    {
        public List<Declaration> Declarations { get; }

        public Program(List<Declaration> declarations)
        {
            Declarations = declarations;
        }
    }
}