using System;
using System.Collections.Generic;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOperatingEntity
    {
        /// <summary>
        /// Do before Operate and OnEntityOperating
        /// </summary>
        /// <param name="e"></param>
        void PreparingOperate(OperateArgs e);

        /// <summary>
        /// Do After Operate and OnEntityOperated
        /// </summary>
        /// <param name="e"></param>
        void PreparedOperate(OperateArgs e);
    }
}
