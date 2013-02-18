/*
* Xceed Grid for .NET - StandardPrinting Sample Application
* Copyright (c) 2006 - Xceed Software Inc.
*
* [MyGridPrintDocument.cs]
*
* This application demonstrates the basic steps necessary to print a grid programmatically.
* It uses a print preview dialog as well as printer and page setup settings forms. 
* 
* This file is part of Xceed Grid for .NET. The source code in this file 
* is only intended as a supplement to the documentation, and is provided 
* "as is", without warranty of any kind, either expressed or implied.
*/
using System;
using Xceed.Grid;
using System.Drawing.Printing;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;

namespace Feng.Grid.Print
{
    /// <summary>
    /// 
    /// </summary>
    public class MyGridPrintDocument : GridPrintDocument
    {
        // These printable elements contain elements that will be used to printed 
        // over each pages. The current implementation prints can print headers 
        // and/or footers containing the page number.
        /// <summary>
        /// 
        /// </summary>
        public Hashtable PrintableElements
        {
            get { return m_additionalPrintableElements; }
        }

        // Exposes the page settings for the different setup forms.
        /// <summary>
        /// 
        /// </summary>
        public PageSettings PageSettings
        {
            get { return m_documentPageSettings; }
            set { m_documentPageSettings = value; }
        }


        // Constructs a MyGridPrintDocument that can be used to print the grid passed 
        // as a parameter.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        public MyGridPrintDocument(GridControl grid)
            : base(grid)
        {
            m_documentPageSettings = (PageSettings)base.DefaultPageSettings.Clone();
            m_additionalPrintableElements = new Hashtable();
        }

        // Query page settings is called for each print or print preview task.
        // For every MyGridPrintDocument created the initial page settings is the base DefaultPageSettings.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnQueryPageSettings(QueryPageSettingsEventArgs e)
        {
            e.PageSettings = m_documentPageSettings;
            base.OnQueryPageSettings(e);
        }

        // Overrides the OnPrintPage to print configured headers and footers.
        // This is called for all pages of the grid.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            base.OnPrintPage(e);

            // Print added text boxes ( header and footers )
            if (!e.Cancel)
            {
                foreach (DictionaryEntry entry in m_additionalPrintableElements)
                {
                    ((IPrintable)entry.Value).Print(this, e);
                }
            }

        }

        // Page Settings for this Grid Document
        private PageSettings m_documentPageSettings;

        // Printable Elements added consistently to all document pages
        private Hashtable m_additionalPrintableElements;
    }
}