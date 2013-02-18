using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Xceed.Grid.Editors;
using Xceed.Grid;
using Xceed.Editors;
using Feng;

namespace Feng.Grid.Editors
{
    ///// <summary>
    ///// 
    ///// </summary>
    //public class ButtonEditor : Xceed.Grid.Editors.CellEditorManager
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public ButtonEditor()
    //        : this("œÍœ∏")
    //    {
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="text"></param>
    //    public ButtonEditor(string text)
    //        : base(new System.Windows.Forms.Button(), "Enabled", true, false)
    //    {
    //        ((System.Windows.Forms.Button)this.TemplateControl).Text = text;
    //        ((System.Windows.Forms.Button)this.TemplateControl).BackColor = SystemColors.ControlDark;
    //        ((System.Windows.Forms.Button)this.TemplateControl).Click += new EventHandler(ButtonEditor_Click);

    //        System.Windows.Forms.Button button = this.TemplateControl as System.Windows.Forms.Button;
    //        button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
    //        button.BackColor = SystemColors.ControlDark;

    //        button.FlatAppearance.BorderSize = 0;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public event EventHandler ButtonClick;

    //    private void ButtonEditor_Click(object sender, EventArgs e)
    //    {
    //        if (ButtonClick != null)
    //        {
    //            ButtonClick(sender, e);
    //        }
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="control"></param>
    //    /// <param name="cell"></param>
    //    /// <param name="mode"></param>
    //    /// <param name="cellDisplayWidth"></param>
    //    /// <param name="graphics"></param>
    //    /// <param name="printing"></param>
    //    /// <returns></returns>
    //    protected override int GetFittedHeightCore(System.Windows.Forms.Control control, Cell cell, AutoHeightMode mode, int cellDisplayWidth, Graphics graphics, bool printing)
    //    {
    //        return (int)Math.Ceiling(cell.Font.GetHeight()) + 4;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="control"></param>
    //    /// <param name="cell"></param>
    //    /// <param name="mode"></param>
    //    /// <param name="graphics"></param>
    //    /// <param name="printing"></param>
    //    /// <returns></returns>
    //    protected override int GetFittedWidthCore(System.Windows.Forms.Control control, Cell cell, AutoWidthMode mode, Graphics graphics, bool printing)
    //    {
    //        return 20;
    //    }

    //    ///// <summary>
    //    ///// 
    //    ///// </summary>
    //    ///// <returns></returns>
    //    //protected override Control CreateControl()
    //    //{
    //    //    return Xceed.UI.ThemedControl.CloneControl(this.TemplateControl);
    //    //}

    //    ///// <summary>
    //    ///// 
    //    ///// </summary>
    //    //protected override CreateControlMode CreateControlMode
    //    //{
    //    //    get
    //    //    {
    //    //        return CreateControlMode.SingleInstance;
    //    //    }
    //    //}

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="control"></param>
    //    /// <param name="cell"></param>
    //    protected override void SetControlAppearanceCore(System.Windows.Forms.Control control, Cell cell)
    //    {
    //        if (control == null)
    //        {
    //            throw new ArgumentNullException("control");
    //        }
    //        StatelessVisualGridElementStyle mergedDisplayVisualStyle = cell.GetDisplayVisualStyle(VisualGridElementState.InactiveSelection);
    //        control.Font = mergedDisplayVisualStyle.Font;
    //        Color backColor = mergedDisplayVisualStyle.BackColor;
    //        try
    //        {
    //            control.BackColor = Color.Empty;
    //        }
    //        catch
    //        {
    //            control.BackColor = Color.FromArgb(0xff, backColor);
    //        }
    //        Color foreColor = mergedDisplayVisualStyle.ForeColor;
    //        try
    //        {
    //            control.ForeColor = foreColor;
    //        }
    //        catch
    //        {
    //            control.ForeColor = Color.FromArgb(0xff, foreColor);
    //        }
    //        ContentAlignment middleCenter = ContentAlignment.MiddleCenter;
    //        switch (mergedDisplayVisualStyle.HorizontalAlignment)
    //        {
    //            case HorizontalAlignment.Left:
    //                switch (mergedDisplayVisualStyle.VerticalAlignment)
    //                {
    //                    case VerticalAlignment.Top:
    //                        middleCenter = ContentAlignment.TopLeft;
    //                        break;

    //                    case VerticalAlignment.Center:
    //                    case VerticalAlignment.Default:
    //                        middleCenter = ContentAlignment.MiddleLeft;
    //                        break;

    //                    case VerticalAlignment.Bottom:
    //                        middleCenter = ContentAlignment.BottomLeft;
    //                        break;
    //                }
    //                break;

    //            case HorizontalAlignment.Center:
    //            case HorizontalAlignment.Default:
    //                switch (mergedDisplayVisualStyle.VerticalAlignment)
    //                {
    //                    case VerticalAlignment.Top:
    //                        middleCenter = ContentAlignment.TopCenter;
    //                        break;

    //                    case VerticalAlignment.Center:
    //                    case VerticalAlignment.Default:
    //                        middleCenter = ContentAlignment.MiddleCenter;
    //                        break;

    //                    case VerticalAlignment.Bottom:
    //                        middleCenter = ContentAlignment.BottomCenter;
    //                        break;
    //                }
    //                break;

    //            case HorizontalAlignment.Right:
    //                switch (mergedDisplayVisualStyle.VerticalAlignment)
    //                {
    //                    case VerticalAlignment.Top:
    //                        middleCenter = ContentAlignment.TopRight;
    //                        break;

    //                    case VerticalAlignment.Center:
    //                    case VerticalAlignment.Default:
    //                        middleCenter = ContentAlignment.MiddleRight;
    //                        break;

    //                    case VerticalAlignment.Bottom:
    //                        middleCenter = ContentAlignment.BottomRight;
    //                        break;
    //                }
    //                break;
    //        }
    //        (control as System.Windows.Forms.Button).TextAlign = middleCenter;

    //    }
    //}
    /// <summary>
    /// ∂‡–– ‰»ÎEditor
    /// </summary>
    public class ButtonEditor : MyTextEditor
    {
        private static WinTextBox GetMemoTextBox()
        {
            WinTextBox memoTextBox = new WinTextBox(EnhancedBorderStyle.None);
            WinButton moreButton = new WinButton("...");
            moreButton.AutoSizeMode = AutoSizeMode.ScrollBarWidth;
            memoTextBox.SideButtons.Add(moreButton);
            memoTextBox.TextBoxArea.ReadOnly = true;
            return memoTextBox;
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public ButtonEditor()
            : base(GetMemoTextBox())
        {
        }

        void ButtonEditor_Click(object sender, EventArgs e)
        {
            if (ButtonClick != null)
            {
                ButtonClick(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override System.Windows.Forms.Control CreateControl()
        {
            WinTextBox textBox = this.TemplateControl.Clone() as WinTextBox;
            textBox.SideButtons[0].Click += new EventHandler(ButtonEditor_Click);
            return textBox;
        }


        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ButtonClick;

        /// <summary>
        /// GetFittedHeightCore
        /// </summary>
        /// <param name="control"></param>
        /// <param name="cell"></param>
        /// <param name="mode"></param>
        /// <param name="cellDisplayWidth"></param>
        /// <param name="graphics"></param>
        /// <param name="printing"></param>
        /// <returns></returns>
        protected override int GetFittedHeightCore(System.Windows.Forms.Control control, Cell cell, AutoHeightMode mode,
                                                   int cellDisplayWidth, System.Drawing.Graphics graphics, bool printing)
        {
            return 20;
        }
    }
}