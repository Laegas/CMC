using System;
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
            var parameterList = o as ParameterList;

            for (int i = 0; i < argumentList.Arguments.Count; i++)
            {
                argumentList.Arguments[i]
                    .Visit(this,
                        parameterList.Parameters[i].ParameterType); //TODO WHAT SHOULD WE DO WITH THE RETURNM VALUE
            }

            return null;
        }

        public object VisitDeclarationVariableDeclaration(DeclarationVariableDeclaration declarationVariableDeclaration,
            object o)
        {
            // here we don't need external information

            //but we will store something into the scope table
            throw new System.NotImplementedException();
        }

        public object VisitDeclarationFunctionDeclaration(DeclarationFunctionDeclaration declarationFunctionDeclaration,
            object o)
        {
            declarationFunctionDeclaration.FunctionDeclaration.Visit(this, null);
            return null;
        }

        public object VisitDeclarationStruct(DeclarationStruct declarationStruct, object o)
        {
            // here we don't need external information

            //but we will store something into the scope table
            throw new System.NotImplementedException();
        }

        public object VisitVariableDeclarationSimple(VariableDeclarationSimple variableDeclarationSimple, object o)
        {
            idTable.Add(variableDeclarationSimple.VariableName, variableDeclarationSimple, IDTable.DeclarationType.VARIABLE);
            return null;
        }

        public object VisitVariableDeclarationStructVariableDeclaration(
            VariableDeclarationStructVariableDeclaration variableDeclarationStructVariableDeclaration, object o)
        {
            variableDeclarationStructVariableDeclaration.StructVariableDeclaration.Visit(this, null);
            return null;
        }

        public object VisitVariableDeclarationList(VariableDeclarationList variableDeclarationList, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitFunctionDeclaration(FunctionDeclaration functionDeclaration, object o)
        {
            idTable.Add(functionDeclaration.FunctionName, functionDeclaration, IDTable.DeclarationType.FUNCTION);

            idTable.EnterNestedScopeLevel();

            functionDeclaration.ParameterList.Visit(this);
            //functionDeclaration.ReturnType.Visit( this ); // this must store what the return type must be on the ID table
            functionDeclaration.Statements.Visit(this, functionDeclaration.ReturnType);

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
            //this is all it needs to be, we think
            return null;
        }


        public object VisitPrimaryIdentifier(PrimaryIdentifier primaryIdentifier, object o)
        {
            return primaryIdentifier.Identifier.Visit(this, null);
            
        }

        public object VisitPrimaryBoolyLiteral(PrimaryBoolyLiteral primaryBoolyLiteral, object o)
        {
            return VariableType.VariableTypeEnum.BOOLY;
        }

        public object VisitPrimaryIntyLiteral(PrimaryIntyLiteral primaryIntyLiteral, object o)
        {
            return VariableType.VariableTypeEnum.INTY;
        }

        public object VisitPrimaryFunctionCall(PrimaryFunctionCall primaryFunctionCall, object o)
        {
            primaryFunctionCall.FunctionCall.Visit(this, o);
            return null;
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
            idTable.EnterNestedScopeLevel();
            
            var variableType = (VariableType)statementIfStatement.Condition.Visit(this);
            if (variableType.VariableType_ != VariableType.VariableTypeEnum.BOOLY)
            {
                throw new Exception("If statement condition must evaluate to booly value");
            }

            statementIfStatement.Statements.Visit(this);
            
            idTable.ExitNestedScopeLevel();

            return null;
        }
        
        public object VisitStatementLoopStatement(StatementLoopStatement statementLoopStatement, object o)
        {
            idTable.EnterNestedScopeLevel(true);
            
            var variableType = (VariableType)statementLoopStatement.Condition.Visit(this);
            if (variableType.VariableType_ != VariableType.VariableTypeEnum.BOOLY)
            {
                throw new Exception("Loop statement condition must evaluate to a booly value");
            }

            statementLoopStatement.Statements.Visit(this);
            
            idTable.ExitNestedScopeLevel();
            
            return null;
        }

        public object VisitStatementStopTheLoop(StatementStopTheLoop statementStopTheLoop, object o)
        {
            if (!idTable.IsInLoopScope())
            {
                throw new Exception("Stop the loop statement must be used only inside a loop");
            }

            return null;
        }

        public object VisitStatementGiveBack(StatementGiveBack statementGiveBack, object o)
        {
            if (o is ReturnTypeVariableType returnType)
            {
                var expressionVariableType = (VariableType)statementGiveBack.Expression.Visit(this);
                if (expressionVariableType.VariableType_ != returnType.VariableType.VariableType_)
                {
                    throw new Exception("Give back must return value of expected type");
                }
            }
            else
            {
                if (statementGiveBack.Expression != null)
                {
                    throw new Exception("Statement give back should not return a value when function return type is nothing");
                }
            }

            return null;
        }

        public object VisitStatements(Statements statements, object o)
        {
            if (o is ReturnTypeVariableType)
            {
                if (!statements.Statements_.Any(statement => statement is StatementGiveBack))
                {
                    throw new Exception("No giveback statements in the function");
                }
            }

            foreach (var statement in statements.Statements_)
            {
                statement.Visit(this, o);
            }

            return null;
        }

        public object VisitStatementVariableDeclaration(StatementVariableDeclaration statementVariableDeclaration,
            object o)
        {
            statementVariableDeclaration.VariableDeclaration.Visit(this, null);
            return null;
        }

        public object VisitStatementAssignment(StatementAssignment statementAssignment, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitStatementFunctionCall(StatementFunctionCall statementFunctionCall, object o)
        {
            statementFunctionCall.FunctionCall.Visit(this, null);
            return null;
        }

        public object VisitStruct(Struct @struct, object o)
        {
            idTable.Add(@struct.StructName, @struct, IDTable.DeclarationType.STRUCT);
            return null;
        }

        public object VisitStructVariableDeclaration(StructVariableDeclaration structVariableDeclaration, object o)
        {
            idTable.Add(structVariableDeclaration.VariableName, structVariableDeclaration, IDTable.DeclarationType.VARIABLE);
            return null;
        }

        public object VisitParameter(Parameter visitor, object o)
        {
            throw new System.NotImplementedException();
        }

        public object VisitFunctionCall(FunctionCall functionCall, object o)
        {
            FunctionDeclaration dec = (FunctionDeclaration)idTable.Lookup(functionCall.FunctionName, IDTable.DeclarationType.FUNCTION);

            functionCall.ArgumentList.Visit(this, dec.ParameterList);

            return null;
        }

        public object VisitIdentifier(Identifier identifier, object o)
        {
            if (identifier.NestedIDs.Any())
            { 
                // lookup type by struct
                var lookedUp = (Struct)idTable.Lookup(identifier.RootID, IDTable.DeclarationType.STRUCT);


                if(identifier.NestedIDs.Count == 1)
                {
                    var where = lookedUp.VariableDeclarationList.VariableDeclarations.Where(dec => ((VariableDeclarationSimple)dec).VariableName.Spelling == identifier.NestedIDs[0].Spelling);

                    if(where.Count() != 1)
                    {
                        throw new Exception("hould not happen");
                    }
                    else
                    {

                        return where.First() // variable type
                    }
                    
                }
                else
                {
                    var rootID = identifier.NestedIDs[0];
                    identifier.NestedIDs.RemoveAt(0);
                    var iden = new Identifier(rootID, identifier.NestedIDs);
                    return iden.Visit(this, null);
                }
            }
            else
            {
                var lookedUp = (VariableDeclarationSimple)idTable.Lookup(identifier.RootID, IDTable.DeclarationType.VARIABLE);

                return lookedUp.VariableType;
            }
            throw new NotImplementedException();
        }

        private class IDTable
        {
            private List<(UserCreatableID structName, Struct @struct)> definedStructs;
            private List<(int scopeLevel, UserCreatableID  ID,    DeclarationType type ,  AST.AST subTreePointer)> enviornment;
            private List<bool> isLoopScopeList = new List<bool>();
            private int _currentScopeLevel;


            public enum DeclarationType { STRUCT, FUNCTION, VARIABLE }
            public IDTable()
            {
                _currentScopeLevel = 1;
                enviornment = new List<(int scopeLevel, UserCreatableID ID,DeclarationType type ,AST.AST subTreePointer)>();
            }

            public void Add(UserCreatableID ID, AST.AST subTreePointer, DeclarationType type)
            {
                if(enviornment.Any(env => env.scopeLevel == _currentScopeLevel && env.ID.Spelling == ID.Spelling && env.type == type))
                {
                    throw new Exception("ID already defined for current scope level");
                }
                else
                {
                    enviornment.Add((_currentScopeLevel, ID, type, subTreePointer));
                }
            }

            public bool IsInLoopScope()
            {
                return isLoopScopeList.Any(x => x == true);
            }

            public AST.AST Lookup(UserCreatableID ID, DeclarationType type)
            {
                var lst = enviornment.FindAll(item => item.ID.Spelling == ID.Spelling && item.type == type);

                if(lst.Count == 0)
                {
                    throw new Exception("Nothing found in lookup exception");
                }
                else
                {
                    var max = lst.Max(item => item.scopeLevel);
                    var result = lst.Find(item => item.scopeLevel == max);
                    return result.subTreePointer;
                }

            }

            public void EnterNestedScopeLevel(bool isLoopScope = false)
            {
                _currentScopeLevel++;
                isLoopScopeList.Add(isLoopScope);
            }

            public void ExitNestedScopeLevel()
            {
                _currentScopeLevel--;
                isLoopScopeList.RemoveAt(isLoopScopeList.Count - 1);
                enviornment.RemoveAll(item => item.scopeLevel > _currentScopeLevel);
            }
        }
    }
}