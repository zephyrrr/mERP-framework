using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    ///  一个和多个的Dao，但不是onetomany，两者无直接关系
    ///  类似于票滞箱费减免-箱滞箱费减免的关系，两者无直接关系，两者都有各自记录，各自进行操作。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public class MasterNoRelationDao<T, S> : AbstractMemoriedRelationalDao<T, S>
        where T : class, IEntity
        where S : class, IEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="detailDao"></param>
        public MasterNoRelationDao(IRepositoryDao<S> detailDao)
            : base(detailDao)
        {
        }


        /// <summary>
        /// 在Entity操作后执行
        /// </summary>
        /// <param name="e"></param>
        public override void OnEntityOperated(OperateArgs<T> e)
        {
            base.OnEntityOperated(e);

            System.Diagnostics.Debug.Assert(this.DetailMemoryDao.SavedEntities.Count == 0);
            System.Diagnostics.Debug.Assert(this.DetailMemoryDao.DeletedEntities.Count == 0);

            switch (e.OperateType)
            {
                case OperateType.Save:
                    break;
                case OperateType.Update:
                    foreach (S i in this.DetailMemoryDao.UpdatedEntities)
                    {
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
