using System;
using System.Collections;
using System.Text;
using Feng.Utils;

namespace Feng.Grid.Comparer
{
    public class IntMultiImageComparer : IComparer
    {
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
            if (x == null && y == null)
                return 0;
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            string[] xx = x.ToString().Split(new char[] { '/', '-' }, StringSplitOptions.RemoveEmptyEntries);
            string[] yy = y.ToString().Split(new char[] { '/', '-' }, StringSplitOptions.RemoveEmptyEntries);
            int? xx1 = ConvertHelper.ToInt(xx[0]);
            int? xx2 = ConvertHelper.ToInt(xx[1]);
            int ixx = (xx1.HasValue ? xx1.Value : 0) + (xx2.HasValue ? xx2.Value : 0);
            int? yy1 = ConvertHelper.ToInt(yy[0]);
            int? yy2 = ConvertHelper.ToInt(yy[1]);
            int iyy = (yy1.HasValue ? yy1.Value : 0) + (yy2.HasValue ? yy2.Value : 0);

            return ixx.CompareTo(iyy);
        }
    }
}
