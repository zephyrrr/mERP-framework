using System;
using System.Collections.Generic;
using System.Text;
using Feng.Data;

namespace Feng.Windows.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AlertHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static void ExecuteAlert()
        {
            IList<AlertRuleInfo> list = ADInfoBll.Instance.GetInfos<AlertRuleInfo>();
            foreach (AlertRuleInfo info in list)
            {
                System.Data.DataTable dt = DbHelper.Instance.ExecuteDataTable(info.Sql);

                using (IRepository rep = ServiceProvider.GetService<IRepositoryFactory>().GenerateRepository<AlertInfo>())
                {
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        AlertInfo alert = new AlertInfo();
                        alert.Action = rep.Get<ActionInfo>(row["ActionId"]);
                        alert.IsActive = true;
                        alert.RecipientRole = info.RecipientRole;
                        alert.SearchExpression = row["SearchExpression"].ToString();
                        alert.RecipientUser = info.RecipientUser;
                        alert.Description = row["Description"].ToString();

                        (new LogEntityDao<AlertInfo>()).Save(alert);
                    }
                }
            }
        }
    }
}
