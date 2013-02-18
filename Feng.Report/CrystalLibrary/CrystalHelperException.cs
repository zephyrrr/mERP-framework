// -------------------------------------------------------------------------------------------
// Author	: Jan Schreuder
//
// This code is provided as freeware. The code has been tested. You are free to use this code
// in whenever and wherever you want, provided the headers in the code remain in place. 
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER 
// EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF 
// MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// -------------------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace CrystalLibrary
{
	/// <author>Jan Schreuder</author>
	/// <summary>
	/// Exception thrown by the CrystalHelper
	/// </summary>
	[Serializable]
	public class CrystalHelperException : Exception
	{
		/// <summary>
		/// 
		/// </summary>
		public CrystalHelperException() : base()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public CrystalHelperException(string message) : base(message)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected CrystalHelperException(SerializationInfo info, StreamingContext context) : base(info,context)
		{			
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public CrystalHelperException(string message,Exception inner) : base(message,inner)
		{
		}	

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		[SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter=true)]
		public override void GetObjectData( SerializationInfo info,	StreamingContext context )
		{				
			base.GetObjectData(info,context);
		}
	}	
}
