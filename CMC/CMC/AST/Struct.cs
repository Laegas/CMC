namespace CMC.AST
{
    public class Struct : AST
    {
        public UserCreatableID StructName { get; }
        public VariableDeclarationList VariableDeclarationList { get; }

        public Struct(UserCreatableID structName, VariableDeclarationList variableDeclarationList)
        {
            StructName = structName;
            VariableDeclarationList = variableDeclarationList;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitStruct(this, arg);
        }
    }

    public class StructVariableDeclaration : AST
    {
        public UserCreatableID StructName { get; }
        public UserCreatableID VariableName { get; }

        public StructVariableDeclaration(UserCreatableID structName, UserCreatableID variableName)
        {
            StructName = structName;
            VariableName = variableName;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitStructVariableDeclaration(this, arg);
        }
    }
}