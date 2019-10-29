namespace CMC.AST
{
    public abstract class Primary : AST
    {
    }

    public class PrimaryIdentifier : Primary
    {
        public PrimaryIdentifier(Identifier identifier)
        {
            Identifier = identifier;
        }

        public Identifier Identifier { get; }
    }

    public abstract class PrimaryLiteral : Primary
    {
    }

    public class PrimaryBoolyLiteral : PrimaryLiteral
    {
        public PrimaryBoolyLiteral(BoolyLiteral value)
        {
            Value = value;
        }

        public BoolyLiteral Value { get; }
    }

    public class PrimaryIntyLiteral : PrimaryLiteral
    {
        public PrimaryIntyLiteral(IntyLiteral value)
        {
            Value = value;
        }

        public IntyLiteral Value { get; }
    }

    public class PrimaryFunctionCall : Primary
    {
        public PrimaryFunctionCall(FunctionCall functionCall)
        {
            FunctionCall = functionCall;
        }

        public FunctionCall FunctionCall { get; }
    }

    public class PrimaryExpression : Primary
    {
        public PrimaryExpression(Expression1 expression)
        {
            Expression = expression;
        }

        public Expression1 Expression { get; }
    }
}