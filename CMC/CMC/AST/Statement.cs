using System.Collections.Generic;

namespace CMC.AST
{
    public abstract class Statement : AST
    {
    }

    public class StatementVariableDeclaration : Statement
    {
        public StatementVariableDeclaration(VariableDeclaration variableDeclaration)
        {
            VariableDeclaration = variableDeclaration;
        }

        public VariableDeclaration VariableDeclaration { get; }
    }

    public class StatementAssignment : Statement
    {
        public StatementAssignment(Identifier identifier, Expression1 expression)
        {
            Identifier = identifier;
            Expression = expression;
        }

        public Identifier Identifier { get; }
        public Expression1 Expression { get; }
    }

    public class StatementFunctionCall : Statement
    {
        public StatementFunctionCall(FunctionCall functionCall)
        {
            FunctionCall = functionCall;
        }

        public FunctionCall FunctionCall { get; }
    }

    public class StatementIfStatement : Statement
    {
        public StatementIfStatement(Expression1 condition, Statements statements)
        {
            Condition = condition;
            Statements = statements;
        }

        public Expression1 Condition { get; }
        public Statements Statements { get; }
    }

    public class StatementLoopStatement : Statement
    {
        public StatementLoopStatement(Expression1 condition, Statements statements)
        {
            Condition = condition;
            Statements = statements;
        }

        public Expression1 Condition { get; }
        public Statements Statements { get; }
    }

    public class StatementStopTheLoop : Statement
    {
    }

    public class StatementGiveBack : Statement
    {
        public StatementGiveBack(Expression1 expression)
        {
            Expression = expression;
        }

        public Expression1 Expression { get; }
    }

    public class Statements : AST
    {
        public Statements(List<Statement> statements)
        {
            Statements_ = statements;
        }

        public List<Statement> Statements_ { get; }
    }
}