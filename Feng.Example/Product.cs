using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng.Example
{
    [Class(Table = "Product", OptimisticLock = OptimisticLockMode.Version)]
    public class Product : BaseBOEntity
    {
        [Property(NotNull = true)]
        public virtual string Name
        {
            get;
            set;
        }

        [Property(NotNull = false)]
        public virtual string Description
        {
            get;
            set;
        }

        [Property(NotNull = true)]
        public virtual decimal Price
        {
            get;
            set;
        }
    }
}
