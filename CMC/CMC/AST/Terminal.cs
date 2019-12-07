using System;
using static CMC.AST.TerminalUtil;

namespace CMC.AST
{
    public abstract class Terminal
    {
        public string Spelling { get; }

        public Terminal(string spelling)
        {
            Spelling = spelling;
        }
    }

    public class Operator1 : Terminal
    {
        public Operator1(Token token) : base(token.Spelling)
        {
            CheckTokenType(Token.TokenType.OPERATOR_1, token.TheTokenType);
        }
    }

    public class Operator2 : Terminal
    {
        public Operator2(Token token) : base(token.Spelling)
        {
            CheckTokenType(Token.TokenType.OPERATOR_2, token.TheTokenType);
        }
    }

    public class Operator3 : Terminal
    {
        public Operator3(Token token) : base(token.Spelling)
        {
            CheckTokenType(Token.TokenType.OPERATOR_3, token.TheTokenType);
        }
    }

    public class BoolyLiteral : Terminal
    {
        public BoolyLiteral(Token token) : base(token.Spelling)
        {
            CheckTokenType(Token.TokenType.BOOLY_LITERAL, token.TheTokenType);
        }
    }

    public class IntyLiteral : Terminal
    {
        public IntyLiteral(Token token) : base(token.Spelling)
        {
            CheckTokenType(Token.TokenType.INTY_LITERAL, token.TheTokenType);
        }
    }


    public class UserCreatableID : Terminal
    {
        public UserCreatableID(Token token) : base(token.Spelling)
        {
            CheckTokenType(Token.TokenType.USER_CREATABLE_ID, token.TheTokenType);
        }
    }


    public class VariableType : Terminal
    {

        public enum ValueTypeEnum
        {
            INTY, BOOLY, NOTHING, ANY
        }
        
        public ValueTypeEnum VariableType_ { get; }
        
        public VariableType(Token token) : base(token.Spelling)
        {
            CheckTokenType(Token.TokenType.VARIABLE_TYPE, token.TheTokenType);
            VariableType_ = token.Spelling == "booly"
                ? ValueTypeEnum.BOOLY
                : ValueTypeEnum.INTY;
        }

        public VariableType(ValueTypeEnum variableType) : base(variableType.ToString())
        {
            VariableType_ = variableType;
        }
    }

    public static class TerminalUtil
    {
        public static void CheckTokenType(Token.TokenType expectedTokenType, Token.TokenType actualTokenType)
        {
            if (actualTokenType != expectedTokenType)
                throw new Exception($"Wrong token type: Expected - {expectedTokenType}, Actual - {actualTokenType}");
        }
    }
}