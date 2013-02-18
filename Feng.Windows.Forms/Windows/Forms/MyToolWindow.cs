using System;
using System.Collections.Generic;
using System.Text;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public class MyToolWindow : Xceed.DockingWindows.ToolWindow
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key"></param>
        public MyToolWindow(string key)
            : base(key)
        {
            InitializeComponent();
            this.Text = key;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MyToolWindow
            // 
            this.Name = "MyToolWindow";
            this.Size = new System.Drawing.Size(180, 400);
            this.State = Xceed.DockingWindows.ToolWindowState.AutoHide;
            this.ResumeLayout(false);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override System.Drawing.Size DefaultSize
        {
            get { return new System.Drawing.Size(180, 400); }
        }

        //protected override void OnToolWindowVisibleChanged()
        //{
        //    if (!this.ToolWindowVisible)
        //    {
        //        CloseDropDown(this);
        //    }
        //    base.OnToolWindowVisibleChanged();
        //}

        //private void CloseDropDown(System.Windows.Forms.Control control)
        //{
        //    if (control == null)
        //        return;
        //    Xceed.Editors.WinTextBox winTextBox = control as Xceed.Editors.WinTextBox;
        //    if (winTextBox != null)
        //    {
        //        winTextBox.CloseDropDown();
        //    }
        //    else
        //    {
        //        foreach (System.Windows.Forms.Control subControl in control.Controls)
        //        {
        //            CloseDropDown(subControl);
        //        }
        //    }
        //}
    }
}