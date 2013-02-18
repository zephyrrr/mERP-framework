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
using System.Collections.Generic;
using System.Text;

namespace CrystalLibrary
{
    internal sealed class Helpers
    {
        public static string FileName(string fullPath)
        {
            if (fullPath != null)
            {
                int pos = fullPath.LastIndexOf(@"\");

                if (pos > 0)
                {
                    return fullPath.Substring(pos + 1);
                }
            }
            return fullPath;
        }

        /// <summary>
		/// Returns the extension for the file name
		/// </summary>
		/// <param name="fullPath">Full path for a file, including folder names.</param>
		/// <returns>The extension for the file.</returns>
		/// <example>
		/// The following code
		/// <code>
		///		string fullPath = @"C:\Projects\Demo\LFile.cs";
		///	
		///		string result = LFile.Extension(fullPath);	
		/// </code>
		/// will return <c>cs</c> as result.
		/// </example>
		public static string FileExtension(string fullPath)
		{
			if (fullPath != null && fullPath.Length > 0)
			{
				string fileName = FileName(fullPath);

				int pos = fileName.LastIndexOf(".");

				if (pos > 0)
				{
					return fileName.Substring(pos + 1);
				}
			}
			return string.Empty;
		}
    }
}
