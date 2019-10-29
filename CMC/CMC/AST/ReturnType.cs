namespace CMC.AST
{
    public abstract class ReturnType : AST
    {
    }

    public class ReturnTypeVariableType : ReturnType
    {
        public ReturnTypeVariableType(VariableType variableType)
        {
            VariableType = variableType;
        }

        public VariableType VariableType { get; }
    }

    public class ReturnTypeNothing : ReturnType
    {
    }
}