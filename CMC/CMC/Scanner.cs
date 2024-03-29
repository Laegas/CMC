﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using static CMC.Token;

namespace CMC
{
    public class Scanner
    {
        private char currentChar;
        private readonly SourceFile sourceFile;

        public Scanner(SourceFile sourcerFile)
        {
            sourceFile = sourcerFile;
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
                if ('#' == currentChar)
                {
                    //read till new line
                    while ('\n' != currentChar && '\0' != currentChar) ReadNextCharacterIntoCurrentChar();

                    if ('\n' == currentChar) ReadNextCharacterIntoCurrentChar(); // removes the '\n'
                }
                else
                {
                    ReadNextCharacterIntoCurrentChar();
                }
        }

        private Token GetToken()
        {
            var stringBuilder = new StringBuilder();

            var tokenType = TokenType.NOT_YET_ASSIGNED;


            var listOfOneCharTokens = new List<TokenType>(new[]
            {
                TokenType.DOT,
                TokenType.ASSIGNMENT,
                TokenType.SEMICOLON,
                TokenType.LEFT_PAREN,
                TokenType.RIGHT_PAREN,
                TokenType.LEFT_SQUARE,
                TokenType.RIGHT_SQUARE,
                TokenType.END_OF_TEXT,
                TokenType.COMMA
            });

            foreach (TokenType item in listOfOneCharTokens)
                if (CharIsTokenType(item, currentChar))
                {
                    char oldChar = currentChar;

                    ReadNextCharacterIntoCurrentChar();
                    return new Token(oldChar.ToString(), item);
                }

            char spelling = currentChar;

            //additional single char tokens
            switch (currentChar)
            {
                case '+':
                case '-':
                    ReadNextCharacterIntoCurrentChar();
                    return new Token(spelling.ToString(), TokenType.OPERATOR_2);
                    break;
                case '*':
                case '/':
                    ReadNextCharacterIntoCurrentChar();
                    return new Token(spelling.ToString(), TokenType.OPERATOR_3);
                    break;
            }

            // is inty literal?
            if (IsDigit(currentChar))
            {
                while (IsDigit(currentChar))
                {
                    stringBuilder.Append(currentChar);
                    ReadNextCharacterIntoCurrentChar();
                }

                return new Token(stringBuilder.ToString(), TokenType.INTY_LITERAL);
            }

            //should now be an identifyer
            if (IsLetter(currentChar))
            {
                while (IsLetter(currentChar))
                {
                    stringBuilder.Append(currentChar);
                    ReadNextCharacterIntoCurrentChar();
                }

                string identifierSpelling = stringBuilder.ToString();
                var listOfKeywords = new List<TokenType>(new[]
                {
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
                    TokenType.OPERATOR_1,
                    TokenType.OPERATOR_2,
                    TokenType.OPERATOR_3,
                    TokenType.COOK
                });

                foreach (TokenType item in listOfKeywords)
                    if (IdentifierIsTokenType(item, identifierSpelling))
                        return new Token(identifierSpelling, item);
                return new Token(identifierSpelling, TokenType.USER_CREATABLE_ID);
            }

            return new Token("", TokenType.ERROR);
        }


        private bool IsLetter(char c)
        {
            return c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z';
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        public Token ScanToken()
        {
            //check for comment and other garbage
            RemoveGarbage();

            if ('\0' == currentChar || '\uffff' == currentChar) return new Token("", TokenType.END_OF_TEXT);

            return GetToken();
        }
    }
}