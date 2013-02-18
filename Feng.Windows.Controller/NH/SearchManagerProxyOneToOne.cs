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
    public class SearchManagerProxyOneToOne<T, S> : SearchManagerWithParent<S>
        where T : class, IOnetoOneParentEntity<T, S>
        where S : class, IOnetoOneChildEntity<T, S>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dmParent"></param>
        public SearchManagerProxyOneToOne(IDisplayManager dmParent)
            : base(dmParent, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cmParent"></param>
        public SearchManagerProxyOneToOne(IControlManager cmParent)
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
            IList<S> list;
            var parent = parentItem as T;
            if (parent != null && parent.ChildEntity != null)
            {
                if (base.IsReload || NHibernateHelper.IsProxy(parent.ChildEntity))
                {
                    //NHibernateHelper.Initialize(master.ChildEntity, master, reload);
                    //List<S> list = new List<S>();
                    //list.Add(master.ChildEntity);
                    using (var rep = new Repository(this.RepositoryCfgName))
                    {
                        rep.BeginTransaction();
                        object id = EntityScript.GetPropertyValue(parent, TypedEntityMetadata.GenerateEntityInfo(rep.Session.SessionFactory, typeof(T)).IdName);
                        NHibernate.ICriteria criteria = rep.Session.CreateCriteria(typeof(S))
                            .Add(NHibernate.Criterion.Expression.IdEq(id));
                        OnCriteriaCreated(criteria);
                        list = criteria.List<S>();

                        rep.CommitTransaction();
                    }
                    if (list.Count > 0)
                    {
                        parent.ChildEntity = list[0];
                        list[0].ParentEntity = parent;
                    }
                }
                else
                {
                    list = new List<S>();
                    list.Add(parent.ChildEntity);
                }
            }
            else
            {
                list = new List<S>();
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        public override System.Collections.IEnumerable GetData(ISearchExpression searchExpression, IList<ISearchOrder> searchOrders)
        {
            var parent = ParentDisplayManager.CurrentItem;

            return GetData(searchExpression, searchOrders, parent);
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            SearchManagerProxyOneToOne<T, S> sm = new SearchManagerProxyOneToOne<T, S>(this.ParentDisplayManager);
            Copy(this, sm);
            return sm;
        }
    }
}
