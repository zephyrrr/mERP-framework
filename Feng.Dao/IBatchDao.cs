using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 支持批量整体操作的Bll。
    /// 提供开始操作(例如BeginTransaction)<see cref="SuspendOperation"/>、结束操作(例如CommitTransaction)<see cref="ResumeOperation"/>和
    /// 取消操作(例如RollbackTransaction)<see cref="CancelSuspendOperation"/>功能。
    /// </summary>
    public interface IBatchDao
    {
        /// <summary>
        /// 不立即提交操作，而是放在缓存里，等ResumeOperation时提交
        /// </summary>
        void SuspendOperation();

        /// <summary>
        /// 提交操作
        /// </summary>
        void ResumeOperation();

        /// <summary>
        /// 取消挂起的操作
        /// </summary>
        void CancelSuspendOperation();
    }
}
