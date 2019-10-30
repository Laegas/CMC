namespace CMC.AST
{
    public abstract class Primary : AST
    {
    }

    public class PrimaryIdentifier : Primary
    {
        public Identifier Identifier { get; }

        public PrimaryIdentifier(Identifier identifier)
        {
            Identifier = identifier;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitPrimaryIdentifier(this, arg);
        }
    }

    public class PrimaryBoolyLiteral : Primary
    {
        public BoolyLiteral Value { get; }

        public PrimaryBoolyLiteral(BoolyLiteral value)
        {
            Value = value;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitPrimaryBoolyLiteral(this, arg);
        }
    }

    public class PrimaryIntyLiteral : Primary
    {
        public IntyLiteral Value { get; }

        public PrimaryIntyLiteral(IntyLiteral value)
        {
            Value = value;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitPrimaryIntyLiteral(this, arg);
        }
    }

    public class PrimaryFunctionCall : Primary
    {
        public FunctionCall FunctionCall { get; }

        public PrimaryFunctionCall(FunctionCall functionCall)
        {
            FunctionCall = functionCall;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitPrimaryFunctionCall(this, arg);
        }
    }

    public class PrimaryExpression : Primary
    {
        public Expression1 Expression { get; }

        public PrimaryExpression(Expression1 expression)
        {
            Expression = expression;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitPrimaryExpression(this, arg);
        }
    }
}