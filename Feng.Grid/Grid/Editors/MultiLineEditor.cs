using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Xceed.Grid.Editors;
using Xceed.Grid;
using Xceed.Editors;
using Feng;

namespace Feng.Grid.Editors
{
    /// <summary>
    /// 多行输入Editor
    /// </summary>
    public class MultiLineEditor : MyTextEditor
    {
        private static WinTextBox GetMemoTextBox(string format)
        {
            //WinTextBox memoTextBox = new WinTextBox(EnhancedBorderStyle.None);
            //WinButton moreButton = new WinButton(new Xceed.Editors.ButtonType(Xceed.Editors.ButtonBackgroundImageType.Combo, Xceed.Editors.ButtonImageType.ScrollDown));
            //memoTextBox.SideButtons.Add(moreButton);
            //memoTextBox.DropDownButton = moreButton;
            //memoTextBox.DropDownAllowFocus = true;
            //memoTextBox.TextBoxArea.WordWrap = false;
            //memoTextBox.TextBoxArea.Multiline = false;
            //memoTextBox.TextBoxArea.AcceptsReturn = false;

            //WinTextBox multilineTextBox = new WinTextBox();
            //multilineTextBox.TextBoxArea.WordWrap = true;
            //multilineTextBox.TextBoxArea.Multiline = true;
            //multilineTextBox.TextBoxArea.ScrollBars = ScrollBars.Vertical;
            //multilineTextBox.TextBoxArea.AcceptsTab = true;
            //multilineTextBox.TextBoxArea.AcceptsReturn = false;

            //memoTextBox.DropDownControl = multilineTextBox;
            //memoTextBox.DropDownSize = new System.Drawing.Size(300, 150);
            WinTextBox memoTextBox = new Feng.Windows.Forms.MyMultilineTextBox();
            return memoTextBox;
        }

        /// <summary>
        /// 默认CharacterCasing.Normal
        /// </summary>
        public MultiLineEditor()
            : this("Normal")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MultiLineEditor(string format)
            : base(GetMemoTextBox(format))
        {
            //switch (format)
            //{
            //    case "Upper":
            //        this.TemplateControl.TextBoxArea.CharacterCasing = CharacterCasing.Upper;
            //        break;
            //    case "Lower":
            //        this.TemplateControl.TextBoxArea.CharacterCasing = CharacterCasing.Lower;
            //        break;
            //    case "Normal":
            //        this.TemplateControl.TextBoxArea.CharacterCasing = CharacterCasing.Normal;
            //        break;
            //}
        }

        /// <summary>
        /// ActivateControlCore
        /// </summary>
        /// <param name="control"></param>
        /// <param name="cell"></param>
        protected override void ActivateControlCore(System.Windows.Forms.Control control, Cell cell)
        {
            WinTextBox memoTextBox = (WinTextBox) control;
            //memoTextBox.DroppedDownChanged += new EventHandler(MemoTextBox_DroppedDownChanged);

            base.ActivateControlCore(control, cell);
        }

        /// <summary>
        /// DeactivateControlCore
        /// </summary>
        /// <param name="control"></param>
        /// <param name="cell"></param>
        protected override void DeactivateControlCore(System.Windows.Forms.Control control, Cell cell)
        {
            WinTextBox memoTextBox = (WinTextBox) control;
            //memoTextBox.DroppedDownChanged -= new EventHandler(MemoTextBox_DroppedDownChanged);

            base.DeactivateControlCore(control, cell);
        }

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

        //private void MemoTextBox_DroppedDownChanged(object sender, EventArgs e)
        //{
        //    WinTextBox memoTextBox = (WinTextBox) sender;

        //    if (memoTextBox.DroppedDown)
        //    {
        //        if (!memoTextBox.DropDownControl.Focused)
        //        {
        //            memoTextBox.DropDownControl.Focus();
        //        }

        //        memoTextBox.DropDownControl.Text = memoTextBox.TextBoxArea.Text;
        //    }
        //    else
        //    {
        //        memoTextBox.TextBoxArea.Text = memoTextBox.DropDownControl.Text;
        //    }
        //}
    }
}