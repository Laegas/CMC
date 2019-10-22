using System;
using System.Collections.Generic;

namespace CMC.AST
{
    public abstract class ArgumentList
    {
    }

    public class NothingArgumentList : ArgumentList
    {
    }

    public class NonEmptyArgumentList : ArgumentList
    {
        public Expression1 FirstExpression { get; }
        public List<Expression1> OtherExpressions { get; }

        public NonEmptyArgumentList(Expression1 firstExpression, List<Expression1> otherExpressions)
        {
            FirstExpression = firstExpression;
            OtherExpressions = otherExpressions;
        }
    }
}