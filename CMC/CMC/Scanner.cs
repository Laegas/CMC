using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CMC
{
    class Scanner
    {
        private SourceFile sourceFile;
        private char currentChar;

        public Scanner(SourceFile sourcerFile)
        {
            this.sourceFile = sourcerFile;
            ReadNextCharacterIntoCurrentChar();
        }


        private void ReadNextCharacterIntoCurrentChar()
        {
            try
            {
                currentChar = sourceFile.GetChar();
            }
            catch (EndOfStreamException ex)
            {
                //return end of file char
                currentChar = '\0';
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

            var garbageCharSet = new HashSet<char>(chars);
            while (garbageCharSet.Contains(currentChar))
            {
                if ('#' == currentChar)
                {
                    //read till new line
                    while ('\n' != currentChar || '\0' != currentChar)
                    {
                        ReadNextCharacterIntoCurrentChar();
                    }

                    if ('\n' == currentChar)
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

            TokenType tokenType = null;
            switch (currentChar)
            {
                case '+': case '-': case '*':
                    tokenType = TokenType.INTY_OPERATOR;
                    break;
                case '=':
                    tokenType = TokenType.ASSIGNMENT;
                    break;
                case ';':
                    tokenType = TokenType.SEMICOLON;
                    break;
                case ''

            }

            if (tokenType != null)
            {
                return new Token(currentChar.ToString(), tokenType);
            }

            if (IsDigit(currentChar))
            {
                while (IsDigit(currentChar))
                {
                    stringBuilder.Append(currentChar);
                    ReadNextCharacterIntoCurrentChar();
                }

                return new Token(stringBuilder.ToString(), TokenType.INTY_LITERAL);
            }
            if (IsLetter(currentChar))
            {
                while (IsLetter(currentChar))
                {
                    stringBuilder.Append(currentChar);
                    ReadNextCharacterIntoCurrentChar();
                }

                return new Token(stringBuilder.ToString(), TokenType.IDENTIFIER);
            }



        }

        private bool IsLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }
        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }
        public Token ScanToken()
        {
            //check for comment and other garbage
            RemoveGarbage();

            if ('\0' == currentChar)
            {
                throw new Exception("There are no more tokens in the file");
            }

            currentWord = new StringBuilder();
            var token = GetToken();

            //generate token


            //returh token
        }
    }
}
