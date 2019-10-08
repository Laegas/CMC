using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMC
{
    class Token
    {

        private static Dictionary<TokenType, List<string>> tokenTypeToSpellingDictionary = new Dictionary<TokenType, List<string>>();
        static Token() {

            var allEnumVals = Enum.GetValues( typeof( TokenType ) ).Cast<TokenType>();
            foreach(var item in allEnumVals )
            {
                tokenTypeToSpellingDictionary.Add( item, new List<string>());
            }
            tokenTypeToSpellingDictionary[ TokenType.INTY_OPERATOR ].AddRange( new string[] { "+", "-", "*", "is" } );
            tokenTypeToSpellingDictionary[ TokenType.DOT ].AddRange( new string[] { "." } );
            tokenTypeToSpellingDictionary[ TokenType.COMMA ].AddRange( new string[] { "," } );
            tokenTypeToSpellingDictionary[ TokenType.ASSIGNMENT ].AddRange( new string[] { "=" } );
            tokenTypeToSpellingDictionary[ TokenType.SEMICOLON ].AddRange( new string[] { ";" } );
            tokenTypeToSpellingDictionary[ TokenType.LEFT_PAREN ].AddRange( new string[] { "(" } );
            tokenTypeToSpellingDictionary[ TokenType.RIGHT_PAREN ].AddRange( new string[] { ")" } );
            tokenTypeToSpellingDictionary[ TokenType.LEFT_SQUARE ].AddRange( new string[] { "[" } );
            tokenTypeToSpellingDictionary[ TokenType.RIGHT_SQUARE ].AddRange( new string[] { "]" } );
            tokenTypeToSpellingDictionary[ TokenType.KEBAB ].AddRange( new string[] { "kebab" } );
            tokenTypeToSpellingDictionary[ TokenType.GIVE_BACK ].AddRange( new string[] { "giveBack" } );
            tokenTypeToSpellingDictionary[ TokenType.GIVES_BACK ].AddRange( new string[] { "givesBack" } );
            tokenTypeToSpellingDictionary[ TokenType.TAKES ].AddRange( new string[] { "takes" } );
            tokenTypeToSpellingDictionary[ TokenType.STOP_THE_LOOP ].AddRange( new string[] { "stopTheLoop" } );
            tokenTypeToSpellingDictionary[ TokenType.LOOP ].AddRange( new string[] { "loop" } );
            tokenTypeToSpellingDictionary[ TokenType.IF ].AddRange( new string[] { "if" } );
            tokenTypeToSpellingDictionary[ TokenType.CALL ].AddRange( new string[] { "call" } );
            tokenTypeToSpellingDictionary[ TokenType.WITH ].AddRange( new string[] { "with" } );
            tokenTypeToSpellingDictionary[ TokenType.FUNCTION ].AddRange( new string[] { "function" } );
            tokenTypeToSpellingDictionary[ TokenType.END_OF_TEXT ].AddRange( new string[] { "\0" } );
            tokenTypeToSpellingDictionary[ TokenType.VARIABLE_TYPE ].AddRange( new string[] { "inty","booly" } );
            tokenTypeToSpellingDictionary[ TokenType.BOOLY_LITERAL ].AddRange( new string[] { "aye","nay" } );
            tokenTypeToSpellingDictionary[ TokenType.NOTHING ].AddRange( new string[] { "nothing" } );
            tokenTypeToSpellingDictionary[ TokenType.BOOLY_OPERATOR ].AddRange( new string[] { "or", "and" } );
        }
        
        //this probably works
        public static bool CharIsTokenType(TokenType type, char theChar )
        {
            return tokenTypeToSpellingDictionary[ type ].Contains( theChar.ToString() );
        }
        public static bool IdentifierIsTokenType( TokenType type, string identifierSpelling )
        {
            return tokenTypeToSpellingDictionary[ type ].Contains( identifierSpelling );
        }

        public string Spelling { get; set; }
        public TokenType TheTokenType { get; set; }

        /// <summary>
        /// this is a dumb class
        /// </summary>
        /// <param name="spelling"></param>
        /// <param name="tokenType"></param>
        public Token(string spelling, TokenType tokenType)
        {
            this.Spelling = spelling;
            this.TheTokenType = tokenType;

        }


        public enum TokenType {

            USER_CREATABLE_ID,
            IDENTIFIER,
            INTY_OPERATOR,
            BOOLY_OPERATOR,
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

    }
}
