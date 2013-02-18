using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// OneToMany的关系，Many由One产生
    /// 类似于票-费用的关系，费用记录由票产生
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public class MasterGenerateDetailDao<T, S> : AbstractRelationalDao<T, S>
        where T : class, IMasterGenerateDetailEntity<T, S>
        where S : class, IDetailGenerateDetailEntity<T, S>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="detailDao"></param>
        public MasterGenerateDetailDao(IRepositoryDao<S> detailDao)
            : base(detailDao)
        {
        }


        /// <summary>
        /// 在Entity操作前执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnEntityOperating(OperateArgs<T> e)
        {
            base.OnEntityOperating(e);

            switch (e.OperateType)
            {
                case OperateType.Save:
                    break;
                case OperateType.Update:
                    break;
                case OperateType.Delete:
                    throw new ArgumentException("u should not call delete in MasterGenerateDetailDao!");
                default:
                    throw new InvalidOperationException("Invalid OperateType");
            }
        }

        /// <summary>
        /// 在Entity操作后执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnEntityOperated(OperateArgs<T> e)
        {
            base.OnEntityOperated(e);

            switch (e.OperateType)
            {
                case OperateType.Save:
                case OperateType.Update:
                    //if (e.Entity.Submitted)
                    {
                        IList<S> newDetails = e.Entity.GenerateDetails();
                        e.Repository.Initialize(e.Entity.DetailEntities, e.Entity);
                        IList<S> oldDetails = e.Entity.DetailEntities;

                        // Merge
                        Dictionary<int, bool> newDetailUsed = new Dictionary<int, bool>();
                        if (oldDetails != null)     // 可能是新生成的记录，oldDetails不是Proxy，而是null
                        {
                            for (int i = 0; i < oldDetails.Count; ++i)
                            {
                                bool find = false;
                                // find if exist in old
                                for (int j = 0; j < newDetails.Count; ++j)
                                {
                                    if (newDetailUsed.ContainsKey(j))
                                        continue;

                                    bool match = oldDetails[i].CopyIfMatch(newDetails[j]);
                                    if (match)
                                    {
                                        this.DetailDao.Update(e.Repository, oldDetails[i]);
                                        find = true;
                                        newDetailUsed[j] = true;
                                        break;
                                    }
                                }

                                // 不删除旧记录
                                //// 如果未在新纪录里找到，则删除旧记录
                                if (!find)
                                {
                                    //m_detailDao.Delete(e.Repository, oldDetails[i]);
                                }
                            }
                        }
                        // 找到新生成的记录，添加
                        for (int j = 0; j < newDetails.Count; ++j)
                        {
                            if (!newDetailUsed.ContainsKey(j))
                            {
                                this.DetailDao.Save(e.Repository, newDetails[j]);
                            }
                        }
                    }
                    break;
                case OperateType.Delete:
                    break;
                default:
                    throw new InvalidOperationException("Invalid OperateType");
            }
        }
    }
}
