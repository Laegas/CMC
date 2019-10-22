using System.Collections.Generic;

namespace CMC.AST
{
    public class Expression1 : AST
    {
    }

    public class Expression3 : AST
    {
    }

    public abstract class Primary : AST
    {
    }

    public class PrimaryIdentifier : Primary
    {
        public UserCreatableID RootID { get; }
        public List<UserCreatableID> NestedIDs { get; }

        public PrimaryIdentifier(UserCreatableID rootId, List<UserCreatableID> nestedIDs)
        {
            RootID = rootId;
            NestedIDs = nestedIDs;
        }
    }

    public abstract class PrimaryLiteral : Primary
    {
    }

    public class PrimaryBoolyLiteral : PrimaryLiteral
    {
        public BoolyLiteral Value { get; }

        public PrimaryBoolyLiteral(BoolyLiteral value)
        {
            Value = value;
        }
    }

    public class PrimaryIntyLiteral : PrimaryLiteral
    {
        public IntyLiteral Value { get; }

        public PrimaryIntyLiteral(IntyLiteral value)
        {
            Value = value;
        }
    }

    public class PrimaryFunctionCall : Primary
    {
        public FunctionCall FunctionCall { get; }

        public PrimaryFunctionCall(FunctionCall functionCall)
        {
            FunctionCall = functionCall;
        }
    }

    public class PrimaryExpression : Primary
    {
        public Expression1 Expression { get; }

        public PrimaryExpression(Expression1 expression)
        {
            Expression = expression;
        }
    }
}