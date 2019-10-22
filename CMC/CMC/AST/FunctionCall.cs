namespace CMC.AST
{
    public class FunctionCall
    {
        public UserCreatableID FunctionName { get; }
        public ArgumentList ArgumentList { get; }

        public FunctionCall(UserCreatableID functionName, ArgumentList argumentList)
        {
            FunctionName = functionName;
            ArgumentList = argumentList;
        }
    }
}