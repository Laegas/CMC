using System;
using CMC.AST;
using System.Collections.Generic;
using System.Linq;

namespace CMC
{
    public class Checker : IASTVisitor
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
                var typeOfValueReturnedByExpression = (VariableType)argumentList.Arguments[i].Visit(this, null); //TODO WHAT SHOULD WE DO WITH THE RETURNM VALUE

                if (typeOfValueReturnedByExpression.VariableType_ == parameterList.Parameters[i].ParameterType.VariableType_)
                {// bind to idTable
                    idTable.Add(parameterList.Parameters[i].ParameterName, argumentList.Arguments[i], IDTable.DeclarationType.VARIABLE);
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
            declarationVariableDeclaration.VariableDeclaration.Visit(this, null);
            return null;
        }

        public object VisitDeclarationFunctionDeclaration(DeclarationFunctionDeclaration declarationFunctionDeclaration,
            object o)
        {
            declarationFunctionDeclaration.FunctionDeclaration.Visit(this, null);
            return null;
        }

        public object VisitDeclarationStruct(DeclarationStruct declarationStruct, object o)
        {
            declarationStruct.Struct.Visit(this, null);
            return null;
        }

        public object VisitVariableDeclarationSimple(VariableDeclarationSimple variableDeclarationSimple, object o)
        {
            idTable.Add(variableDeclarationSimple.VariableName, variableDeclarationSimple,
                IDTable.DeclarationType.VARIABLE);

            if (variableDeclarationSimple.Expression == null)
            {
                return null;
            }
            else
            {
                var t1 = (VariableType.ValueTypeEnum)variableDeclarationSimple.Expression.Visit(this);
                var t2 = variableDeclarationSimple.VariableType.VariableType_;

                if (t1 != t2)
                {
                    throw new Exception("Type is not as expected; Actual: " + t1 + "; Expected: " + t2);
                }

                return null;
            }

        }

        public object VisitVariableDeclarationStructVariableDeclaration(
            VariableDeclarationStructVariableDeclaration variableDeclarationStructVariableDeclaration, object o)
        {
            variableDeclarationStructVariableDeclaration.StructVariableDeclaration.Visit(this, null);
            return null;
        }

        public object VisitVariableDeclarationList(VariableDeclarationList variableDeclarationList, object o)
        {
            variableDeclarationList.VariableDeclarations.ForEach(item =>
           {
               var count = variableDeclarationList.VariableDeclarations.Where(i => i.Name == item.Name).Count();
               if (count != 1)
               {
                   throw new Exception("Duplicate name in struct definition");
               }
               item.Visit(this, null);
           });
            return null;
        }

        public object VisitFunctionDeclaration(FunctionDeclaration functionDeclaration, object o)
        {
            idTable.Add(functionDeclaration.FunctionName, functionDeclaration, IDTable.DeclarationType.FUNCTION);

            idTable.EnterNestedScopeLevel(expectedReturnType: functionDeclaration.ReturnType.ValueType);

            functionDeclaration.ParameterList.Visit(this);

            functionDeclaration.Statements.Visit(this);

            idTable.ExitNestedScopeLevel();

            return null;
        }

        public object VisitExpression1(Expression1 expression1, object o)
        {
            if (expression1.Operator1 != null)
            {
                var t1 = (VariableType.ValueTypeEnum)expression1.Expression2.Visit(this);
                var t2 = (VariableType.ValueTypeEnum)expression1.Expression1_.Visit(this);

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
            else
            {
                return expression1.Expression2.Visit(this);
            }
        }

        public object VisitExpression2(Expression2 expression2, object o)
        {

            if (expression2.Operator2 == null)
            {
                return expression2.Expression3.Visit(this);
            }


            var lv2IntyOpr = new List<string>() { "+", "-" };

            var t1 = (VariableType.ValueTypeEnum)expression2.Expression1.Visit(this);
            var t2 = (VariableType.ValueTypeEnum)expression2.Expression3.Visit(this);

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


            var lv3IntyOpr = new List<string>() { "*", "/" };

            var t1 = (VariableType.ValueTypeEnum)expression3.Expression1.Visit(this);
            var t2 = (VariableType.ValueTypeEnum)expression3.Primary.Visit(this);

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
                item.Visit(this, null);
            }
            //this is all it needs to be, we think
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

        public object VisitProgram(AST.Program program, object o)
        {
            program.Declarations.ForEach(item => item.Visit(this, null));
            if (idTable.HasStartFunction == false)
            {
                throw new Exception("PRogram must have a start function");
            }
            return null;
        }

        public object VisitReturnVariableType(ReturnTypeVariableType returnTypeVariableType, object o)
        {
            //WE DONT THINK THIS IS USED
            return null;
        }

        public object VisitReturnTypeNothing(ReturnTypeNothing returnTypeNothing, object o)
        {
            //WE DONT THINK THIS IS USED
            return null;
        }

        public object VisitStatementIfStatement(StatementIfStatement statementIfStatement, object o)
        {
            idTable.EnterNestedScopeLevel();

            var variableType = (VariableType.ValueTypeEnum)statementIfStatement.Condition.Visit(this);
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

            var variableType = (VariableType.ValueTypeEnum)statementLoopStatement.Condition.Visit(this);
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
            var expressionVariableType = (VariableType)statementGiveBack.Expression.Visit(this);

            if (expressionVariableType.VariableType_ != idTable.ExpectedReturnType)
            {
                throw new Exception("Give back must return value of expected type");
            }

            return null;
        }

        public void Hello()
        {


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

        //identifier already in the table
        public object VisitStatementAssignment(StatementAssignment statementAssignment, object o)
        {
            var literalType = (VariableType.ValueTypeEnum)statementAssignment.Expression.Visit(this);

            var declaration = (VariableDeclaration)idTable.Lookup(statementAssignment.Identifier, IDTable.DeclarationType.VARIABLE);

            if (declaration is VariableDeclarationSimple declarationSimple)
            {
                if (literalType != declarationSimple.VariableType.VariableType_)
                    throw new Exception("Type is not as expected; Actual: " + literalType + "; Expected: " + declarationSimple.VariableType.VariableType_);
                return null;
            }
            else
            {
                throw new Exception("Something bad happened: 2345432343ffyhvgffrgbhnj");
            }
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

        public object VisitParameter(Parameter parameter, object o)
        {
            idTable.Add(parameter.ParameterName, parameter, IDTable.DeclarationType.VARIABLE);
            return null;
        }

        public object VisitFunctionCall(FunctionCall functionCall, object o)
        {
            FunctionDeclaration dec = (FunctionDeclaration)idTable.Lookup(functionCall.FunctionName, IDTable.DeclarationType.FUNCTION);

            functionCall.ArgumentList.Visit(this, dec.ParameterList);

            // TODO decorate tree

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
            var simpleVar = (VariableDeclarationSimple)idTable.Lookup(identifier, IDTable.DeclarationType.VARIABLE);
            return simpleVar.VariableType.VariableType_;
            //if( identifier.NestedIDs.Any() )
            //{  // identifier for struct's value

            //    // lookup type by struct
            //    var lookedUp = (Struct)idTable.Lookup( identifier.RootID, IDTable.DeclarationType.STRUCT );


            //    if( identifier.NestedIDs.Count == 1 )
            //    { // pointing to inty / booly
            //        var where = lookedUp.VariableDeclarationList.VariableDeclarations.Where( dec => ( (VariableDeclarationSimple)dec ).VariableName.Spelling == identifier.NestedIDs[ 0 ].Spelling );

            //        if( where.Count() != 1 )
            //        {
            //            throw new Exception( "hould not happen" );
            //        }
            //        else
            //        {

            //            return where.First() // variable type
            //        }

            //        //hello.otherStruct.hello1

            //        //    cook Struct1 hello;
            //        //    cook Struct2 other;
            //        //hello.otherStruct = other;
            //        //kebab Struct1[
            //        //        Struct2 otherStruct;
            //        //        ]
            //        //kebab Struct2[
            //        //        booly ThisIsBooly;
            //        //        ]

            //    }
            //    else
            //    { // pointing to another struct

            //        var rootID = identifier.NestedIDs[ 0 ];
            //        identifier.NestedIDs.RemoveAt( 0 );
            //        var iden = new Identifier( rootID, identifier.NestedIDs );
            //        return iden.Visit( this, null );
            //    }
            //}
            //else
            //{
            //    var lookedUp = (VariableDeclarationSimple)idTable.Lookup( identifier.RootID, IDTable.DeclarationType.VARIABLE );

            //    return lookedUp.VariableType;
            //}
            //throw new NotImplementedException();
        }
    }
}