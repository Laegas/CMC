using System.Collections.Generic;

namespace CMC.AST
{
    public abstract class Declaration : AST
    {
    }

    public class DeclarationVariableDeclaration : Declaration
    {
        public DeclarationVariableDeclaration(VariableDeclaration variableDeclaration)
        {
            VariableDeclaration = variableDeclaration;
        }

        public VariableDeclaration VariableDeclaration { get; }
    }

    public class DeclarationFunctionDeclaration : Declaration
    {
        public DeclarationFunctionDeclaration(FunctionDeclaration functionDeclaration)
        {
            FunctionDeclaration = functionDeclaration;
        }

        public FunctionDeclaration FunctionDeclaration { get; }
    }

    public class DeclarationStruct : Declaration
    {
        public DeclarationStruct(Struct @struct)
        {
            Struct = @struct;
        }

        public Struct Struct { get; }
    }


    public abstract class VariableDeclaration : AST
    {
    }

    public class VariableDeclarationSimple : VariableDeclaration
    {
        public VariableDeclarationSimple(VariableType variableType, UserCreatableID variableName,
            Expression1 expression)
        {
            VariableType = variableType;
            VariableName = variableName;
            Expression = expression;
        }

        public VariableType VariableType { get; }
        public UserCreatableID VariableName { get; }
        public Expression1 Expression { get; }
    }

    public class VariableDeclarationStructVariableDeclaration : VariableDeclaration
    {
        public VariableDeclarationStructVariableDeclaration(StructVariableDeclaration structVariableDeclaration)
        {
            StructVariableDeclaration = structVariableDeclaration;
        }

        public StructVariableDeclaration StructVariableDeclaration { get; }
    }


    public class VariableDeclarationList : AST
    {
        public VariableDeclarationList(List<VariableDeclaration> variableDeclarations)
        {
            VariableDeclarations = variableDeclarations;
        }

        public List<VariableDeclaration> VariableDeclarations { get; }
    }

    public class FunctionDeclaration : AST
    {
        public FunctionDeclaration(UserCreatableID functionName, ParameterList parameterList, ReturnType returnType,
            Statements statements)
        {
            FunctionName = functionName;
            ParameterList = parameterList;
            ReturnType = returnType;
            Statements = statements;
        }

        public UserCreatableID FunctionName { get; }
        public ParameterList ParameterList { get; }
        public ReturnType ReturnType { get; }
        public Statements Statements { get; }
    }
}