using System.Collections.Generic;

namespace CMC.AST
{
    public abstract class ArgumentList
    {
    }

    public class ArgumentListNothing : ArgumentList
    {
    }

    public class ArgumentListSimple : ArgumentList
    {
        public ArgumentListSimple(Expression1 firstExpression, List<Expression1> otherExpressions)
        {
            FirstExpression = firstExpression;
            OtherExpressions = otherExpressions;
        }

        public Expression1 FirstExpression { get; }
        public List<Expression1> OtherExpressions { get; }
    }
}