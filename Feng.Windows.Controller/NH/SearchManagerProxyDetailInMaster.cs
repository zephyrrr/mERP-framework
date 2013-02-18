using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;

namespace Feng.NH
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
     public class SearchManagerProxyDetailInMaster<T, S> :  SearchManagerProxyDetailInMaster<T, S, S>
            where T : class, IMasterEntity<T, S>
            where S : class, IDetailEntity<T, S>
     {
         /// <summary>
         /// Constructor
         /// </summary>
         /// <param name="dmParent"></param>
         public SearchManagerProxyDetailInMaster(IDisplayManager dmParent)
             : base(dmParent)
         {
         }

         /// <summary>
         /// Constructor
         /// </summary>
         /// <param name="cmParent"></param>
         public SearchManagerProxyDetailInMaster(IControlManager cmParent)
             : base(cmParent)
         {
         }
     }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="SR">继承自S，用来进一步限定DetailEntity的类型</typeparam>
    public class SearchManagerProxyDetailInMaster<T, S, SR> : SearchManagerWithParent<SR>
        where T : class, IMasterEntity<T, S>
        where S : class, IDetailEntity<T, S>
        where SR : class, S
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dmParent"></param>
        public SearchManagerProxyDetailInMaster(IDisplayManager dmParent)
            : base(dmParent, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cmParent"></param>
        public SearchManagerProxyDetailInMaster(IControlManager cmParent)
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
            var master = parentItem as T;

            IList<SR> list;
            if (master != null && master.DetailEntities != null)
            {
                // 需要和界面上一致，例如凭证费用明细.费用是程序里设置的，还未保存，此时如果查找一遍，会导致信息丢失
                if (base.IsReload || NHibernateHelper.IsProxy(master.DetailEntities))
                {
                    //NHibernateHelper.Initialize(master.DetailEntities, master, reload);
                    //List<S> list = new List<S>(master.DetailEntities);
                    //string idDetailName = EntityInfo.GenerateEntityInfo(typeof(SR)).IdName;

                    using (var rep = new Repository(this.RepositoryCfgName))
                    {
                        rep.BeginTransaction();
                        string propertyName = NHibernateHelper.GetOnetoManyPropertyName(rep.Session.SessionFactory, typeof(T), typeof(S));
                        string idMasterName = TypedEntityMetadata.GenerateEntityInfo(rep.Session.SessionFactory, typeof(T)).IdName;

                        NHibernate.ICriteria criteria = rep.Session.CreateCriteria(typeof(SR))
                            .Add(NHibernate.Criterion.Expression.Eq(propertyName + "." + idMasterName,
                            EntityScript.GetPropertyValue(master, idMasterName)));

                        OnCriteriaCreated(criteria);
                        list = criteria.List<SR>();

                        rep.CommitTransaction();
                    }
                    var list2 = new List<S>();
                    foreach (SR i in list)
                    {
                        i.MasterEntity = master;
                        list2.Add(i);
                    }
                    // can't do it here because DetailEntities maybe is proxy
                    //if (master.DetailEntities != null)
                    //{
                    //    master.DetailEntities.Clear();
                    //}
                    master.DetailEntities = list2;

                    this.Result = list;
                    this.Count = list.Count;
                }
                //// DetailEntities 需要是SR类型，但Proxy出来的是S类型，所以需要直接查找
                else
                {
                    list = new List<SR>();
                    foreach (S i in master.DetailEntities)
                    {
                        list.Add(i as SR);
                    }
                }
            }
            else
            {
                list = new List<SR>();
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
            var master = ParentDisplayManager.CurrentItem as T;

            return GetData(searchExpression, searchOrders, master);
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            SearchManagerProxyDetailInMaster<T, S, SR> sm = new SearchManagerProxyDetailInMaster<T, S, SR>(this.ParentDisplayManager);
            Copy(this, sm);
            return sm;
        }
    }
}
