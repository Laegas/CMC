using System;
using System.Collections.Generic;
using NUnit.Framework;
using CMC;

namespace CMC_UnitTest
{
    public class ScannerUnitTest
    {
        [Test]
        public void Scanner_KebabDefinition()
        {
            // Arrange
            var expectedTokens = new List<Token>()
            {
                new Token("kebab", Token.TokenType.KEBAB),
                new Token("myKebab", Token.TokenType.USER_CREATABLE_ID),
                new Token("[", Token.TokenType.LEFT_SQUARE),
                new Token("inty", Token.TokenType.VARIABLE_TYPE),
                new Token("maInt", Token.TokenType.USER_CREATABLE_ID),
                new Token(";", Token.TokenType.SEMICOLON),
                new Token("]", Token.TokenType.RIGHT_SQUARE),
                new Token("", Token.TokenType.END_OF_TEXT)
            };

            // Act
            var tokens = Runner(@"examples/kebabDefinition.pudekcuf");
            
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
                new Token("myKebab", Token.TokenType.USER_CREATABLE_ID),
                new Token("myNamedKebab", Token.TokenType.USER_CREATABLE_ID),
                new Token(";", Token.TokenType.SEMICOLON),
                new Token("myNamedKebab", Token.TokenType.USER_CREATABLE_ID),
                new Token(".", Token.TokenType.DOT),
                new Token("maInt", Token.TokenType.USER_CREATABLE_ID),
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

        [Test]
        public void Scanner_FunctionDefinition()
        {
            // Arrange
            var expectedTokens = new List<Token>()
            {
                new Token("function", Token.TokenType.FUNCTION),
                new Token("multiply", Token.TokenType.USER_CREATABLE_ID),
                new Token("takes", Token.TokenType.TAKES),
                new Token("inty", Token.TokenType.VARIABLE_TYPE),
                new Token("a", Token.TokenType.USER_CREATABLE_ID),
                new Token(",", Token.TokenType.COMMA),
                new Token("inty", Token.TokenType.VARIABLE_TYPE),
                new Token("b", Token.TokenType.USER_CREATABLE_ID),
                new Token("givesBack", Token.TokenType.GIVES_BACK),
                new Token("inty", Token.TokenType.VARIABLE_TYPE),
                new Token("[", Token.TokenType.LEFT_SQUARE),
                new Token("giveBack", Token.TokenType.GIVE_BACK),
                new Token("a", Token.TokenType.USER_CREATABLE_ID),
                new Token("*", Token.TokenType.OPERATOR_3),
                new Token("b", Token.TokenType.USER_CREATABLE_ID),
                new Token(";", Token.TokenType.SEMICOLON),
                new Token("]", Token.TokenType.RIGHT_SQUARE),
                new Token("", Token.TokenType.END_OF_TEXT),
            };

            // Act
            var tokens = Runner(@"examples/functionDefinition.pudekcuf");
            
            // Assert
            Assert.AreEqual(18, tokens.Count);
            Assert.AreEqual(expectedTokens, tokens);
        }

        [Test]
        public void Scanner_FunctionCall()
        {
            // Arrange
            var expectedTokens = new List<Token>()
            {
                new Token("inty", Token.TokenType.VARIABLE_TYPE),
                new Token("result", Token.TokenType.USER_CREATABLE_ID),
                new Token("=", Token.TokenType.ASSIGNMENT),
                new Token("call", Token.TokenType.CALL),
                new Token("multiply", Token.TokenType.USER_CREATABLE_ID),
                new Token("with", Token.TokenType.WITH),
                new Token("1", Token.TokenType.INTY_LITERAL),
                new Token(",", Token.TokenType.COMMA),
                new Token("2", Token.TokenType.INTY_LITERAL),
                new Token(";", Token.TokenType.SEMICOLON),
                new Token("", Token.TokenType.END_OF_TEXT),
            };

            // Act
            var tokens = Runner(@"examples/functionCall.pudekcuf");
            
            // Assert
            Assert.AreEqual(11, tokens.Count);
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

            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }
            return tokens;
        }
    }
}