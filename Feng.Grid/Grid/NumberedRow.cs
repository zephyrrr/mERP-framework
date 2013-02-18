/*
 * Xceed Grid for .NET - Custom UI Sample Application
 * Copyright (c) 2002-2007 Xceed Software Inc.
 * 
 * [CustomDataRow.cs]
 * 
 * This file demonstrates how to create classes that derive from 
 * the DataRow class in order to do custom painting.
 * 
 * This file is part of Xceed Grid for .NET. The source code in this file 
 * is only intended as a supplement to the documentation, and is provided 
 * "as is", without warranty of any kind, either expressed or implied.
 */

using System;

namespace Feng.Grid
{
	public class NumberedRow : Xceed.Grid.DataRow
	{    
		/// <summary>
		/// Constructor
		/// </summary>
		public NumberedRow():
				base(new NumberedRowSelector())
	    {
	    }

		/// <summary>
	    /// Initializes a new instance of the <see cref="DataRow"/> class
	    /// specifying the template that will be used to create the data rows
	    /// contained within the grid.
	    /// </summary>
	    /// <param name="template">A reference to a <see cref="DataRow"/> object that
	    /// will be used to create the data rows contained within the grid.</param>
	    protected internal NumberedRow( NumberedRow template )
	      : base( template )
	    {
	    }

	    protected override Xceed.Grid.Row CreateInstance()
	    {
	      return new NumberedRow( this );
	    }
	
//		protected override Cell CreateCell(Column parentColumn)
//		{
//			return new CustomDataCell( m_customUI, parentColumn );
//		}		
	}
}
