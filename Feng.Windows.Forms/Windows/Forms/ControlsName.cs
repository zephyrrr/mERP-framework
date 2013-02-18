using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 数据及查找控件名称
    /// </summary>
	public static class ControlsName
	{
        /// <summary>
        /// 数据控件名称列表
        /// </summary>
        public static string[] DataControlsTypeName = {
            "Feng.Windows.Forms.MyLabel, Feng.Windows.Forms", 
			"Feng.Windows.Forms.MyTextBox, Feng.Windows.Forms", 
			"Feng.Windows.Forms.MyMultilineTextBox, Feng.Windows.Forms", 
            "Feng.Windows.Forms.MyMultilineTextBox2, Feng.Windows.Forms", 
			"Feng.Windows.Forms.MyRichTextBox, Feng.Windows.Forms", 
			"Feng.Windows.Forms.MyIntegerTextBox, Feng.Windows.Forms", 
            "Feng.Windows.Forms.MyLongTextBox, Feng.Windows.Forms", 
			"Feng.Windows.Forms.MyNumericTextBox, Feng.Windows.Forms", 
			"Feng.Windows.Forms.MyCurrencyTextBox, Feng.Windows.Forms", 
            "Feng.Windows.Forms.MyComboBox, Feng.Grid", 
			"Feng.Windows.Forms.MyFreeComboBox, Feng.Grid", 
			"Feng.Windows.Forms.MyListBox, Feng.Windows.Forms", 
			"Feng.Windows.Forms.MyRadioListBox, Feng.Windows.Forms", 
			"Feng.Windows.Forms.MyCheckedListBox, Feng.Windows.Forms", 
			"Feng.Windows.Forms.MyOptionPicker, Feng.Grid",
			"Feng.Windows.Forms.MyRadioButton, Feng.Windows.Forms", 
			"Feng.Windows.Forms.MyCheckBox, Feng.Windows.Forms", 
            "Feng.Windows.Forms.MyThreeStateCheckbox, Feng.Windows.Forms",
			"Feng.Windows.Forms.MyPictureBox, Feng.Windows.Forms", 
			"Feng.Windows.Forms.MyDateTimeTextBox, Feng.Windows.Forms", 
			"Feng.Windows.Forms.MyDatePicker, Feng.Windows.Forms", 
			"Feng.Windows.Forms.MyMonthPicker, Feng.Windows.Forms", 
			"Feng.Windows.Forms.MyFileBox, Feng.Windows.Forms", 
            "Feng.Windows.Forms.MyButtonTextBox, Feng.Windows.Forms", 
			"Feng.Windows.Grid.MyGrid, Feng.Grid"};

        /// <summary>
        /// 查找控件名称列表
        /// </summary>
        public static string[] SearchControlsTypeName = {
			"Feng.Windows.Forms.MyIntegerTextBox, Feng.Windows.Forms", 
            "Feng.Windows.Forms.MyLongTextBox, Feng.Windows.Forms", 
			"Feng.Windows.Forms.MyNumericTextBox, Feng.Windows.Forms", 
			"Feng.Windows.Forms.MyCurrencyTextBox, Feng.Windows.Forms",
			"Feng.Windows.Forms.MyDateTimeTextBox, Feng.Windows.Forms", 
			"Feng.Windows.Forms.MyDatePicker, Feng.Windows.Forms", 
			"Feng.Windows.Forms.MyMonthPicker, Feng.Windows.Forms"};

        /// <summary>
        /// 从控件长名得到短名
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static string GetNameFromFullName(string fullName)
		{
            string[] names = fullName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			int idx = names[0].LastIndexOf('.');
			if (idx == -1)
                return names[0];
			else
                return names[0].Substring(idx + 1);
		}
	}
}
