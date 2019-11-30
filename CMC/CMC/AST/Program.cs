using System.Collections.Generic;

namespace CMC.AST
{
    public class Program : AST
    {
        public List<Declaration> Declarations { get; }
        public DeclarationFunctionDeclaration StartDeclaration { get; set; }
        public Program(List<Declaration> declarations)
        {
            Declarations = declarations;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitProgram(this, arg);
        }
    }
}