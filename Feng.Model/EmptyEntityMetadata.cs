using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public class EmptyEntityMetadata : Singleton<EmptyEntityMetadata>, IEntityMetadata
    {
        /// <summary>
        /// 
        /// </summary>
        public string IdName
        {
            get { return null; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TableName
        {
            get { return null; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proeprtyName"></param>
        /// <returns></returns>
        public IPropertyMetadata GetPropertMetadata(string proeprtyName)
        {
            return null;
        }
    }
}
