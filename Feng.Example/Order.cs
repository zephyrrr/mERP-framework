using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng.Example
{
    [Class(Table = "Order", OptimisticLock = OptimisticLockMode.Version)]
    public class Order : BaseBOEntity
    {
        [Property(NotNull = true)]
        public virtual string Name
        {
            get;
            set;
        }

        [Property(NotNull = true)]
        public virtual string Receipt
        {
            get;
            set;
        }

        [Property(NotNull = true)]
        public virtual string OrderTime
        {
            get;
            set;
        }

        [Bag(0, Cascade = "none", Inverse = true)]
        [Key(1, Column = "Order")]
        [OneToMany(2, ClassType = typeof(OrderLine), NotFound = NotFoundMode.Ignore)]
        public virtual IList<OrderLine> OrderLines
        {
            get;
            set;
        }
    }
}
