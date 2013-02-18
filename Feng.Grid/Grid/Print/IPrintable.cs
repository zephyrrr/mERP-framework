/*
* Xceed Grid for .NET - StandardPrinting Sample Application
* Copyright (c) 2006 - Xceed Software Inc.
*
* [IPrintable.cs]
*
* This application demonstrates the basic steps necessary to print a grid programmatically.
* It uses a print preview dialog as well as printer and page setup settings forms. 
* 
* This file is part of Xceed Grid for .NET. The source code in this file 
* is only intended as a supplement to the documentation, and is provided 
* "as is", without warranty of any kind, either expressed or implied.
*/

using System;
using System.Drawing.Printing;
using System.Drawing;

namespace Feng.Grid.Print
{
    // Element that is printable on a page.
    /// <summary>
    /// 
    /// </summary>
    public interface IPrintable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridPrintDocument"></param>
        /// <param name="e"></param>
        void Print(MyGridPrintDocument gridPrintDocument, PrintPageEventArgs e);
    }
}
