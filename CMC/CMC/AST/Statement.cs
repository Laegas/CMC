using System.Collections.Generic;

namespace CMC.AST
{
    public abstract class Statement : AST
    {
    }

    public class StatementVariableDeclaration : Statement
    {
        public VariableDeclaration VariableDeclaration { get; }

        public StatementVariableDeclaration(VariableDeclaration variableDeclaration)
        {
            VariableDeclaration = variableDeclaration;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitStatementVariableDeclaration(this, arg);
        }
    }

    public class StatementAssignment : Statement
    {
        public Identifier Identifier { get; }
        public Expression1 Expression { get; }

        public StatementAssignment(Identifier identifier, Expression1 expression)
        {
            Identifier = identifier;
            Expression = expression;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitStatementAssignment(this, arg);
        }
    }

    public class StatementFunctionCall : Statement
    {
        public FunctionCall FunctionCall { get; }

        public StatementFunctionCall(FunctionCall functionCall)
        {
            FunctionCall = functionCall;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitStatementFunctionCall(this, arg);
        }
    }

    public class StatementIfStatement : Statement
    {
        public Expression1 Condition { get; }
        public Statements Statements { get; }

        public StatementIfStatement(Expression1 condition, Statements statements)
        {
            Condition = condition;
            Statements = statements;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitStatementIfStatement(this, arg);
        }
    }

    public class StatementLoopStatement : Statement
    {
        public Expression1 Condition { get; }
        public Statements Statements { get; }

        public StatementLoopStatement(Expression1 condition, Statements statements)
        {
            Condition = condition;
            Statements = statements;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitStatementLoopStatement(this, arg);
        }
    }

    public class StatementStopTheLoop : Statement
    {
        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitStatementStopTheLoop(this, arg);
        }
    }

    public class StatementGiveBack : Statement
    {
        public Expression1 Expression { get; }

        public StatementGiveBack(Expression1 expression)
        {
            Expression = expression;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitStatementGiveBack(this, arg);
        }
    }

    public class Statements : AST
    {
        public List<Statement> Statements_ { get; }

        public Statements(List<Statement> statements)
        {
            Statements_ = statements;
        }

        public override object Visit(IASTVisitor visitor, object arg = null)
        {
            return visitor.VisitStatements(this, arg);
        }
    }
}