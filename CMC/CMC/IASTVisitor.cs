using CMC.AST;

namespace CMC
{
    public interface IASTVisitor
    {
        object VisitArgumentList(ArgumentList argumentList, object o);
        object VisitDeclarationVariableDeclaration(DeclarationVariableDeclaration declarationVariableDeclaration, object o);
        object VisitDeclarationFunctionDeclaration(DeclarationFunctionDeclaration declarationFunctionDeclaration, object o);
        object VisitDeclarationStruct(DeclarationStruct declarationStruct, object o);
        object VisitVariableDeclarationSimple(VariableDeclarationSimple variableDeclarationSimple, object o);
        object VisitVariableDeclarationStructVariableDeclaration(VariableDeclarationStructVariableDeclaration variableDeclarationStructVariableDeclaration, object o);
        object VisitVariableDeclarationList(VariableDeclarationList variableDeclarationList, object o);
        object VisitFunctionDeclaration(FunctionDeclaration functionDeclaration, object o);
        object VisitExpression1(Expression1 expression1, object o);
        object VisitExpression2(Expression2 expression2, object o);
        object VisitExpression3(Expression3 expression3, object o);
        object VisitParameterList(ParameterList parameterList, object o);
        object VisitParameter( Parameter parameter, object o );
        object VisitPrimaryIdentifier(PrimaryIdentifier primaryIdentifier, object o);
        object VisitPrimaryBoolyLiteral(PrimaryBoolyLiteral primaryBoolyLiteral, object o);
        object VisitPrimaryIntyLiteral(PrimaryIntyLiteral primaryIntyLiteral, object o);
        object VisitPrimaryFunctionCall(PrimaryFunctionCall primaryFunctionCall, object o);
        object VisitPrimaryExpression(PrimaryExpression primaryExpression, object o);
        object VisitProgram(AST.Program program, object o);
        object VisitReturnVariableType(ReturnTypeVariableType returnTypeVariableType, object o);
        object VisitReturnTypeNothing(ReturnTypeNothing returnTypeNothing, object o);
        object VisitStatementIfStatement(StatementIfStatement statementIfStatement, object o);
        object VisitStatementLoopStatement(StatementLoopStatement statementLoopStatement, object o);
        object VisitStatementStopTheLoop(StatementStopTheLoop statementStopTheLoop, object o);
        object VisitStatementGiveBack(StatementGiveBack statementGiveBack, object o);
        object VisitStatements(Statements statements, object o);
        object VisitStatementVariableDeclaration(StatementVariableDeclaration statementVariableDeclaration, object o);
        object VisitStatementAssignment(StatementAssignment statementAssignment, object o);
        object VisitStatementFunctionCall(StatementFunctionCall statementFunctionCall, object o);
        object VisitStruct(Struct @struct, object o);
        object VisitStructVariableDeclaration(StructVariableDeclaration structVariableDeclaration, object o);

        object VisitFunctionCall(FunctionCall functionCall, object o);
        object VisitIdentifier(Identifier identifier, object o);


    }
}