using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Feng.Utils
{
    /// <summary>
    /// 字符串序列
    /// </summary>
    public struct StringSequence
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="preCode"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        public StringSequence(string preCode, int begin, int end)
        {
            m_preCode = preCode;
            m_begin = begin;
            m_end = end;
        }
        string m_preCode;
        int m_begin, m_end;

        /// <summary>
        /// 前缀
        /// </summary>
        public string Precode
        {
            get { return m_preCode; }
        }

        /// <summary>
        /// 起始编号
        /// </summary>
        public int Begin
        {
            get { return m_begin; }
        }

        /// <summary>
        /// 结束编号
        /// </summary>
        public int End
        {
            get { return m_end; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return m_preCode.GetHashCode() + 29 * m_begin ^ m_end;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is StringSequence))
                return false;

            return Equals((StringSequence)obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(StringSequence other)
        {
            if (m_preCode != other.m_preCode)
                return false;
            if (m_begin != other.m_begin)
                return false;

            return m_end == other.m_end;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="here"></param>
        /// <param name="there"></param>
        /// <returns></returns>
        public static bool operator ==(StringSequence here, StringSequence there)
        {
            return here.Equals(there);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="here"></param>
        /// <param name="there"></param>
        /// <returns></returns>
        public static bool operator !=(StringSequence here, StringSequence there)
        {
            return !here.Equals(there);
        }    

    }

    /// <summary>
    /// Helper for String
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string NormalizePropertyName(string s)
        {
            return s.Replace("/", "").Replace(".", "_");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string NullToEmpty(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            else
                return s;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(object value)
        {
            return ( value == null ) || (object.Equals( string.Empty, value));
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sectionName"></param>
        //public static void RemoveConfigSection(string sectionName)
        //{
        //    System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //    ConfigurationSection section = config.GetSection(sectionName);
        //    section.SectionInformation.SetRawXml("");
        //    config.Save();
        //}

        /// <summary>
        /// 获取字符串序列。
        /// 例如，strBegin = ABC123, strEnd = ABC126, 则返回StringSequence(ABC, 123, 126)
        /// </summary>
        /// <param name="strBegin"></param>
        /// <param name="strEnd"></param>
        /// <returns></returns>
        public static StringSequence GetSequnceString(string strBegin, string strEnd)
        {
            if (strBegin.Length != strEnd.Length)
            {
                throw new ArgumentException("号码长度不一致！");
            }
            int preLength = 0;
            int length = strBegin.Length;
            for (int i = 0; i < length; ++i)
            {
                if (strBegin[i] != strEnd[i])
                {
                    preLength = i;
                    break;
                }
            }
            int? begin = ConvertHelper.ToInt(strBegin.Substring(preLength));
            if (!begin.HasValue)
            {
                throw new ArgumentException("号码数据不合理！");
            }
            int? end = ConvertHelper.ToInt(strEnd.Substring(preLength));
            if (!end.HasValue)
            {
                throw new ArgumentException("号码数据不合理！");
            }
            if (begin.Value > end.Value)
            {
                throw new ArgumentException("号码起始号小于终止号！");
            }
            return new StringSequence(strBegin.Substring(0, preLength), begin.Value, end.Value);
        }

        private static bool IsCommandLineArgPrefix(string arg)
        {
            return (arg[0] == '-' || arg[0] == '/');
        }

        /// <summary>
        /// 分析命令行参数
        /// 参数以空格风格，具体参数用"-"或"/"连接
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ParseCommandLineArgs(string[] args)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            for (int i = 0; i < args.Length; ++i)
            {
                if (!IsCommandLineArgPrefix(args[i]))
                {
                    throw new ArgumentException("Invalid args format " + args[i]);
                }
                int idx = args[i].IndexOf(':');
                if (idx == -1)
                {
                    ret[args[i].Substring(1)] = null;
                }
                else
                {
                    ret[args[i].Substring(1, idx - 1)] = args[i].Substring(idx + 1);
                }
            }

            return ret;
        }

        /// <summary>
        /// Split, 但是""内的内容不Split
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string[] Split(string expression, char delimiter)
        {
            return Split(expression, delimiter.ToString());
        }

        /// <summary>
        /// Split, 但是""内的内容不Split
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string[] Split(string expression, string delimiter)
        {
            return Split(expression, delimiter, "\"", true, true);
        }

        /// <summary>
        /// Split
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="delimiter"></param>
        /// <param name="qualifier"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="removeQualifier"></param>
        /// <returns></returns>
        public static string[] Split(string expression, string delimiter, string qualifier, bool ignoreCase, bool removeQualifier)
        {
            string statement = String.Format("{0}(?=(?:[^{1}]*{1}[^{1}]*{1})*(?![^{1}]*{1}))", 
                                Regex.Escape(delimiter), Regex.Escape(qualifier));

            RegexOptions options = RegexOptions.Multiline; // RegexOptions.Compiled | 
            if (ignoreCase) options = options | RegexOptions.IgnoreCase;

            Regex regex = new Regex(statement, options);
            string[] ret = regex.Split(expression);
            for (int i = 0; i < ret.Length; ++i)
            {
                ret[i] = ret[i].Trim();
                if (removeQualifier)
                {
                    if (ret[i][0] == '"' && ret[i][ret[i].Length - 1] == '"')
                    {
                        ret[i] = ret[i].Substring(1, ret[i].Length - 2);
                        ret[i] = ret[i].Trim();
                    }
                }
                
            }
            return ret;
        }

        ///// <summary>
        ///// Split, 但是""内的内容不Split
        ///// </summary>
        ///// <param name="text"></param>
        ///// <param name="delimiter"></param>
        ///// <returns></returns>
        //public static string[] SplitQuoted(string text, char delimiter)
        //{
        //    List<string> ret = new List<string>();
        //    int start = 0;
        //    bool inQuoto = false;
        //    for (int i = 0; i <= text.Length; ++i)
        //    {
        //        if ((i == text.Length || text[i] == delimiter) && !inQuoto)
        //        {
        //            if (i - 1 > start)
        //            {
        //                string s = text.Substring(start, i - start);
        //                s = s.Trim();
        //                if (s[0] == '"' && s[s.Length - 1] == '"')
        //                {
        //                    s = s.Substring(1, s.Length - 2).Trim();
        //                }
        //                if (!string.IsNullOrEmpty(s))
        //                {
        //                    ret.Add(s);
        //                }
        //                start = i + 1;
        //            }
        //        }
        //        else if (text[i] == '"')
        //        {
        //            inQuoto = !inQuoto;
        //        }
        //    }
        //    return ret.ToArray();
        //}

        /// <summary>
        /// 生成连续的有分隔符分开的字符串
        /// </summary>
        /// <param name="sc">要连接的字符串集</param>
        /// <param name="delimite">分隔字符串</param>
        /// <returns></returns>
        public static string DelimiteStrings(IList<string> sc, string delimite)
        {
            if (sc == null)
            {
                throw new ArgumentNullException("sc");
            }
            string deli;
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < sc.Count; ++i)
            {
                if (i == sc.Count - 1)
                {
                    deli = "";
                }
                else
                {
                    deli = delimite;
                }

                str.Append(sc[i] + deli);
            }
            return str.ToString();
        }
    }
}
