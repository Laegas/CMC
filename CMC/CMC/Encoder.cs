﻿using System;
using System.IO;
using System.Text;
using CMC.AST;
using CMC.TAM;

namespace CMC
{
    public class Encoder : IASTVisitor
    {
        private int nextAdr = Machine.CB; // from Jan
        public static readonly string FILE_NAME = "combiled.TAM";
        private StackManager _stackManager = new StackManager();

        // Standard environment
        public static DeclarationFunctionDeclaration printFunction;

        
        private void Emit( int op, int n, int r, int d ) // from Jan
        {
            if( n > 255 )
            {
                throw new Exception( "Operand too long" );
            }

            Instruction instr = new Instruction();
            instr.op = op;
            instr.n = n;
            instr.r = r;
            instr.d = d;

            switch (instr.op)
            {
                case Machine.LOADop:
                    _stackManager.IncrementOffset(n);
                    break;
                case Machine.LOADAop:
                case Machine.LOADLop:
                    _stackManager.IncrementOffset();
                    break;
                case Machine.LOADIop:
                    _stackManager.IncrementOffset(n - 1);
                    break;
                case Machine.STOREop:
                    _stackManager.DecrementOffset(n);
                    break;
                case Machine.STOREIop:
                    _stackManager.DecrementOffset(n + 1);
                    break;
//                case Machine.CALLop:
//                    _stackManager.IncrementFrameLevel();
//                    break;
//                case Machine.CALLIop:
//                    _stackManager.DecrementOffset(2);
//                    _stackManager.IncrementFrameLevel();
//                    break;
//                case Machine.RETURNop:
//                    _stackManager.DecrementFrameLevel();
//                    _stackManager.DecrementOffset(d);
//                    _stackManager.IncrementOffset(n);
//                    break;
                case Machine.PUSHop:
                    _stackManager.IncrementOffset(d);
                    break;
                case Machine.POPop:
                    _stackManager.DecrementOffset(d);
                    break;
                case Machine.JUMPIop:
                case Machine.JUMPIFop:
                    _stackManager.DecrementOffset();
                    break;
            }

            if( nextAdr >= Machine.PB )
            {
                throw new Exception( "Program too large" );
            }
            else
            {
                Machine.code[ nextAdr++ ] = instr;
            }
        }



        private void Patch( int adr, int d )//from Jan
        {
            Machine.code[ adr ].d = d;
        }

        private int DisplayRegister( Address address ) //from Jan
        {
            return address.IsGlobalScope ? Machine.SBr : Machine.LBr;
        }


        public void SaveTargetProgram( String fileName ) //from Jan
        {
            BinaryWriter writer = new BinaryWriter( new FileStream( fileName, FileMode.OpenOrCreate ) );

            for( int i = Machine.CB; i < nextAdr; ++i )
            {
                Machine.code[ i ].write( writer );
            }

            writer.Close();
        }


        public void Encode( AST.Program p )
        {
            p.Visit( this, null );
        }


        public object VisitArgumentList( ArgumentList argumentList, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitDeclarationFunctionDeclaration( DeclarationFunctionDeclaration declarationFunctionDeclaration, object o )
        {
            var savedAddr = nextAdr;
            Emit(Machine.JUMPop, 0, Machine.CBr, -1);
            declarationFunctionDeclaration.FunctionDeclaration.Address.Offset = nextAdr;
            _stackManager.IncrementFrameLevel();
            declarationFunctionDeclaration.FunctionDeclaration.Visit(this);
            _stackManager.DecrementFrameLevel();
            Patch(savedAddr, nextAdr);
            return null;
        }

        public object VisitDeclarationStruct( DeclarationStruct declarationStruct, object o )
        {
            return null;
        }

        public object VisitDeclarationVariableDeclaration( DeclarationVariableDeclaration declarationVariableDeclaration, object o )
        {
            declarationVariableDeclaration.VariableDeclaration.Visit(this);
            return null;
        }

        public object VisitExpression1( Expression1 expression1, object o )
        {
            if (expression1.Operator1 == null)
            {
                expression1.Expression2.Visit(this);
                return null;
            }
            
            expression1.Expression2.Visit(this);
            expression1.Expression1_.Visit(this);
            int operatorAsDisplacement;
            switch (expression1.Operator1.Spelling)
            {
                case "is":
                    operatorAsDisplacement = Machine.eqDisplacement;
                    break;
                default:
                    throw new ArgumentException("Invalid operator");
            }
            Emit(Machine.LOADLop, 0, 0, 1);
            Emit(Machine.CALLop, Machine.CB, Machine.PBr, operatorAsDisplacement);
            _stackManager.DecrementOffset();
            _stackManager.DecrementOffset();
            return null;
        }

        public object VisitExpression2( Expression2 expression2, object o )
        {
            if (expression2.Operator2 == null)
            {
                expression2.Expression3.Visit(this);
                return null;
            }

            expression2.Expression3.Visit(this);
            expression2.Expression1.Visit(this);
            int operatorAsDisplacement;
            switch (expression2.Operator2.Spelling)
            {
                case "+":
                    operatorAsDisplacement = Machine.addDisplacement;
                    break;
                case "-":
                    operatorAsDisplacement = Machine.subDisplacement;
                    break;
                case "or":
                    operatorAsDisplacement = Machine.orDisplacement;
                    break;
                default:
                    throw new ArgumentException("Invalid operator");
            }
            Emit(Machine.CALLop, Machine.CB, Machine.PBr, operatorAsDisplacement);
            _stackManager.DecrementOffset();
            return null;
        }

        public object VisitExpression3( Expression3 expression3, object o )
        {
            if (expression3.Operator3 == null)
            {
                expression3.Primary.Visit(this);
                return null;
            }

            expression3.Primary.Visit(this);
            expression3.Expression1.Visit(this);
            int operatorAsDisplacement;
            switch (expression3.Operator3.Spelling)
            {
                case "*":
                    operatorAsDisplacement = Machine.multDisplacement;
                    break;
                case "/":
                    operatorAsDisplacement = Machine.divDisplacement;
                    break;
                case "and":
                    operatorAsDisplacement = Machine.andDisplacement;
                    break;
                default:
                    throw new ArgumentException("Invalid operator");
            }
            Emit(Machine.CALLop, Machine.CB, Machine.PBr, operatorAsDisplacement);
            _stackManager.DecrementOffset();
            return null;
        }

        public object VisitFunctionCall( FunctionCall functionCall, object o )
        {
            foreach( var item in functionCall.ArgumentList.Arguments )
            {
                item.Visit(this);
            }

            Emit( Machine.CALLop, 0 ,Machine.CBr, functionCall.FunctionDeclaration.FunctionDeclaration.Address.Offset);
            return null;
        }

        public object VisitFunctionDeclaration( FunctionDeclaration functionDeclaration, object o )
        {
            functionDeclaration.ParameterList.Visit(this);
            functionDeclaration.Statements.Visit(this, functionDeclaration);

            return null;
        }

        public object VisitIdentifier( Identifier identifier, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitParameter( Parameter visitor, object o )
        {
            return null;
        }

        public object VisitParameterList( ParameterList parameterList, object o )
        {
            parameterList.Parameters.ForEach(item => item.Visit(this));
            return null;
        }

        public object VisitPrimaryBoolyLiteral( PrimaryBoolyLiteral primaryBoolyLiteral, object o )
        {
            Emit(Machine.LOADLop, 0, 0, Convert.ToInt32(primaryBoolyLiteral.Value.Spelling == "aye" ? 1 : 0));
            return null;
        }

        public object VisitPrimaryExpression( PrimaryExpression primaryExpression, object o )
        {
            primaryExpression.Expression.Visit(this);
            return null;
        }

        public object VisitPrimaryFunctionCall( PrimaryFunctionCall primaryFunctionCall, object o )
        {
            primaryFunctionCall.FunctionCall.Visit(this);
            return null;
        }

        public object VisitPrimaryIdentifier( PrimaryIdentifier primaryIdentifier, object o )
        {
            var address = primaryIdentifier.IDsDeclaration.Address;
            int register = DisplayRegister(address);
            Emit(Machine.LOADop, 1, register, address.Offset);
            return null;
        }

        public object VisitPrimaryIntyLiteral( PrimaryIntyLiteral primaryIntyLiteral, object o )
        {
            Emit(Machine.LOADLop, 0, 0, Convert.ToInt32(primaryIntyLiteral.Value.Spelling));
            return null;
        }

        private void DefineSTD()
        {
            var savedAddr = nextAdr;
            Emit(Machine.JUMPop, 0, Machine.CBr, -1);
//            _stackManager.IncrementFrameLevel();
            printFunction.FunctionDeclaration.Address.Offset = nextAdr;

            Emit(Machine.LOADop, 1, Machine.LBr, -1);
            Emit(Machine.CALLop, 0, Machine.PBr, Machine.putintDisplacement);
            _stackManager.DecrementOffset();
            Emit( Machine.CALLop, 0, Machine.PBr, Machine.puteolDisplacement );
            // Because return statement is not in our source code.
            Emit(Machine.RETURNop, 0, 0, 1);
//            _stackManager.DecrementFrameLevel();

            Patch(savedAddr, nextAdr);
        }

        public object VisitProgram( AST.Program program, object o )
        {
            DefineSTD();
            
            program.Declarations.ForEach( item => item.Visit( this, null ) );

            //call start function 
            Emit(Machine.CALLop, 0, Machine.CBr, program.StartDeclaration.FunctionDeclaration.Address.Offset);

            Emit( Machine.HALTop, 0, 0, 0 );
            return null;
        }

        public object VisitReturnTypeNothing( ReturnTypeNothing returnTypeNothing, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitReturnVariableType( ReturnTypeVariableType returnTypeVariableType, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitStatementAssignment( StatementAssignment statementAssignment, object o )
        {
            statementAssignment.Expression.Visit(this);
            var statementAddress = statementAssignment.IDsDeclaration.Address;
            var register = DisplayRegister(statementAddress);
            Emit(Machine.STOREop, 1, register, statementAddress.Offset);
            return null;
        }

        public object VisitStatementFunctionCall( StatementFunctionCall statementFunctionCall, object o )
        {
            statementFunctionCall.FunctionCall.Visit(this);
            int returnSize = statementFunctionCall.FunctionCall.FunctionDeclaration.FunctionDeclaration.ReturnType.ValueType == VariableType.ValueTypeEnum.NOTHING ? 0 : 1;
            Emit(Machine.POPop, 0, 0, returnSize);
            return null;
        }

        public object VisitStatementGiveBack( StatementGiveBack statementGiveBack, object o )
        {
            var functionDeclaration = o as FunctionDeclaration;
            var parametersSize = functionDeclaration.ParameterList.Parameters.Count; // TODO Parameter size won't work for structs.
            if (statementGiveBack.Expression == null)
            {
                Emit(Machine.RETURNop, 0, 0, parametersSize);
            }
            else
            {
                statementGiveBack.Expression.Visit(this);
                Emit(Machine.RETURNop, 1, 0, parametersSize);
            }

            return null;
        }

        public object VisitStatementIfStatement( StatementIfStatement statementIfStatement, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitStatementLoopStatement( StatementLoopStatement statementLoopStatement, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitStatements( Statements statements, object o )
        {
            statements.Statements_.ForEach(item => item.Visit(this, o));
            return null;
        }

        public object VisitStatementStopTheLoop( StatementStopTheLoop statementStopTheLoop, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitStatementVariableDeclaration( StatementVariableDeclaration statementVariableDeclaration, object o )
        {
            statementVariableDeclaration.VariableDeclaration.Visit( this );
            return null;
        }

        public object VisitStruct( Struct @struct, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitStructVariableDeclaration( StructVariableDeclaration structVariableDeclaration, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitVariableDeclarationList( VariableDeclarationList variableDeclarationList, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitVariableDeclarationSimple( VariableDeclarationSimple variableDeclarationSimple, object o )
        {
            variableDeclarationSimple.Address.Offset = _stackManager.CurrentOffset;
            if (variableDeclarationSimple.Expression == null)
            {
                Emit(Machine.PUSHop, 0, 0, 1);
            }
            else
            {
                variableDeclarationSimple.Expression.Visit(this);
            }
            return null;
        }

        public object VisitVariableDeclarationStructVariableDeclaration( VariableDeclarationStructVariableDeclaration variableDeclarationStructVariableDeclaration, object o )
        {
            throw new NotImplementedException();
        }
    }
}
