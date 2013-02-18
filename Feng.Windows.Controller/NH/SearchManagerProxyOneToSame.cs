using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.NH
{
    /// <summary>
    /// 到自己的查找控制器（同一个实体用于在不同地方显示）
    /// 可能是继承关系的两个类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SearchManagerProxyOneToSame<T> : SearchManagerWithParent<T>
        where T : IEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dmParent"></param>
        public SearchManagerProxyOneToSame(IDisplayManager dmParent)
            : base(dmParent, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cmParent"></param>
        public SearchManagerProxyOneToSame(IControlManager cmParent)
            : base(cmParent, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <param name="parentItem"></param>
        public override System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders, object parentItem)
        {
            IList<T> list;
            object masterItem = parentItem;
            if (masterItem != null)
            {
                if (masterItem.GetType() == typeof(T))
                {
                    list = new List<T>();
                    list.Add((T)masterItem);
                }
                else
                {
                    using (var rep = new Repository(this.RepositoryCfgName))
                    {
                        rep.BeginTransaction();
                        object id = EntityScript.GetPropertyValue(masterItem, TypedEntityMetadata.GenerateEntityInfo(rep.Session.SessionFactory, masterItem.GetType()).IdName);
                        NHibernate.ICriteria criteria = rep.Session.CreateCriteria(typeof(T))
                            .Add(NHibernate.Criterion.Expression.IdEq(id));
                        OnCriteriaCreated(criteria);
                        list = criteria.List<T>();

                        rep.CommitTransaction();
                    }
                }
            }
            else
            {
                list = new List<T>();
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        public override System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            var masterItem = ParentDisplayManager.CurrentItem;

            return GetData(searchExpression, searchOrders, masterItem);
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            SearchManagerProxyOneToSame<T> sm = new SearchManagerProxyOneToSame<T>(this.ParentDisplayManager);
            Copy(this, sm);
            return sm;
        }
    }
}

