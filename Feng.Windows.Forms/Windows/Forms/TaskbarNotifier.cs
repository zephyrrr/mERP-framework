// C# TaskbarNotifier Class v1.0
// by John O'Byrne - 02 december 2002
// 01 april 2003 : Small fix in the OnMouseUp handler
// 11 january 2003 : Patrick Vanden Driessche <pvdd@devbrains.be> added a few enhancements
//           Small Enhancements/Bugfix
//           Small bugfix: When Content text measures larger than the corresponding ContentRectangle
//                         the focus rectangle was not correctly drawn. This has been solved.
//           Added KeepVisibleOnMouseOver
//           Added ReShowOnMouseOver
//           Added If the Title or Content are too long to fit in the corresponding Rectangles,
//                 the text is truncateed and the ellipses are appended (StringTrimming).

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// TaskbarNotifier allows to display MSN style/Skinned instant messaging popups
    /// </summary>
    public class TaskbarNotifier : System.Windows.Forms.Form
    {
        #region TaskbarNotifier Protected Members

        private Bitmap BackgroundBitmap = null;
        private Bitmap CloseBitmap = null;
        private Point CloseBitmapLocation;
        private Size CloseBitmapSize;
        private Rectangle RealTitleRectangle;
        private Rectangle RealContentRectangle;
        private Rectangle WorkAreaRectangle;
        private Timer timer = new Timer();
        private TaskbarStates m_taskbarState = TaskbarStates.hidden;
        private string m_titleText;
        private string m_contentText;
        private Color m_normalTitleColor = Color.FromArgb(255, 0, 0);
        private Color m_hoverTitleColor = Color.FromArgb(255, 0, 0);
        private Color m_normalContentColor = Color.FromArgb(0, 0, 0);
        private Color m_hoverContentColor = Color.FromArgb(0, 0, 0x66);
        private Font m_normalTitleFont = new Font("Arial", 12, FontStyle.Regular, GraphicsUnit.Pixel);
        private Font m_hoverTitleFont = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel);
        private Font m_normalContentFont = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Pixel);
        private Font m_hoverContentFont = new Font("Arial", 11, FontStyle.Regular, GraphicsUnit.Pixel);
        private int nShowEvents;
        private int nHideEvents;
        private int nVisibleEvents;
        private int nIncrementShow;
        private int nIncrementHide;
        private bool bIsMouseOverPopup = false;
        private bool bIsMouseOverClose = false;
        private bool bIsMouseOverContent = false;
        private bool bIsMouseOverTitle = false;
        private bool bIsMouseDown = false;
        private bool bKeepVisibleOnMouseOver = true; // Added Rev 002
        private bool bReShowOnMouseOver = false; // Added Rev 002

        #endregion

        #region TaskbarNotifier Public Members

        /// <summary>
        /// 
        /// </summary>
        public Rectangle TitleRectangle;

        /// <summary>
        /// 
        /// </summary>
        public Rectangle ContentRectangle;

        /// <summary>
        /// 
        /// </summary>
        public bool TitleClickable = false;

        /// <summary>
        /// 
        /// </summary>
        public bool ContentClickable = true;

        /// <summary>
        /// 
        /// </summary>
        public bool CloseClickable = true;

        /// <summary>
        /// 
        /// </summary>
        public bool EnableSelectionRectangle = true;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CloseClick = null;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler TitleClick = null;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ContentClick = null;

        #endregion

        #region TaskbarNotifier Enums

        /// <summary>
        /// List of the different popup animation status
        /// </summary>
        public enum TaskbarStates
        {
            /// <summary>
            /// 
            /// </summary>
            hidden = 0,
            /// <summary>
            /// 
            /// </summary>
            appearing = 1,
            /// <summary>
            /// 
            /// </summary>
            visible = 2,
            /// <summary>
            /// 
            /// </summary>
            disappearing = 3
        }

        #endregion

        #region TaskbarNotifier Constructor

        /// <summary>
        /// The Constructor for TaskbarNotifier
        /// </summary>
        public TaskbarNotifier()
        {
            // Window Style
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Minimized;
            base.Show();
            base.Hide();
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = false;
            TopMost = true;
            MaximizeBox = false;
            MinimizeBox = false;
            ControlBox = false;

            timer.Enabled = true;
            timer.Tick += new EventHandler(OnTimer);
        }

        #endregion

        #region TaskbarNotifier Properties

        /// <summary>
        /// Get the current TaskbarState (hidden, showing, visible, hiding)
        /// </summary>
        public TaskbarStates TaskbarState
        {
            get { return m_taskbarState; }
        }

        /// <summary>
        /// Get/Set the popup Title Text
        /// </summary>
        public string TitleText
        {
            get { return m_titleText; }
            set
            {
                m_titleText = value;
                Refresh();
            }
        }

        /// <summary>
        /// Get/Set the popup Content Text
        /// </summary>
        public string ContentText
        {
            get { return m_contentText; }
            set
            {
                m_contentText = value;
                Refresh();
            }
        }

        /// <summary>
        /// Get/Set the Normal Title Color
        /// </summary>
        public Color NormalTitleColor
        {
            get { return m_normalTitleColor; }
            set
            {
                m_normalTitleColor = value;
                Refresh();
            }
        }

        /// <summary>
        /// Get/Set the Hover Title Color
        /// </summary>
        public Color HoverTitleColor
        {
            get { return m_hoverTitleColor; }
            set
            {
                m_hoverTitleColor = value;
                Refresh();
            }
        }

        /// <summary>
        /// Get/Set the Normal Content Color
        /// </summary>
        public Color NormalContentColor
        {
            get { return m_normalContentColor; }
            set
            {
                m_normalContentColor = value;
                Refresh();
            }
        }

        /// <summary>
        /// Get/Set the Hover Content Color
        /// </summary>
        public Color HoverContentColor
        {
            get { return m_hoverContentColor; }
            set
            {
                m_hoverContentColor = value;
                Refresh();
            }
        }

        /// <summary>
        /// Get/Set the Normal Title Font
        /// </summary>
        public Font NormalTitleFont
        {
            get { return m_normalTitleFont; }
            set
            {
                m_normalTitleFont = value;
                Refresh();
            }
        }

        /// <summary>
        /// Get/Set the Hover Title Font
        /// </summary>
        public Font HoverTitleFont
        {
            get { return m_hoverTitleFont; }
            set
            {
                m_hoverTitleFont = value;
                Refresh();
            }
        }

        /// <summary>
        /// Get/Set the Normal Content Font
        /// </summary>
        public Font NormalContentFont
        {
            get { return m_normalContentFont; }
            set
            {
                m_normalContentFont = value;
                Refresh();
            }
        }

        /// <summary>
        /// Get/Set the Hover Content Font
        /// </summary>
        public Font HoverContentFont
        {
            get { return m_hoverContentFont; }
            set
            {
                m_hoverContentFont = value;
                Refresh();
            }
        }

        /// <summary>
        /// Indicates if the popup should remain visible when the mouse pointer is over it.
        /// Added Rev 002
        /// </summary>
        public bool KeepVisibleOnMousOver
        {
            get { return bKeepVisibleOnMouseOver; }
            set { bKeepVisibleOnMouseOver = value; }
        }

        /// <summary>
        /// Indicates if the popup should appear again when mouse moves over it while it's disappearing.
        /// Added Rev 002
        /// </summary>
        public bool ReShowOnMouseOver
        {
            get { return bReShowOnMouseOver; }
            set { bReShowOnMouseOver = value; }
        }

        #endregion

        #region TaskbarNotifier Public Methods

        [DllImport("user32.dll")]
        private static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);

        /// <summary>
        /// Displays the popup for a certain amount of time
        /// </summary>
        /// <param name="strTitle">The string which will be shown as the title of the popup</param>
        /// <param name="strContent">The string which will be shown as the content of the popup</param>
        /// <param name="nTimeToShow">Duration of the showing animation (in milliseconds)</param>
        /// <param name="nTimeToStay">Duration of the visible state before collapsing (in milliseconds)</param>
        /// <param name="nTimeToHide">Duration of the hiding animation (in milliseconds)</param>
        /// <returns>Nothing</returns>
        public void Show(string strTitle, string strContent, int nTimeToShow, int nTimeToStay, int nTimeToHide)
        {
            WorkAreaRectangle = Screen.GetWorkingArea(WorkAreaRectangle);
            m_titleText = strTitle;
            m_contentText = strContent;
            nVisibleEvents = nTimeToStay;
            CalculateMouseRectangles();

            // We calculate the pixel increment and the timer value for the showing animation
            int nEvents;
            if (nTimeToShow > 10)
            {
                nEvents = Math.Min((nTimeToShow / 10), BackgroundBitmap.Height);
                nShowEvents = nTimeToShow / nEvents;
                nIncrementShow = BackgroundBitmap.Height / nEvents;
            }
            else
            {
                nShowEvents = 10;
                nIncrementShow = BackgroundBitmap.Height;
            }

            // We calculate the pixel increment and the timer value for the hiding animation
            if (nTimeToHide > 10)
            {
                nEvents = Math.Min((nTimeToHide / 10), BackgroundBitmap.Height);
                nHideEvents = nTimeToHide / nEvents;
                nIncrementHide = BackgroundBitmap.Height / nEvents;
            }
            else
            {
                nHideEvents = 10;
                nIncrementHide = BackgroundBitmap.Height;
            }

            switch (m_taskbarState)
            {
                case TaskbarStates.hidden:
                    m_taskbarState = TaskbarStates.appearing;
                    SetBounds(WorkAreaRectangle.Right - BackgroundBitmap.Width - 17, WorkAreaRectangle.Bottom - 1,
                              BackgroundBitmap.Width, 0);
                    timer.Interval = nShowEvents;
                    timer.Start();
                    // We Show the popup without stealing focus
                    ShowWindow(this.Handle, 4);
                    break;

                case TaskbarStates.appearing:
                    Refresh();
                    break;

                case TaskbarStates.visible:
                    timer.Stop();
                    timer.Interval = nVisibleEvents;
                    timer.Start();
                    Refresh();
                    break;

                case TaskbarStates.disappearing:
                    timer.Stop();
                    m_taskbarState = TaskbarStates.visible;
                    SetBounds(WorkAreaRectangle.Right - BackgroundBitmap.Width - 17,
                              WorkAreaRectangle.Bottom - BackgroundBitmap.Height - 1, BackgroundBitmap.Width,
                              BackgroundBitmap.Height);
                    timer.Interval = nVisibleEvents;
                    timer.Start();
                    Refresh();
                    break;
            }
        }

        /// <summary>
        /// Hides the popup
        /// </summary>
        /// <returns>Nothing</returns>
        public new void Hide()
        {
            if (m_taskbarState != TaskbarStates.hidden)
            {
                timer.Stop();
                m_taskbarState = TaskbarStates.hidden;
                base.Hide();
            }
        }

        /// <summary>
        /// Sets the background bitmap and its transparency color
        /// </summary>
        /// <param name="strFilename">Path of the Background Bitmap on the disk</param>
        /// <param name="transparencyColor">Color of the Bitmap which won't be visible</param>
        /// <returns>Nothing</returns>
        public void SetBackgroundBitmap(string strFilename, Color transparencyColor)
        {
            BackgroundBitmap = new Bitmap(strFilename);
            Width = BackgroundBitmap.Width;
            Height = BackgroundBitmap.Height;
            Region = BitmapToRegion(BackgroundBitmap, transparencyColor);
        }

        /// <summary>
        /// Sets the background bitmap and its transparency color
        /// </summary>
        /// <param name="image">Image/Bitmap object which represents the Background Bitmap</param>
        /// <param name="transparencyColor">Color of the Bitmap which won't be visible</param>
        /// <returns>Nothing</returns>
        public void SetBackgroundBitmap(Image image, Color transparencyColor)
        {
            BackgroundBitmap = new Bitmap(image);
            Width = BackgroundBitmap.Width;
            Height = BackgroundBitmap.Height;
            Region = BitmapToRegion(BackgroundBitmap, transparencyColor);
        }

        /// <summary>
        /// Sets the 3-State Close Button bitmap, its transparency color and its coordinates
        /// </summary>
        /// <param name="strFilename">Path of the 3-state Close button Bitmap on the disk (width must a multiple of 3)</param>
        /// <param name="transparencyColor">Color of the Bitmap which won't be visible</param>
        /// <param name="position">Location of the close button on the popup</param>
        /// <returns>Nothing</returns>
        public void SetCloseBitmap(string strFilename, Color transparencyColor, Point position)
        {
            CloseBitmap = new Bitmap(strFilename);
            CloseBitmap.MakeTransparent(transparencyColor);
            CloseBitmapSize = new Size(CloseBitmap.Width / 3, CloseBitmap.Height);
            CloseBitmapLocation = position;
        }

        /// <summary>
        /// Sets the 3-State Close Button bitmap, its transparency color and its coordinates
        /// </summary>
        /// <param name="image">Image/Bitmap object which represents the 3-state Close button Bitmap (width must be a multiple of 3)</param>
        /// <param name="transparencyColor">Color of the Bitmap which won't be visible</param>
        /// /// <param name="position">Location of the close button on the popup</param>
        /// <returns>Nothing</returns>
        public void SetCloseBitmap(Image image, Color transparencyColor, Point position)
        {
            CloseBitmap = new Bitmap(image);
            CloseBitmap.MakeTransparent(transparencyColor);
            CloseBitmapSize = new Size(CloseBitmap.Width / 3, CloseBitmap.Height);
            CloseBitmapLocation = position;
        }

        #endregion

        #region TaskbarNotifier Protected Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grfx"></param>
        protected void DrawCloseButton(Graphics grfx)
        {
            if (CloseBitmap != null)
            {
                Rectangle rectDest = new Rectangle(CloseBitmapLocation, CloseBitmapSize);
                Rectangle rectSrc;

                if (bIsMouseOverClose)
                {
                    if (bIsMouseDown)
                    {
                        rectSrc = new Rectangle(new Point(CloseBitmapSize.Width * 2, 0), CloseBitmapSize);
                    }
                    else
                    {
                        rectSrc = new Rectangle(new Point(CloseBitmapSize.Width, 0), CloseBitmapSize);
                    }
                }
                else
                {
                    rectSrc = new Rectangle(new Point(0, 0), CloseBitmapSize);
                }


                grfx.DrawImage(CloseBitmap, rectDest, rectSrc, GraphicsUnit.Pixel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grfx"></param>
        protected void DrawText(Graphics grfx)
        {
            if (m_titleText != null && m_titleText.Length != 0)
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Center;
                sf.FormatFlags = StringFormatFlags.NoWrap;
                sf.Trimming = StringTrimming.EllipsisCharacter; // Added Rev 002
                if (bIsMouseOverTitle)
                {
                    grfx.DrawString(m_titleText, m_hoverTitleFont, new SolidBrush(m_hoverTitleColor), TitleRectangle, sf);
                }
                else
                {
                    grfx.DrawString(m_titleText, m_normalTitleFont, new SolidBrush(m_normalTitleColor), TitleRectangle,
                                    sf);
                }
            }

            if (m_contentText != null && m_contentText.Length != 0)
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                sf.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
                sf.Trimming = StringTrimming.Word; // Added Rev 002

                if (bIsMouseOverContent)
                {
                    grfx.DrawString(m_contentText, m_hoverContentFont, new SolidBrush(m_hoverContentColor),
                                    ContentRectangle, sf);
                    if (EnableSelectionRectangle)
                    {
                        ControlPaint.DrawBorder3D(grfx, RealContentRectangle, Border3DStyle.Etched,
                                                  Border3DSide.Top | Border3DSide.Bottom | Border3DSide.Left |
                                                  Border3DSide.Right);
                    }
                }
                else
                {
                    grfx.DrawString(m_contentText, m_normalContentFont, new SolidBrush(m_normalContentColor),
                                    ContentRectangle, sf);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void CalculateMouseRectangles()
        {
            Graphics grfx = CreateGraphics();
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            sf.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
            SizeF sizefTitle = grfx.MeasureString(m_titleText, m_hoverTitleFont, TitleRectangle.Width, sf);
            SizeF sizefContent = grfx.MeasureString(m_contentText, m_hoverContentFont, ContentRectangle.Width, sf);
            grfx.Dispose();

            // Added Rev 002
            //We should check if the title size really fits inside the pre-defined title rectangle
            if (sizefTitle.Height > TitleRectangle.Height)
            {
                RealTitleRectangle = new Rectangle(TitleRectangle.Left, TitleRectangle.Top, TitleRectangle.Width,
                                                   TitleRectangle.Height);
            }
            else
            {
                RealTitleRectangle = new Rectangle(TitleRectangle.Left, TitleRectangle.Top, (int) sizefTitle.Width,
                                                   (int) sizefTitle.Height);
            }
            RealTitleRectangle.Inflate(0, 2);

            // Added Rev 002
            //We should check if the Content size really fits inside the pre-defined Content rectangle
            if (sizefContent.Height > ContentRectangle.Height)
            {
                RealContentRectangle =
                    new Rectangle((ContentRectangle.Width - (int) sizefContent.Width) / 2 + ContentRectangle.Left,
                                  ContentRectangle.Top, (int) sizefContent.Width, ContentRectangle.Height);
            }
            else
            {
                RealContentRectangle =
                    new Rectangle((ContentRectangle.Width - (int) sizefContent.Width) / 2 + ContentRectangle.Left,
                                  (ContentRectangle.Height - (int) sizefContent.Height) / 2 + ContentRectangle.Top,
                                  (int) sizefContent.Width, (int) sizefContent.Height);
            }
            RealContentRectangle.Inflate(0, 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="transparencyColor"></param>
        /// <returns></returns>
        protected Region BitmapToRegion(Bitmap bitmap, Color transparencyColor)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException("Bitmap", "Bitmap cannot be null!");
            }

            int height = bitmap.Height;
            int width = bitmap.Width;

            GraphicsPath path = new GraphicsPath();

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (bitmap.GetPixel(i, j) == transparencyColor)
                    {
                        continue;
                    }

                    int x0 = i;

                    while ((i < width) && (bitmap.GetPixel(i, j) != transparencyColor))
                    {
                        i++;
                    }

                    path.AddRectangle(new Rectangle(x0, j, i - x0, 1));
                }
            }

            Region region = new Region(path);
            path.Dispose();
            return region;
        }

        #endregion

        #region TaskbarNotifier Events Overrides

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ea"></param>
        protected void OnTimer(Object obj, EventArgs ea)
        {
            switch (m_taskbarState)
            {
                case TaskbarStates.appearing:
                    if (Height < BackgroundBitmap.Height)
                    {
                        SetBounds(Left, Top - nIncrementShow, Width, Height + nIncrementShow);
                    }
                    else
                    {
                        timer.Stop();
                        Height = BackgroundBitmap.Height;
                        timer.Interval = nVisibleEvents;
                        m_taskbarState = TaskbarStates.visible;
                        timer.Start();
                    }
                    break;

                case TaskbarStates.visible:
                    timer.Stop();
                    timer.Interval = nHideEvents;
                    // Added Rev 002
                    if ((bKeepVisibleOnMouseOver && !bIsMouseOverPopup) || (!bKeepVisibleOnMouseOver))
                    {
                        m_taskbarState = TaskbarStates.disappearing;
                    }
                    //taskbarState = TaskbarStates.disappearing;		// Rev 002
                    timer.Start();
                    break;

                case TaskbarStates.disappearing:
                    // Added Rev 002
                    if (bReShowOnMouseOver && bIsMouseOverPopup)
                    {
                        m_taskbarState = TaskbarStates.appearing;
                    }
                    else
                    {
                        if (Top < WorkAreaRectangle.Bottom)
                        {
                            SetBounds(Left, Top + nIncrementHide, Width, Height - nIncrementHide);
                        }
                        else
                        {
                            Hide();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ea"></param>
        protected override void OnMouseEnter(EventArgs ea)
        {
            base.OnMouseEnter(ea);
            bIsMouseOverPopup = true;
            Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ea"></param>
        protected override void OnMouseLeave(EventArgs ea)
        {
            base.OnMouseLeave(ea);
            bIsMouseOverPopup = false;
            bIsMouseOverClose = false;
            bIsMouseOverTitle = false;
            bIsMouseOverContent = false;
            Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mea"></param>
        protected override void OnMouseMove(MouseEventArgs mea)
        {
            base.OnMouseMove(mea);

            bool bContentModified = false;

            if ((mea.X > CloseBitmapLocation.X) && (mea.X < CloseBitmapLocation.X + CloseBitmapSize.Width) &&
                (mea.Y > CloseBitmapLocation.Y) && (mea.Y < CloseBitmapLocation.Y + CloseBitmapSize.Height) &&
                CloseClickable)
            {
                if (!bIsMouseOverClose)
                {
                    bIsMouseOverClose = true;
                    bIsMouseOverTitle = false;
                    bIsMouseOverContent = false;
                    Cursor = Cursors.Hand;
                    bContentModified = true;
                }
            }
            else if (RealContentRectangle.Contains(new Point(mea.X, mea.Y)) && ContentClickable)
            {
                if (!bIsMouseOverContent)
                {
                    bIsMouseOverClose = false;
                    bIsMouseOverTitle = false;
                    bIsMouseOverContent = true;
                    Cursor = Cursors.Hand;
                    bContentModified = true;
                }
            }
            else if (RealTitleRectangle.Contains(new Point(mea.X, mea.Y)) && TitleClickable)
            {
                if (!bIsMouseOverTitle)
                {
                    bIsMouseOverClose = false;
                    bIsMouseOverTitle = true;
                    bIsMouseOverContent = false;
                    Cursor = Cursors.Hand;
                    bContentModified = true;
                }
            }
            else
            {
                if (bIsMouseOverClose || bIsMouseOverTitle || bIsMouseOverContent)
                {
                    bContentModified = true;
                }

                bIsMouseOverClose = false;
                bIsMouseOverTitle = false;
                bIsMouseOverContent = false;
                Cursor = Cursors.Default;
            }

            if (bContentModified)
            {
                Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mea"></param>
        protected override void OnMouseDown(MouseEventArgs mea)
        {
            base.OnMouseDown(mea);
            bIsMouseDown = true;

            if (bIsMouseOverClose)
            {
                Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mea"></param>
        protected override void OnMouseUp(MouseEventArgs mea)
        {
            base.OnMouseUp(mea);
            bIsMouseDown = false;

            if (bIsMouseOverClose)
            {
                Hide();

                if (CloseClick != null)
                {
                    CloseClick(this, new EventArgs());
                }
            }
            else if (bIsMouseOverTitle)
            {
                if (TitleClick != null)
                {
                    TitleClick(this, new EventArgs());
                }
            }
            else if (bIsMouseOverContent)
            {
                if (ContentClick != null)
                {
                    ContentClick(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pea"></param>
        protected override void OnPaintBackground(PaintEventArgs pea)
        {
            Graphics grfx = pea.Graphics;
            grfx.PageUnit = GraphicsUnit.Pixel;

            Graphics offScreenGraphics;
            Bitmap offscreenBitmap;

            offscreenBitmap = new Bitmap(BackgroundBitmap.Width, BackgroundBitmap.Height);
            offScreenGraphics = Graphics.FromImage(offscreenBitmap);

            if (BackgroundBitmap != null)
            {
                offScreenGraphics.DrawImage(BackgroundBitmap, 0, 0, BackgroundBitmap.Width, BackgroundBitmap.Height);
            }

            DrawCloseButton(offScreenGraphics);
            DrawText(offScreenGraphics);

            grfx.DrawImage(offscreenBitmap, 0, 0);
        }

        #endregion
    }
}