using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Reflection;
using Xceed.Grid.Viewers;
using Xceed.Grid;
using Xceed.Editors;
using Feng;

namespace Feng.Grid.Viewers
{
    /// <summary>
    /// 多行显示Viewer。当多行时
    /// </summary>
    public class MultiLineViewer : TextViewer
    {
        private static WinTextBox m_memoTextBox;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridControl"></param>
        /// <returns></returns>
        public static WinTextBox GetMemoTextBox(GridControl gridControl)
        {
            if (m_memoTextBox == null || m_memoTextBox.IsDisposed)
            {
                WinTextBox memoTextBox = new WinTextBox(EnhancedBorderStyle.None);
                //WinButton moreButton = new WinButton(new Xceed.Editors.ButtonType(Xceed.Editors.ButtonBackgroundImageType.Combo, Xceed.Editors.ButtonImageType.ScrollDown));
                //memoTextBox.SideButtons.Add(moreButton);
                //memoTextBox.DropDownButton = moreButton;

                memoTextBox.TextBoxArea.WordWrap = false;
                memoTextBox.TextBoxArea.Multiline = false;
                memoTextBox.TextBoxArea.AcceptsReturn = false;
                memoTextBox.TextBoxArea.ReadOnly = false;

                WinTextBox multilineTextBox = new WinTextBox();
                multilineTextBox.TextBoxArea.WordWrap = true;
                multilineTextBox.TextBoxArea.Multiline = true;
                multilineTextBox.TextBoxArea.ReadOnly = true;

                memoTextBox.DropDownAnchor = DropDownAnchor.Left;
                memoTextBox.DropDownDirection = DropDownDirection.Automatic;
                memoTextBox.DropDownAllowFocus = true;
                memoTextBox.DropDownControl = multilineTextBox;
                memoTextBox.DropDownSize = new System.Drawing.Size(300, 150);
                memoTextBox.DroppedDownChanged += new EventHandler(memoTextBox_DroppedDownChanged);

                m_memoTextBox = memoTextBox;
            }
            SetMemoTextBoxParent(m_memoTextBox, gridControl);
            return m_memoTextBox;
        }

        static void memoTextBox_DroppedDownChanged(object sender, EventArgs e)
        {
            WinTextBox memoTextBox = sender as WinTextBox;
            if (!memoTextBox.DroppedDown)
            {
                memoTextBox.Parent = null;
            }
        }

        private static void SetMemoTextBoxParent(WinTextBox memoTextBox, GridControl gridControl)
        {
            if (gridControl != null)
            {
                System.Windows.Forms.ContainerControl viewerContainer = typeof(Xceed.Grid.GridControl).InvokeMember("ViewerContainer",
                    BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, gridControl, null) as System.Windows.Forms.ContainerControl; 

                if (viewerContainer == null)
                {
                    viewerContainer = new System.Windows.Forms.ContainerControl
                    {
                        Size = new Size(0, 0),
                        TabStop = false
                    };
                    typeof(Xceed.Grid.GridControl).InvokeMember("ViewerContainer", System.Reflection.BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                        null, gridControl, new object[] { viewerContainer }, null, null, null);
                }
                memoTextBox.Parent = viewerContainer;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MultiLineViewer()
            : base()
        {
        }


        /// <summary>
        /// GetFittedHeightCore
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="mode"></param>
        /// <param name="cellDisplayWidth"></param>
        /// <param name="graphics"></param>
        /// <param name="printing"></param>
        /// <returns></returns>
        protected override int GetFittedHeightCore(Cell cell, AutoHeightMode mode, int cellDisplayWidth,
                                                   Graphics graphics, bool printing)
        {
            return 20;
        }

        /// <summary>
        /// GetFittedWidthCore
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="mode"></param>
        /// <param name="graphics"></param>
        /// <param name="printing"></param>
        /// <returns></returns>
        protected override int GetFittedWidthCore(Cell cell, AutoWidthMode mode, System.Drawing.Graphics graphics,
                                                  bool printing)
        {
            return Math.Min(1000, base.GetFittedWidthCore(cell, mode, graphics, printing));
        }

        /// <summary>
        /// GetTextCore
        /// </summary>
        /// <param name="value"></param>
        /// <param name="formatInfo"></param>
        /// <param name="gridElement"></param>
        /// <returns></returns>
        protected override string GetTextCore(object value, CellTextFormatInfo formatInfo, GridElement gridElement)
        {
            string rawText = base.GetTextCore(value, formatInfo, gridElement);

            //bool more = false;
            //if (rawText.Length > 20)
            //{
            //    rawText = rawText.Substring(0, 20);
            //    more = true;
            //}
            //int idx = rawText.IndexOf(System.Environment.NewLine);
            //if (idx != -1)
            //{
            //    rawText = rawText.Substring(0, idx);
            //    more = true;
            //}

            //if (more)
            //{
            //    rawText += "   ...";
            //}

            rawText = rawText.Replace(System.Environment.NewLine, " ");
            return rawText;
        }
    }
}