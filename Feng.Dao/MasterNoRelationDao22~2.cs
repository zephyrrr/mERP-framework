using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public class MasterNoRelationDao22<T, S>: AbstractMemoriedRelationalDao<T, S>
        where T : class, IEntity
        where S : class, IDetailEntity<T, S>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="detailDao"></param>
        public MasterNoRelationDao22(IRepositoryDao<S> detailDao)
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
                    System.Diagnostics.Debug.Assert(this.DetailMemoryDao.UpdatedEntities.Count == 0);
                    break;
                case OperateType.Delete:
                    IMasterEntity<T, S> masterEntity = e.Entity as IMasterEntity<T, S>;
                    if (masterEntity != null)
                    {
                        if (masterEntity.DetailEntities != null)
                        {
                            e.Repository.Initialize(masterEntity.DetailEntities, e.Entity);
                            foreach (S i in masterEntity.DetailEntities)
                            {
                                i.MasterEntity = null;
                                this.DetailDao.Update(e.Repository, i);
                            }
                        }
                    }
                    break;
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
                    foreach (S i in this.DetailMemoryDao.SavedEntities)
                    {
                        i.MasterEntity = e.Entity;
                        this.DetailDao.Update(e.Repository, i);
                    }
                    System.Diagnostics.Debug.Assert(this.DetailMemoryDao.UpdatedEntities.Count == 0);
                    System.Diagnostics.Debug.Assert(this.DetailMemoryDao.DeletedEntities.Count == 0);
                    break;
                case OperateType.Update:
                    foreach (S i in this.DetailMemoryDao.SavedEntities)
                    {
                        i.MasterEntity = e.Entity;
                        this.DetailDao.Update(e.Repository, i);
                    }
                    System.Diagnostics.Debug.Assert(this.DetailMemoryDao.UpdatedEntities.Count == 0);
                    foreach (S i in this.DetailMemoryDao.DeletedEntities)
                    {
                        i.MasterEntity = null;
                        this.DetailDao.Update(e.Repository, i);
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
