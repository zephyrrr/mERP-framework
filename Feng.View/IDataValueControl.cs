namespace Feng
{
    using System.ComponentModel;

    /// <summary>
    /// 单值数据控件
    /// </summary>
    public interface IDataValueControl : IStateControl, IReadOnlyControl
    {
        /// <summary>
        /// 选定值
        /// </summary>
        object SelectedDataValue
        {
            get;
            set;
        }
    }
}
