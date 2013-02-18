namespace Feng.Windows.Forms
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Security.Permissions;
    using System.Windows.Forms;
    using Xceed.Grid;
    using Xceed.UI;

    [ToolboxItem(false)]
    public class ThemedDateTimePicker : DateTimePicker
    {
        private Rectangle m_buttonRectangle = Rectangle.Empty;
        private int m_overButton = -1;

        internal ThemedDateTimePicker()
        {
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnCloseUp(EventArgs e)
        {
            base.OnCloseUp(e);
            base.Invalidate();
        }

        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);
            base.Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            base.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            base.Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            base.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            base.Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if ((this.m_buttonRectangle.Contains(base.PointToClient(Control.MousePosition)) && (this.m_overButton == 0)) || (!this.m_buttonRectangle.Contains(base.PointToClient(Control.MousePosition)) && (this.m_overButton == 1)))
            {
                base.Invalidate(this.m_buttonRectangle);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            base.Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            base.Invalidate();
        }

        [SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true), SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
        protected override void WndProc(ref Message m)
        {
            Graphics graphics = null;
            switch (m.Msg)
            {
                case 15:
                    break;

                case 0x84:
                    base.Invalidate();
                    base.WndProc(ref m);
                    return;

                case 0x317:
                    graphics = Graphics.FromHdc(m.WParam);
                    break;

                default:
                    base.WndProc(ref m);
                    return;
            }
            if (graphics == null)
            {
                graphics = Graphics.FromHwnd(base.Handle);
            }
            GraphicsContainer container = graphics.BeginContainer();
            base.WndProc(ref m);
            Rectangle empty = Rectangle.Empty;
            this.Theme.CalculateComboBoxRectangles(base.ClientRectangle, this.RightToLeft == RightToLeft.Yes, out empty, out this.m_buttonRectangle);
            int uiStateFlags = 0;
            if (!base.Enabled)
            {
                uiStateFlags = ButtonUIState.Disabled;
            }
            else if (!this.m_buttonRectangle.Contains(base.PointToClient(Control.MousePosition)))
            {
                this.m_overButton = 0;
            }
            else
            {
                this.m_overButton = 1;
                if ((Control.MouseButtons == MouseButtons.Left) && this.Focused)
                {
                    uiStateFlags = ButtonUIState.Down;
                }
                else
                {
                    uiStateFlags = ButtonUIState.Hot;
                }
            }
            graphics.BeginContainer();
            ButtonUIState uiState = new ButtonUIState(uiStateFlags);
            graphics.Clip = new Region(new Rectangle(empty.X, empty.Y, 2, empty.Height));
            this.Theme.PaintComboBox(graphics, base.ClientRectangle, uiState, this.ForeColor, this.BackColor, 1.0, empty, this.m_buttonRectangle);
            graphics.Clip = new Region(new Rectangle(2, empty.Y, empty.Width, 2));
            this.Theme.PaintComboBox(graphics, base.ClientRectangle, uiState, this.ForeColor, this.BackColor, 1.0, empty, this.m_buttonRectangle);
            graphics.Clip = new Region(new Rectangle(2, empty.Bottom - 2, empty.Width, 2));
            this.Theme.PaintComboBox(graphics, base.ClientRectangle, uiState, this.ForeColor, this.BackColor, 1.0, empty, this.m_buttonRectangle);
            graphics.Clip = new Region(new Rectangle(empty.Right - 2, 2, 2, empty.Height - 4));
            this.Theme.PaintComboBox(graphics, base.ClientRectangle, uiState, this.ForeColor, this.BackColor, 1.0, empty, this.m_buttonRectangle);
            graphics.Clip = new Region(this.m_buttonRectangle);
            this.Theme.PaintComboBox(graphics, base.ClientRectangle, uiState, this.ForeColor, this.BackColor, 1.0, empty, this.m_buttonRectangle);
            graphics.EndContainer(container);
            graphics.Dispose();
        }

        internal Xceed.UI.Theme AmbientTheme
        {
            get
            {
                //if ((base.Parent != null) && (base.Parent is EditorContainer))
                //{
                //    return ((EditorContainer) base.Parent).Theme;
                //}
                return this.DefaultTheme;
            }
        }

        private Xceed.UI.Theme m_defaultTheme = Feng.Utils.ReflectionHelper.CreateInstanceFromName("Xceed.UI.WindowsXP.Silver.v1.4", "Xceed.UI.WindowsXP.Silver.WindowsXPSilver") as Xceed.UI.Theme;
        internal Xceed.UI.Theme DefaultTheme
        {
            get
            {
                return m_defaultTheme;// Xceed.Grid.ThemeCache.System;
            }
        }

        internal Xceed.UI.Theme Theme
        {
            get
            {
                return this.AmbientTheme;
            }
        }
    }
}

