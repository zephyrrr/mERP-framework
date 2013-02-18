//Copyright (C) 2004 Microsoft Corporation
//All rights reserved.
//
//THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER
//EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF
//MERCHANTIBILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//
//Date: August, 2004
//Author: Duncan Mackenzie
//http://www.duncanmackenzie.net
//Requires the release version of .NET Framework 1.1

using System.Windows.Forms;
using System.Drawing;
using System;

namespace Feng.Windows.Forms
{
    /// <summary>
    /// NoProgressBar
    /// </summary>
    public class NoProgressBar : Control, ITimerControl
    {
        public NoProgressBar()
        {
            //I put these at the top of every owner-drawn control...
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            //set up timer object
            m_Timer = new Timer();
            m_Timer.Interval = this.m_CycleSpeed;
            m_Timer.Tick += new System.EventHandler(aTimer_Tick);
            ActiveTimer();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_Timer.Tick -= new EventHandler(aTimer_Tick);
            }
            base.Dispose(disposing);
        }

        private bool m_IsTimerActive;
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:LoadingCircle"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active
        {
            get { return m_IsTimerActive; }
            set
            {
                m_IsTimerActive = value;
                ActiveTimer();
            }
        }

        /// <summary>
        /// Actives the timer.
        /// </summary>
        private void ActiveTimer()
        {
            if (m_IsTimerActive)
            {
                m_Timer.Start();
            }
            else
            {
                m_Timer.Stop();
                currentActiveItem = 0;
            }

            Invalidate();
        }

        #region "Internal Property Variables"
        ElementStyle m_ShapeToDraw = ElementStyle.Square;
        int m_ShapeSize = 8;
        int m_ShapeSpacing = 5;
        int m_CycleSpeed = 5000;
        int m_ShapeCount;
        Border3DStyle m_BorderStyle = Border3DStyle.Flat;
        #endregion

        #region "Internal Variables"
        Timer m_Timer;
        int currentActiveItem = m_initialActiveItem;
        private const int m_initialActiveItem = 1;

        #endregion

        #region "Properties"
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.Category("Appearance")]
        public ElementStyle ShapeToDraw
        {
            get { return m_ShapeToDraw; }
            set
            {
                if (value != m_ShapeToDraw)
                {
                    m_ShapeToDraw = value;
                    currentActiveItem = m_initialActiveItem;
                    this.Invalidate();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.Category("Appearance")]
        public int ShapeSize
        {
            get { return m_ShapeSize; }
            set
            {
                if (value != m_ShapeSize && value > 0)
                {
                    m_ShapeSize = value;
                    currentActiveItem = m_initialActiveItem;
                    RecalcCountAndInterval();
                    this.Invalidate();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.Category("Appearance")]
        public int ShapeSpacing
        {
            get { return m_ShapeSpacing; }
            set
            {
                if (value != m_ShapeSpacing && value > 0)
                {
                    m_ShapeSpacing = value;
                    currentActiveItem = m_initialActiveItem;
                    RecalcCountAndInterval();
                    this.Invalidate();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ShapeCount
        {
            get { return m_ShapeCount; }
        }
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.Category("Behavior")]
        public int CycleSpeed
        {
            get { return m_CycleSpeed; }
            set
            {
                if (m_CycleSpeed != value)
                {
                    m_CycleSpeed = value;
                    m_ShapeCount = 0;
                    RecalcCountAndInterval();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.Category("Appearance")]
        public Border3DStyle BorderStyle
        {
            get { return m_BorderStyle; }
            set
            {
                if (value != m_BorderStyle)
                {
                    m_BorderStyle = value;
                    this.Invalidate();
                }
            }
        }

        #endregion

        #region "Graphic Routines"
        private const int SIZE_INCR = 2;
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            ControlPaint.DrawBorder3D(g, this.ClientRectangle, this.BorderStyle);
            for (int i = 0; i < m_ShapeCount; i++)
            {
                Point pos = default(Point);
                int x = 0;
                int y = 0;
                pos = CalculateItemPosition(i);
                x = pos.X;
                y = pos.Y;
                if (i == currentActiveItem % m_ShapeCount || (i + m_ShapeCount - 1) % m_ShapeCount == currentActiveItem % m_ShapeCount
                    || (i + m_ShapeCount - 2) % m_ShapeCount == currentActiveItem % m_ShapeCount)
                {
                    DrawShape(g, this.m_ShapeToDraw, x - SIZE_INCR, y - SIZE_INCR, m_ShapeSize + (SIZE_INCR * 2));
                }
                else
                {
                    DrawShape(g, this.m_ShapeToDraw, x, y, m_ShapeSize);
                }
            }
        }

        private Point CalculateItemPosition(int index)
        {
            Point pos = default(Point);
            pos.X = (m_ShapeSpacing * (index)) + (m_ShapeSize * (index + 1));
            pos.Y = (this.Height / 2) - (m_ShapeSize / 2);
            return pos;
        }

        private void DrawShape(Graphics g, ElementStyle shape, int x, int y, int size)
        {

            SolidBrush shapeBrush = new SolidBrush(this.ForeColor);
            switch (shape)
            {
                case ElementStyle.Circle:
                    g.FillEllipse(shapeBrush, x, y, size, size);

                    break;
                case ElementStyle.Square:
                    g.FillRectangle(shapeBrush, x, y, size, size);
                    break;
            }
        }

        #endregion

        #region "Timer Event to advance animation"
        private void aTimer_Tick(object sender, System.EventArgs e)
        {
            int oldActiveItem = currentActiveItem;

            if (this.currentActiveItem >= m_ShapeCount)
            {
                this.currentActiveItem = 1;
            }
            else
            {
                this.currentActiveItem += 1;
            }

            //this.Invalidate(CalcItemRectangle((oldActiveItem + m_ShapeCount - 1) % m_ShapeCount));
            //this.Invalidate(CalcItemRectangle((currentActiveItem + 1) % m_ShapeCount));
            this.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        public void StopCycle()
        {
            m_Timer.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartCycle()
        {
            m_Timer.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public void PerformStep()
        {
            aTimer_Tick(m_Timer, System.EventArgs.Empty);
        }

        private Rectangle CalcItemRectangle(int i)
        {
            Rectangle rect = default(Rectangle);
            Point pos = default(Point);
            pos = CalculateItemPosition(i);
            {
                rect.X = pos.X - SIZE_INCR;
                rect.Y = pos.Y - SIZE_INCR;
                rect.Width = this.m_ShapeSize + (2 * SIZE_INCR);
                rect.Height = rect.Width;
            }
            return rect;
        }
        #endregion

        #region "Resize Event means recalculating the # of shapes to draw"
        protected override void OnResize(System.EventArgs e)
        {
            RecalcCountAndInterval();
            base.OnResize(e);
        }

        private const int MIN_INTERVAL = 100;
        private void RecalcCountAndInterval()
        {
            int w = this.Width;
            int newShapeCount = 0;
            if (m_ShapeSize > 0 & m_ShapeSpacing > 0)
            {
                newShapeCount = (int)Math.Floor((double)(w - m_ShapeSpacing) / (m_ShapeSize + m_ShapeSpacing));
            }
            else
            {
                newShapeCount = 1;
            }
            if (newShapeCount != m_ShapeCount && newShapeCount > 0)
            {
                int interval = this.m_CycleSpeed / newShapeCount;
                if (interval >= MIN_INTERVAL)
                {
                    m_Timer.Interval = interval;
                }
                else
                {
                    m_Timer.Interval = MIN_INTERVAL;
                }
                m_ShapeCount = newShapeCount;
            }
        }
        #endregion

        #region "Animation Stops when Enabled = False"
        protected override void OnEnabledChanged(System.EventArgs e)
        {
            m_Timer.Enabled = this.Enabled;
        }
        #endregion

    }
    /// <summary>
    /// 
    /// </summary>
    public enum ElementStyle : byte
    {
        /// <summary>
        /// 
        /// </summary>
        Square = 0,
        /// <summary>
        /// 
        /// </summary>
        Circle = 1
    }
}