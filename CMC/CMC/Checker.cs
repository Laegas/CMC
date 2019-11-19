using System;
using CMC.AST;
using System.Collections.Generic;
using System.Linq;

namespace CMC
{
    public class Checker : IASTVisitor
    {
        private IDTable idTable = new IDTable();


        public object VisitArgumentList( ArgumentList argumentList, object o ) //this should be good
        {
            var parameterList = o as ParameterList;

            if( parameterList.Parameters.Count != argumentList.Arguments.Count )
            {
                throw new Exception( "Length of argument list does not match length of parameter list" );
            }


            for( int i = 0; i < argumentList.Arguments.Count; i++ )
            {
                var typeOfValueReturnedByExpression = (VariableType)argumentList.Arguments[ i ].Visit( this, null ); //TODO WHAT SHOULD WE DO WITH THE RETURNM VALUE

                if( typeOfValueReturnedByExpression.VariableType_ == parameterList.Parameters[ i ].ParameterType.VariableType_ )
                {// bind to idTable
                    idTable.Add( parameterList.Parameters[ i ].ParameterName, argumentList.Arguments[ i ], IDTable.DeclarationType.VARIABLE );
                }
                else
                {
                    throw new Exception( "Type of argument not as expected " );
                }
            }

            return null;
        }

        public object VisitDeclarationVariableDeclaration( DeclarationVariableDeclaration declarationVariableDeclaration,
            object o )
        {
            declarationVariableDeclaration.VariableDeclaration.Visit( this, null );
            return null;
        }

        public object VisitDeclarationFunctionDeclaration( DeclarationFunctionDeclaration declarationFunctionDeclaration,
            object o )
        {
            declarationFunctionDeclaration.FunctionDeclaration.Visit( this, null );
            return null;
        }

        public object VisitDeclarationStruct( DeclarationStruct declarationStruct, object o )
        {
            declarationStruct.Struct.Visit( this, null );
            return null;
        }

        public object VisitVariableDeclarationSimple( VariableDeclarationSimple variableDeclarationSimple, object o )
        {
            idTable.Add( variableDeclarationSimple.VariableName, variableDeclarationSimple, IDTable.DeclarationType.VARIABLE );
            return null;
        }

        public object VisitVariableDeclarationStructVariableDeclaration(
            VariableDeclarationStructVariableDeclaration variableDeclarationStructVariableDeclaration, object o )
        {
            variableDeclarationStructVariableDeclaration.StructVariableDeclaration.Visit( this, null );
            return null;
        }

        public object VisitVariableDeclarationList( VariableDeclarationList variableDeclarationList, object o )
        {
            variableDeclarationList.VariableDeclarations.ForEach( item =>
            {
                var count = variableDeclarationList.VariableDeclarations.Where( i => i.Name == item.Name ).Count();
                if( count != 1 )
                {
                    throw new Exception( "Duplicate name in struct definition" );
                }
                item.Visit( this, null );
            } );
            return null;
        }

        public object VisitFunctionDeclaration( FunctionDeclaration functionDeclaration, object o )
        {
            idTable.Add( functionDeclaration.FunctionName, functionDeclaration, IDTable.DeclarationType.FUNCTION );

            idTable.EnterNestedScopeLevel();

            functionDeclaration.ParameterList.Visit( this );
            //functionDeclaration.ReturnType.Visit( this ); // this must store what the return type must be on the ID table
            functionDeclaration.Statements.Visit( this, functionDeclaration.ReturnType );

            idTable.ExitNestedScopeLevel();

            return null;
        }

        public object VisitExpression1( Expression1 expression1, object o )
        {
            if(expression1.Operator1 != null )
            {
                var t1 = (VariableType.ValueTypeEnum)expression1.Expression2.Visit(this);
                var t2 = (VariableType.ValueTypeEnum)expression1.Expression1_.Visit( this );

                if(t1 != t2 )
                {
                    throw new Exception( "Value type for operands in 'is' operator not the same" );
                }
                if(t1 == VariableType.ValueTypeEnum.NOTHING || t2 == VariableType.ValueTypeEnum.NOTHING )
                {
                    throw new Exception( "<Nothing> not a valid operand for 'is' operator" );
                }
                return VariableType.ValueTypeEnum.BOOLY;
            }
            else
            {
                return expression1.Expression2.Visit( this );
            }
        }

        public object VisitExpression2( Expression2 expression2, object o )
        {

            if(expression2.Operator2 == null )
            {
                return expression2.Expression3.Visit( this );
            }


            var lv2IntyOpr = new List<string>() { "+", "-" };

            var t1 = (VariableType.ValueTypeEnum)expression2.Expression1.Visit( this );
            var t2 = (VariableType.ValueTypeEnum)expression2.Expression3.Visit( this );

            if( t1 == VariableType.ValueTypeEnum.NOTHING || t2 == VariableType.ValueTypeEnum.NOTHING )
            {
                throw new Exception( "<Nothing> not a valid operand for "  + expression2.Operator2.Spelling + " operator" );
            }

            if(t1 != t2 )
            {
                throw new Exception( "The operands for a Operator2 operator must be of same type" );
            }

            if( lv2IntyOpr.Contains( expression2.Operator2.Spelling ) )
            {
                if(t1 == VariableType.ValueTypeEnum.INTY )
                {
                    return VariableType.ValueTypeEnum.INTY;
                }
                else
                {
                    throw new Exception( "Operator " + expression2.Operator2.Spelling + " expexted inty operands" );
                }
            }
            else if( "or".Equals( expression2.Operator2.Spelling ) )
            {
                if( t1 == VariableType.ValueTypeEnum.BOOLY)
                {
                    return VariableType.ValueTypeEnum.BOOLY;
                }
                else
                {
                    throw new Exception( "Operator " + expression2.Operator2.Spelling + " expexted booly operands" );
                }
            }

            throw new Exception( "Something bad happened, code: 98465+4865" );

        }

        public object VisitExpression3( Expression3 expression3, object o )
        {
            if( expression3.Operator3 == null )
            {
                return expression3.Primary.Visit( this );
            }


            var lv3IntyOpr = new List<string>() { "*", "/" };

            var t1 = (VariableType.ValueTypeEnum)expression3.Expression1.Visit( this );
            var t2 = (VariableType.ValueTypeEnum)expression3.Primary.Visit( this );

            if( t1 == VariableType.ValueTypeEnum.NOTHING || t2 == VariableType.ValueTypeEnum.NOTHING )
            {
                throw new Exception( "<Nothing> not a valid operand for " + expression3.Operator3.Spelling + " operator" );
            }

            if( t1 != t2 )
            {
                throw new Exception( "The operands for a Operator3 operator must be of same type" );
            }

            if( lv3IntyOpr.Contains( expression3.Operator3.Spelling ) )
            {
                if( t1 == VariableType.ValueTypeEnum.INTY )
                {
                    return VariableType.ValueTypeEnum.INTY;
                }
                else
                {
                    throw new Exception( "Operator " + expression3.Operator3.Spelling + " expexted inty operands" );
                }
            }
            else if( "and".Equals( expression3.Operator3.Spelling ) )
            {
                if( t1 == VariableType.ValueTypeEnum.BOOLY )
                {
                    return VariableType.ValueTypeEnum.BOOLY;
                }
                else
                {
                    throw new Exception( "Operator " + expression3.Operator3.Spelling + " expected booly operands" );
                }
            }

            throw new Exception( "Something bad happened, code: 32546863135" );
        }

        public object VisitParameterList( ParameterList parameterList, object o )
        {
            foreach( var item in parameterList.Parameters )
            {
                item.Visit( this, null );
            }
            //this is all it needs to be, we think
            return null;
        }


        public object VisitPrimaryIdentifier( PrimaryIdentifier primaryIdentifier, object o )
        {
            return primaryIdentifier.Identifier.Visit( this );

        }

        public object VisitPrimaryBoolyLiteral( PrimaryBoolyLiteral primaryBoolyLiteral, object o )
        {
            return VariableType.ValueTypeEnum.BOOLY;
        }

        public object VisitPrimaryIntyLiteral( PrimaryIntyLiteral primaryIntyLiteral, object o )
        {
            return VariableType.ValueTypeEnum.INTY;
        }

        public object VisitPrimaryFunctionCall( PrimaryFunctionCall primaryFunctionCall, object o )
        {
            var funcDec = (FunctionDeclaration)idTable.Lookup( primaryFunctionCall.FunctionCall.FunctionName, IDTable.DeclarationType.FUNCTION );

            return funcDec.ReturnType.ValueType;
        }

        public object VisitPrimaryExpression( PrimaryExpression primaryExpression, object o )
        {
            return primaryExpression.Expression.Visit( this );
        }

        public object VisitProgram( AST.Program program, object o )
        {
            program.Declarations.ForEach( item => item.Visit(this,null) );
            return null;
        }

        public object VisitReturnVariableType( ReturnTypeVariableType returnTypeVariableType, object o )
        {
            //WE DONT THINK THIS IS USED
            return null;
        }

        public object VisitReturnTypeNothing( ReturnTypeNothing returnTypeNothing, object o )
        {
            //WE DONT THINK THIS IS USED
            return null;
        }

        public object VisitStatementIfStatement( StatementIfStatement statementIfStatement, object o )
        {
            idTable.EnterNestedScopeLevel();

            var variableType = (VariableType)statementIfStatement.Condition.Visit( this );
            if( variableType.VariableType_ != VariableType.ValueTypeEnum.BOOLY )
            {
                throw new Exception( "If statement condition must evaluate to booly value" );
            }

            statementIfStatement.Statements.Visit( this );

            idTable.ExitNestedScopeLevel();

            return null;
        }

        public object VisitStatementLoopStatement( StatementLoopStatement statementLoopStatement, object o )
        {
            idTable.EnterNestedScopeLevel( true );

            var variableType = (VariableType)statementLoopStatement.Condition.Visit( this );
            if( variableType.VariableType_ != VariableType.ValueTypeEnum.BOOLY )
            {
                throw new Exception( "Loop statement condition must evaluate to a booly value" );
            }

            statementLoopStatement.Statements.Visit( this );

            idTable.ExitNestedScopeLevel();

            return null;
        }

        public object VisitStatementStopTheLoop( StatementStopTheLoop statementStopTheLoop, object o )
        {
            if( !idTable.IsInLoopScope() )
            {
                throw new Exception( "Stop the loop statement must be used only inside a loop" );
            }

            return null;
        }

        public object VisitStatementGiveBack( StatementGiveBack statementGiveBack, object o )
        {
            if( o is ReturnTypeVariableType returnType )
            {
                var expressionVariableType = (VariableType)statementGiveBack.Expression.Visit( this );
                if( expressionVariableType.VariableType_ != returnType.VariableType.VariableType_ )
                {
                    throw new Exception( "Give back must return value of expected type" );
                }
            }
            else
            {
                if( statementGiveBack.Expression != null )
                {
                    throw new Exception( "Statement give back should not return a value when function return type is nothing" );
                }
            }

            return null;
        }

        public object VisitStatements( Statements statements, object o )
        {
            if( o is ReturnTypeVariableType )
            {
                if( !statements.Statements_.Any( statement => statement is StatementGiveBack ) )
                {
                    throw new Exception( "No giveback statements in the function" );
                }
            }

            foreach( var statement in statements.Statements_ )
            {
                statement.Visit( this, o );
            }

            return null;
        }

        public object VisitStatementVariableDeclaration( StatementVariableDeclaration statementVariableDeclaration,
            object o )
        {
            statementVariableDeclaration.VariableDeclaration.Visit( this, null );
            return null;
        }

        //identifier already in the table
        public object VisitStatementAssignment( StatementAssignment statementAssignment, object o )
        {
            var literalType = (VariableType.ValueTypeEnum)statementAssignment.Expression.Visit( this );

            idTable.Lookup( statementAssignment.Identifier, IDTable.DeclarationType.VARIABLE );


            if( literalType.VariableType_ != statementAssignment.)
                throw new System.NotImplementedException();
        }

        public object VisitStatementFunctionCall( StatementFunctionCall statementFunctionCall, object o )
        {
            statementFunctionCall.FunctionCall.Visit( this, null );
            return null;
        }

        public object VisitStruct( Struct @struct, object o )
        {
            idTable.Add( @struct.StructName, @struct, IDTable.DeclarationType.STRUCT );
            return null;
        }

        public object VisitStructVariableDeclaration( StructVariableDeclaration structVariableDeclaration, object o )
        {
            idTable.Add( structVariableDeclaration.VariableName, structVariableDeclaration, IDTable.DeclarationType.VARIABLE );
            return null;
        }

        public object VisitParameter( Parameter parameter, object o )
        {
            idTable.Add( parameter.ParameterName, parameter, IDTable.DeclarationType.VARIABLE );
            return null;
        }

        public object VisitFunctionCall( FunctionCall functionCall, object o )
        {
            FunctionDeclaration dec = (FunctionDeclaration)idTable.Lookup( functionCall.FunctionName, IDTable.DeclarationType.FUNCTION );

            idTable.EnterNestedScopeLevel();

            // check type of argument match type of parameter, and bind
            functionCall.ArgumentList.Visit( this, dec.ParameterList );

            // TODO decorate tree

            idTable.ExitNestedScopeLevel();

            return null;
        }

        /// <summary>
        /// Is goiong to return VariableType.ValueTypeEnum
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public object VisitIdentifier( Identifier identifier, object o )
        {
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

        private class IDTable
        {
            private List<(UserCreatableID structName, Struct @struct)> definedStructs;
            private List<(int scopeLevel, UserCreatableID ID, DeclarationType type, AST.AST subTreePointer)> enviornment;
            private List<bool> isLoopScopeList = new List<bool>();
            private int _currentScopeLevel;


            public enum DeclarationType { STRUCT, FUNCTION, VARIABLE }
            public IDTable()
            {
                _currentScopeLevel = 1;
                enviornment = new List<(int scopeLevel, UserCreatableID ID, DeclarationType type, AST.AST subTreePointer)>();
            }

            public void Add( UserCreatableID ID, AST.AST subTreePointer, DeclarationType type )
            {
                if( enviornment.Any( env => env.scopeLevel == _currentScopeLevel && env.ID.Spelling == ID.Spelling && env.type == type ) )
                {
                    throw new Exception( "ID already defined for current scope level" );
                }
                else
                {
                    enviornment.Add( (_currentScopeLevel, ID, type, subTreePointer) );
                }
            }

            public bool IsInLoopScope()
            {
                return isLoopScopeList.Any( x => x == true );
            }

            public AST.AST Lookup( UserCreatableID ID, DeclarationType type )
            {
                var lst = enviornment.FindAll( item => item.ID.Spelling == ID.Spelling && item.type == type );

                if( lst.Count == 0 )
                {
                    throw new Exception( "Nothing found in lookup exception" );
                }
                else
                {
                    var max = lst.Max( item => item.scopeLevel );
                    var result = lst.Find( item => item.scopeLevel == max );
                    return result.subTreePointer;
                }

            }
            public AST.AST Lookup( Identifier ID, DeclarationType type )
            {
                if( false == ID.NestedIDs.Any() )
                {
                    return Lookup( ID, type );
                }

                var struc = (Struct)Lookup( ID.RootID, type );

                var loopingStruct = struc;
                for( int i = 0; i < ID.NestedIDs.Count; i++ )
                {
                    var bla = loopingStruct.VariableDeclarationList.VariableDeclarations;

                    var searchedList = bla.Where( item =>
                         {
                             var name = "";
                             if( item is VariableDeclarationSimple _simple )
                             {
                                 name = _simple.VariableName.Spelling;
                             }
                             else if( item is VariableDeclarationStructVariableDeclaration _structVar )
                             {
                                 name = _structVar.StructVariableDeclaration.VariableName.Spelling;
                             }
                             return ( name == ID.NestedIDs[ i ].Spelling );
                         }
                    );

                    if( searchedList.Count() != 1 )
                    {
                        throw new Exception( "ID not defined" );
                    }

                    if( ( i + 1 ) == ID.NestedIDs.Count )
                    {
                        var simple = (VariableDeclarationSimple)searchedList.First();
                        return simple;
                    }
                    else
                    {
                        var strucDeclare = (VariableDeclarationStructVariableDeclaration)searchedList.First();
                        loopingStruct = (Struct)Lookup( strucDeclare.StructVariableDeclaration.StructName, DeclarationType.STRUCT );
                    }
                }

                throw new Exception( "Something went wrong in the checker" );
            }

            public void EnterNestedScopeLevel( bool isLoopScope = false )
            {
                _currentScopeLevel++;
                isLoopScopeList.Add( isLoopScope );
            }

            public void ExitNestedScopeLevel()
            {
                _currentScopeLevel--;
                isLoopScopeList.RemoveAt( isLoopScopeList.Count - 1 );
                enviornment.RemoveAll( item => item.scopeLevel > _currentScopeLevel );
            }
        }
    }
}