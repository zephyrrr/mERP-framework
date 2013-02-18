using System;
using System.Collections.Generic;
using System.Text;
using Xceed.Grid.Editors;
using Xceed.Editors;

namespace Feng.Grid.Editors
{
    /// <summary>
    /// »’∆⁄ ‰»ÎEditor
    /// </summary>
    public class DateEditor : Xceed.Grid.Editors.DateEditor
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DateEditor()
            : base()
        {
            WinCalendar calendar = this.TemplateControl.DropDownControl;
            Feng.Windows.Forms.MyDatePickerXceed.ChangeCalendar(calendar);
        }
    }
}