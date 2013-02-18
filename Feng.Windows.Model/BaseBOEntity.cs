using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng
{
    public abstract class BaseBOEntity : BaseEntity<Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public sealed override Guid Identity
        {
            get { return this.Id; }
        }

        [Id(0, Name = "Id", Column = "Id")]
        [Generator(1, Class = "guid.comb")]
        public virtual Guid Id
        {
            get;
            set;
        }
    }
}
