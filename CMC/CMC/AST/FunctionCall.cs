namespace CMC.AST
{
    public class FunctionCall
    {
        public FunctionCall(UserCreatableID functionName, ArgumentList argumentList)
        {
            FunctionName = functionName;
            ArgumentList = argumentList;
        }

        public UserCreatableID FunctionName { get; }
        public ArgumentList ArgumentList { get; }
    }
}