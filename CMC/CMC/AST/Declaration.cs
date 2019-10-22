namespace CMC.AST
{
    public abstract class Declaration : AST
    {
    }

    public abstract class VariableDeclaration : Declaration
    {
    }

    public class VariableDeclarationSimple : VariableDeclaration
    {
        public VariableType VariableType { get; }
        public UserCreatableID UserCreatableID { get; }
        public Expression1 Expression { get; }
    }
}