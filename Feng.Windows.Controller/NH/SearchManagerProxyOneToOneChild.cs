using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.NH
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public class SearchManagerProxyOneToOneChild<S, T> : SearchManagerWithParent<T>
        where T : class, IOnetoOneParentEntity<T, S>
        where S : class, IOnetoOneChildEntity<T, S>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dmParent"></param>
        public SearchManagerProxyOneToOneChild(IDisplayManager dmParent)
            : base(dmParent, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cmParent"></param>
        public SearchManagerProxyOneToOneChild(IControlManager cmParent)
            : base(cmParent, null)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        /// <param name="parentItem"></param>
        /// <returns></returns>
        public override System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders, object parentItem)
        {
            IList<T> list;
            var child = parentItem as S;
            if (child != null && child.ParentEntity != null)
            {
                if (base.IsReload || NHibernateHelper.IsProxy(child.ParentEntity))
                {
                    //NHibernateHelper.Initialize(child.ParentEntity, child, reload);
                    //List<T> list = new List<T>();
                    //list.Add(child.ParentEntity);

                    using (var rep = new Repository(this.RepositoryCfgName))
                    {
                        rep.BeginTransaction();
                        //rep.Session.Lock(child, NHibernate.LockMode.None);
                        NHibernate.ICriteria criteria = rep.Session.CreateCriteria(typeof(T))
                            .Add(NHibernate.Criterion.Expression.IdEq(EntityScript.GetPropertyValue(child, TypedEntityMetadata.GenerateEntityInfo(rep.Session.SessionFactory, typeof(T)).IdName)));
                        OnCriteriaCreated(criteria);
                        list = criteria.List<T>();

                        rep.CommitTransaction();
                    }
                    if (list.Count > 0)
                    {
                        list[0].ChildEntity = child;
                        child.ParentEntity = list[0];
                    }
                }
                else
                {
                    list = new List<T>();
                    list.Add(child.ParentEntity);
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
        /// <param name="searchExpression"></param>
        /// <param name="searchOrders"></param>
        public override System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            var child = ParentDisplayManager.CurrentItem;
            return GetData(searchExpression, searchOrders, child);
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            SearchManagerProxyOneToOneChild<S, T> sm = new SearchManagerProxyOneToOneChild<S, T>(this.ParentDisplayManager);
            Copy(this, sm);
            return sm;
        }
    }
}

