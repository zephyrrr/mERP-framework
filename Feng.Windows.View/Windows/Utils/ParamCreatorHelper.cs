using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class ParamCreatorHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static object[] TryGetParams(string paramName)
        {
            if (string.IsNullOrEmpty(paramName))
                return null;

            return Cache.TryGetCache<object[]>(paramName, new Func<object[]>(delegate()
            {
                bool hasParamCreatorInfos;
                object ret = ParamCreatorHelper.CreateParam(paramName, out hasParamCreatorInfos);
                if (!hasParamCreatorInfos)
                {
                    string[] ss = paramName.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    return ss;
                }

                if (ret is object[])
                {
                    return ret as object[];
                }
                else
                {
                    return new object[] { ret };
                }
            }));
        }

        /// <summary>
        /// 根据名字创建参数。如不在表AD_Param_Creator中，则直接返回paramName（为了和以前兼容）
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static object TryGetParam(string paramName)
        {
            if (string.IsNullOrEmpty(paramName))
                return null;
            if (paramName.IndexOf(' ') >= 0)    // 如果包含空格，则肯定不是ParamName
                return paramName;

            return Cache.TryGetCache<object>(paramName, new Func<object>(delegate()
            {
                bool hasParamCreatorInfos;
                object ret = ParamCreatorHelper.CreateParam(paramName, out hasParamCreatorInfos);
                if (!hasParamCreatorInfos)
                {
                    return paramName;
                }

                return ret;
            }));
        }

        /// <summary>
        /// 根据参数名创建参数（如是单个参数则返回此参数，如是多个，则返回object[]）
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="hasParamCreatorInfos"></param>
        /// <returns></returns>
        public static object CreateParam(string paramName, out bool hasParamCreatorInfos)
        {
            IList<ParamCreatorInfo> list = ADInfoBll.Instance.GetParamCreatorInfos(paramName);
            if (list.Count == 0)
            {
                hasParamCreatorInfos = false;
                return null;
            }
            hasParamCreatorInfos = true;
            IList<ParamCreatorInfo> realList = new List<ParamCreatorInfo>();
            foreach (ParamCreatorInfo i in list)
            {
                if (string.IsNullOrEmpty(i.Permission)
                    || Authority.AuthorizeByRule(i.Permission))
                {
                    realList.Add(i);
                }
            }

            if (realList.Count == 0)
            {
                return null;
            }
            else if (realList.Count == 1)
            {
                return CreateParam(realList[0]);
            }
            else
            {
                object[] arr = new object[realList.Count];
                for (int i = 0; i < realList.Count; ++i)
                {
                    arr[i] = CreateParam(realList[i]);
                }
                return arr;
            }
        }

        private static object CreateParam(ParamCreatorInfo info)
        {
            return Feng.Utils.ConvertHelper.ChangeType(info.ParamValue, Feng.Utils.ReflectionHelper.GetTypeFromName(info.Type));
        }
    }
}
