namespace CMC.AST
{
    public abstract class ReturnType : AST
    {
    }

    public class ReturnTypeVariableType : ReturnType
    {
        public VariableType VariableType { get; }

        public ReturnTypeVariableType(VariableType variableType)
        {
            VariableType = variableType;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitReturnVariableType(this, arg);
        }
    }

    public class ReturnTypeNothing : ReturnType
    {
        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitReturnTypeNothing(this, arg);
        }
    }
}