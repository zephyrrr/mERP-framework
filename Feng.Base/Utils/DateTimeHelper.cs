using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Utils
{
	/// <summary>
	/// 时间帮助类
	/// </summary>
	public sealed class DateTimeHelper
	{
		private DateTimeHelper()
		{
		}

		/// <summary>
		/// 设置时间为一天中的开始
		/// </summary>
		/// <param name="time"></param>
		public static DateTime GetDateTimeStartofDay(DateTime time)
		{
			return new DateTime(time.Year, time.Month, time.Day);
		}

		/// <summary>
		/// 设置时间为一天中的结束
		/// </summary>
		/// <param name="time"></param>
		public static DateTime GetDateTimeEndofDay(DateTime time)
		{
			return new DateTime(time.Year, time.Month, time.Day, 23, 59, 59);
		}

		/// <summary>
		/// 较早的时间
		/// </summary>
		/// <param name="date1"></param>
		/// <param name="date2"></param>
		/// <returns></returns>
		public static DateTime MinDateTime(DateTime date1, DateTime date2)
		{
			if (date1 < date2)
				return date1;
			else
				return date2;
		}

		/// <summary>
		/// 较晚的时间
		/// </summary>
		/// <param name="date1"></param>
		/// <param name="date2"></param>
		/// <returns></returns>
		public static DateTime MaxDateTime(DateTime date1, DateTime date2)
		{
			if (date1 > date2)
				return date1;
			else
				return date2;
		}
	}
}
