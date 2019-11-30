using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CMC.AST;
using CMC.TAM;

namespace CMC
{
    public class Encoder : IASTVisitor
    {
        private int nextAdr = Machine.CB; // from Jan
        private int currentLevel = 0; // from Jan
        private static readonly string FILE_NAME = "combiled.TAM";

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

        private int DisplayRegister( int currentLevel, int entityLevel ) //from Jan
        {
            if( entityLevel == 0 )
                return Machine.SBr;
            else if( currentLevel - entityLevel <= 6 )
                return Machine.LBr + currentLevel - entityLevel;
            else
            {
                throw new Exception( "Accessing across to many levels" );
                //return Machine.L6r;
            }
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


        public static void Testing()
        {
            var encoder = new Encoder();

            encoder.Emit( Machine.LOADLop, 0, 0, 6 );
            encoder.Emit( Machine.LOADLop, 0, 0, 7 );
            encoder.Emit( Machine.CALLop, 0, Machine.PBr, Machine.putintDisplacement );
            encoder.Emit( Machine.HALTop, 0, 0, 0 );

            // this workes amazingly!!

            encoder.SaveTargetProgram( @"C:\Users\Kenneth\Desktop\comiledTAM\" + FILE_NAME );


        }



        public object VisitArgumentList( ArgumentList argumentList, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitDeclarationFunctionDeclaration( DeclarationFunctionDeclaration declarationFunctionDeclaration, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitDeclarationStruct( DeclarationStruct declarationStruct, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitDeclarationVariableDeclaration( DeclarationVariableDeclaration declarationVariableDeclaration, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitExpression1( Expression1 expression1, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitExpression2( Expression2 expression2, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitExpression3( Expression3 expression3, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitFunctionCall( FunctionCall functionCall, object o )
        {
            foreach( var item in functionCall.ArgumentList.Arguments )
            {
                Emit( Machine.LOADLop, 0, 0, (int)item.Visit( this ) );
            }

            Emit( Machine.CALLop, 0 ,Machine.SBr, functionCall.FunctionDeclaration.addr);
            return null;
        }

        public object VisitFunctionDeclaration( FunctionDeclaration functionDeclaration, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitIdentifier( Identifier identifier, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitParameter( Parameter visitor, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitParameterList( ParameterList parameterList, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitPrimaryBoolyLiteral( PrimaryBoolyLiteral primaryBoolyLiteral, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitPrimaryExpression( PrimaryExpression primaryExpression, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitPrimaryFunctionCall( PrimaryFunctionCall primaryFunctionCall, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitPrimaryIdentifier( PrimaryIdentifier primaryIdentifier, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitPrimaryIntyLiteral( PrimaryIntyLiteral primaryIntyLiteral, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitProgram( AST.Program program, object o )
        {
            //define default methods TODO

            program.Declarations.ForEach( item => item.Visit( this, null ) );


            //call function start TODO

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
            throw new NotImplementedException();
        }

        public object VisitStatementFunctionCall( StatementFunctionCall statementFunctionCall, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitStatementGiveBack( StatementGiveBack statementGiveBack, object o )
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public object VisitStatementStopTheLoop( StatementStopTheLoop statementStopTheLoop, object o )
        {
            throw new NotImplementedException();
        }

        public object VisitStatementVariableDeclaration( StatementVariableDeclaration statementVariableDeclaration, object o )
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public object VisitVariableDeclarationStructVariableDeclaration( VariableDeclarationStructVariableDeclaration variableDeclarationStructVariableDeclaration, object o )
        {
            throw new NotImplementedException();
        }
    }
}
