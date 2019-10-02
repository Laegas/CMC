using System.Collections.Generic;
using System.IO;
using System.Text;
using static CMC.Token;

namespace CMC
{
    public class Scanner
    {
        private SourceFile sourceFile;
        private char _currentChar;

        public Scanner( SourceFile sourceFile )
        {
            this.sourceFile = sourceFile;
            ReadNextCharacterIntoCurrentChar();
        }


        private void ReadNextCharacterIntoCurrentChar()
        {
            try
            {
                _currentChar = sourceFile.GetChar();
            }
            catch(EndOfStreamException)
            {
                //return end of file char
                _currentChar = '\0';
            }
        }

        private void RemoveGarbage()
        {
            char[] chars =
            {
                '\n',
                '\t',
                '\r',
                '#',
                ' '
            };

            var garbageCharSet = new HashSet<char>( chars );
            while( garbageCharSet.Contains( _currentChar ) )
            {
                if( '#' == _currentChar )
                {
                    //read till new line
                    while( '\n' != _currentChar && '\0' != _currentChar )
                    {
                        ReadNextCharacterIntoCurrentChar();
                    }

                    if( '\n' == _currentChar )
                    {
                        ReadNextCharacterIntoCurrentChar(); // removes the '\n'
                    }
                }
                else
                {
                    ReadNextCharacterIntoCurrentChar();
                }
            }
        }

        private Token GetToken()
        {
            var stringBuilder = new StringBuilder();

            var listOfOneCharTokens = new List<TokenType>( new [] {
                TokenType.INTY_OPERATOR,
                TokenType.DOT,
                TokenType.ASSIGNMENT,
                TokenType.SEMICOLON,
                TokenType.LEFT_PAREN,
                TokenType.RIGHT_PAREN,
                TokenType.LEFT_SQUARE,
                TokenType.RIGHT_SQUARE,
                TokenType.END_OF_TEXT,
                TokenType.COMMA
            } );

            foreach( var item in listOfOneCharTokens )
            {
                if( Token.CharIsTokenType( item, _currentChar ) )
                {
                    var oldChar = _currentChar;

                    ReadNextCharacterIntoCurrentChar();
                    return new Token( oldChar.ToString(), item );
                }
            }

            // is inty literal?
            if( IsDigit( _currentChar ) )
            {
                while( IsDigit( _currentChar ) )
                {
                    stringBuilder.Append( _currentChar );
                    ReadNextCharacterIntoCurrentChar();
                }

                return new Token( stringBuilder.ToString(), TokenType.INTY_LITERAL );
            }

            //should now be an identifyer
            if( IsLetter( _currentChar ) )
            {
                while( IsLetter( _currentChar ) )
                {
                    stringBuilder.Append( _currentChar );
                    ReadNextCharacterIntoCurrentChar();
                }

                string identifierSpelling = stringBuilder.ToString();
                var listOfKeywords = new List<TokenType>( new [] {
                    TokenType.KEBAB,
                    TokenType.GIVE_BACK,
                    TokenType.GIVES_BACK,
                    TokenType.TAKES,
                    TokenType.STOP_THE_LOOP,
                    TokenType.LOOP,
                    TokenType.IF,
                    TokenType.CALL,
                    TokenType.WITH,
                    TokenType.FUNCTION,
                    TokenType.VARIABLE_TYPE,
                    TokenType.BOOLY_LITERAL,
                    TokenType.NOTHING,
                    TokenType.BOOLY_OPERATOR
                } );

                foreach( var item in listOfKeywords )
                {
                    if( Token.IdentifierIsTokenType( item, identifierSpelling ) )
                    {
                        return new Token( identifierSpelling, item);
                    }
                }
                return new Token( identifierSpelling, TokenType.IDENTIFIER );
            }

            return new Token( "", TokenType.ERROR );
        }

        private bool IsLetter( char c )
        {
            return ( c >= 'a' && c <= 'z' ) || ( c >= 'A' && c <= 'Z' );
        }
        private bool IsDigit( char c )
        {
            return c >= '0' && c <= '9';
        }
        public Token ScanToken()
        {
            //check for comment and other garbage
            RemoveGarbage();

            if( '\0' == _currentChar  || '\uffff' == _currentChar)
            {
                return new Token( "", TokenType.END_OF_TEXT );
            }

            return GetToken();
        }
    }
}
