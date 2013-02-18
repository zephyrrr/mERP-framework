using System;

namespace Feng.Grid.Columns
{
	/// <summary>
	/// Description of MyColumn.
	/// </summary>
	public class MyColumn : Xceed.Grid.Column
	{
		/// <summary>
		/// 
		/// </summary>
		public MyColumn()
			: base()
		{
			
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		public MyColumn(string fileName)
			: base(fileName)
		{
			
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fieldName"></param>
		/// <param name="dataType"></param>
		public MyColumn(string fieldName, Type dataType)
			: base(fieldName, dataType)
		{
			
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="template"></param>
		public MyColumn(Xceed.Grid.Column template)
			: base(template)
		{
		}
	}
}
