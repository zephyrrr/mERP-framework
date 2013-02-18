using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using NHibernate.Mapping.Attributes;

namespace Feng.GPS
{
	/// <summary>
	/// 最简单路线（起始结束，对应地图里道路）
	/// </summary>
	[Class(Table = "监控_路径表", Lazy = false)]
	public class Path : IVersionedEntity
	{
		#region "Constructor"
		/// <summary>
		/// default constructor
		/// </summary>
		public Path()
		{
		}

		/// <summary>
		/// named simplePath with start and end site
		/// </summary>
		/// <param name="pathName"></param>
		/// <param name="regexPattern"></param>
		public Path(string pathName, string regexPattern)
		{
			this.PathName = pathName;
			this.RegexPattern = regexPattern;
		}
		#endregion

		#region  Properties
		///<summary>
		/// ID
		///</summary>
		[Id(0, Name = "ID", Column = "ID")]
		[Generator(1, Class = "guid")]
		public virtual Guid ID
		{
            get;
            set;
		}

		///<summary>
		/// 路径名称
		///</summary>
		[Property(Length = 20, NotNull = true)]
		public virtual string PathName
		{
            get;
            set;
		}

		///<summary>
		/// 开始地点
		///</summary>
		[Property(Length = 20, NotNull = true)]
		public virtual string StartSite
		{
            get;
            set;
		}

		///<summary>
		/// 结束地点
		///</summary>
		[Property(Length = 20, NotNull = true)]
		public virtual string EndSite
		{
            get;
            set;
		}

		///<summary>
		/// 此路径对应的正则表达式
		///</summary>
		[Property(Length = 500, NotNull = false)]
		public virtual string RegexPattern
		{
            get;
            set;
		}

		///<summary>
		/// 此路径需要的时间
		///</summary>
		[Property(NotNull = true)]
		public virtual int PathTime
		{
            get;
            set;
		}

		///<summary>
		/// 此路径类型
		///</summary>
        [Property(Length = 1, NotNull = false)]
        public virtual string PathType
        {
            get;
            set;
        }

        /// <summary>
        /// 版本号
        /// </summary>
        [Version(Column = "Version", Type = "Int32", UnsavedValue = "0")]
        public virtual int Version
        {
            get;
            set;
        }
		#endregion

		#region "Methods"
		public override string ToString()
		{
			return this.PathName;
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			Path that = (Path)obj;
			if (that == null)
			{
				return false;
			}
            if (this.PathName == null || that.PathName == null || !this.PathName.Equals(that.PathName))
			{
				return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
            return this.PathName.GetHashCode();
		}

		#endregion
	}
}
