using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Xceed.SmartUI;
using Xceed.SmartUI.Controls;
using Xceed.DockingWindows;
using Xceed.DockingWindows.TabbedMdi;
using Xceed.SmartUI.Controls.OutlookShortcutBar;
using Feng.Windows.Utils;
using Feng.Grid;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// MainForm
    /// </summary>
    public partial class TabbedMdiForm2 : TabbedMdiForm
    {
        public TabbedMdiForm2()
            : base()
        {
            Type type = typeof(TabbedMdiForm);
            Type type2 = typeof(System.Windows.Forms.ToolStripMenuItem);
            Feng.Utils.ReflectionHelper.SetObjectValue(type2, Feng.Utils.ReflectionHelper.GetObjectValue(type, this, "tsmSearchWindow"), "Visible", false);
            Feng.Utils.ReflectionHelper.SetObjectValue(type2, Feng.Utils.ReflectionHelper.GetObjectValue(type, this, "tsmReleatedWindow"), "Visible", false);
            Feng.Utils.ReflectionHelper.SetObjectValue(type2, Feng.Utils.ReflectionHelper.GetObjectValue(type, this, "tsmTaskWindow"), "Visible", false);
        }

        protected override MyToolWindow CreateToolWindow(string twName)
        {
            if (twName == "导航")
            {
                return new MyToolWindow(twName);
            }
            return null;
        }
    }
}