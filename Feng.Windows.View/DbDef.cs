using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 数据库定义的一些内容，保存在<see cref="SimpleParamInfo"/>
    /// </summary>
    public class DBDef : Singleton<DBDef>, IDefinition, IDataBuffer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DBDef()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadData()
        {
            dict = new Dictionary<string, string>();

            IList<SimpleParamInfo> list = ADInfoBll.Instance.GetInfos<SimpleParamInfo>();
            foreach (SimpleParamInfo info in list)
            {
                dict[info.Name] = info.Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reload()
        {
            Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            if (dict != null)
            {
                dict.Clear();
                dict = null;
            }
        }

        private Dictionary<string, string> dict;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string TryGetValue(string key)
        {
            if (dict == null)
            {
                LoadData();
            }
            if (dict.ContainsKey(key))
                return dict[key];
            else
                return null;
        }

        ///// <summary>
        ///// 主页。数据库中名称为HomePage
        ///// </summary>
        //public override string HomePage
        //{
        //    get
        //    {
        //        return TryGetValue("HomePage");
        //    }
        //}

        ///// <summary>
        ///// 程序选项对话框。数据库中名称为OptionDialog
        ///// </summary>
        //public override string OptionDialog
        //{
        //    get
        //    {
        //        return TryGetValue("OptionDialog");
        //    }
        //}

        ///// <summary>
        ///// 用户管理网址。数据库中名称为UserManagerAddress
        ///// </summary>
        //public override string UserManagerAddress
        //{
        //    get
        //    {
        //        return TryGetValue("UserManagerAddress");
        //    }
        //}
    }
}
