using System;
using System.Collections.Generic;
using NUnit.Framework;
using CMC;

namespace CMC_UnitTest
{
    public class ScannerUnitTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Scanner_Kebab()
        {
            // Arrange
            var expectedTokens = new List<Token>()
            {
                new Token("kebab", Token.TokenType.KEBAB),
                new Token("myKebab", Token.TokenType.IDENTIFIER),
                new Token("[", Token.TokenType.LEFT_SQUARE),
                new Token("inty", Token.TokenType.VARIABLE_TYPE),
                new Token("maInt", Token.TokenType.IDENTIFIER),
                new Token(";", Token.TokenType.SEMICOLON),
                new Token("]", Token.TokenType.RIGHT_SQUARE),
                new Token("", Token.TokenType.END_OF_TEXT)
            };

            // Act
            var tokens = Runner(@"examples/kebab.pudekcuf");
            
            // Assert
            Assert.AreEqual(8, tokens.Count);
            Assert.AreEqual(expectedTokens, tokens);
        }

        [Test]
        public void Scanner_KebabAccess()
        {
            // Arrange
            var expectedTokens = new List<Token>()
            {
                new Token("myKebab", Token.TokenType.IDENTIFIER),
                new Token("myNamedKebab", Token.TokenType.IDENTIFIER),
                new Token(";", Token.TokenType.SEMICOLON),
                new Token("myNamedKebab", Token.TokenType.IDENTIFIER),
                new Token(".", Token.TokenType.DOT),
                new Token("maInt", Token.TokenType.IDENTIFIER),
                new Token("=", Token.TokenType.ASSIGNMENT),
                new Token("123", Token.TokenType.INTY_LITERAL),
                new Token(";", Token.TokenType.SEMICOLON),
                new Token("", Token.TokenType.END_OF_TEXT)
            };

            // Act
            var tokens = Runner(@"examples/kebabAccess.pudekcuf");
            
            // Assert
            Assert.AreEqual(10, tokens.Count);
            Assert.AreEqual(expectedTokens, tokens);
        }

        private List<Token> Runner(string pathFile)
        {
            var sourceFile = new SourceFile(pathFile);
            var scanner = new Scanner(sourceFile);
            var tokens = new List<Token>();

            while (true)
            {
                var token = scanner.ScanToken();
                tokens.Add(token);
                if (token.TheTokenType.Equals(Token.TokenType.END_OF_TEXT))
                {
                    break;
                }
            }

            return tokens;
        }

        private void PrintTokens(List<Token> tokens)
        {
            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }
        }
    }
}