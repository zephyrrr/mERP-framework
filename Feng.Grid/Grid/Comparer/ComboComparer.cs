using System;
using System.Collections;
using System.Text;

namespace Feng.Grid.Comparer
{
    /// <summary>
    /// 用于Grid中ComboViewer的比较器
    /// </summary>
    public sealed class ComboComparer : IComparer
    {
        private string m_nvName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nvName"></param>
        public ComboComparer(string nvName)
        {
            m_nvName = nvName;
        }

        /// <summary>
        /// Compare
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        int IComparer.Compare(object x, object y)
        {
            return Compare(x, y);
        }

        /// <summary>
        /// Compare
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            string vx = NameValueMappingCollection.Instance.FindNameFromId(m_nvName, x);
            string vy = NameValueMappingCollection.Instance.FindNameFromId(m_nvName, y);
            return string.Compare(vx, vy, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}