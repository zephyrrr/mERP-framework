using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseBOEntity : BaseEntity<Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public sealed override Guid Identity
        {
            get { return this.ID; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Id(0, Name = "ID", Column = "Id")]
        [Generator(1, Class = "guid.comb")]
        public virtual Guid ID
        {
            get;
            set;
        }
    }
}
