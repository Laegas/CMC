using System;
using System.Collections.Generic;
using System.Linq;

namespace CMC
{
    public class Token
    {
        private static readonly Dictionary<TokenType, List<string>> TokenTypeToSpellingDictionary = new Dictionary<TokenType, List<string>>();
        public string Spelling { get; }
        public TokenType TheTokenType { get; set; }
        
        public Token(string spelling, TokenType tokenType)
        {
            this.Spelling = spelling;
            this.TheTokenType = tokenType;
        }

        static Token() {

            var allEnumVals = Enum.GetValues( typeof( TokenType ) ).Cast<TokenType>();
            foreach(var item in allEnumVals )
            {
                TokenTypeToSpellingDictionary.Add( item, new List<string>());
            }
            TokenTypeToSpellingDictionary[ TokenType.INTY_OPERATOR ].AddRange( new [] { "+", "-", "*", "is" } );
            TokenTypeToSpellingDictionary[ TokenType.DOT ].AddRange( new [] { "." } );
            TokenTypeToSpellingDictionary[ TokenType.COMMA ].AddRange( new [] { "," } );
            TokenTypeToSpellingDictionary[ TokenType.ASSIGNMENT ].AddRange( new [] { "=" } );
            TokenTypeToSpellingDictionary[ TokenType.SEMICOLON ].AddRange( new [] { ";" } );
            TokenTypeToSpellingDictionary[ TokenType.LEFT_PAREN ].AddRange( new [] { "(" } );
            TokenTypeToSpellingDictionary[ TokenType.RIGHT_PAREN ].AddRange( new [] { ")" } );
            TokenTypeToSpellingDictionary[ TokenType.LEFT_SQUARE ].AddRange( new [] { "[" } );
            TokenTypeToSpellingDictionary[ TokenType.RIGHT_SQUARE ].AddRange( new [] { "]" } );
            TokenTypeToSpellingDictionary[ TokenType.KEBAB ].AddRange( new [] { "kebab" } );
            TokenTypeToSpellingDictionary[ TokenType.GIVE_BACK ].AddRange( new [] { "giveBack" } );
            TokenTypeToSpellingDictionary[ TokenType.GIVES_BACK ].AddRange( new [] { "givesBack" } );
            TokenTypeToSpellingDictionary[ TokenType.TAKES ].AddRange( new [] { "takes" } );
            TokenTypeToSpellingDictionary[ TokenType.STOP_THE_LOOP ].AddRange( new [] { "stopTheLoop" } );
            TokenTypeToSpellingDictionary[ TokenType.LOOP ].AddRange( new [] { "loop" } );
            TokenTypeToSpellingDictionary[ TokenType.IF ].AddRange( new [] { "if" } );
            TokenTypeToSpellingDictionary[ TokenType.CALL ].AddRange( new [] { "call" } );
            TokenTypeToSpellingDictionary[ TokenType.WITH ].AddRange( new [] { "with" } );
            TokenTypeToSpellingDictionary[ TokenType.FUNCTION ].AddRange( new [] { "function" } );
            TokenTypeToSpellingDictionary[ TokenType.END_OF_TEXT ].AddRange( new [] { "\0" } );
            TokenTypeToSpellingDictionary[ TokenType.VARIABLE_TYPE ].AddRange( new [] { "inty","booly" } );
            TokenTypeToSpellingDictionary[ TokenType.BOOLY_LITERAL ].AddRange( new [] { "aye","nay" } );
            TokenTypeToSpellingDictionary[ TokenType.NOTHING ].AddRange( new [] { "nothing" } );
            TokenTypeToSpellingDictionary[ TokenType.BOOLY_OPERATOR ].AddRange( new [] { "or", "and" } );
        }
        
        public static bool CharIsTokenType(TokenType type, char theChar )
        {
            return TokenTypeToSpellingDictionary[ type ].Contains( theChar.ToString() );
        }
        public static bool IdentifierIsTokenType( TokenType type, string identifierSpelling )
        {
            return TokenTypeToSpellingDictionary[ type ].Contains( identifierSpelling );
        }

        public enum TokenType {

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
            ERROR
        }

        public override string ToString()
        {
            return TheTokenType + "(" + Spelling + ")";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Token item))
            {
                return false;
            }

            return Spelling.Equals(item.Spelling) &&
                TheTokenType.Equals(item.TheTokenType);
        }

        public override int GetHashCode()
        {
            return Spelling.GetHashCode();
        }
    }
}
