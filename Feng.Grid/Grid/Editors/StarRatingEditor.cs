using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Xceed.Editors;
using Xceed.Grid;
using Xceed.UI;
using Xceed.Utils;
using Xceed.Grid.Editors;
using Feng.Windows.Forms;

namespace Feng.Grid.Editors
{
    public class StarRatingEditor : CellEditorManager
    {
        internal static readonly System.Type TextEditorType = typeof(StarRatingEditor);

        /// <summary>Initializes a new instance of the TextEditor class.</summary>
        public StarRatingEditor() 
            : this(new MyImageRating())
        {
        }

        public StarRatingEditor(MyImageRating template)
            : this(template, "SelectedValueValue", true)
        {
        }

        protected StarRatingEditor(MyImageRating templateControl, string propertyName, bool handleActivationClick)
            : base(templateControl, propertyName, true, handleActivationClick)
        {
        }

        ///// <summary>Calculates the bounds of the editor.</summary>
        //protected override void CalculateEditorBoundsCore(Control control, Cell cell, ref Rectangle bounds)
        //{
        //    if ((this.TemplateControl != null) && (this.TemplateControl.BorderStyle == EnhancedBorderStyle.None))
        //    {
        //        bounds.Inflate(-1, -1);
        //    }
        //}

        //internal static void CommonSetControlAppearance(WinTextBox control, Cell cell)
        //{
        //    if (control == null)
        //    {
        //        throw new ArgumentNullException("control");
        //    }
        //    StatelessVisualGridElementStyle mergedDisplayVisualStyle = cell.GetMergedDisplayVisualStyle();
        //    control.Font = mergedDisplayVisualStyle.Font;
        //    Color backColor = mergedDisplayVisualStyle.BackColor;
        //    try
        //    {
        //        control.BackColor = backColor;
        //    }
        //    catch
        //    {
        //        control.BackColor = Color.FromArgb(0xff, backColor);
        //    }
        //    Color foreColor = mergedDisplayVisualStyle.ForeColor;
        //    try
        //    {
        //        control.ForeColor = foreColor;
        //    }
        //    catch
        //    {
        //        control.ForeColor = Color.FromArgb(0xff, foreColor);
        //    }
        //    TextBoxArea textBoxArea = control.TextBoxArea;
        //    if (textBoxArea != null)
        //    {
        //        textBoxArea.WordWrap = mergedDisplayVisualStyle.WordWrap;
        //        switch (mergedDisplayVisualStyle.HorizontalAlignment)
        //        {
        //            case Xceed.Grid.HorizontalAlignment.Left:
        //            case Xceed.Grid.HorizontalAlignment.Default:
        //                textBoxArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
        //                return;

        //            case Xceed.Grid.HorizontalAlignment.Center:
        //                textBoxArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        //                return;

        //            case Xceed.Grid.HorizontalAlignment.Right:
        //                textBoxArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        //                return;

        //            default:
        //                return;
        //        }
        //    }
        //}

        /// <summary>Creates the <see cref="T:System.Windows.Forms.Control"></see> that will be used to edit the content of cells.</summary>
        /// <returns>A <see cref="T:System.Windows.Forms.Control"></see> that will be used to edit the content of cells. If <see cref="P:Xceed.Grid.Editors.TextEditor.CreateControlMode"></see> is set to
        /// <see cref="T:Xceed.Grid.CreateControlMode"></see>.ClonedInstance, a new instance of the control should be returned.</returns>
        protected override Control CreateControl()
        {
            return (this.TemplateControl.Clone() as Control);
        }

        /// <summary>Deactivates the <see cref="control"></see> that will be used to edit the content of the <see cref="cell"></see>.</summary>
        protected override void DeactivateControlCore(Control control, Cell cell)
        {
            base.DeactivateControlCore(control, cell);
        }

        /// <summary>Retrieves the value of the <see cref="control"></see>.</summary>
        /// <returns>The value that will be assigned to the cell being edited by the <see cref="control"></see>, in the correct datatype.</returns>
        protected override object GetControlValueCore(Control control, Cell cell, System.Type returnDataType)
        {
            object rawText = ((MyImageRating)control).SelectedDataValue;
            if (rawText == null)
            {
                return cell.NullValue;
            }
            System.Type underlyingType = Nullable.GetUnderlyingType(returnDataType);
            if (underlyingType != null)
            {
                returnDataType = underlyingType;
            }
            return Convert.ChangeType(rawText, returnDataType, cell.FormatProvider);
        }

        ///// <summary>Retrieves a value representing the fitted height of the editor in pixels.</summary>
        ///// <returns>A value representing the fitted height of the editor in pixels. If -1, then no fitted height is required or desired.</returns>
        //protected override int GetFittedHeightCore(Control control, Cell cell, AutoHeightMode mode, int cellDisplayWidth, Graphics graphics, bool printing)
        //{
        //    if ((graphics == null) || (mode == AutoHeightMode.None))
        //    {
        //        return -1;
        //    }
        //    WinTextBox box = control as WinTextBox;
        //    if (box != null)
        //    {
        //        Xceed.Editors.Margins textPadding = box.TextPadding;
        //        Xceed.Editors.Margins borders = box.GetBorders();
        //        return ((((((int) Math.Ceiling((double) box.TextBoxArea.Font.GetHeight(graphics))) + textPadding.Top) + textPadding.Bottom) + borders.Top) + borders.Bottom);
        //    }
        //    Theme theme = cell.Theme;
        //    EnhancedBorderStyle borderStyle = this.TemplateControl.BorderStyle;
        //    Xceed.Editors.Margins defaultTextPadding = WinTextBoxBase.GetDefaultTextPadding(borderStyle, theme);
        //    Xceed.Editors.Margins defaultBorders = WinTextBoxBase.GetDefaultBorders(borderStyle, theme);
        //    return ((((((int) Math.Ceiling((double) cell.Font.GetHeight(graphics))) + defaultTextPadding.Top) + defaultTextPadding.Bottom) + defaultBorders.Top) + defaultBorders.Bottom);
        //}

        /// <summary>Gets a boolean value indicating if the control should handle the mouse click once it is activated.</summary>
        /// <returns>true if the control should handle the mouse click once it is activated; false otherwise.</returns>
        protected override bool HandleActivationClick(Control control, Cell cell, Point mousePosition)
        {
            WinTextBox box = control as WinTextBox;
            if ((box != null) && box.TextBoxArea.SelectOnFocus)
            {
                return !box.RectangleToScreen(box.TextBoxArea.Bounds).Contains(mousePosition);
            }
            return base.HandleActivationClick(control, cell, mousePosition);
        }


        /// <summary>Sets the appearance of the control that will be used to edit the content of the <see cref="cell"></see>.</summary>
        protected override void SetControlAppearanceCore(Control control, Cell cell)
        {
            //CommonSetControlAppearance(control as WinTextBox, cell);
        }

        /// <summary>Assigns the value of the <see cref="cell"></see> to the <see cref="control"></see>.</summary>
        protected override void SetControlValueCore(Control control, Cell cell)
        {
            object obj2 = cell.Value;
            bool valueWasNull = ((obj2 == null) || (obj2 == DBNull.Value)) || obj2.Equals(cell.NullValue);
            if (valueWasNull)
            {
                ((MyImageRating)control).SelectedDataValue = 0;
            }
            else
            {
                int str = (int)obj2;
                ((MyImageRating)control).SelectedDataValue = str;
            }
        }

        /// <summary>Gets a value representing the mode in which the control used to edit the content of cells is created.</summary>
        protected override Xceed.Grid.CreateControlMode CreateControlMode
        {
            get
            {
                return Xceed.Grid.CreateControlMode.ClonedInstance;
            }
        }

        public new MyImageRating TemplateControl
        {
            get
            {
                return (base.TemplateControl as MyImageRating);
            }
        }
    }
}

