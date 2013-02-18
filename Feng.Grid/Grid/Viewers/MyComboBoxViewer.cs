using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Grid;
using Feng.Utils;

namespace Feng.Grid.Viewers
{
    /// <summary>
    /// 
    /// </summary>
    public class MyComboBoxViewer : Xceed.Grid.Viewers.ComboBoxViewer, INameValueControl
    {
        private string m_nvName;

         /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nvName"></param>
        public MyComboBoxViewer(string nvName)
            : base()
        {
            this.m_nvName = nvName;
        }

        ///// <summary>
        ///// Consturctor
        ///// </summary>
        ///// <param name="dataSource"></param>
        ///// <param name="dataMember"></param>
        ///// <param name="valueMember"></param>
        ///// <param name="displayFormat"></param>
        //public MyComboBoxViewer(object dataSource, string dataMember, string valueMember, string displayFormat)
        //    : base(dataSource, dataMember, valueMember, displayFormat)
        //{
        //}

        /// <summary>
        /// GetTextCore
        /// </summary>
        /// <param name="value"></param>
        /// <param name="formatInfo"></param>
        /// <param name="gridElement"></param>
        /// <returns></returns>
        protected override string GetTextCore(object value, CellTextFormatInfo formatInfo, GridElement gridElement)
        {
            return GetDisplay(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string GetDisplay(object value)
        {
            return NameValueControlHelper.GetMultiString(m_nvName, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayText"></param>
        /// <returns></returns>
        public object GetValue(string displayText)
        {
            return NameValueControlHelper.GetMultiValue(m_nvName, displayText);
        }
    }
}
