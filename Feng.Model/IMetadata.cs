using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityPropertyMetadata : IPropertyMetadata
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool NotNull
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Length
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IEntityMetadataGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityParam"></param>
        /// <returns></returns>
        IEntityMetadata GenerateEntityMetadata(object entityParam);
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IPropertyMetadata
    {
        /// <summary>
        /// 
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        bool NotNull
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        int Length
        {
            get;
        }
    }

	/// <summary>
	/// Entity–≈œ¢
	/// </summary>
	public interface IEntityMetadata
	{
        /// <summary>
        /// 
        /// </summary>
        string IdName
        {
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        string TableName
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proeprtyName"></param>
        /// <returns></returns>
        IPropertyMetadata GetPropertMetadata(string proeprtyName);
	}
}
