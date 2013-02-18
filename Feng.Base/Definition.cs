using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 自定义数据
    /// </summary>
    public interface IDefinition
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string TryGetValue(string key);

        ///// <summary>
        ///// 主页
        ///// </summary>
        //string HomePage
        //{
        //    get;
        //}

        ///// <summary>
        ///// 选项对话框
        ///// </summary>
        //string OptionDialog
        //{
        //    get;
        //}

        ///// <summary>
        ///// 用户管理地址
        ///// </summary>
        //string UserManagerAddress
        //{
        //    get;
        //}
    }

    /// <summary>
    /// 预先定义的一些用户字段，用于<see cref="IDefinition"/>
    /// </summary>
    public static class DefinitionString
    {
        private const string s_strHomePage = "HomePage";
        //private const string s_strUserManagerAddress = "UserManagerAddress";
        private const string s_strOptionDialog = "OptionDialog";

        /// <summary>
        /// 主页
        /// </summary>
        public static string HomePage
        {
            get { return s_strHomePage; }
        }

        ///// <summary>
        ///// 用户管理地址
        ///// </summary>
        //public static string UserManagerAddress
        //{
        //    get { return s_strUserManagerAddress; }
        //}

        /// <summary>
        /// 选项框
        /// </summary>
        public static string OptionDialog
        {
            get { return s_strOptionDialog; }
        }
    }

    ///// <summary>
    ///// 
    ///// </summary>
    //public abstract class Definition
    //{
    //    private static Definition m_instance;
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public static Definition Instance
    //    {
    //        get { return m_instance; }
    //        internal set { m_instance = value; }
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <returns></returns>
    //    public abstract string TryGetValue(string key);

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public virtual string HomePage
    //    {
    //        get { return string.Empty; }
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public virtual string OptionDialog
    //    {
    //        get { return string.Empty; }
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public virtual string UserManagerAddress
    //    {
    //        get { return string.Empty; }
    //    }
    //}
}
