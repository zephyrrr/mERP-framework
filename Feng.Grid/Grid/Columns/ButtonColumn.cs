using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Xceed.Grid;

namespace Feng.Grid.Columns
{
    /// <summary>
    /// 
    /// </summary>
    public class ButtonColumn : Column
    {
        /// <summary>
        /// 
        /// </summary>
        public ButtonColumn()
            : base("ฯ๊ว้", typeof(string))
        {
            //this.CellViewerManager = new Viewer.ButtonViewer();
            this.CellEditorManager = new Editors.ButtonEditor();

            ((Editors.ButtonEditor) this.CellEditorManager).ButtonClick += new EventHandler(ButtonColumn_ButtonClick);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="caption"></param>
        public ButtonColumn(string caption)
            : base(caption, typeof(string))
        {
            //this.CellViewerManager = new Viewer.ButtonViewer(viewText);
            this.CellEditorManager = new Editors.ButtonEditor();

            ((Editors.ButtonEditor) this.CellEditorManager).ButtonClick += new EventHandler(ButtonColumn_ButtonClick);
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ButtonClick;

        private void ButtonColumn_ButtonClick(object sender, EventArgs e)
        {
            if (ButtonClick != null)
            {
                ButtonClick(sender, e);
            }
        }
    }
}