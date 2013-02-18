using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseDataEntity : BaseEntity<long>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public sealed override long Identity
        {
            get { return this.Id; }
        }

        /// <summary>
        /// Id
        /// </summary>
        [Id(0, Name = "Id")]
        [Generator(1, Class = "identity")]
        public virtual long Id
        {
            get;
            set;
        }
    }
}
