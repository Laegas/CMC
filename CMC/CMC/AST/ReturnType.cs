namespace CMC.AST
{
    public abstract class ReturnType : AST
    {
        public abstract VariableType.ValueTypeEnum ValueType { get; }
    }

    public class ReturnTypeVariableType : ReturnType
    {
        public VariableType VariableType { get; }

        public override VariableType.ValueTypeEnum ValueType => VariableType.VariableType_;

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
        public override VariableType.ValueTypeEnum ValueType => VariableType.ValueTypeEnum.NOTHING;

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitReturnTypeNothing(this, arg);
        }
    }
}