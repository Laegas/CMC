﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CMC
{
    public class Token
    {
        public enum TokenType
        {
            USER_CREATABLE_ID,
            OPERATOR_1,
            OPERATOR_2,
            OPERATOR_3,
            COOK,

            INTY_LITERAL,
            BOOLY_LITERAL,
            VARIABLE_TYPE,
            DOT,
            COMMA,
            ASSIGNMENT,
            SEMICOLON,
            LEFT_PAREN,
            RIGHT_PAREN,
            LEFT_SQUARE,
            RIGHT_SQUARE,
            KEBAB,
            GIVE_BACK,
            GIVES_BACK,
            TAKES,
            STOP_THE_LOOP,
            LOOP,
            IF,
            CALL,
            WITH,
            FUNCTION,
            NOTHING,

            END_OF_TEXT,
            ERROR,

            NOT_YET_ASSIGNED
        }

        public static Dictionary<TokenType, List<string>> tokenTypeToSpellingDictionary =
            new Dictionary<TokenType, List<string>>();

        static Token()
        {
            IEnumerable<TokenType> allEnumVals = Enum.GetValues(typeof(TokenType)).Cast<TokenType>();
            foreach (TokenType item in allEnumVals) tokenTypeToSpellingDictionary.Add(item, new List<string>());
            tokenTypeToSpellingDictionary[TokenType.COOK].AddRange(new[] {"cook"});
            tokenTypeToSpellingDictionary[TokenType.OPERATOR_1].AddRange(new[] {"is"});
            tokenTypeToSpellingDictionary[TokenType.OPERATOR_2].AddRange(new[] {"+", "-", "or"});
            tokenTypeToSpellingDictionary[TokenType.OPERATOR_3].AddRange(new[] {"/", "*", "and"});
            tokenTypeToSpellingDictionary[TokenType.DOT].AddRange(new[] {"."});
            tokenTypeToSpellingDictionary[TokenType.COMMA].AddRange(new[] {","});
            tokenTypeToSpellingDictionary[TokenType.ASSIGNMENT].AddRange(new[] {"="});
            tokenTypeToSpellingDictionary[TokenType.SEMICOLON].AddRange(new[] {";"});
            tokenTypeToSpellingDictionary[TokenType.LEFT_PAREN].AddRange(new[] {"("});
            tokenTypeToSpellingDictionary[TokenType.RIGHT_PAREN].AddRange(new[] {")"});
            tokenTypeToSpellingDictionary[TokenType.LEFT_SQUARE].AddRange(new[] {"["});
            tokenTypeToSpellingDictionary[TokenType.RIGHT_SQUARE].AddRange(new[] {"]"});
            tokenTypeToSpellingDictionary[TokenType.KEBAB].AddRange(new[] {"kebab"});
            tokenTypeToSpellingDictionary[TokenType.GIVE_BACK].AddRange(new[] {"giveBack"});
            tokenTypeToSpellingDictionary[TokenType.GIVES_BACK].AddRange(new[] {"givesBack"});
            tokenTypeToSpellingDictionary[TokenType.TAKES].AddRange(new[] {"takes"});
            tokenTypeToSpellingDictionary[TokenType.STOP_THE_LOOP].AddRange(new[] {"stopTheLoop"});
            tokenTypeToSpellingDictionary[TokenType.LOOP].AddRange(new[] {"loop"});
            tokenTypeToSpellingDictionary[TokenType.IF].AddRange(new[] {"if"});
            tokenTypeToSpellingDictionary[TokenType.CALL].AddRange(new[] {"call"});
            tokenTypeToSpellingDictionary[TokenType.WITH].AddRange(new[] {"with"});
            tokenTypeToSpellingDictionary[TokenType.FUNCTION].AddRange(new[] {"function"});
            tokenTypeToSpellingDictionary[TokenType.END_OF_TEXT].AddRange(new[] {"\0"});
            tokenTypeToSpellingDictionary[TokenType.VARIABLE_TYPE].AddRange(new[] {"inty", "booly"});
            tokenTypeToSpellingDictionary[TokenType.BOOLY_LITERAL].AddRange(new[] {"aye", "nay"});
            tokenTypeToSpellingDictionary[TokenType.NOTHING].AddRange(new[] {"nothing"});
        }

        /// <summary>
        ///     this is a dumb class
        /// </summary>
        /// <param name="spelling"></param>
        /// <param name="tokenType"></param>
        public Token(string spelling, TokenType tokenType)
        {
            Spelling = spelling;
            TheTokenType = tokenType;
        }

        public string Spelling { get; set; }
        public TokenType TheTokenType { get; set; }

        //this probably works
        public static bool CharIsTokenType(TokenType type, char theChar)
        {
            return tokenTypeToSpellingDictionary[type].Contains(theChar.ToString());
        }

        public static bool IdentifierIsTokenType(TokenType type, string identifierSpelling)
        {
            return tokenTypeToSpellingDictionary[type].Contains(identifierSpelling);
        }
    }
}