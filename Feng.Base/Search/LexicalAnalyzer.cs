using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Feng.Search
{
    /// <summary>
    /// Represents a lexical analyzer for boolean expressions.
    /// </summary>
    class LexicalAnalyzer
    {
        private const string Pattern =
            @"\(|\)|[Oo][Rr]|[Aa][Nn][Dd]|[Nn][Oo][Tt]
            |<[>=]?|>=?|=[pP]?|[lL][iI][kK][eE]|[iI][sS][nN][uU][lL][lL]|[iI][sS][Nn][Oo][Tt][nN][uU][lL][lL]
            |[iI][nN][gG]?|ISSQL|
            |(""[^""]*"")|[^\u0000-\u0020""\(\)]+";

        private Match match;
        private Match previousMatch;
        private string expression;

        /// <summary>
        /// The expression to analyze.
        /// </summary>
        /// <param name="expression">A boolean expression.</param>
        public LexicalAnalyzer(string expression)
        {
            this.expression = expression;
        }

        /// <summary>
        /// Gets the string value of the current token.
        /// </summary>
        /// <returns>The value of the current</returns>
        public string Current
        {
            get { return this.match.Value; }
        }

        /// <summary>
        /// Gets the previous match of the regular expression.
        /// </summary>
        public Match PreviousMatch
        {
            get { return this.previousMatch; }
        }

        /// <summary>
        /// Gets the current match of the regular expression.
        /// </summary>
        /// <value>A <see cref="Match"/></value>
        public Match CurrentMatch
        {
            get { return this.match; }
        }

        /// <summary>
        /// Gets the type of the next token.
        /// </summary>
        /// <returns>A <see cref="TokenType"/> value.</returns>
        public TokenType MoveNext()
        {
            TokenType token;
            this.previousMatch = this.match;
            if (this.match == null)
            {
                this.match = Regex.Match(this.expression, Pattern, RegexOptions.None/*RegexOptions.Compiled*/);
            }
            else
            {
                this.match = this.match.NextMatch();
            }

            if (this.match.Success)
            {
                string current = this.Current.ToLowerInvariant();
                switch (current)
                {
                    case "or":
                        token = TokenType.Or;
                        break;
                    case "and":
                        token = TokenType.And;
                        break;
                    case "not":
                        token = TokenType.Not;
                        break;
                    case "(":
                        token = TokenType.LeftParenthesis;
                        break;
                    case ")":
                        token = TokenType.RightParenthesis;
                        break;
                    case "=":
                        token = TokenType.Eq;
                        break;
                    case "<>":
                        token = TokenType.NotEq;
                        break;
                    case ">=":
                        token = TokenType.Ge;
                        break;
                    case ">":
                        token = TokenType.Gt;
                        break;
                    case "<":
                        token = TokenType.Lt;
                        break;
                    case "<=":
                        token = TokenType.Le;
                        break;
                    case "=p":
                        token = TokenType.EqProperty;
                        break;
                    case "like":
                        token = TokenType.Like;
                        break;
                    case "isnull":
                        token = TokenType.IsNull;
                        break;
                    case "isnotnull":
                        token = TokenType.IsNotNull;
                        break;
                    case "in":
                        token = TokenType.InG;
                        break;
                    case "ing":
                        token = TokenType.GInG;
                        break;
                    case "issql":
                        token = TokenType.Sql;
                        break;
                    default:
                        if (current.IndexOf("\"", StringComparison.InvariantCulture) == 0)
                        {
                            if (current.LastIndexOf("\"", StringComparison.InvariantCulture) == current.Length - 1)
                            {
                                token = TokenType.QuotedString;
                            }
                            else
                            {
                                token = TokenType.InvalidCharacter;
                            }
                        }
                        else if (current.IndexOf("'", StringComparison.InvariantCulture) == 0)
                        {
                            if (current.LastIndexOf("'", StringComparison.InvariantCulture) == current.Length - 1)
                            {
                                token = TokenType.QuotedString;
                            }
                            else
                            {
                                token = TokenType.InvalidCharacter;
                            }
                        }
                        else
                        {
                            token = TokenType.Word;
                        }
                        break;
                }
            }
            else
            {
                token = TokenType.EndOfFile;
            }
            return token;
        }
    }

    /// <summary>
    /// Specifies the type of a token in an expression.
    /// </summary>
    public enum TokenType : int
    {
        /// <summary>
        /// Invalid character
        /// </summary>
        InvalidCharacter = -1,
        /// <summary>
        /// No token found
        /// </summary>
        NoToken = 0,
        /// <summary>
        /// Or token
        /// </summary>
        Or = 1,
        /// <summary>
        /// And token
        /// </summary>
        And = 2,
        /// <summary>
        /// Not token
        /// </summary>
        Not = 5,
        /// <summary>
        /// Word token
        /// </summary>
        Word = 6,
        /// <summary>
        /// Left Paren token
        /// </summary>
        LeftParenthesis = 9,
        /// <summary>
        /// Right paren token
        /// </summary>
        RightParenthesis = 10,
        /// <summary>
        /// Quoted string token
        /// </summary>
        QuotedString = 11,
        /// <summary>
        /// EOF token
        /// </summary>
        EndOfFile = 12,
        /// <summary>
        /// 等于
        /// </summary>
        Eq = 20,
        /// <summary>
        /// 不等于
        /// </summary>
        NotEq = 21,
        /// <summary>
        /// 等于属性
        /// </summary>
        EqProperty = 22,
        /// <summary>
        /// 不等于属性
        /// </summary>
        NotEqProperty = 23,
        /// <summary>
        /// 大于
        /// </summary>
        Gt = 24,
        /// <summary>
        /// 大于或等于
        /// </summary>
        Ge = 25,
        /// <summary>
        /// 小于
        /// </summary>
        Lt = 26,
        /// <summary>
        /// 小于或等于
        /// </summary>
        Le = 27,
        /// <summary>
        /// 包含
        /// </summary>
        Like = 28,
        /// <summary>
        /// 包含集合
        /// </summary>
        InG = 29,
        /// <summary>
        /// 查询集合中的值在数据库字段中出现（不同于包含集合，包含集合为查询集合中的值=数据库字段）
        /// 数据库字段一般为以“，”分割的值列表
        /// </summary>
        GInG = 30,
        /// <summary>
        /// 为空
        /// </summary>
        IsNull = 31,
        /// <summary>
        /// 不为空
        /// </summary>
        IsNotNull = 32,
        /// <summary>
        /// Sql
        /// </summary>
        Sql = 40
    }
}
