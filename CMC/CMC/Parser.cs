using System;
using System.Collections.Generic;
using System.Text;

namespace CMC
{
    public class Parser
    {
        private Scanner scanner;
        private Token currentToken;

        public Parser(SourceFile sourceFile)
        {
            this.scanner = new Scanner(sourceFile);
            this.currentToken = scanner.ScanToken();
        }

        public void ParseProgram()
        {
            ParseDeclarations();
        }

        private void ParseDeclarations()
        {
            while (false == currentToken.TheTokenType.Equals(Token.TokenType.END_OF_TEXT))
            {
                ParseDeclaration();
            }
        }

        private void ParseDeclaration()
        {
            switch (currentToken.TheTokenType)
            {
                case Token.TokenType.VARIABLE_TYPE: //varriable declaration
                case Token.TokenType.USER_CREATABLE_ID:
                    ParseVarriableDeclaration();
                    Accept(Token.TokenType.SEMICOLON);
                    break;
                case Token.TokenType.FUNCTION:
                case Token.TokenType.LEFT_SQUARE:
                    //Function declaration
                    ParseFunctionDeclaration();
                    break;
                case Token.TokenType.KEBAB:
                    //struct
                    ParseStruct();
                    break;
            }

        }

        private void ParseVarriableDeclaration()
        {
            if (currentToken.TheTokenType.Equals(Token.TokenType.VARIABLE_TYPE))
            {
                Accept(Token.TokenType.VARIABLE_TYPE);
            }
            else
            {
                Accept(Token.TokenType.USER_CREATABLE_ID);
            }

            Accept(Token.TokenType.USER_CREATABLE_ID);

            if (currentToken.TheTokenType.Equals(Token.TokenType.ASSIGNMENT))
            {
                Accept(Token.TokenType.ASSIGNMENT);
                ParseExpression();
            }

        }
        private void ParseFunctionDeclaration()
        {
            Accept(Token.TokenType.FUNCTION);
            Accept(Token.TokenType.USER_CREATABLE_ID);
            Accept(Token.TokenType.TAKES);
            ParseParameterList();
            Accept(Token.TokenType.GIVES_BACK);
            ParseReturnType();
            Accept(Token.TokenType.LEFT_SQUARE);
            ParseStatements();
            Accept(Token.TokenType.RIGHT_SQUARE);

        }
        private void ParseStruct()
        {

            Accept(Token.TokenType.KEBAB);
            Accept(Token.TokenType.USER_CREATABLE_ID);
            Accept(Token.TokenType.LEFT_SQUARE);
            ParseVarriableDeclarationList();
            Accept(Token.TokenType.RIGHT_SQUARE);

        }
        private void ParseReturnType()
        {
            if (currentToken.TheTokenType.Equals(Token.TokenType.NOTHING))
            {
                Accept(Token.TokenType.NOTHING);
            }
            else
            {
                Accept(Token.TokenType.VARIABLE_TYPE);
            }
        }

        private void ParseParameterList()
        {
            if (currentToken.TheTokenType.Equals(Token.TokenType.NOTHING))
            {
                Accept(Token.TokenType.NOTHING);
            }
            else
            {
                Accept(Token.TokenType.VARIABLE_TYPE);
                Accept(Token.TokenType.USER_CREATABLE_ID);
                while (currentToken.TheTokenType.Equals(Token.TokenType.COMMA))
                {
                    Accept(Token.TokenType.COMMA);
                    Accept(Token.TokenType.VARIABLE_TYPE);
                    Accept(Token.TokenType.USER_CREATABLE_ID);
                }
            }
        }

        private void ParseExpression()
        {
            switch (currentToken.TheTokenType)
            {
                case Token.TokenType.LEFT_PAREN:
                    Accept(Token.TokenType.LEFT_PAREN);
                    ParseExpression();
                    Accept(Token.TokenType.RIGHT_PAREN);
                    break;

                default:
                    if (currentToken.TheTokenType.Equals(Token.TokenType.USER_CREATABLE_ID))
                    {
                        ParseIdentifier();
                    }
                    else
                    {
                        //accept literal
                        if (currentToken.TheTokenType.Equals(Token.TokenType.BOOLY_LITERAL))
                        {
                            Accept(Token.TokenType.BOOLY_LITERAL);
                        }
                        else
                        {
                            Accept(Token.TokenType.INTY_LITERAL);
                        }
                    }

                    if (currentToken.TheTokenType.Equals(Token.TokenType.INTY_OPERATOR))
                    {
                        Accept(Token.TokenType.INTY_OPERATOR);
                        ParseExpression();
                    }
                    else if (currentToken.TheTokenType.Equals(Token.TokenType.BOOLY_OPERATOR))
                    {
                        Accept(Token.TokenType.BOOLY_OPERATOR);
                        ParseExpression();
                    }
                    break;
            }

        }
        private void ParseVarriableDeclarationList()
        {
            while (currentToken.TheTokenType.Equals(Token.TokenType.VARIABLE_TYPE))
            {
                ParseVarriableDeclaration();
                Accept(Token.TokenType.SEMICOLON);
            }
        }

        private void ParseStatements()
        {
            while (CurrentTokenStartOfStatement())
            {
                ParseStatement();
            }
        }
        private bool CurrentTokenStartOfStatement()
        {
            switch (currentToken.TheTokenType)
            {
                case Token.TokenType.GIVE_BACK:
                case Token.TokenType.STOP_THE_LOOP:
                case Token.TokenType.LOOP:
                case Token.TokenType.IF:
                case Token.TokenType.CALL:
                case Token.TokenType.USER_CREATABLE_ID:
                case Token.TokenType.VARIABLE_TYPE:
                    return true;
            }
            return false;
        }

        private bool CurrentTokenStartOfExpression()
        {
            switch (currentToken.TheTokenType)
            {
                case Token.TokenType.LEFT_PAREN:
                case Token.TokenType.IDENTIFIER:
                case Token.TokenType.INTY_LITERAL:
                case Token.TokenType.BOOLY_LITERAL:
                    return true;
            }
            return false;
        }
        private void ParseStatement()
        {
            switch (currentToken.TheTokenType)
            {
                case Token.TokenType.GIVE_BACK:
                    Accept(Token.TokenType.GIVES_BACK);
                    if (CurrentTokenStartOfExpression())
                    {
                        ParseExpression();
                        Accept(Token.TokenType.SEMICOLON);
                    }
                    break;
                case Token.TokenType.STOP_THE_LOOP:
                    Accept(Token.TokenType.STOP_THE_LOOP);
                    Accept(Token.TokenType.SEMICOLON);
                    break;
                case Token.TokenType.LOOP:
                    Accept(Token.TokenType.LOOP);
                    ParseExpression();
                    Accept(Token.TokenType.LEFT_SQUARE);
                    ParseStatements();
                    Accept(Token.TokenType.RIGHT_SQUARE);
                    break;
                case Token.TokenType.IF:
                    Accept(Token.TokenType.IF);
                    ParseExpression();
                    Accept(Token.TokenType.LEFT_SQUARE);
                    ParseStatements();
                    Accept(Token.TokenType.RIGHT_SQUARE);
                    break;
                case Token.TokenType.CALL:
                    Accept(Token.TokenType.CALL);
                    Accept(Token.TokenType.USER_CREATABLE_ID);
                    Accept(Token.TokenType.WITH);
                    ParseArgumentList();
                    Accept(Token.TokenType.SEMICOLON);
                    break;
                case Token.TokenType.USER_CREATABLE_ID:
                    ParseIdentifier();
                    Accept(Token.TokenType.ASSIGNMENT);
                    ParseExpression();
                    Accept(Token.TokenType.SEMICOLON);
                    break;
                case Token.TokenType.VARIABLE_TYPE:
                    ParseVarriableDeclaration();
                    Accept(Token.TokenType.SEMICOLON);
                    break;
                default:
                    throw new Exception("Exception in parsing statement");
            }
        }


        private void ParseArgumentList()
        {
            ParseIdentifier();
            while (currentToken.TheTokenType.Equals(Token.TokenType.COMMA))
            {
                Accept(Token.TokenType.COMMA);
                ParseIdentifier();
            }

        }

        private void ParseIdentifier()
        {
            Accept(Token.TokenType.USER_CREATABLE_ID);
            while (currentToken.TheTokenType.Equals(Token.TokenType.DOT))
            {
                Accept(Token.TokenType.DOT);
                Accept(Token.TokenType.USER_CREATABLE_ID);
            }
        }

        private void Accept(Token.TokenType tokenType)
        {
            if (currentToken.TheTokenType.Equals(tokenType))
            {
                Console.WriteLine("Accepted token of type: " + tokenType.ToString() + ", with spelling: " + currentToken.Spelling);
                this.currentToken = scanner.ScanToken();
            }
            else
            {
                Console.WriteLine("The parser failed, got token:" + currentToken + ", but expected token of type: " + tokenType.ToString());
            }
        }
    }
}
