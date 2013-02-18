/*
* Xceed Grid for .NET - StandardPrinting Sample Application
* Copyright (c) 2006 - Xceed Software Inc.
*
* [PrintableTextBox.cs]
*
* This application demonstrates the basic steps necessary to print a grid programmatically.
* It uses a print preview dialog as well as printer and page setup settings forms. 
* 
* This file is part of Xceed Grid for .NET. The source code in this file 
* is only intended as a supplement to the documentation, and is provided 
* "as is", without warranty of any kind, either expressed or implied.
*/

using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Text.RegularExpressions;

namespace Feng.Grid.Print
{
    /// <summary>
    /// 
    /// </summary>
    public class PrintableTextBox : IPrintable
    {
        /// <summary>
        /// 
        /// </summary>
        public Font TextFont
        {
            get { return m_font; }
            set { m_font = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get { return m_text; }
            set { m_text = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Brush DrawingBrush
        {
            get { return m_brush; }
            set { this.m_brush = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public float Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public PrintableTextBox()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_pageNumber"></param>
        /// <returns></returns>
        public string SubstituteSpecialCommands(int _pageNumber)
        {
            return Regex.Replace(m_text, @"&page;", Convert.ToString(_pageNumber));
        }

        void IPrintable.Print(MyGridPrintDocument gridPrintDocument, PrintPageEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            e.Graphics.DrawString(SubstituteSpecialCommands(gridPrintDocument.CurrentPageNumber),
                                    m_font,
                                    m_brush,
                                    m_x,
                                    m_y);
        }

        private Font m_font = new Font("Arial", 20);
        private string m_text = "Hello World";
        private Brush m_brush = Brushes.Black;
        private float m_x = 50;
        private float m_y = 50;
    }
}
