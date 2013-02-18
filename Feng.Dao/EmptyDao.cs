using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 不做任何动作的Dao
    /// </summary>
    public class EmptyDao : AbstractEventDao, IBatchDao
    {
        /// <summary>
        /// 不立即提交操作，而是放在缓存里，等ResumeOperation时提交
        /// </summary>
        public void SuspendOperation() { }

        /// <summary>
        /// 提交操作
        /// </summary>
        public void ResumeOperation() { }

        /// <summary>
        /// 取消挂起的操作
        /// </summary>
        public void CancelSuspendOperation() { }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="entity"></param>
        public override void Save(object entity) { }

        /// <summary>
        /// 增加或更新
        /// </summary>
        /// <param name="entity"></param>
        public override void SaveOrUpdate(object entity) { }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        public override void Update(object entity) { }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public override void Delete(object entity) { }
    }
}
