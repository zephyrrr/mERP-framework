using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing;
using System.ComponentModel;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// RadioListBox
    /// </summary>
    public class MyRadioListBox : MyListBox
    {
        #region "Default Property"

        private StringFormat Align;
        private bool IsTransparent;
        private Brush TransparentBrush = SystemBrushes.Control;

        /// <summary>
        /// Constructor
        /// </summary>
        public MyRadioListBox()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;

            this.Align = new StringFormat(StringFormat.GenericDefault);
            this.Align.LineAlignment = StringAlignment.Center;

            base.Size = new System.Drawing.Size(120, 42);
            base.ItemHeight = 20;
        }

        /// <summary>
        /// DrawMode
        /// </summary>
        [DefaultValue(DrawMode.OwnerDrawFixed)]
        public new DrawMode DrawMode
        {
            get { return base.DrawMode; }
            set { base.DrawMode = value; }
        }

        /// <summary>
        /// Transparent
        /// </summary>
        [DefaultValue(false)]
        public bool Transparent
        {
            set
            {
                IsTransparent = value;

                if (IsTransparent)
                    // Mimic parent form color, and hide border
                {
                    if (this.Parent != null)
                    {
                        //Prevent an exception if control still has no parent
                        this.BackColor = this.Parent.BackColor;
                    }
                    else
                    {
                        this.BackColor = SystemColors.Control;
                    }
                    this.TransparentBrush = new SolidBrush(this.BackColor);
                    this.BorderStyle = BorderStyle.None;
                }
                else
                {
                    this.BackColor = SystemColors.Window;
                    this.BorderStyle = BorderStyle.Fixed3D;
                }
            }
            get { return IsTransparent; }
        }

        /// <summary>
        /// OnDrawItem
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }
            if (e.Index > this.Items.Count - 1)
            {
                return;
            }

            int size = e.Font.Height + 5; // button size depends on font height, not on item height

            if (IsTransparent && e.State != DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(TransparentBrush, e.Bounds);
            }
            else
            {
                e.DrawBackground();
            }

            Brush textBrush;
            bool isChecked = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            RadioButtonState state = isChecked ? RadioButtonState.CheckedNormal : RadioButtonState.UncheckedNormal;
            if ((e.State & DrawItemState.Disabled) == DrawItemState.Disabled)
            {
                textBrush = SystemBrushes.GrayText;
                state = isChecked ? RadioButtonState.CheckedDisabled : RadioButtonState.UncheckedDisabled;
            }
            else if ((e.State & DrawItemState.Grayed) == DrawItemState.Grayed)
            {
                textBrush = SystemBrushes.GrayText;
                state = isChecked ? RadioButtonState.CheckedDisabled : RadioButtonState.UncheckedDisabled;
            }
            else if ((e.State & DrawItemState.Selected) == DrawItemState.Selected && !Transparent)
            {
                textBrush = SystemBrushes.HighlightText;
            }
            else
            {
                textBrush = SystemBrushes.FromSystemColor(this.ForeColor);
            }

            // Draw radio button
            Rectangle bounds = e.Bounds;
            bounds.Height += 10;
            bounds.Width = size;
            RadioButtonRenderer.DrawRadioButton(e.Graphics, bounds.Location, state);

            // Draw text
            bounds = new Rectangle(e.Bounds.X + size + 2, e.Bounds.Y, e.Bounds.Width - size - 2, e.Bounds.Height);
            if (!string.IsNullOrEmpty(DisplayMember)) // Bound Datatable? Then show the column written in Displaymember
            {
                object item = this.Items[e.Index];
                object subItem = base.FilterItemOnProperty(item, base.ValueMember);
                e.Graphics.DrawString(subItem.ToString(), e.Font, textBrush, bounds, this.Align);
            }
            else
            {
                e.Graphics.DrawString(this.Items[e.Index].ToString(), e.Font, textBrush, bounds, this.Align);
            }

            // If the ListBox has focus, 
            // draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }

        /// <summary>
        /// OnParentChanged
        /// </summary>
        /// <param name="e"></param>
        protected override void OnParentChanged(EventArgs e)
        {
            // Force to change backcolor
            this.Transparent = this.IsTransparent;
        }

        /// <summary>
        /// OnParentBackColorChanged
        /// </summary>
        /// <param name="e"></param>
        protected override void OnParentBackColorChanged(EventArgs e)
        {
            // Force to change backcolor
            this.Transparent = this.IsTransparent;
        }

        #endregion
    }
}