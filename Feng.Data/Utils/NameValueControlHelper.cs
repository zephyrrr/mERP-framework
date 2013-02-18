using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class NameValueControlHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nvName"></param>
        /// <param name="displayText"></param>
        /// <returns></returns>
        public static object GetMultiValue(string nvName, string displayText)
        {
            if (string.IsNullOrEmpty(displayText))
                return null;

            NameValueMapping nv = NameValueMappingCollection.Instance[nvName];
            string valueMember = nv.ValueMember;
            string displayMember = nv.DisplayMember;

            string[] ss = displayText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            bool atFirst = true;
            if (ss.Length == 1)
            {
                return NameValueMappingCollection.Instance.FindColumn2FromColumn1(nvName, displayMember, valueMember, ss[0]);
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (string s in ss)
                {
                    if (!atFirst)
                    {
                        sb.Append(",");
                    }
                    atFirst = false;
                    sb.Append(NameValueMappingCollection.Instance.FindColumn2FromColumn1(nvName, displayMember,
                                                                                        valueMember, s));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nvName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetMultiString(string nvName, object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            NameValueMapping nv = NameValueMappingCollection.Instance[nvName];
            string valueMember = nv.ValueMember;
            string displayMember = nv.DisplayMember;

            string rawText = value.ToString();
            StringBuilder sb = new StringBuilder();
            string[] ss = rawText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            bool atFirst = true;
            foreach (string s in ss)
            {
                if (!atFirst)
                {
                    sb.Append(",");
                }
                atFirst = false;
                sb.Append(NameValueMappingCollection.Instance.FindColumn2FromColumn1(nvName, valueMember, displayMember, s));
            }
            return sb.ToString();
        }
    }
}
