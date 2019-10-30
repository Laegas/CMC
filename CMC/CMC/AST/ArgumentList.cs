using System.Collections.Generic;

namespace CMC.AST
{
    public class ArgumentList : AST
    {
        public List<Expression1> Arguments { get; }

        public ArgumentList(List<Expression1> arguments)
        {
            Arguments = arguments;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitArgumentList(this, arg);
        }
    }
}