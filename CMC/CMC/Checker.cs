using CMC.AST;
using System.Collections.Generic;
using System.Linq;

namespace CMC
{



    public class Checker : IASTVisitor
    {

        private IDTable idTable = new IDTable();



        public object VisitArgumentList(ArgumentList argumentList, object o)
        {
            ParameterList x = o as ParameterList;

            for(int i = 0; i < argumentList.Arguments.Count; i++ )
            {
                argumentList.Arguments[ i ].Visit( this, x.Parameters[ i ].ParameterType ); //TODO WHAT SHOULD WE DO WITH THE RETURNM VALUE
            }
            return null;
        }

        public object VisitDeclarationVariableDeclaration(DeclarationVariableDeclaration declarationVariableDeclaration, object o)
        {
            // here we don't need external information

            //but we will store something into the scope table
            throw new System.NotImplementedException();
        }

        public object VisitDeclarationFunctionDeclaration(DeclarationFunctionDeclaration declarationFunctionDeclaration, object o)
        {
            // here we don't need external information

            //but we will store something into the scope table
            throw new System.NotImplementedException();
        }

        public object VisitDeclarationStruct(DeclarationStruct declarationStruct, object o)
        {
            // here we don't need external information

            //but we will store something into the scope table
            throw new System.NotImplementedException();
        }

        public object VisitVariableDeclarationSimple(VariableDeclarationSimple variableDeclarationSimple, object o)
        {
            // here we don't need external information

            //but we will store something into the scope table
            throw new System.NotImplementedException();
        }

        public object VisitVariableDeclarationStructVariableDeclaration(
            VariableDeclarationStructVariableDeclaration variableDeclarationStructVariableDeclaration, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitVariableDeclarationList(VariableDeclarationList variableDeclarationList, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitFunctionDeclaration(FunctionDeclaration functionDeclaration, object o)
        {
            idTable.Add( functionDeclaration.FunctionName, functionDeclaration );

            idTable.EnterNestedScopeLevel();

            functionDeclaration.ParameterList.Visit( this );
            //functionDeclaration.ReturnType.Visit( this ); // this must store what the return type must be on the ID table
            functionDeclaration.Statements.Visit( this,  functionDeclaration.ReturnType);

            idTable.ExitNestedScopeLevel();

            return null;
        }

        public object VisitExpression1(Expression1 expression1, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitExpression2(Expression2 expression2, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitExpression3(Expression3 expression3, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitParameterList(ParameterList parameterList, object o)
        {
            foreach(var item in parameterList.Parameters )
            {
                idTable.Add( item.ParameterName, item );
            }
            return null;
        }


        public object VisitPrimaryIdentifier(PrimaryIdentifier primaryIdentifier, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitPrimaryBoolyLiteral(PrimaryBoolyLiteral primaryBoolyLiteral, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitPrimaryIntyLiteral(PrimaryIntyLiteral primaryIntyLiteral, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitPrimaryFunctionCall(PrimaryFunctionCall primaryFunctionCall, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitPrimaryExpression(PrimaryExpression primaryExpression, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitProgram(AST.Program program, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitReturnVariableType(ReturnTypeVariableType returnTypeVariableType, object o)
        {
            //WE DONT THINK THIS IS USED
            throw new System.NotImplementedException();
        }

        public object VisitReturnTypeNothing(ReturnTypeNothing returnTypeNothing, object o)
        {
            //WE DONT THINK THIS IS USED
            throw new System.NotImplementedException();
        }

        public object VisitStatementIfStatement(StatementIfStatement statementIfStatement, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitStatementLoopStatement(StatementLoopStatement statementLoopStatement, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitStatementStopTheLoop(StatementStopTheLoop statementStopTheLoop, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitStatementGiveBack(StatementGiveBack statementGiveBack, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitStatements(Statements statements, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitStatementVariableDeclaration(StatementVariableDeclaration statementVariableDeclaration, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitStatementAssignment(StatementAssignment statementAssignment, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitStatementFunctionCall(StatementFunctionCall statementFunctionCall, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitStruct(Struct @struct, object o)
        {

            throw new System.NotImplementedException();
        }

        public object VisitStructVariableDeclaration(StructVariableDeclaration structVariableDeclaration, object o)
        {
            // here we don't need external information

            //but we will store something into the scope table
            throw new System.NotImplementedException();
        }

        public object VisitParameter( Parameter visitor, object o )
        {
            throw new System.NotImplementedException();
        }

        private class IDTable
        {
            private List<(int scopeLevel, UserCreatableID ID, AST.AST subTreePointer)> enviornment;
            private int _currentScopeLevel;

            public IDTable( )
            {
                _currentScopeLevel = 1;
                enviornment = new List<(int scopeLevel, UserCreatableID ID, AST.AST subTreePointer)>();
            }
            
            public void Add( UserCreatableID ID, AST.AST subTreePointer) {
                enviornment.Add( (_currentScopeLevel, ID, subTreePointer) );
            }

            public AST.AST Lookup( UserCreatableID ID )
            {
                var lst = enviornment.FindAll( item => item.ID.Spelling == ID.Spelling );
                var max = lst.Max( item => item.scopeLevel );
                
                return lst.Find( item => item.scopeLevel == max ).subTreePointer;
            }

            public void EnterNestedScopeLevel()
            {
                _currentScopeLevel++;
            }
            public void ExitNestedScopeLevel()
            {
                _currentScopeLevel--;
                enviornment.RemoveAll( item => item.scopeLevel > _currentScopeLevel );
            }




        }


    }
}