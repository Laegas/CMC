using System;
using System.Collections.Generic;
using System.Text;

namespace CMC
{
    class Token
    {
        public string Spelling { get; set; }
        public TokenType TokenType { get; set; }

        /// <summary>
        /// this is a dumb class
        /// </summary>
        /// <param name="spelling"></param>
        /// <param name="tokenType"></param>
        public Token(string spelling, TokenType tokenType)
        {
            this.Spelling = spelling;
            this.TokenType = tokenType;
        }
    }
}
