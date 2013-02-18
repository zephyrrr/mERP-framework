using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// ProcessInfoDao
    /// </summary>
    public class ProcessInfoDao : DelegateDao
    {
        private DelegateDaoOperation Generate(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                if (s.ToLower() != "null")
                {
                    return new DelegateDaoOperation(delegate(object entity)
                    {
                        object o = ProcessInfoHelper.ExecuteProcess(ADInfoBll.Instance.GetProcessInfo(s),
                            new Dictionary<string, object> { { "entity", entity } });
                    });
                }
            }
            return null;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="processIds"></param>
        public ProcessInfoDao(string processIds)
        {
            string[] ss = Feng.Utils.StringHelper.Split(processIds, ',');
            DelegateDaoOperation save = null, update = null, delete = null;
            if (ss.Length >= 1)
            {
                save = Generate(ss[0]);
            }
            if (ss.Length >= 2)
            {
                update = Generate(ss[1]);
            }
            if (ss.Length >= 3)
            {
                delete = Generate(ss[2]);
            }

            base.Initialize(save, update, delete);
        }
    }
}
