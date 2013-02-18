using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Feng.Collections
{
    /// <summary>
    /// 检查控件集合
    /// </summary>
    public class CheckControlCollection : ControlCollection<ICheckControl, IControlManager>, ICheckControlCollection
    {
		/// <summary>
		/// 检查控件组的所有控件
		/// </summary>
		public void CheckControlsValue()
		{
            foreach (ICheckControl cc in this)
            {
                if (!cc.CheckControlValue())
                {
                    throw new ControlCheckException("输入有误，请检查！", cc);
                }
            }
		}

		//private static int compareTabIndex(ICheckControl a, ICheckControl b)
		//{
		//    LabeledDataControl dca = a as LabeledDataControl;
		//    LabeledDataControl dcb = b as LabeledDataControl;
		//    if (dca == null || dcb == null)
		//        return 0;

		//    return dca.TabIndex.CompareTo(dcb.TabIndex);
		//}

		///// <summary>
		///// 按照TabIndex排序
		///// </summary>
		//public new void Sort()
		//{
		//    base.Sort(compareTabIndex);
		//}
    }
}
