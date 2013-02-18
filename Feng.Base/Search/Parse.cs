using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Search
{
    /// <summary>
    /// Represents a parser for identity role rule expresssions.
    /// </summary>
    public class Parser
    {
        private LexicalAnalyzer lexer;
        private TokenType token;

        /// <summary>
        /// Parses the the specified expression into a
        /// <see cref="ISearchExpression"/>.
        /// </summary>
        /// <param name="expression">An expression.</param>
        /// <returns>A <see cref="ISearchExpression"/>
        /// object that is the root of the parse tree.</returns>
        public ISearchExpression Parse(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return null;

            this.lexer = new LexicalAnalyzer(expression);
            this.MoveNext();
            ISearchExpression c = this.ParseOrOperator();
            this.AssertTokenType(TokenType.EndOfFile);
            return c;
        }

        private ISearchExpression ParseOrOperator()
        {
            ISearchExpression c = this.ParseAndOperator();
            while (this.token == TokenType.Or)
            {
                this.MoveNext();
                c = new LogicalExpression(c, this.ParseAndOperator(), LogicalOperator.Or);
            }
            return c;
        }

        private ISearchExpression ParseAndOperator()
        {
            ISearchExpression c = this.ParseComplexExpression();
            while (this.token == TokenType.And)
            {
                this.MoveNext();
                c = new LogicalExpression(c, this.ParseComplexExpression(), LogicalOperator.And);
            }
            return c;
        }

        private ISearchExpression ParseComplexExpression()
        {
            ISearchExpression expression = null;

            switch (this.token)
            {
                case TokenType.LeftParenthesis:
                    this.MoveNext();
                    ISearchExpression c = this.ParseOrOperator();
                    this.AssertTokenType(TokenType.RightParenthesis);
                    expression = c;
                    break;
                case TokenType.Not:
                    this.MoveNext();
                    expression = new LogicalExpression(this.ParseComplexExpression(), null, LogicalOperator.Not);
                    break;
                default:
                    expression = ParseSimpleExpression();
                    break;
            }

            return expression;
        }

        private ISearchExpression ParseSimpleExpression()
        {
            string s1 = ParseWordExpression();
            string s2;
            ISearchExpression exp;
            switch (this.token)
            {
                case TokenType.Eq:
                    this.MoveNext();
                    s2 = ParseWordExpression();
                    return SearchExpression.Eq(s1, s2);
                case TokenType.NotEq:
                    this.MoveNext();
                    s2 = ParseWordExpression();
                    return SearchExpression.NotEq(s1, s2);
                case TokenType.EqProperty:
                    this.MoveNext();
                    s2 = ParseWordExpression();
                    return SearchExpression.EqProperty(s1, s2);
                case TokenType.Gt:
                    this.MoveNext();
                    s2 = ParseWordExpression();
                    return SearchExpression.Gt(s1, s2);
                case TokenType.Ge:
                    this.MoveNext();
                    s2 = ParseWordExpression();
                    return SearchExpression.Ge(s1, s2);
                case TokenType.Lt:
                    this.MoveNext();
                    s2 = ParseWordExpression();
                    return SearchExpression.Lt(s1, s2);
                case TokenType.Le:
                    this.MoveNext();
                    s2 = ParseWordExpression();
                    return SearchExpression.Le(s1, s2);
                case TokenType.InG:
                    this.MoveNext();
                    s2 = ParseWordExpression();
                    return SearchExpression.InG(s1, GetArrayList(s2));
                case TokenType.GInG:
                    this.MoveNext();
                    s2 = ParseWordExpression();
                    return SearchExpression.GInG(s1, GetArrayList(s2));
                case TokenType.Like:
                    this.MoveNext();
                    s2 = ParseWordExpression();
                    return SearchExpression.Like(s1, s2);
                case TokenType.IsNull:
                    exp = SearchExpression.IsNull(s1);
                    this.MoveNext();
                    return exp;
                case TokenType.IsNotNull:
                    exp = SearchExpression.IsNotNull(s1);
                    this.MoveNext();
                    return exp;
                case TokenType.Sql:
                    exp = SearchExpression.Sql(s1);
                    this.MoveNext();
                    return exp;
                default:
                    throw new ArgumentException("Invalid token of " + token);
            }
        }

        private static System.Collections.IList GetArrayList(string str)
        {
            if (str[0] != '[' && str[str.Length - 1] != ']')
            {
                throw new ArgumentException("Arraylist format is wrong!");
            }
            string[] ss = str.Substring(1, str.Length - 2).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<object> arr = new List<object>();
            arr.AddRange(ss);
            return arr;
        }

        private string ParseWordExpression()
        {
            if (this.token != TokenType.Word
                && this.token != TokenType.QuotedString)
            {
                this.AssertTokenType(TokenType.Word);
            }

            string word = this.lexer.Current;

            switch (this.token)
            {
                case TokenType.Word:
                    break;
                case TokenType.QuotedString:
                    word = word.Substring(1, word.Length - 2);
                    break;
            }

            this.MoveNext();
            return word;
        }

        /// <devdoc>Get the next token from the lexer.</devdoc>
        private void MoveNext()
        {
            this.token = this.lexer.MoveNext();
        }

        /// <devdoc>Asserts that the current token is 
        /// of the specified type.</devdoc>
        private void AssertTokenType(TokenType expected)
        {
            if (this.token != expected)
            {
                throw new ArgumentException("Invalid tokenType");
            }
            this.MoveNext();
        }

        private int GetIndex()
        {
            int index = 0;
            if (this.lexer.PreviousMatch != null)
            {
                index = this.lexer.PreviousMatch.Index +
                    this.lexer.PreviousMatch.Length - 1;
            }
            return index;
        }

        private static string GetTokenName(TokenType t)
        {
            switch (t)
            {
                case TokenType.Or:
                    return "OR";
                case TokenType.And:
                    return "AND";
                case TokenType.Not:
                    return "NOT";
                case TokenType.Word:
                    return "word";
                case TokenType.LeftParenthesis:
                    return "(";
                case TokenType.RightParenthesis:
                    return ")";
                case TokenType.QuotedString:
                    return "quoted string";
                case TokenType.EndOfFile:
                    return "end of file";
                default:
                    return "???";
            }
        }

        private string ConcatTokenNames(TokenType[] tokenTypes)
        {
            StringBuilder message = new StringBuilder();

            for (int i = 0; i < tokenTypes.Length; i++)
            {
                message.Append('"');
                message.Append(GetTokenName(tokenTypes[i]));
                message.Append('"');

                if (tokenTypes.Length > 2 && i < tokenTypes.Length - 1)
                {
                    message.Append(",");
                }

                if (i == tokenTypes.Length - 2)
                {
                    message.Append("Or");
                    message.Append(' ');
                }
            }

            return message.ToString();
        }
    }
}
