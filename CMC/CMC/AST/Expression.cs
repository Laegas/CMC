namespace CMC.AST
{
    public class Expression1 : AST
    {
        public Expression2 Expression2 { get; }
        public Operator1 Operator1 { get; }
        public Expression1 Expression1_ { get; }

        public Expression1(Expression2 expression2, Operator1 operator1, Expression1 expression1)
        {
            Expression2 = expression2;
            Operator1 = operator1;
            Expression1_ = expression1;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitExpression1(this, arg);
        }
    }

    public class Expression2 : AST
    {
        public Expression3 Expression3 { get; }
        public Operator2 Operator2 { get; }
        public Expression1 Expression1 { get; }

        public Expression2(Expression3 expression3, Operator2 operator2, Expression1 expression1)
        {
            Expression3 = expression3;
            Operator2 = operator2;
            Expression1 = expression1;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitExpression2(this, arg);
        }
    }

    public class Expression3 : AST
    {
        public Primary Primary { get; }
        public Operator3 Operator3 { get; }
        public Expression1 Expression1 { get; }

        public Expression3(Primary primary, Operator3 operator3, Expression1 expression1)
        {
            Primary = primary;
            Operator3 = operator3;
            Expression1 = expression1;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitExpression3(this, arg);
        }
    }
}