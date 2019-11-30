using System;
using CMC.AST;
using System.Collections.Generic;
using System.Linq;

namespace CMC
{
    public class SemanticCheckerAndDecorater : IASTVisitor
    {
        private IDTable idTable = new IDTable();


        public object VisitArgumentList(ArgumentList argumentList, object o) //this should be good
        {
            var parameterList = o as ParameterList;

            if (parameterList.Parameters.Count != argumentList.Arguments.Count)
            {
                throw new Exception("Length of argument list does not match length of parameter list");
            }


            for (int i = 0; i < argumentList.Arguments.Count; i++)
            {
                var typeOfValueReturnedByExpression = (VariableType.ValueTypeEnum) argumentList.Arguments[i].Visit(this);

                if (typeOfValueReturnedByExpression == parameterList.Parameters[i].ParameterType.VariableType_)
                {
                    // bind to idTable
                    var decl = new VariableDeclarationSimple(parameterList.Parameters[i].ParameterType,
                        parameterList.Parameters[i].ParameterName, argumentList.Arguments[i]);
                    idTable.Add(parameterList.Parameters[i].ParameterName, decl, IDTable.DeclarationType.VARIABLE);
                }
                else
                {
                    throw new Exception("Type of argument not as expected ");
                }
            }

            return null;
        }

        public object VisitDeclarationVariableDeclaration(DeclarationVariableDeclaration declarationVariableDeclaration,
            object o)
        {
            declarationVariableDeclaration.VariableDeclaration.Visit(this);
            return null;
        }

        public object VisitDeclarationFunctionDeclaration(DeclarationFunctionDeclaration declarationFunctionDeclaration,
            object o)
        {
            idTable.Add(declarationFunctionDeclaration.FunctionDeclaration.FunctionName, declarationFunctionDeclaration, IDTable.DeclarationType.FUNCTION);

            declarationFunctionDeclaration.FunctionDeclaration.Visit(this);
            return null;
        }

        public object VisitDeclarationStruct(DeclarationStruct declarationStruct, object o)
        {
            idTable.Add(declarationStruct.Struct.StructName, declarationStruct, IDTable.DeclarationType.STRUCT);

            declarationStruct.Struct.Visit(this);
            return null;
        }

        public object VisitVariableDeclarationSimple(VariableDeclarationSimple variableDeclarationSimple, object o)
        {
            idTable.Add(variableDeclarationSimple.VariableName, variableDeclarationSimple, IDTable.DeclarationType.VARIABLE);

            if (variableDeclarationSimple.Expression == null)
            {
                return null;
            }
            else
            {
                var t1 = (VariableType.ValueTypeEnum) variableDeclarationSimple.Expression.Visit(this);
                var t2 = variableDeclarationSimple.VariableType.VariableType_;

                if (t1 != t2)
                {
                    throw new Exception("Type is not as expected; Actual: " + t1 + "; Expected: " + t2);
                }

                return null;
            }
        }

        public object VisitVariableDeclarationStructVariableDeclaration(VariableDeclarationStructVariableDeclaration variableDeclarationStructVariableDeclaration, object o)
        {
            variableDeclarationStructVariableDeclaration.StructVariableDeclaration.Visit(this);
            return null;
        }

        public object VisitVariableDeclarationList(VariableDeclarationList variableDeclarationList, object o)
        {
            variableDeclarationList.VariableDeclarations.ForEach(item => {
                var count = variableDeclarationList.VariableDeclarations.Count(i => i.Name == item.Name);
                if (count != 1)
                {
                    throw new Exception("Duplicate name in struct definition");
                }

                item.Visit(this);
            });
            return null;
        }

        public object VisitFunctionDeclaration(FunctionDeclaration functionDeclaration, object o)
        {
            idTable.EnterNestedScopeLevel(functionDeclaration.ReturnType.ValueType);

            functionDeclaration.ParameterList.Visit(this);

            functionDeclaration.Statements.Visit(this);

            idTable.ExitNestedScopeLevel();

            return null;
        }

        public object VisitExpression1(Expression1 expression1, object o)
        {
            if (expression1.Operator1 == null)
            {
                return expression1.Expression2.Visit(this);
            }
            
            var t1 = (VariableType.ValueTypeEnum) expression1.Expression2.Visit(this);
            var t2 = (VariableType.ValueTypeEnum) expression1.Expression1_.Visit(this);

            if (t1 != t2)
            {
                throw new Exception("Value type for operands in 'is' operator not the same");
            }

            if (t1 == VariableType.ValueTypeEnum.NOTHING || t2 == VariableType.ValueTypeEnum.NOTHING)
            {
                throw new Exception("<Nothing> not a valid operand for 'is' operator");
            }

            return VariableType.ValueTypeEnum.BOOLY;

        }

        public object VisitExpression2(Expression2 expression2, object o)
        {
            if (expression2.Operator2 == null)
            {
                return expression2.Expression3.Visit(this);
            }


            var lv2IntyOpr = new List<string>() {"+", "-"};

            var t1 = (VariableType.ValueTypeEnum) expression2.Expression1.Visit(this);
            var t2 = (VariableType.ValueTypeEnum) expression2.Expression3.Visit(this);

            if (t1 == VariableType.ValueTypeEnum.NOTHING || t2 == VariableType.ValueTypeEnum.NOTHING)
            {
                throw new Exception("<Nothing> not a valid operand for " + expression2.Operator2.Spelling + " operator");
            }

            if (t1 != t2)
            {
                throw new Exception("The operands for a Operator2 operator must be of same type");
            }

            if (lv2IntyOpr.Contains(expression2.Operator2.Spelling))
            {
                if (t1 == VariableType.ValueTypeEnum.INTY)
                {
                    return VariableType.ValueTypeEnum.INTY;
                }
                else
                {
                    throw new Exception("Operator " + expression2.Operator2.Spelling + " expexted inty operands");
                }
            }
            else if ("or".Equals(expression2.Operator2.Spelling))
            {
                if (t1 == VariableType.ValueTypeEnum.BOOLY)
                {
                    return VariableType.ValueTypeEnum.BOOLY;
                }
                else
                {
                    throw new Exception("Operator " + expression2.Operator2.Spelling + " expexted booly operands");
                }
            }

            throw new Exception("Something bad happened, code: 98465+4865");
        }

        public object VisitExpression3(Expression3 expression3, object o)
        {
            if (expression3.Operator3 == null)
            {
                return expression3.Primary.Visit(this);
            }


            var lv3IntyOpr = new List<string>() {"*", "/"};

            var t1 = (VariableType.ValueTypeEnum) expression3.Expression1.Visit(this);
            var t2 = (VariableType.ValueTypeEnum) expression3.Primary.Visit(this);

            if (t1 == VariableType.ValueTypeEnum.NOTHING || t2 == VariableType.ValueTypeEnum.NOTHING)
            {
                throw new Exception("<Nothing> not a valid operand for " + expression3.Operator3.Spelling + " operator");
            }

            if (t1 != t2)
            {
                throw new Exception("The operands for a Operator3 operator must be of same type");
            }

            if (lv3IntyOpr.Contains(expression3.Operator3.Spelling))
            {
                if (t1 == VariableType.ValueTypeEnum.INTY)
                {
                    return VariableType.ValueTypeEnum.INTY;
                }
                else
                {
                    throw new Exception("Operator " + expression3.Operator3.Spelling + " expexted inty operands");
                }
            }
            else if ("and".Equals(expression3.Operator3.Spelling))
            {
                if (t1 == VariableType.ValueTypeEnum.BOOLY)
                {
                    return VariableType.ValueTypeEnum.BOOLY;
                }
                else
                {
                    throw new Exception("Operator " + expression3.Operator3.Spelling + " expected booly operands");
                }
            }

            throw new Exception("Something bad happened, code: 32546863135");
        }

        public object VisitParameterList(ParameterList parameterList, object o)
        {
            foreach (var item in parameterList.Parameters)
            {
                item.Visit(this);
            }

            return null;
        }


        public object VisitPrimaryIdentifier(PrimaryIdentifier primaryIdentifier, object o)
        {
            return primaryIdentifier.Identifier.Visit(this);
        }

        public object VisitPrimaryBoolyLiteral(PrimaryBoolyLiteral primaryBoolyLiteral, object o)
        {
            return VariableType.ValueTypeEnum.BOOLY;
        }

        public object VisitPrimaryIntyLiteral(PrimaryIntyLiteral primaryIntyLiteral, object o)
        {
            return VariableType.ValueTypeEnum.INTY;
        }

        public object VisitPrimaryFunctionCall(PrimaryFunctionCall primaryFunctionCall, object o)
        {
            return primaryFunctionCall.FunctionCall.Visit(this);
        }

        public object VisitPrimaryExpression(PrimaryExpression primaryExpression, object o)
        {
            return primaryExpression.Expression.Visit(this);
        }

        private void DeclareSTD()
        {
            var parameterList = new ParameterList(new List<Parameter>());
            parameterList.Parameters.Add(new Parameter(
                    new VariableType(new Token("inty" /* doesn't matter */, Token.TokenType.VARIABLE_TYPE)),
                    new UserCreatableID(new Token("blah" /* doesn't matter */, Token.TokenType.USER_CREATABLE_ID))));

            var printFunc = new DeclarationFunctionDeclaration(new FunctionDeclaration(
                new UserCreatableID(new Token("print", Token.TokenType.USER_CREATABLE_ID)),
                parameterList,
                new ReturnTypeNothing(), 
                new Statements(new List<Statement>())
            ));

            Encoder.printFunction = printFunc;

            idTable.Add(printFunc.FunctionDeclaration.FunctionName, printFunc, IDTable.DeclarationType.FUNCTION);
        }

        public object VisitProgram(AST.Program program, object o)
        {
            DeclareSTD();

            program.Declarations.ForEach(item => item.Visit(this, null));

            if (idTable.StartFunction == null)
            {
                throw new Exception("Program must have a start function");
            }
            else
            {
                program.StartDeclaration = idTable.StartFunction;
            }

            return null;
        }

        public object VisitReturnVariableType(ReturnTypeVariableType returnTypeVariableType, object o)
        {
            throw new NotImplementedException("Should never be called");
        }

        public object VisitReturnTypeNothing(ReturnTypeNothing returnTypeNothing, object o)
        {
            throw new NotImplementedException("Should never be called");
        }

        public object VisitStatementIfStatement(StatementIfStatement statementIfStatement, object o)
        {
            idTable.EnterNestedScopeLevel();

            var variableType = (VariableType.ValueTypeEnum) statementIfStatement.Condition.Visit(this);
            if (variableType != VariableType.ValueTypeEnum.BOOLY)
            {
                throw new Exception("If statement condition must evaluate to booly value");
            }

            statementIfStatement.Statements.Visit(this);

            idTable.ExitNestedScopeLevel();

            return null;
        }

        public object VisitStatementLoopStatement(StatementLoopStatement statementLoopStatement, object o)
        {
            idTable.EnterNestedScopeLevel(isLoopScope: true);

            var variableType = (VariableType.ValueTypeEnum) statementLoopStatement.Condition.Visit(this);
            if (variableType != VariableType.ValueTypeEnum.BOOLY)
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
            if (idTable.ExpectedReturnType == VariableType.ValueTypeEnum.NOTHING)
            {
                return null;
            }

            if (statementGiveBack.Expression == null)
            {
                throw new Exception("Giveback statement must return value of type: " + idTable.ExpectedReturnType);
            }
            
            var expressionVariableType = (VariableType.ValueTypeEnum) statementGiveBack.Expression.Visit(this);
            if (expressionVariableType != idTable.ExpectedReturnType)
            {
                throw new Exception("Give back must return value of expected type");
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
            statementVariableDeclaration.VariableDeclaration.Visit(this);
            return null;
        }

        //identifier already in the table
        public object VisitStatementAssignment(StatementAssignment statementAssignment, object o)
        {
            VariableType.ValueTypeEnum expressionType = (VariableType.ValueTypeEnum) statementAssignment.Expression.Visit(this);
            VariableType.ValueTypeEnum variableType = (VariableType.ValueTypeEnum) statementAssignment.Identifier.Visit(this);

            if (variableType != expressionType)
            {
                throw new Exception("Invalid expression type for " + statementAssignment.Identifier.GetFullName());
            }

            return null;
        }

        public object VisitStatementFunctionCall(StatementFunctionCall statementFunctionCall, object o)
        {
            statementFunctionCall.FunctionCall.Visit(this);
            return null;
        }

        public object VisitStruct(Struct @struct, object o)
        {
            return null;
        }

        public object VisitStructVariableDeclaration(StructVariableDeclaration structVariableDeclaration, object o)
        {
            idTable.Add(structVariableDeclaration.VariableName, new VariableDeclarationStructVariableDeclaration(structVariableDeclaration), IDTable.DeclarationType.VARIABLE);
            return null;
        }

        public object VisitParameter(Parameter parameter, object o)
        {
            var decl = new VariableDeclarationSimple(parameter.ParameterType, parameter.ParameterName, null);
            idTable.Add(parameter.ParameterName, decl, IDTable.DeclarationType.VARIABLE);
            return null;
        }

        public object VisitFunctionCall(FunctionCall functionCall, object o)
        {
            functionCall.FunctionDeclaration = (DeclarationFunctionDeclaration) idTable.Lookup(functionCall.FunctionName, IDTable.DeclarationType.FUNCTION);
            functionCall.ArgumentList.Visit(this, functionCall.FunctionDeclaration.FunctionDeclaration.ParameterList);
            return null;
        }

        /// <summary>
        /// Is goiong to return VariableType.ValueTypeEnum
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public object VisitIdentifier(Identifier identifier, object o)
        {
            return idTable.Lookup(identifier);
        }
    }
}