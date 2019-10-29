namespace CMC.AST
{
    public class Struct : AST
    {
        public Struct(UserCreatableID structName, VariableDeclarationList variableDeclarationList)
        {
            StructName = structName;
            VariableDeclarationList = variableDeclarationList;
        }

        public UserCreatableID StructName { get; }
        public VariableDeclarationList VariableDeclarationList { get; }
    }

    public class StructVariableDeclaration : AST
    {
        public StructVariableDeclaration(UserCreatableID structName, UserCreatableID variableName)
        {
            StructName = structName;
            VariableName = variableName;
        }

        public UserCreatableID StructName { get; }
        public UserCreatableID VariableName { get; }
    }
}