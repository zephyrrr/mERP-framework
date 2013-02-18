using System;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public static class DisplayManagerExtention
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="dcName"></param>
        /// <returns></returns>
        public static object GetDataValue(this IDisplayManager dm, string dcName)
        {
            return dm.DataControls[dcName].SelectedDataValue;
        }
    }
}
