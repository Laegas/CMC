namespace CMC.AST
{
    public class FunctionCall : AST
    {
        public UserCreatableID FunctionName { get; }
        public ArgumentList ArgumentList { get; }
        
        public FunctionDeclaration FunctionDeclaration { get; set; }

        public FunctionCall(UserCreatableID functionName, ArgumentList argumentList)
        {
            FunctionName = functionName;
            ArgumentList = argumentList;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitFunctionCall(this, arg);
        }
    }
}