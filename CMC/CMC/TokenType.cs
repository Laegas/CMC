using System;
using System.Collections.Generic;
using System.Text;

namespace CMC
{
    class TokenType
    {
        public string[] KeywordSpellings { get; }

        private TokenType(params string[] keywordSpellings)
        {
            KeywordSpellings = keywordSpellings;
        }

        public static TokenType IDENTIFIER = new TokenType();
        public static TokenType INTY_OPERATOR = new TokenType("+", "-", "*", "is");
        public static TokenType BOOLY_OPERATOR = new TokenType();
        public static TokenType INTY_LITERAL = new TokenType();
        public static TokenType BOOLY_LITERAL = new TokenType();
        public static TokenType VARIABLE_TYPE = new TokenType();

        public static TokenType DOT = new TokenType(".");
        public static TokenType ASSIGNMENT = new TokenType("=");
        public static TokenType SEMICOLON = new TokenType(";");
        public static TokenType LEFT_PAREN = new TokenType("(");
        public static TokenType RIGHT_PAREN = new TokenType(")");
        public static TokenType LEFT_SQUARE = new TokenType(")");
        public static TokenType RIGHT_SQUARE = new TokenType(")");
        public static TokenType KEBAB = new TokenType("kebab");
        public static TokenType GIVE_BACK = new TokenType("giveBack");
        public static TokenType GIVES_BACK = new TokenType("givesBack");
        public static TokenType TAKES = new TokenType("takes");
        public static TokenType STOP_THE_LOOP = new TokenType("stopTheLoop");
        public static TokenType LOOP = new TokenType("loop");
        public static TokenType IF = new TokenType("if");
        public static TokenType CALL = new TokenType("call");
        public static TokenType WITH = new TokenType("with");
        public static TokenType FUNCTION = new TokenType("function");


    }
}