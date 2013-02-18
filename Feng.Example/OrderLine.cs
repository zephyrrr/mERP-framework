using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping.Attributes;

namespace Feng.Example
{
    [Class(Table = "OrderLine", OptimisticLock = OptimisticLockMode.Version)]
    public class OrderLine : BaseBOEntity
    {
        [OneToOne(Cascade = "none", Constrained = true, ForeignKey = "FK_OrderLine_Product")]
        public virtual Product Product
        {
            get;
            set;
        }

        [Property(Column = "Product", NotNull = true)]
        public virtual Guid ProductId
        {
            get;
            set;
        }

        [Property(NotNull = true)]
        public virtual int Count
        {
            get;
            set;
        }

        [ManyToOne(NotNull = true, ForeignKey = "FK_Order_OrderLine", Column = "Order")]
        public virtual Order Order
        {
            get;
            set;
        }

    }
}
