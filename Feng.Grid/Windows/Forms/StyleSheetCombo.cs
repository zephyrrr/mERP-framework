using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using Xceed.Grid;
using Feng.Grid;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// StyleSheetCombo
    /// </summary>
    public class StyleSheetCombo : MyComboBox
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StyleSheetCombo()
        {
        }

        /// <summary>
        /// LoadItems
        /// </summary>
        public void LoadItems()
        {
            this.Columns.Clear();
            this.Columns.Add(new Xceed.Editors.ColumnInfo("Ãû³Æ", typeof (string)));

            foreach (string s in GridStyleSheetExtention.GetGridStyles())
            {
                this.Items.Add(new object[] { s });
            }
            this.Items.Add(new object[] { "Custom" });
        }

        
    }
}