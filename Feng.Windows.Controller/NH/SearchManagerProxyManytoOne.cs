using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.NH
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class SearchManagerProxyManytoOne<S, T> : SearchManagerProxyManytoOne<S, T, T>
        where T : class, IMasterEntity<T, S>
        where S : class, IDetailEntity<T, S>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dmParent"></param>
        public SearchManagerProxyManytoOne(IDisplayManager dmParent)
            : base(dmParent)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cmParent"></param>
        public SearchManagerProxyManytoOne(IControlManager cmParent)
            : base(cmParent)
        {
        }
    }

     /// <summary>
     /// 多对一的查找控制器，用于ManyToOne关系
     /// </summary>
     /// <typeparam name="S"></typeparam>
     /// <typeparam name="T"></typeparam>
     /// <typeparam name="TR"></typeparam>
    public class SearchManagerProxyManytoOne<S, T, TR> : SearchManagerWithParent<TR>
        where T : class, IMasterEntity<T, S>
        where S : class, IDetailEntity<T, S>
        where TR: class, T
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dmParent"></param>
        public SearchManagerProxyManytoOne(IDisplayManager dmParent)
            : base(dmParent, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cmParent"></param>
        public SearchManagerProxyManytoOne(IControlManager cmParent)
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
            var child = parentItem as S;
            IList<TR> list;
            if (child != null && child.MasterEntity != null)
            {
                if (base.IsReload || NHibernateHelper.IsProxy(child.MasterEntity))
                {
                    //NHibernateHelper.Initialize(child.MasterEntity, child, reload);
                    //List<T> list = new List<T>();
                    //list.Add(child.MasterEntity);
                    using (var rep = new Repository(this.RepositoryCfgName))
                    {
                        rep.BeginTransaction();
                        //rep.Session.Lock(child.MasterEntity, NHibernate.LockMode.None);
                        NHibernate.ICriteria criteria = rep.Session.CreateCriteria(typeof(TR))
                            .Add(NHibernate.Criterion.Expression.IdEq(EntityScript.GetPropertyValue(child.MasterEntity, TypedEntityMetadata.GenerateEntityInfo(rep.Session.SessionFactory, typeof(T)).IdName)));
                        OnCriteriaCreated(criteria);
                        list = criteria.List<TR>();

                        rep.CommitTransaction();
                    }
                    child.MasterEntity = list[0];
                }
                else
                {
                    list = new List<TR>();
                    list.Add(child.MasterEntity as TR);
                }
            }
            else
            {
                list = new List<TR>();
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
            SearchManagerProxyManytoOne<S, T, TR> sm = new SearchManagerProxyManytoOne<S, T, TR>(this.ParentDisplayManager);
            Copy(this, sm);
            return sm;
        }
    }
}
