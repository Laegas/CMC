using System.Collections.Generic;

namespace CMC.AST
{
    public abstract class Declaration : AST
    {
    }

    public class DeclarationVariableDeclaration : Declaration
    {
        public VariableDeclaration VariableDeclaration { get; }

        public DeclarationVariableDeclaration(VariableDeclaration variableDeclaration)
        {
            VariableDeclaration = variableDeclaration;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitDeclarationVariableDeclaration(this, arg);
        }
    }

    public class DeclarationFunctionDeclaration : Declaration
    {
        public FunctionDeclaration FunctionDeclaration { get; }

        public DeclarationFunctionDeclaration(FunctionDeclaration functionDeclaration)
        {
            FunctionDeclaration = functionDeclaration;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitDeclarationFunctionDeclaration(this, arg);
        }
    }

    public class DeclarationStruct : Declaration
    {
        public Struct Struct { get; }

        public DeclarationStruct(Struct @struct)
        {
            Struct = @struct;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitDeclarationStruct(this, arg);
        }
    }


    public abstract class VariableDeclaration : AST
    {
        public abstract UserCreatableID Name{ get; }
    }

    public class VariableDeclarationSimple : VariableDeclaration
    {
        public VariableType VariableType { get; }
        public UserCreatableID VariableName { get; }
        public Expression1 Expression { get; }

        public override UserCreatableID Name => VariableName;

        public VariableDeclarationSimple(VariableType variableType, UserCreatableID variableName,
            Expression1 expression)
        {
            VariableType = variableType;
            VariableName = variableName;
            Expression = expression;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitVariableDeclarationSimple(this, arg);
        }
    }

    public class VariableDeclarationStructVariableDeclaration : VariableDeclaration
    {
        public StructVariableDeclaration StructVariableDeclaration { get; }

        public override UserCreatableID Name => StructVariableDeclaration.VariableName;

        public VariableDeclarationStructVariableDeclaration(StructVariableDeclaration structVariableDeclaration)
        {
            StructVariableDeclaration = structVariableDeclaration;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitVariableDeclarationStructVariableDeclaration(this, arg);
        }
    }


    public class VariableDeclarationList : AST
    {
        public List<VariableDeclaration> VariableDeclarations { get; }

        public VariableDeclarationList(List<VariableDeclaration> variableDeclarations)
        {
            VariableDeclarations = variableDeclarations;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitVariableDeclarationList(this, arg);
        }
    }

    public class FunctionDeclaration : AST
    {
        public UserCreatableID FunctionName { get; }
        public ParameterList ParameterList { get; }
        public ReturnType ReturnType { get; }
        public Statements Statements { get; }

        public FunctionDeclaration(UserCreatableID functionName, ParameterList parameterList, ReturnType returnType,
            Statements statements)
        {
            FunctionName = functionName;
            ParameterList = parameterList;
            ReturnType = returnType;
            Statements = statements;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitFunctionDeclaration(this, arg);
        }
    }
}