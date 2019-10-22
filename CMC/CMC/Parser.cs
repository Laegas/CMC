using System;
using System.Collections.Generic;
using System.Text;

namespace CMC
{
    public class Parser
    {
        private Scanner scanner;
        private Token currentToken;

        public Parser( SourceFile sourceFile )
        {
            this.scanner = new Scanner( sourceFile );
            this.currentToken = scanner.ScanToken();
        }

        public void ParseProgram()
        {
            while( false == currentToken.TheTokenType.Equals( Token.TokenType.END_OF_TEXT ) )
            {
                ParseDeclaration();
            }
        }

        private void ParseDeclaration()
        {
            switch( currentToken.TheTokenType )
            {
                case Token.TokenType.VARIABLE_TYPE: //varriable declaration
                case Token.TokenType.USER_CREATABLE_ID:
                    ParseVariableDeclaration();
                    break;
                case Token.TokenType.FUNCTION: // function declaration
                    ParseFunctionDeclaration();
                    break;
                case Token.TokenType.KEBAB: //struct
                    ParseStruct(); 
                    break;
            }
        }

        private void ParseVariableDeclaration()
        {
            if( currentToken.TheTokenType.Equals( Token.TokenType.VARIABLE_TYPE ) )
            {
                Accept( Token.TokenType.VARIABLE_TYPE );

                Accept( Token.TokenType.USER_CREATABLE_ID );

                if( currentToken.TheTokenType.Equals( Token.TokenType.ASSIGNMENT ) )
                {
                    Accept( Token.TokenType.ASSIGNMENT );
                    ParseExpression1();
                }
            }
            else if(Token.TokenType.COOK == currentToken.TheTokenType )
            {
                ParseStructVariableDeclaration();
            }

            Accept( Token.TokenType.SEMICOLON );

        }
        private void ParseStructVariableDeclaration()
        {
            Accept( Token.TokenType.COOK );
            Accept( Token.TokenType.USER_CREATABLE_ID );
            Accept( Token.TokenType.USER_CREATABLE_ID );
        }
        private void ParseFunctionDeclaration()
        {
            Accept( Token.TokenType.FUNCTION );
            Accept( Token.TokenType.USER_CREATABLE_ID );
            Accept( Token.TokenType.TAKES );
            ParseParameterList();
            Accept( Token.TokenType.GIVES_BACK );
            ParseReturnType();
            Accept( Token.TokenType.LEFT_SQUARE );
            ParseStatements();
            Accept( Token.TokenType.RIGHT_SQUARE );
        }

        private void ParseStruct()
        {

            Accept( Token.TokenType.KEBAB );
            Accept( Token.TokenType.USER_CREATABLE_ID );
            Accept( Token.TokenType.LEFT_SQUARE );
            ParseVariableDeclarationList();
            Accept( Token.TokenType.RIGHT_SQUARE );

        }
        private void ParseReturnType()
        {
            if( currentToken.TheTokenType.Equals( Token.TokenType.NOTHING ) )
            {
                Accept( Token.TokenType.NOTHING );
            }
            else if( currentToken.TheTokenType.Equals( Token.TokenType.VARIABLE_TYPE ) )
            {
                Accept( Token.TokenType.VARIABLE_TYPE );
            }else{
               throw VeryGenericException( "Exception in parse return type" );
            }
        }

        private void ParseParameterList()
        {
            if( currentToken.TheTokenType.Equals( Token.TokenType.NOTHING ) )
            {
                Accept( Token.TokenType.NOTHING );
            }
            else if( currentToken.TheTokenType.Equals( Token.TokenType.VARIABLE_TYPE) )
            {
                Accept( Token.TokenType.VARIABLE_TYPE );
                Accept( Token.TokenType.USER_CREATABLE_ID );
                while( currentToken.TheTokenType.Equals( Token.TokenType.COMMA ) )
                {
                    Accept( Token.TokenType.COMMA );
                    Accept( Token.TokenType.VARIABLE_TYPE );
                    Accept( Token.TokenType.USER_CREATABLE_ID );
                }
            }
            else
            {
                throw VeryGenericException( "Exception in parse parameter list" );
            }
        }

        private void ParseExpression1()
        {
            ParseExpression2();
            if( Token.TokenType.OPERATOR_1 == currentToken.TheTokenType )
            {
                Accept( Token.TokenType.OPERATOR_1 );
                ParseExpression1();
            }
        }

        private void ParseExpression2()
        {
            ParseExpression3();
            if( Token.TokenType.OPERATOR_2 == currentToken.TheTokenType )
            {
                Accept( Token.TokenType.OPERATOR_2 );
                ParseExpression1();
            }

        }
        private void ParseExpression3()
        {
            ParsePrimary();
            if( Token.TokenType.OPERATOR_3 == currentToken.TheTokenType )
            {
                Accept( Token.TokenType.OPERATOR_3 );
                ParseExpression1();
            }

        }


        private void ParsePrimary()
        {
            switch( currentToken.TheTokenType )
            {
                case Token.TokenType.USER_CREATABLE_ID:
                    ParseIdentifier();
                    break;
                case Token.TokenType.BOOLY_LITERAL:
                case Token.TokenType.INTY_LITERAL:
                    ParseLiteral();
                    break;
                case Token.TokenType.CALL:
                    ParseFunctionCall();
                    break;
                case Token.TokenType.LEFT_PAREN:
                    Accept( Token.TokenType.LEFT_PAREN );
                    ParseExpression1();
                    Accept( Token.TokenType.RIGHT_PAREN );
                    break;
                default:
                    throw new Exception( "Generic exception" );
            }
        }


        //private bool CurrentTokenIsLiteral()
        //{
        //    return currentToken.TheTokenType.Equals( Token.TokenType.BOOLY_LITERAL )
        //        || currentToken.TheTokenType.Equals( Token.TokenType.INTY_LITERAL )
        //        ;
        //}
        private void ParseLiteral()
        {
            if( currentToken.TheTokenType.Equals( Token.TokenType.BOOLY_LITERAL ) )
            {
                Accept( Token.TokenType.BOOLY_LITERAL );
            }
            else if( currentToken.TheTokenType.Equals( Token.TokenType.INTY_LITERAL ) )
            {
                Accept( Token.TokenType.INTY_LITERAL );
            }
            else {
                throw VeryGenericException("exception in ParseLiteral");
            }
        }

        private static Exception VeryGenericException(string message)
        {
            throw new Exception( message);

        }

        private void ParseVariableDeclarationList()
        {
            while(
                currentToken.TheTokenType == Token.TokenType.VARIABLE_TYPE
                || currentToken.TheTokenType == Token.TokenType.USER_CREATABLE_ID
                )
            {
                ParseVariableDeclaration();
            }
        }
        
        private void ParseStatements()
        {
            while( CurrentTokenStartOfStatement() )
            {
                ParseStatement();
            }
        }
        private bool CurrentTokenStartOfStatement()
        {
            switch( currentToken.TheTokenType )
            {
                case Token.TokenType.GIVE_BACK:
                case Token.TokenType.STOP_THE_LOOP:
                case Token.TokenType.LOOP:
                case Token.TokenType.IF:
                case Token.TokenType.CALL:
                case Token.TokenType.USER_CREATABLE_ID:
                case Token.TokenType.VARIABLE_TYPE:
                case Token.TokenType.COOK:
                    return true;
            }
            return false;
        }

        private bool CurrentTokenStartOfExpression()
        {
            switch( currentToken.TheTokenType )
            {
                case Token.TokenType.LEFT_PAREN:
                case Token.TokenType.USER_CREATABLE_ID:
                case Token.TokenType.INTY_LITERAL:
                case Token.TokenType.BOOLY_LITERAL:
                    return true;
            }
            return false;
        }
        private void ParseStatement()
        {
            switch( currentToken.TheTokenType )
            {
                case Token.TokenType.GIVE_BACK:
                    Accept( Token.TokenType.GIVE_BACK );
                    if( CurrentTokenStartOfExpression() )
                    {
                        ParseExpression1();
                    }
                    Accept( Token.TokenType.SEMICOLON );
                    break;
                case Token.TokenType.STOP_THE_LOOP:
                    Accept( Token.TokenType.STOP_THE_LOOP );
                    Accept( Token.TokenType.SEMICOLON );
                    break;
                case Token.TokenType.LOOP:
                    Accept( Token.TokenType.LOOP );
                    ParseExpression1();
                    Accept( Token.TokenType.LEFT_SQUARE );
                    ParseStatements();
                    Accept( Token.TokenType.RIGHT_SQUARE );
                    break;
                case Token.TokenType.IF: //if statement
                    Accept( Token.TokenType.IF );
                    ParseExpression1();
                    Accept( Token.TokenType.LEFT_SQUARE );
                    ParseStatements();
                    Accept( Token.TokenType.RIGHT_SQUARE );
                    break;
                case Token.TokenType.CALL: // function call
                    ParseFunctionCall();
                    Accept( Token.TokenType.SEMICOLON );
                    break;
                case Token.TokenType.USER_CREATABLE_ID: // assignment
                    ParseIdentifier();
                    Accept( Token.TokenType.ASSIGNMENT );
                    ParseExpression1();
                    Accept( Token.TokenType.SEMICOLON );
                    break;
                case Token.TokenType.VARIABLE_TYPE: // variable declaration
                case Token.TokenType.COOK:
                    ParseVariableDeclaration();
                    break;
                default:
                    throw new Exception( "Exception in parsing statement" );
            }
        }


        private void ParseArgumentList()
        {
            if( Token.TokenType.NOTHING == currentToken.TheTokenType )
            {
                Accept( Token.TokenType.NOTHING );
            }
            else // in else we trust 
            {
                ParseExpression1();
                while( currentToken.TheTokenType.Equals( Token.TokenType.COMMA ) )
                {
                    Accept( Token.TokenType.COMMA );
                    ParseExpression1();
                }
            }
        }

        private void ParseFunctionCall()
        {
            Accept( Token.TokenType.CALL );
            Accept( Token.TokenType.USER_CREATABLE_ID );
            Accept( Token.TokenType.WITH );
            ParseArgumentList();
        }

        private bool CurrentTokenStartOfIdentifier()
        {
            return currentToken.TheTokenType.Equals( Token.TokenType.USER_CREATABLE_ID )
                ;
        }
        private void ParseIdentifier()
        {
            Accept( Token.TokenType.USER_CREATABLE_ID );
            while( currentToken.TheTokenType.Equals( Token.TokenType.DOT ) )
            {
                Accept( Token.TokenType.DOT );
                Accept( Token.TokenType.USER_CREATABLE_ID );
            }
        }

        private void Accept( Token.TokenType tokenType )
        {
            if( currentToken.TheTokenType.Equals( tokenType ) )
            {
                Console.WriteLine( "Accepted token of type: " + tokenType.ToString() + ", with spelling: " + currentToken.Spelling );
                this.currentToken = scanner.ScanToken();
            }
            else
            {
                Console.WriteLine( "The parser failed, got token:" + currentToken.TheTokenType + ", but expected token of type: " + tokenType.ToString() );
            }
        }
    }
}
