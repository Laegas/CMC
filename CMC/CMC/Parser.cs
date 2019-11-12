using System;
using System.Collections.Generic;
using CMC.AST;

namespace CMC
{
    public class Parser
    {
        private Token _currentToken;
        private readonly Scanner _scanner;

        public Parser(SourceFile sourceFile)
        {
            _scanner = new Scanner(sourceFile);
            _currentToken = _scanner.ScanToken();
        }

        public AST.Program ParseProgram()
        {
            var declarations = new List<Declaration>();
            while (false == _currentToken.TheTokenType.Equals(Token.TokenType.END_OF_TEXT))
                declarations.Add(ParseDeclaration());
            return new AST.Program(declarations);
        }

        private Declaration ParseDeclaration()
        {
            switch (_currentToken.TheTokenType)
            {
                case Token.TokenType.VARIABLE_TYPE: //varriable declaration
                case Token.TokenType.USER_CREATABLE_ID:
                    return new DeclarationVariableDeclaration(ParseVariableDeclaration());
                case Token.TokenType.FUNCTION: // function declaration
                    return new DeclarationFunctionDeclaration(ParseFunctionDeclaration());
                case Token.TokenType.KEBAB: //struct
                    return new DeclarationStruct(ParseStruct());
                default:
                    throw VeryGenericException("Not valid declaration type");
            }
        }

        private VariableDeclaration ParseVariableDeclaration()
        {
            VariableDeclaration variableDeclaration = null;
            if (_currentToken.TheTokenType.Equals(Token.TokenType.VARIABLE_TYPE))
            {
                var variableType = new VariableType(Accept(Token.TokenType.VARIABLE_TYPE));
                var variableName = new UserCreatableID(Accept(Token.TokenType.USER_CREATABLE_ID));


                Expression1 expression = null;
                if (_currentToken.TheTokenType.Equals(Token.TokenType.ASSIGNMENT))
                {
                    Accept(Token.TokenType.ASSIGNMENT);
                    expression = ParseExpression1();
                }

                variableDeclaration = new VariableDeclarationSimple(variableType, variableName, expression);
            }
            else if (Token.TokenType.COOK == _currentToken.TheTokenType)
            {
                variableDeclaration =
                    new VariableDeclarationStructVariableDeclaration(ParseStructVariableDeclaration());
            }

            Accept(Token.TokenType.SEMICOLON);
            return variableDeclaration;
        }

        private StructVariableDeclaration ParseStructVariableDeclaration()
        {
            Accept(Token.TokenType.COOK);
            var structName = new UserCreatableID(Accept(Token.TokenType.USER_CREATABLE_ID));
            var variableName = new UserCreatableID(Accept(Token.TokenType.USER_CREATABLE_ID));
            return new StructVariableDeclaration(structName, variableName);
        }

        private FunctionDeclaration ParseFunctionDeclaration()
        {
            Accept(Token.TokenType.FUNCTION);
            var functionName = new UserCreatableID(Accept(Token.TokenType.USER_CREATABLE_ID));
            Accept(Token.TokenType.TAKES);
            ParameterList parameterList = ParseParameterList();
            Accept(Token.TokenType.GIVES_BACK);
            ReturnType returnType = ParseReturnType();
            Accept(Token.TokenType.LEFT_SQUARE);
            Statements statements = ParseStatements();
            Accept(Token.TokenType.RIGHT_SQUARE);
            return new FunctionDeclaration(functionName, parameterList, returnType, statements);
        }

        private Struct ParseStruct()
        {
            Accept(Token.TokenType.KEBAB);
            var structName = new UserCreatableID(Accept(Token.TokenType.USER_CREATABLE_ID));
            Accept(Token.TokenType.LEFT_SQUARE);
            VariableDeclarationList variableDeclarationList = ParseVariableDeclarationList();
            Accept(Token.TokenType.RIGHT_SQUARE);
            return new Struct(structName, variableDeclarationList);
        }

        private ReturnType ParseReturnType()
        {
            if (_currentToken.TheTokenType.Equals(Token.TokenType.NOTHING))
            {
                Accept(Token.TokenType.NOTHING);
                return new ReturnTypeNothing();
            }

            if (_currentToken.TheTokenType.Equals(Token.TokenType.VARIABLE_TYPE))
                return new ReturnTypeVariableType(new VariableType(Accept(Token.TokenType.VARIABLE_TYPE)));
            throw VeryGenericException("Exception in parse return type");
        }

        private ParameterList ParseParameterList()
        {
            if (_currentToken.TheTokenType.Equals(Token.TokenType.NOTHING))
            {
                Accept(Token.TokenType.NOTHING);
                return new ParameterList(new List<Parameter>());
            }

            if (_currentToken.TheTokenType.Equals(Token.TokenType.VARIABLE_TYPE))
            {
                var otherParameters = new List<Parameter>();
                otherParameters.Add(new Parameter(new VariableType(Accept(Token.TokenType.VARIABLE_TYPE)),
                    new UserCreatableID(Accept(Token.TokenType.USER_CREATABLE_ID))));
                while (_currentToken.TheTokenType.Equals(Token.TokenType.COMMA))
                {
                    Accept(Token.TokenType.COMMA);
                    otherParameters.Add(new Parameter(new VariableType(Accept(Token.TokenType.VARIABLE_TYPE)),
                        new UserCreatableID(Accept(Token.TokenType.USER_CREATABLE_ID))));
                }

                return new ParameterList(otherParameters);
            }

            throw VeryGenericException("Exception in parse parameter list");
        }

        private Expression1 ParseExpression1()
        {
            Expression2 expression2 = ParseExpression2();
            Operator1 operator1 = null;
            Expression1 expression1 = null;
            if (Token.TokenType.OPERATOR_1 == _currentToken.TheTokenType)
            {
                operator1 = new Operator1(Accept(Token.TokenType.OPERATOR_1));
                expression1 = ParseExpression1();
            }

            return new Expression1(expression2, operator1, expression1);
        }

        private Expression2 ParseExpression2()
        {
            Expression3 expression3 = ParseExpression3();
            Operator2 operator2 = null;
            Expression1 expression1 = null;
            if (Token.TokenType.OPERATOR_2 == _currentToken.TheTokenType)
            {
                operator2 = new Operator2(Accept(Token.TokenType.OPERATOR_2));
                expression1 = ParseExpression1();
            }

            return new Expression2(expression3, operator2, expression1);
        }

        private Expression3 ParseExpression3()
        {
            Primary primary = ParsePrimary();
            Operator3 operator3 = null;
            Expression1 expression1 = null;
            if (Token.TokenType.OPERATOR_3 == _currentToken.TheTokenType)
            {
                operator3 = new Operator3(Accept(Token.TokenType.OPERATOR_3));
                expression1 = ParseExpression1();
            }

            return new Expression3(primary, operator3, expression1);
        }


        private Primary ParsePrimary()
        {
            switch (_currentToken.TheTokenType)
            {
                case Token.TokenType.USER_CREATABLE_ID:
                    return new PrimaryIdentifier(ParseIdentifier());
                case Token.TokenType.BOOLY_LITERAL:
                    return new PrimaryBoolyLiteral(ParseBoolyLiteral());
                case Token.TokenType.INTY_LITERAL:
                    return new PrimaryIntyLiteral(ParseIntyLiteral());
                case Token.TokenType.CALL:
                    return new PrimaryFunctionCall(ParseFunctionCall());
                case Token.TokenType.LEFT_PAREN:
                    Accept(Token.TokenType.LEFT_PAREN);
                    Expression1 expression = ParseExpression1();
                    Accept(Token.TokenType.RIGHT_PAREN);
                    return new PrimaryExpression(expression);
                default:
                    throw new Exception("Generic exception");
            }
        }

        private IntyLiteral ParseIntyLiteral()
        {
            Token token = Accept(Token.TokenType.INTY_LITERAL);
            return new IntyLiteral(token);
        }

        private BoolyLiteral ParseBoolyLiteral()
        {
            Token token = Accept(Token.TokenType.BOOLY_LITERAL);
            return new BoolyLiteral(token);
        }

        private static Exception VeryGenericException(string message)
        {
            throw new Exception(message);
        }

        private VariableDeclarationList ParseVariableDeclarationList()
        {
            var variableDeclarations = new List<VariableDeclaration>();
            while (
                _currentToken.TheTokenType == Token.TokenType.VARIABLE_TYPE
                || _currentToken.TheTokenType == Token.TokenType.USER_CREATABLE_ID
            )
                variableDeclarations.Add(ParseVariableDeclaration());

            return new VariableDeclarationList(variableDeclarations);
        }

        private Statements ParseStatements()
        {
            var statements = new List<Statement>();
            while (CurrentTokenStartOfStatement()) statements.Add(ParseStatement());

            return new Statements(statements);
        }

        private bool CurrentTokenStartOfStatement()
        {
            switch (_currentToken.TheTokenType)
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
            switch (_currentToken.TheTokenType)
            {
                case Token.TokenType.LEFT_PAREN:
                case Token.TokenType.USER_CREATABLE_ID:
                case Token.TokenType.INTY_LITERAL:
                case Token.TokenType.BOOLY_LITERAL:
                    return true;
            }

            return false;
        }

        private Statement ParseStatement()
        {
            switch (_currentToken.TheTokenType)
            {
                case Token.TokenType.GIVE_BACK:
                    Accept(Token.TokenType.GIVE_BACK);
                    Expression1 expressionGiveBack = null;
                    if (CurrentTokenStartOfExpression()) expressionGiveBack = ParseExpression1();

                    Accept(Token.TokenType.SEMICOLON);
                    return new StatementGiveBack(expressionGiveBack);
                case Token.TokenType.STOP_THE_LOOP:
                    Accept(Token.TokenType.STOP_THE_LOOP);
                    Accept(Token.TokenType.SEMICOLON);
                    return new StatementStopTheLoop();
                case Token.TokenType.LOOP:
                    Accept(Token.TokenType.LOOP);
                    Expression1 loopCondition = ParseExpression1();
                    Accept(Token.TokenType.LEFT_SQUARE);
                    Statements loopStatements = ParseStatements();
                    Accept(Token.TokenType.RIGHT_SQUARE);
                    return new StatementLoopStatement(loopCondition, loopStatements);
                case Token.TokenType.IF: //if statement
                    Accept(Token.TokenType.IF);
                    Expression1 ifCondition = ParseExpression1();
                    Accept(Token.TokenType.LEFT_SQUARE);
                    Statements ifStatements = ParseStatements();
                    Accept(Token.TokenType.RIGHT_SQUARE);
                    return new StatementIfStatement(ifCondition, ifStatements);
                case Token.TokenType.CALL: // function call
                    FunctionCall functionCall = ParseFunctionCall();
                    Accept(Token.TokenType.SEMICOLON);
                    return new StatementFunctionCall(functionCall);
                case Token.TokenType.USER_CREATABLE_ID: // assignment
                    Identifier identifier = ParseIdentifier();
                    Accept(Token.TokenType.ASSIGNMENT);
                    Expression1 assignmentExpression = ParseExpression1();
                    Accept(Token.TokenType.SEMICOLON);
                    return new StatementAssignment(identifier, assignmentExpression);
                case Token.TokenType.VARIABLE_TYPE: // variable declaration
                case Token.TokenType.COOK:
                    VariableDeclaration variableDeclaration = ParseVariableDeclaration();
                    return new StatementVariableDeclaration(variableDeclaration);
                default:
                    throw new Exception("Exception in parsing statement");
            }
        }


        private ArgumentList ParseArgumentList()
        {
            var expressions = new List<Expression1>();

            if (Token.TokenType.NOTHING == _currentToken.TheTokenType)
            {
                Accept(Token.TokenType.NOTHING);
            }
            else
            {
                expressions.Add(ParseExpression1());
                while (_currentToken.TheTokenType.Equals(Token.TokenType.COMMA))
                {
                    Accept(Token.TokenType.COMMA);
                    expressions.Add(ParseExpression1());
                }
            }

            return new ArgumentList(expressions);
        }

        private FunctionCall ParseFunctionCall()
        {
            Accept(Token.TokenType.CALL);
            Token functionNameToken = Accept(Token.TokenType.USER_CREATABLE_ID);
            Accept(Token.TokenType.WITH);
            ArgumentList argumentList = ParseArgumentList();
            return new FunctionCall(new UserCreatableID(functionNameToken), argumentList);
        }

        private Identifier ParseIdentifier()
        {
            Token rootId = Accept(Token.TokenType.USER_CREATABLE_ID);
            var nestedIds = new List<UserCreatableID>();
            while (_currentToken.TheTokenType.Equals(Token.TokenType.DOT))
            {
                Accept(Token.TokenType.DOT);
                Token nestedId = Accept(Token.TokenType.USER_CREATABLE_ID);
                nestedIds.Add(new UserCreatableID(nestedId));
            }

            return new Identifier(new UserCreatableID(rootId), nestedIds);
        }

        private Token Accept(Token.TokenType tokenType)
        {
            if (_currentToken.TheTokenType.Equals(tokenType))
            {
                Console.WriteLine("Accepted token of type: " + tokenType + ", with spelling: " +
                                  _currentToken.Spelling);
                Token oldToken = _currentToken;
                _currentToken = _scanner.ScanToken();
                return oldToken;
            }

            Console.WriteLine("The parser failed, got token:" + _currentToken.TheTokenType +
                              ", but expected token of type: " + tokenType);
            throw new Exception("kebab");
        }
    }
}