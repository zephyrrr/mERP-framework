using System;
using System.Collections.Generic;
using System.Text;


namespace Feng.Windows.Utils
{
    /// <summary>
    /// 性能查看
    /// </summary>
    public sealed class QueryPerformance
    {
        private QueryPerformance()
        {
        }

        private static long freq;
        private static long count_s, count_f;

        /// <summary>
        /// 开始性能查看
        /// </summary>
        public static void StartQuery()
        {
            Feng.Windows.NativeMethods.QueryPerformanceFrequency(ref freq);
            Feng.Windows.NativeMethods.QueryPerformanceCounter(ref count_s);
        }

        /// <summary>
        /// 结束性能查看。返回距离开始时的时间，单位是秒
        /// </summary>
        /// <returns></returns>
        public static double StopQuery()
        {
            Feng.Windows.NativeMethods.QueryPerformanceCounter(ref count_f);

            return (double) (count_f - count_s) / (double) freq;
        }
    }
}